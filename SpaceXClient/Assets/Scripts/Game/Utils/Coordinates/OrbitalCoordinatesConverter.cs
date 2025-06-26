using RG.OrbitalElements;

namespace Game.Utils.Coordinates
{
    public class OrbitalCoordinatesConverter: IOrbitalCoordinatesConverter
    {
        public Vector3Double CalculateOrbitalPosition(double semimajorAxis, double eccentricity, double inclination, double longitudeOfAscendingNode, double periapsisArgument, double trueAnomaly)
        {
            return Calculations.CalculateOrbitalPosition(semimajorAxis, eccentricity, inclination, longitudeOfAscendingNode,
                periapsisArgument, trueAnomaly);
        }
    }
}