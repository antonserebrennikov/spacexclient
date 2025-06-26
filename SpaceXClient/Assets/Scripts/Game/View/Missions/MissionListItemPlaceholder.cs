using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View.Missions
{
    public class MissionListItemPlaceholder: MonoBehaviour
    {
        private bool wasVisible;
        
        public event Action OnShow;
        public event Action OnHide;
        
        private void Update()
        {
            if (IsVisible())
            {
                if (!wasVisible)
                {
                    wasVisible = true;
                    OnShow?.Invoke();
                }
            }
            else
            {
                if (wasVisible)
                {
                    wasVisible = false;
                    OnHide?.Invoke();
                }
            }
        }

        private bool IsVisible()
        {
            var scrollRect = GetComponentInParent<ScrollRect>();
            
            if (scrollRect == null)
            {
                Debug.LogError("Parent ScrollRect not found!");
                return false;
            }

            var itemRect = GetComponent<RectTransform>();
            // Get the world corners of the item and viewport
            var itemCorners = new Vector3[4];
            var viewportCorners = new Vector3[4];

            itemRect.GetWorldCorners(itemCorners);
            scrollRect.viewport.GetWorldCorners(viewportCorners);

            // Convert corners into Rects to check overlap
            var itemWorldRect = new Rect(itemCorners[0].x, itemCorners[0].y,
                itemCorners[2].x - itemCorners[0].x,
                itemCorners[2].y - itemCorners[0].y);
            var viewportWorldRect = new Rect(viewportCorners[0].x, viewportCorners[0].y,
                viewportCorners[2].x - viewportCorners[0].x,
                viewportCorners[2].y - viewportCorners[0].y);

            // Check if the item rect overlaps with the viewport rect
            return viewportWorldRect.Overlaps(itemWorldRect);
        }
    }
}