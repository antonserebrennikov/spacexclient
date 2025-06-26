using UnityEngine;

namespace Game.Utils.Camera
{
    public static class CameraDebugHelper
    {
        public static void DrawCameraFrustum(UnityEngine.Camera camera, Color color)
        {
            if (camera == null)
            {
                Debug.LogError("Camera is null");
                return;
            }
            // Get the frustum corners of the near and far planes
            var nearCorners = new Vector3[4]; // Bottom-left, Top-left, Top-right, Bottom-right (near plane)
            var farCorners = new Vector3[4];  // Bottom-left, Top-left, Top-right, Bottom-right (far plane)

            // Get the corners of the camera's frustum in world space
            camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), camera.nearClipPlane, UnityEngine.Camera.MonoOrStereoscopicEye.Mono, nearCorners);
            camera.CalculateFrustumCorners(new Rect(0, 0, 1, 1), camera.farClipPlane, UnityEngine.Camera.MonoOrStereoscopicEye.Mono, farCorners);
            
            // Convert the corners to world space
            for (int i = 0; i < 4; i++)
            {
                nearCorners[i] = camera.transform.TransformPoint(nearCorners[i]);
                farCorners[i] = camera.transform.TransformPoint(farCorners[i]);
            }

            // Draw lines between the near and far plane corners
            for (int i = 0; i < 4; i++)
            {
                Debug.DrawLine(nearCorners[i], farCorners[i], color); // Edges connecting near and far planes
            }

            // Draw lines around the near plane
            Debug.DrawLine(nearCorners[0], nearCorners[1], color);
            Debug.DrawLine(nearCorners[1], nearCorners[2], color);
            Debug.DrawLine(nearCorners[2], nearCorners[3], color);
            Debug.DrawLine(nearCorners[3], nearCorners[0], color);

            // Draw lines around the far plane
            Debug.DrawLine(farCorners[0], farCorners[1], color);
            Debug.DrawLine(farCorners[1], farCorners[2], color);
            Debug.DrawLine(farCorners[2], farCorners[3], color);
            Debug.DrawLine(farCorners[3], farCorners[0], color);
            
            Debug.DrawLine(camera.transform.position, camera.transform.position + camera.transform.forward * camera.farClipPlane, color);
        }
    }
}