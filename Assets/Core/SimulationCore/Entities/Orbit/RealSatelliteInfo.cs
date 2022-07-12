
using System.Collections.Generic;

namespace Entities.Orbit.SateliteInfoParser
{
    public class ParsedSatelliteInfo
    {
        public ParsedSatelliteInfo(OrbitType r, string model)
        {
            _radius = r;
            _model = model;
        }
        private OrbitType _radius;
        private string _model;

        public OrbitType Radius { get => _radius; set => _radius = value; }
    }
    public static class RealSatelliteInfo
    {
        public static List<ParsedSatelliteInfo> info = new List<ParsedSatelliteInfo>();
        public static void DataParse()
        {
            info.Add(new ParsedSatelliteInfo(OrbitType.Random, "default"));
            info.Add(new ParsedSatelliteInfo(OrbitType.Random, "default"));
            info.Add(new ParsedSatelliteInfo(OrbitType.Random, "default"));
            info.Add(new ParsedSatelliteInfo(OrbitType.Random, "default"));
            info.Add(new ParsedSatelliteInfo(OrbitType.Random, "default"));
            info.Add(new ParsedSatelliteInfo(OrbitType.Random, "default"));
            info.Add(new ParsedSatelliteInfo(OrbitType.Random, "default"));
            info.Add(new ParsedSatelliteInfo(OrbitType.Random, "default"));
            info.Add(new ParsedSatelliteInfo(OrbitType.Random, "default"));
            info.Add(new ParsedSatelliteInfo(OrbitType.Random, "default"));
        }
    }
}
