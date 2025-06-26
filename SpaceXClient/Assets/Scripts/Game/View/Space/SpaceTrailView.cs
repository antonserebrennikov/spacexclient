using System;
using System.Collections.Generic;
using MVP.View;
using UnityEngine;

namespace Game.View.Space
{
    public class SpaceTrailView: MonoBehaviour, IView
    {
        [Header("Trail line renderer (Can't be null)")]
        public LineRenderer lineRenderer;
        [Range(100, 2000)]
        public float lineThickness;
        
        private Queue<Vector3> trail = new Queue<Vector3>(20);
        
        public void Awake()
        {
            if (!lineRenderer)
                throw new NullReferenceException("LineRenderer cannot be null");
            
            lineRenderer.startWidth = lineThickness;
            lineRenderer.endWidth = lineThickness;
            lineRenderer.useWorldSpace = true;
            
            OnReset();
        }
        
        /// <summary>
        /// Draws a connected line using a series of positions from the Queue<Vector3>.
        /// </summary>
        /// <param name="positions">A queue containing a series of Vector3 points to connect.</param>
        private void DrawConnectedLine(Queue<Vector3> positions)
        {
            if (positions == null)
                return;

            if (positions.Count < 2)
            {
                lineRenderer.positionCount = 0;
                return;
            }

            // Convert Queue<Vector3> to an array
            var positionArray = positions.ToArray();

            // Set the number of positions in the LineRenderer
            lineRenderer.positionCount = positionArray.Length;

            // Assign positions to the LineRenderer
            for (int i = 0; i < positionArray.Length; i++)
            {
                lineRenderer.SetPosition(i, positionArray[i]);
            }
        }
        
        public void OnNextPosition(Vector3 position)
        {
            trail.Enqueue(position);
            
            if (trail.Count > 20)
                trail.Dequeue();

            
            DrawConnectedLine(trail);
        }

        public void OnReset()
        {
            trail.Clear();
        }
    }
}