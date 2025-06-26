using RG.OrbitalElements;

namespace Game.Utils.Coordinates
{
    public interface IOrbitalCoordinatesConverter
    {
        public Vector3Double CalculateOrbitalPosition(double semimajorAxis, double eccentricity, double inclination,
            double longitudeOfAscendingNode, double periapsisArgument, double trueAnomaly);
    }
}