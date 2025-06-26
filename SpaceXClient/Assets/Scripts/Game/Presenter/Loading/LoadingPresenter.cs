using System;
using Game.Utils.UI;
using Game.View.Loading;
using UnityEngine;

namespace Game.Presenter.Loading
{
    public class LoadingPresenter: MonoBehaviour, ILoadingPresenter
    {
        public LoadingUIView UIView;
        
        public void Awake()
        {
            if (UIView == null)
                throw new NullReferenceException("UIView cannot be null");
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}