using System;
using Game.Utils.MissionData;
using MVP.View;
using TMPro;
using UnityEngine;

namespace Game.View.Missions
{
    public class MissionDetailsUIView: MonoBehaviour, IView
    {
        [Header("Item objects (Can't be null)")]
        public TMP_Text missionDataText;
        
        public event Action OnClose;

        public void Awake()
        {
            if (missionDataText == null)
                throw new NullReferenceException("Mission data text cannot be null");
        }
        
        public void SetMissionInfo(MissionInfo missionInfo)
        {
            missionDataText.text = GetDescriptionText(missionInfo);
        }
        
        private string GetDescriptionText(MissionInfo missionInfo)
        {
            //TODO: show more relevant data also
            return $"Name: {missionInfo.Spaceship.Name}\nShip type: {missionInfo.Spaceship.Type}\n" +
                   $"Home port: {missionInfo.Spaceship.HomePort}\n Missions number: {missionInfo.Spaceship.MissionsNumber}";
        }
        
        public void OnCloseClick()
        {
            OnClose?.Invoke();
        }
    }
}