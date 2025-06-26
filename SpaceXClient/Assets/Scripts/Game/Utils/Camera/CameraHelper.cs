using Game.Utils.Geometry;
using UnityEngine;

namespace Game.Utils.Camera
{
    public static class CameraHelper
    {
        public static void UpdateCameraPosition(UnityEngine.Camera camera, Vector3 pos1, Vector3 pos2, Vector3 pos3, float offsetDistance)
        {
            // Calculate two edges of the triangle
            var edge1 = pos2 - pos1;
            var edge2 = pos3 - pos1;

            // Check if the points are sufficiently far apart to avoid instability
            const float epsilon = 1e-5f; // A small value to handle precision issues
            if (edge1.magnitude < epsilon || edge2.magnitude < epsilon || Vector3.Cross(edge1, edge2).magnitude < epsilon)
            {
                // Default to a safe position if points are too close or collinear
                camera.transform.position = Vector3.Lerp(
                    camera.transform.position,
                    new Vector3(0, 0, -offsetDistance), // Default fallback position
                    0.1f // Smooth transition
                );
                return;
            }

            // Compute the normal of the plane
            var normal = Vector3.Cross(edge1, edge2).normalized;

            // Ensure the normal points in a consistent direction relative to the camera
            if (Vector3.Dot(normal, camera.transform.forward) > 0)
            {
                normal = -normal; // Flip the normal
            }

            // Calculate the centroid of the plane
            var centroid = GeometryHelper.FindCentroid(pos1, pos2, pos3);

            // Position the camera at a fixed distance along the normal vector (with smoothing)
            var targetPosition = centroid + normal * offsetDistance;
            camera.transform.position = Vector3.Lerp(camera.transform.position, targetPosition, 0.1f); // Smoothly interpolate
        }

        public static void UpdateCameraTarget(UnityEngine.Camera camera, Vector3 pos1, Vector3 pos2, Vector3 pos3)
        {
            // Make the camera look at the centroid of the plane
            camera.transform.LookAt(GeometryHelper.FindCentroid(pos1, pos2, pos3));
        }
        
        public static float GetCameraOffset(Vector3 pos1, Vector3 pos2, Vector3 pos3, float offsetMultiplier)
        {
            return FindLargestDistance(pos1, pos2, pos3) * offsetMultiplier;
        }
        
        public static float FindLargestDistance(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            // Calculate pairwise distances
            var distance1 = Vector3.Distance(v1, v2); // Distance between v1 and v2
            var distance2 = Vector3.Distance(v2, v3); // Distance between v2 and v3
            var distance3 = Vector3.Distance(v1, v3); // Distance between v1 and v3

            // Find the maximum distance
            return Mathf.Max(distance1, distance2, distance3);
        }

    }
}