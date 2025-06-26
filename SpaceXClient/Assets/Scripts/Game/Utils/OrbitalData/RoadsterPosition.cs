using System;
using System.Text;

namespace Game.Utils.OrbitalData
{
    public struct RoadsterPosition
    {
        public double EpochJD { get; set; }
        public DateTime DateUTC { get; set; }
        public double SemiMajorAxisAU { get; set; }
        public double Eccentricity { get; set; }
        public double InclinationDegrees { get; set; }
        public double LongitudeOfAscNodeDegrees { get; set; }
        public double ArgumentOfPeriapsisDegrees { get; set; }
        public double MeanAnomalyDegrees { get; set; }
        public double TrueAnomalyDegrees { get; set; }

        public override string ToString()
        {
            // Convert UTC to local time zone
            var localDateTime = DateUTC.ToLocalTime();
            var localizedDateString = localDateTime.ToString("f", System.Globalization.CultureInfo.CurrentCulture);
            var stringBuilder = new StringBuilder();
            
            stringBuilder.Append("Local Date: ").Append(localizedDateString).Append(",\n")
                         .Append("EpochJD: ").Append(EpochJD).Append(",\n")
                         .Append("Semi-Major Axis (AU): ").Append(SemiMajorAxisAU).Append(",\n")
                         .Append("Eccentricity: ").Append(Eccentricity).Append(",\n")
                         .Append("Inclination (Degrees): ").Append(InclinationDegrees).Append(",\n")
                         .Append("Longitude of Ascending Node (Degrees): ").Append(LongitudeOfAscNodeDegrees).Append(",\n")
                         .Append("Argument of Periapsis (Degrees): ").Append(ArgumentOfPeriapsisDegrees).Append(",\n")
                         .Append("Mean Anomaly (Degrees): ").Append(MeanAnomalyDegrees).Append(",\n")
                         .Append("True Anomaly (Degrees): ").Append(TrueAnomalyDegrees);

            return stringBuilder.ToString();
        }
    }
}