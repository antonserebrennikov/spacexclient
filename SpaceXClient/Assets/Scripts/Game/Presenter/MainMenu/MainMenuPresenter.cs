using System;
using System.Threading.Tasks;
using Game.Presenter.Missions;
using Game.Scene;
using Game.Utils.Scene;
using Game.Utils.UI;
using Game.View.MainMenu;
using Jnk.TinyContainer;
using MVP.Presenter;
using UnityEngine;

namespace Game.Presenter.MainMenu
{
    public class MainMenuPresenter: MonoBehaviour, IPresenter
    {
        public MainMenuUIView UIView;
        
        private ISceneLoader sceneLoader;
        private ILoadingPresenter loadingPresenter;
        private IPresenterLoader presenterLoader;
        
        public void Awake()
        {
            if (UIView == null)
                throw new NullReferenceException("UIView cannot be null");
            
            TinyContainer.For(this).Get(out sceneLoader);
            TinyContainer.For(this).Get(out loadingPresenter);
            TinyContainer.For(this).Get(out presenterLoader);
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            UIView.OnGoToSpace += OnGoToSpaceHandler;
            UIView.OnGoToMissions += OnGoToMissionsHandler;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            UIView.OnGoToSpace -= OnGoToSpaceHandler;
            UIView.OnGoToMissions -= OnGoToMissionsHandler;
        }

        private void OnGoToSpaceHandler()
        {
            _ = ShowSpaceAsync();
        }

        private async Task ShowSpaceAsync()
        {
            try
            {
                Hide();
                loadingPresenter.Show();
                
                await sceneLoader.LoadSceneAsync(Scenes.Space);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
        
        private void OnGoToMissionsHandler()
        {
            _ = ShowMissionsAsync();
        }

        private async Task ShowMissionsAsync()
        {
            try
            {
                var missionsPresenter = await presenterLoader.LoadPresenterAsync<MissionsPresenter>();
                
                missionsPresenter.Show();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}