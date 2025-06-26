using System;
using System.Threading.Tasks;
using Game.Events;
using Game.Scene;
using Game.Utils.Scene;
using Game.Utils.UI;
using Game.View.Space;
using Jnk.TinyContainer;
using MVP.Presenter;
using UnityEngine;

namespace Game.Presenter.Space
{
    public class SpaceUIPresenter: MonoBehaviour, IPresenter
    {
        public SpaceUIView UIView;
        
        private ICurrentDateStringProvider dateStringProvider;
        private IOrbitalDataStringProvider orbitalDataStringProvider;
        private ISceneLoader sceneLoader;
        private ILoadingPresenter loadingPresenter;
        private IPresenterLoader presenterLoader;

        public void Awake()
        {
            if (UIView == null)
                throw new NullReferenceException("UIView cannot be null");
            
            TinyContainer.For(this).Get(out sceneLoader);
            TinyContainer.For(this).Get(out loadingPresenter);
        }

        public void OnEnable()
        {
            MonoBehaviour sceneGo = FindAnyObjectByType<TinyContainerScene>();
            
            //Resolve dependencies for scene with scene GO
            TinyContainer.ForSceneOf(sceneGo ?? this).Get(out dateStringProvider);
            TinyContainer.ForSceneOf(sceneGo ?? this).Get(out orbitalDataStringProvider);
            UIView.OnGoToMainMenu += OnGoToMainMenuHandler;
            dateStringProvider.OnCurrentDateStringChanged += UIView.UpdateDate;
            orbitalDataStringProvider.OnOrbitalDataChanged += UIView.UpdateOrbitalData;
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            UIView.OnGoToMainMenu -= OnGoToMainMenuHandler;
            dateStringProvider.OnCurrentDateStringChanged -= UIView.UpdateDate;
            orbitalDataStringProvider.OnOrbitalDataChanged -= UIView.UpdateOrbitalData;
        }

        private void OnGoToMainMenuHandler()
        {
            _ = ShowMainMenuAsync();
        }

        private async Task ShowMainMenuAsync()
        {
            try
            {
                Hide();
                loadingPresenter.Show();
                
                await sceneLoader.LoadSceneAsync(Scenes.MainMenu);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}