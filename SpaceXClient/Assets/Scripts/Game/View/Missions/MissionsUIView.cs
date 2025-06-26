using System;
using System.Collections.Generic;
using Game.Utils.MissionData;
using Game.Utils.ObjectPool;
using MVP.View;
using TMPro;
using UnityEngine;

namespace Game.View.Missions
{
    public class MissionsUIView: MonoBehaviour, IView
    {
        public event Action OnGoToMainMenu;
        public event Action<MissionInfo> OnItemClicked;
        
        public GameObject loadingScreen;
        public Transform missionsContainer;
        public MissionListItemPresenter itemPresenter;
        public MissionListItemPlaceholder placeholder;
        public TMP_Text noMissionsText;
        
        private SimpleGoPool missionItemsPool;
        private readonly Dictionary<MissionListItemPlaceholder, MissionInfo> missionPlaceholders = new();
        
        public void Awake()
        {
            ValidateReferences();
            
            itemPresenter.gameObject.SetActive(false);
            placeholder.gameObject.SetActive(false);
            missionItemsPool = new SimpleGoPool();
            
            FillMissionItemsPool(20);
        }

        public void OnEnable()
        {
            noMissionsText.gameObject.SetActive(false);
        }

        private void ValidateReferences()
        {
            if (loadingScreen == null)
                throw new NullReferenceException("Loading screen cannot be null");

            if (missionsContainer == null)
                throw new NullReferenceException("Missions container cannot be null");

            if (itemPresenter == null)
                throw new NullReferenceException("Missions item presenter cannot be null");

            if (placeholder == null)
                throw new NullReferenceException("Placeholder cannot be null");
            
            if (noMissionsText == null)
                throw new NullReferenceException("No missions text cannot be null");
        }

        
        public void ShowLoadingScreen(bool show)
        {
            loadingScreen.SetActive(show);
        }

        public void ShowMissionItems(List<MissionInfo> missions)
        {
            if (missions == null || missions.Count == 0)
            {
                noMissionsText.gameObject.SetActive(true);
                return;
            }
            
            // Create copies of item presenter for each mission.
            foreach (var mission in missions)
            {
                if (missionPlaceholders.ContainsValue(mission))
                    continue;
                
                var placeholderItem = Instantiate(placeholder, missionsContainer);
                // Assuming itemPresenter has a method to update its content.
                var presenterComponent = placeholderItem.GetComponent<MissionListItemPlaceholder>();
                
                placeholderItem.gameObject.SetActive(true);
                presenterComponent.OnShow += () => OnItemShow(placeholderItem);
                presenterComponent.OnHide += () => OnItemHide(placeholderItem);
                // Map the placeholder to its corresponding mission data
                missionPlaceholders[placeholderItem] = mission;
            }
        }

        private void FillMissionItemsPool(int count)
        {
            if (missionItemsPool.Count > 0)
                return;
            
            for (var i = 0; i < count; i++)
            {
                var missionItem = Instantiate(itemPresenter.gameObject);
                
                missionItem.SetActive(false);
                missionItemsPool.ReturnToPool(missionItem);
            }
        }
        
        private void OnItemShow(MissionListItemPlaceholder placeholderItem)
        {
            if (!missionPlaceholders.TryGetValue(placeholderItem, out var missionInfo))
            {
                Debug.LogError("MissionInfo not found for the given placeholder.");
                return;
            }
            
            var missionItem = missionItemsPool.GetFromPool();
            
            missionItem.transform.SetParent(placeholderItem.transform, false);
            missionItem.gameObject.SetActive(true);
            
            var presenterComponent = missionItem.GetComponent<MissionListItemPresenter>();
            
            if (presenterComponent != null)
            {
                presenterComponent.SetMissionData(missionInfo);
                presenterComponent.OnItemClicked += OnItemClickedHandler;
            }
            else
            {
                Debug.LogError("MissionListItemPresenter component is missing on the instantiated object.");
            }
        }
        
        private void OnItemHide(MissionListItemPlaceholder placeholderItem)
        {
            if (!missionPlaceholders.TryGetValue(placeholderItem, out var missionInfo))
            {
                Debug.LogError("MissionInfo not found for the given placeholder during hide.");
                return;
            }
            
            var presenterComponent = placeholderItem.transform.GetComponentInChildren<MissionListItemPresenter>();
            
            if (presenterComponent != null)
            {
                presenterComponent.OnItemClicked -= OnItemClickedHandler;
                presenterComponent.gameObject.SetActive(false);
                presenterComponent.transform.SetParent(null, false);
            
                missionItemsPool.ReturnToPool(presenterComponent.gameObject);
            }
            else
            {
                Debug.LogError("MissionListItemPresenter component is missing on the instantiated object.");
            }
        }
        
        public void GoToMainMenu()
        {
            OnGoToMainMenu?.Invoke();
        }
        
        private void OnItemClickedHandler(MissionInfo missionInfo)
        {
            OnItemClicked?.Invoke(missionInfo);
        }
    }
}