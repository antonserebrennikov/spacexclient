using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Game.Utils.MissionData;
using Newtonsoft.Json.Serialization;
using Oddity;
using Oddity.Models.Launches;
using Oddity.Models.Rockets;
using Oddity.Models.Ships;
using UnityEngine;

namespace Game.Utils.MissionDataLoader
{
    public class OddityMissionDataLoader : IMissionDataLoader
    {
        private const string NoData = "No data";
        private const int LoadingTimeoutInMilliseconds = 60 * 1000; // 1 minute
        
        public async Task<List<MissionInfo>> LoadMissions()
        {
            // TODO: Consider adding progress reporting if needed
            try
            {
                using var cancellationTokenSource = new CancellationTokenSource(LoadingTimeoutInMilliseconds);

                // Run LoadDataAsync and CancelTimer concurrently
                var loadDataTask = LoadDataAsync(cancellationTokenSource.Token);

                // If LoadDataAsync completed successfully without cancellation, return the results
                return await loadDataTask;

            }
            catch (OperationCanceledException)
            {
                Debug.LogError($"Data loading took too long and was not completed within {LoadingTimeoutInMilliseconds / 1000} seconds.");
                return null;
            }

            catch (Exception ex)
            {
                Debug.LogError($"Failed to load mission data: {ex.Message}");
                return null;
            }
        }

        private async Task<List<MissionInfo>> LoadDataAsync(CancellationToken ct)
        {
            var missions = new List<MissionInfo>();
            var oddity = InitializeOddityCore();
            
            try
            {
                var allLaunches = await oddity.LaunchesEndpoint.GetAll().ExecuteAsync();

                foreach (var launch in allLaunches)
                {
                    ct.ThrowIfCancellationRequested();
                    
                    var loadShipDataTask = LoadShipDataAsync(oddity, launch);
                    var loadRocketDataTask = LoadRocketDataAsync(oddity, launch);
                    
                    await Task.WhenAll(loadShipDataTask, loadRocketDataTask);
                    
                    var missionStatus = DetermineMissionStatus(launch);
                    var shipInfo = ParseSpaceShipInfo(loadShipDataTask.Result, loadRocketDataTask.Result);
                    var missionInfo = ParseMissionInfo(launch, shipInfo, missionStatus);

                    missions.Add(missionInfo);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return missions;
        }

        private OddityCore InitializeOddityCore()
        {
            var oddity = new OddityCore();
            
            oddity.OnDeserializationError += HandleDeserializationError;
            
            return oddity;
        }

        private static async Task<ShipInfo> LoadShipDataAsync(OddityCore oddity, LaunchInfo launch)
        {
            if (launch.ShipsId == null || launch.ShipsId.Count == 0)
                return new ShipInfo();

            try
            {
                return await oddity.ShipsEndpoint.Get(launch.ShipsId.First()).ExecuteAsync();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to load ship data for launch {launch.Id}: {ex.Message}");
                return new ShipInfo();
            }
        }

        private static async Task<RocketInfo> LoadRocketDataAsync(OddityCore oddity, LaunchInfo launch)
        {
            if (string.IsNullOrEmpty(launch.RocketId))
                return new RocketInfo();

            try
            {
                return await oddity.RocketsEndpoint.Get(launch.RocketId).ExecuteAsync();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Failed to load rocket data for launch {launch.Id}: {ex.Message}");
                return new RocketInfo();
            }
        }

        private static MissionStatus DetermineMissionStatus(LaunchInfo launch)
        {
            if (launch.Failures == null || launch.Failures.Count == 0)
                return launch.Upcoming.HasValue && launch.Upcoming.Value
                    ? MissionStatus.Upcoming
                    : MissionStatus.Completed;

            return MissionStatus.Failed;
        }

        private static SpaceShipInfo ParseSpaceShipInfo(ShipInfo ship, RocketInfo rocket)
        {
            return new SpaceShipInfo
            {
                Name = ship.Name ?? NoData,
                OriginCountry = rocket.Country ?? NoData,
                MissionsNumber = ship.LaunchesId?.Count ?? 0,
                Type = ship.Type ?? NoData,
                HomePort = ship.HomePort ?? NoData
            };
        }

        private static MissionInfo ParseMissionInfo(LaunchInfo launch, SpaceShipInfo shipInfo, MissionStatus status)
        {
            return new MissionInfo
            {
                Id = launch.Id ?? NoData,
                Name = launch.Name ?? NoData,
                PayloadsNumber = launch.Payloads?.Count ?? 0,
                Status = status,
                Spaceship = shipInfo,
                DateUTC = launch.DateUtc ?? DateTime.MinValue
            };
        }

        private void HandleDeserializationError(object sender, ErrorEventArgs errorEventArgs)
        {
            var currentObject = errorEventArgs.CurrentObject;
            var errorContext = errorEventArgs.ErrorContext;

            Debug.LogError($"Deserialization error: Object: {currentObject?.GetType().Name ?? "Unknown Object"}, " +
                           $"Path: {errorContext.Path}, Message: {errorContext.Error.Message}");
            
            // Mark the error as handled
            errorContext.Handled = true;
        }
    }
}