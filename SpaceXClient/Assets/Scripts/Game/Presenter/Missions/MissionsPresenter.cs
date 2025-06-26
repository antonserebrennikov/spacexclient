using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Model.Missions;
using Game.Utils.MissionData;
using Game.Utils.UI;
using Game.View.Missions;
using Jnk.TinyContainer;
using MVP.Presenter;
using UnityEngine;

namespace Game.Presenter.Missions
{
    public class MissionsPresenter: MonoBehaviour, IPresenter
    {
        public MissionsUIView UIView;
        
        private MissionsModel missionsModel;
        private List<MissionInfo> missions;
        private IPresenterLoader presenterLoader;
        
        public void Awake()
        {
            if (UIView == null)
                throw new NullReferenceException("UIView cannot be null");
            
            TinyContainer.For(this).Get(out missionsModel);
            TinyContainer.For(this).Get(out presenterLoader);
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            UIView.OnGoToMainMenu += OnGoToMainMenuHandler;
            UIView.OnItemClicked += OnItemClickedHandler;
            
            if (missions == null)
                _ = GetLaunchesData();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            UIView.OnGoToMainMenu -= OnGoToMainMenuHandler;
            UIView.OnItemClicked -= OnItemClickedHandler;
        }
        
        private async Task GetLaunchesData()
        {
            UIView.ShowLoadingScreen(true);
            
            try
            {
                missions = await missionsModel.GetLaunches();
                missions = missions?.OrderByDescending(mission => mission.DateUTC).ToList();

                UIView.ShowMissionItems(missions);
                UIView.ShowLoadingScreen(false);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }

        private void OnGoToMainMenuHandler()
        {
            Hide();
        }
        
        private void OnItemClickedHandler(MissionInfo missionInfo)
        {
            _ = ShowMissionDetailsViewAsync(missionInfo);
        }

        private async Task ShowMissionDetailsViewAsync(MissionInfo missionInfo)
        {
            try
            {
                var missionDetailsPresenter = await presenterLoader.LoadPresenterAsync<MissionDetailsPresenter>();
            
                missionDetailsPresenter.Show();
                missionDetailsPresenter.SetMissionInfo(missionInfo);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}