using System.Threading.Tasks;
using UnityEngine;

namespace Game.Utils.Assets.Prefab
{
    public interface IPrefabLoader
    {
        Task<GameObject> LoadAsync(string path);
    }
}