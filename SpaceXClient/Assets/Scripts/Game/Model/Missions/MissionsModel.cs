using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Utils.MissionData;
using Game.Utils.MissionDataLoader;
using MVP.Model;

namespace Game.Model.Missions
{
    public class MissionsModel: IModel
    {
        private List<MissionInfo> missions;
        private IMissionDataLoader missionDataLoader;
        
        public async Task InitAsync(IMissionDataLoader missionDataLoader)
        {
            if (missionDataLoader == null)
                throw new ArgumentNullException(nameof(missionDataLoader));
            
            this.missionDataLoader = missionDataLoader;
        }

        public async Task<List<MissionInfo>> GetLaunches()
        {
            if (missions != null)
                return missions;
            
            if (missionDataLoader == null)
                throw new Exception("Mission data loader is not set");
            
            missions = await missionDataLoader.LoadMissions();
            
            return missions;
        }
    }
}