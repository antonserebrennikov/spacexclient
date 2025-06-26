using System;
using Game.Utils.MissionData;
using Game.View.Missions;
using MVP.Presenter;
using UnityEngine;

namespace Game.Presenter.Missions
{
    public class MissionDetailsPresenter: MonoBehaviour, IPresenter
    {
        public MissionDetailsUIView UIView;
        
        public void Awake()
        {
            if (UIView == null)
                throw new NullReferenceException("UIView cannot be null");
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            UIView.OnClose += OnCloseClick;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            UIView.OnClose -= OnCloseClick;
        }

        public void SetMissionInfo(MissionInfo missionInfo)
        {
            UIView.SetMissionInfo(missionInfo);
        }
        
        private void OnCloseClick()
        {
            Hide();
        }
    }
}