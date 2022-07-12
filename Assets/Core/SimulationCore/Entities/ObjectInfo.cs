using System;

namespace Entities
{

    [Serializable]
    public class ObjectInfo
    {
        private string _name;
        private string _country;
        private string _mass;
        private string _dedication;
        private string _launchData;
        private string _mse;
        private string _lifetime;
        private string _constructionDetailedInfo;

        public string Name { get => _name; set => _name = value; }
        public string Country { get => _country; set => _country = value; }
        public string Mass { get => _mass; set => _mass = value; }
        public string Dedication { get => _dedication; set => _dedication = value; }
        public string LaunchData { get => _launchData; set => _launchData = value; }
        public string MSE1 { get => _mse; set => _mse = value; }
        public string Lifetime { get => _lifetime; set => _lifetime = value; }
        public string ConstructionDetailedInfo { get => _constructionDetailedInfo; set => _constructionDetailedInfo = value; }

        public ObjectInfo(string name, string country, string mass, string dedication, string launchData, string mSE, string lifetime, string constructionDetailedInfo)
        {
            this.Name = name;
            this.Country = country;
            this.Mass = mass;
            this.Dedication = dedication;
            this.LaunchData = launchData;
            MSE1 = mSE;
            this.Lifetime = lifetime;
            this.ConstructionDetailedInfo = constructionDetailedInfo;
        }
        public ObjectInfo(int index)
        {
            this.Name = "Sputnik.Satellites.Info."+index.ToString()+".name";
            this.Country = "Sputnik.Satellites.Info."+index.ToString()+".country";
            this.Mass = "Sputnik.Satellites.Info."+index.ToString()+".mass";
            this.Dedication = "Sputnik.Satellites.Info."+index.ToString()+".dedication";
            this.LaunchData = "Sputnik.Satellites.Info."+index.ToString()+".launchData";
            MSE1 = "Sputnik.Satellites.Info."+index.ToString()+".MSE";
            this.Lifetime = "Sputnik.Satellites.Info."+index.ToString()+".lifetime";
            this.ConstructionDetailedInfo = "Sputnik.Satellites.Info."+index.ToString()+".constructionDetailedInfo";
             
        }
    }

}