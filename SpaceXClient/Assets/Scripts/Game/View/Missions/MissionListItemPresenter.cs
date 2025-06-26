using System;
using Game.Utils.MissionData;
using TMPro;
using UnityEngine;

namespace Game.View.Missions
{
    public class MissionListItemPresenter: MonoBehaviour
    {
        [Header("Item objects (Can't be null)")]
        public TMP_Text missionDataText;
        public GameObject failedStatus;
        public GameObject completedStatus;
        public GameObject upcomingStatus;
        
        public event Action<MissionInfo> OnItemClicked;
        
        private MissionInfo missionInfo;

        public void Awake()
        {
            if (missionDataText == null)
                throw new NullReferenceException("Mission data text cannot be null");
            
            if (failedStatus == null)
                throw new NullReferenceException("Failed status cannot be null");
            
            if (completedStatus == null)
                throw new NullReferenceException("Completed status cannot be null");
            
            if (upcomingStatus == null)
                throw new NullReferenceException("Upcoming status cannot be null");
        }

        public void SetMissionData(MissionInfo missionInfo)
        {
            this.missionInfo = missionInfo;
            SetStatus(missionInfo.Status);
            missionDataText.text = GetDescriptionText(missionInfo);
        }

        private string GetDescriptionText(MissionInfo missionInfo)
        {
            //TODO: show more relevant data also
            return $"Status: {missionInfo.Status}\nName: {missionInfo.Name}\nPayloads number: {missionInfo.PayloadsNumber}\n" +
                   $"Rocket name: {missionInfo.Spaceship.Name}\n Origin country: {missionInfo.Spaceship.OriginCountry}";
        }

        private void SetStatus(MissionStatus status)
        {
            switch (status)
            {
                case MissionStatus.Failed:
                    failedStatus.SetActive(true);
                    completedStatus.SetActive(false);
                    upcomingStatus.SetActive(false);
                    break;
                case MissionStatus.Completed:
                    failedStatus.SetActive(false);
                    completedStatus.SetActive(true);
                    upcomingStatus.SetActive(false);
                    break;
                case MissionStatus.Upcoming:
                    failedStatus.SetActive(false);
                    completedStatus.SetActive(false);
                    upcomingStatus.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        public void OnMissionItemClicked()
        {
            OnItemClicked?.Invoke(missionInfo);
        }
    }
}