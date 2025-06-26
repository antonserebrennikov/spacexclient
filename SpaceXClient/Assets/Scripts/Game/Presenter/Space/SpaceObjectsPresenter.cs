using System;
using System.Text;
using Game.Events;
using Game.Model.Space;
using Game.Utils.Camera;
using Game.Utils.Coordinates;
using Game.Utils.Time;
using Game.View.Space;
using Jnk.TinyContainer;
using MVP.Presenter;
using UnityEngine;

namespace Game.Presenter.Space
{
    public class SpaceObjectsPresenter: MonoBehaviour, IPresenter, ICurrentDateStringProvider, IOrbitalDataStringProvider, ISpaceSimulationTrailProvider
    {
        [Header("Simulation space objects (Can't be null)")]
        public Transform roadster;
        public Transform sun;
        public Transform earth;
        
        [Header("Simulation spaceCamera (Can't be null)")]
        public Camera spaceCamera;
        [Range(0.5f, 2)]
        public float cameraOffsetMultiplier = 2f;
        
        [Header("Simulation Settings")]
        [Range(1, 100)]
        public float simulationSpeedDaysPerSecond;
        //TODO: make configurable
        private DateTime fromDate = new DateTime(2018, 2, 7);
        private DateTime toDate = new DateTime(2019, 10, 8);
        
        //TODO: remove all references from here (to the SpaceStarter)
        public SpaceTrailView TrailView;
        
        //TODO: need to refactor and move out functionality
        //ICurrentDateStringProvider
        public event Action<string> OnCurrentDateStringChanged;
        //IOrbitalDataStringProvider
        public event Action<string> OnOrbitalDataChanged;
        //ISpaceSimulationTrailProvider
        public event Action<Vector3> OnNextPosition;
        public event Action OnReset;

        private const string currenDateText = "Current date: {0}";
        private SpaceModel spaceModel;
        private IOrbitalCoordinatesConverter coordinatesConverter;
        private RoadsterPositionToVector3Converter roadsterPositionToVector3Converter = new();
        private DateSimulator dateSimulator;
        private Vector3 lastPosition;
        private DateTime lastDate;
        private StringBuilder stringBuilder = new StringBuilder();
        
        private void Awake()
        {
            if (roadster == null)
                throw new NullReferenceException("Roadster cannot be null");
            
            if (sun == null)
                throw new NullReferenceException("Sun cannot be null");
            
            if (earth == null)
                throw new NullReferenceException("Earth cannot be null");
            
            if (spaceCamera == null)
                throw new NullReferenceException("Camera cannot be null");
            
            if (simulationSpeedDaysPerSecond <= 0)
                throw new ArgumentException("Simulation speed must be greater than 0");
            
            if (TrailView == null)
                throw new NullReferenceException("TrailPresenter cannot be null");
            
            TinyContainer.For(this).Get(out spaceModel);
            TinyContainer.For(this).Get(out coordinatesConverter);
            TinyContainer.ForSceneOf(this).Register<ICurrentDateStringProvider>(this);
            TinyContainer.ForSceneOf(this).Register<IOrbitalDataStringProvider>(this);
            TinyContainer.ForSceneOf(this).Register<ISpaceSimulationTrailProvider>(this);
        }
        
        private void Start()
        {
            sun.transform.position = Vector3.zero;

            var launchSitePosition = roadsterPositionToVector3Converter.Convert(coordinatesConverter, spaceModel.GetRoadsterPosition(0));
            
            earth.transform.position = launchSitePosition;
            dateSimulator = new DateSimulator(fromDate, toDate, Time.time);

            OnNextPosition += TrailView.OnNextPosition;
            OnReset += TrailView.OnReset;
            
            Update();
        }

        public void Update()
        {
            UpdateRoadsterPosition();
            UpdateCameraPosition();
            DrawDebugInfo();
        }

        private void UpdateRoadsterPosition()
        {
            var currentDate = dateSimulator.Update(Time.time, simulationSpeedDaysPerSecond);
            var currentOrbitalData = spaceModel.GetRoadsterPosition(currentDate);
            var currentPosition = roadsterPositionToVector3Converter.Convert(coordinatesConverter, currentOrbitalData);

            if (currentPosition != lastPosition)
            {
                roadster.transform.position = currentPosition;
                OnOrbitalDataChanged?.Invoke(currentOrbitalData.ToString());

                if (lastDate > currentDate)
                    OnReset?.Invoke();
                else
                    OnNextPosition?.Invoke(currentPosition);

                lastPosition = currentPosition;
            }

            lastDate = currentDate;

            // Format the date as a localized string
            stringBuilder.Clear();
            stringBuilder.Append("Current date: ").Append(currentDate.ToString("f", System.Globalization.CultureInfo.CurrentCulture));
            OnCurrentDateStringChanged?.Invoke(stringBuilder.ToString());
        }

        private void UpdateCameraPosition()
        {
            var cameraOffset = CameraHelper.GetCameraOffset(sun.transform.position, earth.transform.position,
                roadster.transform.position, cameraOffsetMultiplier);
            
            //TODO: fix hardcoded value
            //TODO: add interpolation
            CameraHelper.UpdateCameraPosition(spaceCamera, sun.transform.position, earth.transform.position, roadsterPositionToVector3Converter.Convert(coordinatesConverter, spaceModel.GetRoadsterPosition(150)), cameraOffset);
            CameraHelper.UpdateCameraTarget(spaceCamera, sun.transform.position, earth.transform.position, roadster.transform.position);
        }

        private void DrawDebugInfo()
        {
#if UNITY_EDITOR
            Debug.DrawLine(sun.position, earth.position, Color.red);
            Debug.DrawLine(sun.position, roadster.position, Color.green);
            Debug.DrawLine(roadster.position, earth.position, Color.blue);
            CameraDebugHelper.DrawCameraFrustum(spaceCamera, Color.yellow);
#endif
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}