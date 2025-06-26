using System;
using MVP.View;
using TMPro;
using UnityEngine;

namespace Game.View.Space
{
    public class SpaceUIView: MonoBehaviour, IView
    {
        //TODO: put refs into specific views
        public TMP_Text orbitalDataText;
        public TMP_Text dateText;
        public event Action OnGoToMainMenu;
        
        public void Awake()
        {
            if (orbitalDataText == null)
                throw new System.NullReferenceException("Orbital data text cannot be null");
            
            if (dateText == null)
                throw new System.NullReferenceException("Date text cannot be null");
            
            UpdateOrbitalData("");
            UpdateDate("");
        }

        public void UpdateOrbitalData(string orbitalData)
        {
            orbitalDataText.text = orbitalData;
        }

        public void UpdateDate(string date)
        {
            dateText.text = date;
        }
        
        public void GoToMainMenu()
        {
            OnGoToMainMenu?.Invoke();
        }
    }
}