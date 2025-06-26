using System;
using System.Text;

namespace Game.Utils.MissionData
{
    public enum MissionStatus
    {
        Completed,
        Upcoming,
        Failed,
    }
    
    //TODO: add more info about missions
    public struct MissionInfo : IEquatable<MissionInfo>
    {
        public string Id;
        public string Name;
        public int PayloadsNumber;
        public MissionStatus Status;
        public SpaceShipInfo Spaceship;
        public DateTime DateUTC;
        
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.Append("MissionInfo: Id = ").Append(Id)
                         .Append(", Name = ").Append(Name)
                         .Append(", PayloadsNumber = ").Append(PayloadsNumber)
                         .Append(", Status = ").Append(Status)
                         .Append(", Spaceship = ").Append(Spaceship)
                         .Append(", DateUTC = ").Append(DateUTC);

            return stringBuilder.ToString();
        }

        public bool Equals(MissionInfo other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is MissionInfo other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, PayloadsNumber, (int)Status, Spaceship, DateUTC);
        }
    }
}