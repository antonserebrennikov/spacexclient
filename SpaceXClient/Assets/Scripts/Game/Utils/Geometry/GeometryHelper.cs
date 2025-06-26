using UnityEngine;

namespace Game.Utils.Geometry
{
    public static class GeometryHelper
    {
        public static Vector3 FindCentroid(Vector3 a, Vector3 b, Vector3 c)
        {
            return (a + b + c) / 3f;
        }
    }
}