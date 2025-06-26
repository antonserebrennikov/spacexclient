using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Utils.Assets.Text;
using Game.Utils.OrbitalData;
using Game.Utils.Parser.Csv;
using MVP.Model;
using UnityEngine;

namespace Game.Model.Space
{
    public class SpaceModel: IModel
    {
        //TODO: make configurable
        private const string RoadsterCsvFilename = "Text/Roadster";
        private List<RoadsterPosition> roadsterPositions;
        
        public async Task InitAsync(ITextLoader textLoader)
        {
            if (textLoader == null)
                Debug.LogError($"{nameof(textLoader)} cannot be null.");
            
            try
            {
                var textContent = await textLoader.LoadAsync(RoadsterCsvFilename);

                roadsterPositions = new RoadsterDataCsvParser().ParseFromString(textContent);
                // Sort the list by DateTime to ensure the correct ordering
                roadsterPositions.Sort((a, b) => a.DateUTC.CompareTo(b.DateUTC));
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while initializing {nameof(SpaceModel)}: {e.Message}");
                throw;
            }
        }

        public RoadsterPosition GetRoadsterPosition(int index)
        {
            return roadsterPositions[index];
        }

        public int GetRoadsterPositionsCount()
        {
            return roadsterPositions.Count;
        }
        
        public RoadsterPosition GetRoadsterPosition(DateTime dateTime)
        {
            if (roadsterPositions == null || roadsterPositions.Count == 0)
                throw new InvalidOperationException("The roadsterPositions list is empty or not initialized.");
            
            var firstPosition = roadsterPositions.First();
            
            if (dateTime < firstPosition.DateUTC)
                return firstPosition;
            
            // Iterate through the list to find the closest match
            var previous = firstPosition;

            foreach (var position in roadsterPositions)
            {
                if (position.DateUTC > dateTime)
                {
                    // Found a position beyond the provided DateTime: return the previous (if any)
                    return previous;
                }
                
                // Update the previous position
                previous = position;
            }

            // If given DateTime is beyond the last position, return the last position
            return roadsterPositions.Last();
        }

    }
}