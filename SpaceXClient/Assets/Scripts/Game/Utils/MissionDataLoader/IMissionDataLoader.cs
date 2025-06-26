using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Utils.MissionData;

namespace Game.Utils.MissionDataLoader
{
    public interface IMissionDataLoader
    {
        Task<List<MissionInfo>> LoadMissions();
    }
}