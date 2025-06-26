using UnityEngine;

namespace Game.Utils.Assets.Prefab
{
    public class DontDestroyOnLoad: MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }
    }
}