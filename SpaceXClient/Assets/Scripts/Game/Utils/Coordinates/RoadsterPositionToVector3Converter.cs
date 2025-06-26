using Game.Utils.OrbitalData;
using RG.OrbitalElements;
using UnityEngine;

namespace Game.Utils.Coordinates
{
    public class RoadsterPositionToVector3Converter
    {
        public Vector3 Convert(IOrbitalCoordinatesConverter converter, RoadsterPosition position)
        {
            return Convert(converter.CalculateOrbitalPosition(position.SemiMajorAxisAU, position.Eccentricity, position.InclinationDegrees,
                position.LongitudeOfAscNodeDegrees, position.ArgumentOfPeriapsisDegrees, position.TrueAnomalyDegrees));
        }

        private Vector3 Convert(Vector3Double vector)
        {
            return new Vector3((float)vector.x, (float)vector.y, (float)vector.z);
        }
    }
}