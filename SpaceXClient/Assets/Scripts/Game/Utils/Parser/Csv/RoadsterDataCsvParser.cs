using System;
using System.Collections.Generic;
using System.IO;
using Game.Utils.OrbitalData;
using UnityEngine;

namespace Game.Utils.Parser.Csv
{
    public class RoadsterDataCsvParser
    {
        public List<RoadsterPosition> ParseFromString(string csvContent)
        {
            var result = new List<RoadsterPosition>();
            
            try
            {
                using (var reader = new StringReader(csvContent))
                {
                    // Read the header line
                    var header = reader.ReadLine();
                    
                    if (header == null)
                    {
                        throw new InvalidDataException("CSV content is empty.");
                    }

                    // Process each subsequent line
                    var line = default(string);
                    
                    while ((line = reader.ReadLine()) != null)
                    {
                        var fields = line.Split(',');

                        // Parse each field into the RoadsterPosition object
                        var data = new RoadsterPosition
                        {
                            EpochJD = double.Parse(fields[0]),
                            DateUTC = DateTime.Parse(fields[1]),
                            SemiMajorAxisAU = double.Parse(fields[2]),
                            Eccentricity = double.Parse(fields[3]),
                            InclinationDegrees = double.Parse(fields[4]),
                            LongitudeOfAscNodeDegrees = double.Parse(fields[5]),
                            ArgumentOfPeriapsisDegrees = double.Parse(fields[6]),
                            MeanAnomalyDegrees = double.Parse(fields[7]),
                            TrueAnomalyDegrees = double.Parse(fields[8])
                        };

                        result.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error parsing CSV content: {ex.Message}");
            }

            return result;
        }
    }
}