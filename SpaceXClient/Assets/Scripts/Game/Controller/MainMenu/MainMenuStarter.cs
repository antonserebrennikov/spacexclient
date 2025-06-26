using System;
using System.Threading.Tasks;
using Game.Presenter.MainMenu;
using Game.Utils.UI;
using Jnk.TinyContainer;
using UnityEngine;

namespace Game.Controller.MainMenu
{
    public class MainMenuStarter: MonoBehaviour
    {
        private ILoadingPresenter loadingPresenter;
        private IPresenterLoader presenterLoader;
        
        public void Awake()
        {
            TinyContainer.For(this).Get(out loadingPresenter);
            TinyContainer.For(this).Get(out presenterLoader);
        }
        
        public void Start()
        {
            _ = InitMainMenuAsync();
        }
        
        private async Task InitMainMenuAsync()
        {
            try
            {
                var mainMenuPresenter = await presenterLoader.LoadPresenterAsync<MainMenuPresenter>();
            
                mainMenuPresenter.Show();
                loadingPresenter.Hide();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }
        }
    }
}