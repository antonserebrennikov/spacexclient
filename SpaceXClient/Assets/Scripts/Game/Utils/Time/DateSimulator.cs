using System;

namespace Game.Utils.Time
{
    public class DateSimulator
    {
        private DateTime fromDate;
        private DateTime toDate;

        private float lastUpdateTime; // Tracks the last update time in seconds
        private double accumulatedSimulationDays; // Accumulated simulation days
        private double totalSimulationDays; // Total days in the date range
        private DateTime currentSimulationDate;

        public DateSimulator(DateTime fromDate, DateTime toDate, float startTime)
        {
            this.fromDate = fromDate;
            this.toDate = toDate;
            SetUp(startTime);
        }

        public DateTime Update(float currentTime, float daysPerSecond)
        {
            // Calculate elapsed real-world time (time in seconds since last update)
            var deltaTime = currentTime - lastUpdateTime;

            // Update accumulated simulation days based on elapsed real-world time and speed
            accumulatedSimulationDays += deltaTime * daysPerSecond;

            // Wrap the accumulated simulation days within the allowed simulation range
            var wrappedDays = accumulatedSimulationDays % totalSimulationDays;

            // Update the current simulation date based on the wrapped days
            currentSimulationDate = fromDate.AddDays(wrappedDays);

            // Update the last update time
            lastUpdateTime = currentTime;

            return currentSimulationDate;
        }

        private void SetUp(float startTime)
        {
            lastUpdateTime = startTime; // Initialize the last update time
            currentSimulationDate = fromDate; // Start the simulation at the initial date
            // Calculate the total days in the simulation date range
            totalSimulationDays = (toDate - fromDate).TotalDays;
            accumulatedSimulationDays = 0; // Start with no accumulated days
        }
    }
}