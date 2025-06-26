using System.Text;

namespace Game.Utils.MissionData
{
    //TODO: add more info about spaceships
    public struct SpaceShipInfo
    {
        public string Name;
        public string OriginCountry;
        public int MissionsNumber;
        public string Type;
        public string HomePort;
        
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            
            stringBuilder.Append("SpaceshipInfo: Name = ").Append(Name)
                .Append(", OriginCountry = ").Append(OriginCountry)
                .Append(", MissionsNumber = ").Append(MissionsNumber)
                .Append(", Type = ").Append(Type)
                .Append(", HomePort = ").Append(HomePort);

            return stringBuilder.ToString();


        }
    }
}