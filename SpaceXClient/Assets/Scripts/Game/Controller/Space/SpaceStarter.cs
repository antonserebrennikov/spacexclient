using System;
using System.Threading.Tasks;
using Game.Presenter.Space;
using Game.Utils.UI;
using Jnk.TinyContainer;
using UnityEngine;

namespace Game.Controller.Space
{
    public class SpaceStarter: MonoBehaviour
    {
        public SpaceObjectsPresenter SpaceObjectsPresenter;
        
        private ILoadingPresenter loadingPresenter;
        private IPresenterLoader presenterLoader;
        
        public void Awake()
        {
            if (SpaceObjectsPresenter == null)
                throw new System.NullReferenceException("SpaceObjectsPresenter cannot be null");
        }
        
        public void Start()
        {
            TinyContainer.For(this).Get(out loadingPresenter);
            TinyContainer.For(this).Get(out presenterLoader);
            
            SpaceObjectsPresenter.Show();
            _ = InitSpaceAsync();
        }

        private async Task InitSpaceAsync()
        {
            try
            {
                var spaceUIPresenter = await presenterLoader.LoadPresenterAsync<SpaceUIPresenter>();
            
                spaceUIPresenter.Show();
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