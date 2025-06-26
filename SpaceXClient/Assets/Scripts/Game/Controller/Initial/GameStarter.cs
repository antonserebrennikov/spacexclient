using System;
using System.Threading.Tasks;
using Game.Presenter.Loading;
using Game.Scene;
using Game.Utils.Assets.Prefab;
using Game.Utils.Assets.Text;
using Game.Utils.Coordinates;
using Game.Utils.MissionDataLoader;
using Game.Utils.Scene;
using Game.Utils.UI;
using Jnk.TinyContainer;
using UnityEngine;

namespace Game.Controller.Initial
{
    public class GameStarter: MonoBehaviour
    {
        [Range(30, 120)]
        public int TargetFps = 60;
        
        private ISceneLoader sceneLoader;
        private IPresenterLoader presenterLoader;
        private ModelInitializer modelInitializer;
        
        public void Awake()
        {
            Application.targetFrameRate = TargetFps;
            modelInitializer = new ModelInitializer();
            
            RegisterGlobal();
        }
        
        public void Start()
        {
            Resolve();
            _ = InitAsync();
        }

        private void RegisterGlobal()
        {
            TinyContainer.Global.Register<ISceneLoader>(new SceneLoader());
            TinyContainer.Global.Register<IPresenterLoader>(new PresenterLoader(new AddressablePrefabLoader()));
            TinyContainer.Global.Register<ITextLoader>(new AddressableTextLoader());
            TinyContainer.Global.Register<IOrbitalCoordinatesConverter>(new OrbitalCoordinatesConverter());
            TinyContainer.Global.Register<IMissionDataLoader>(new OddityMissionDataLoader());
            
            modelInitializer.RegisterModels();
        }

        private void Resolve()
        {
            TinyContainer.For(this).Get(out sceneLoader);
            TinyContainer.For(this).Get(out presenterLoader);
        }

        private async Task InitAsync()
        {
            await modelInitializer.InitModelsAsync();
            await StartGameAsync();
        }

        private async Task StartGameAsync()
        {
            try
            {
                var loadingPresenter = await presenterLoader.LoadPresenterAsync<LoadingPresenter>();
            
                TinyContainer.Global.Register<ILoadingPresenter>(loadingPresenter);
                
                await sceneLoader.LoadSceneAsync(Scenes.MainMenu);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}