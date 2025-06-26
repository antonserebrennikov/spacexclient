using System;
using MVP.View;
using UnityEngine;

namespace Game.View.MainMenu
{
    public class MainMenuUIView: MonoBehaviour, IView
    {
        public event Action OnGoToSpace;
        public event Action OnGoToMissions;

        public void GoToSpace()
        {
            OnGoToSpace?.Invoke();
        }
        
        public void GoToMissions()
        {
            OnGoToMissions?.Invoke();
        }
    }
}