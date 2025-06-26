using System;
using System.Threading.Tasks;
using Game.Model.Missions;
using Game.Model.Space;
using Game.Utils.Assets.Text;
using Game.Utils.MissionDataLoader;
using Jnk.TinyContainer;
using UnityEngine;

namespace Game.Controller.Initial
{
    public class ModelInitializer
    {
        public void RegisterModels()
        {
            TinyContainer.Global.Register(new SpaceModel());
            TinyContainer.Global.Register(new MissionsModel());
        }

        public async Task InitModelsAsync()
        {
            try
            {
                TinyContainer.Global.Get(out ITextLoader textLoader);
                TinyContainer.Global.Get(out IMissionDataLoader missionDataLoader);
                TinyContainer.Global.Get(out SpaceModel spaceModel);
                TinyContainer.Global.Get(out MissionsModel missionsModel);
                
                var spaceModelInit = spaceModel.InitAsync(textLoader);
                var missionModelInit = missionsModel.InitAsync(missionDataLoader);
                
                await Task.WhenAll(spaceModelInit, missionModelInit);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
