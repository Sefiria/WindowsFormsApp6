using Core.Utils;
using System;
using System.Collections.Generic;
using static Core.Definitions;

namespace Core
{
    public class Tile
    {
        public int Index;
        public byte[] ImageBytes { get; set; }
        public Dictionary<PathDots, PathDotType> pathDots { get; set; }
        public Dictionary<PathDots, TileSignObj> pathDotsSign { get; set; }
        public string Name;
        
        public Tile()
        {
            Name = "";
            Index = 0;
            ImageBytes = null;
            pathDots = new Dictionary<PathDots, PathDotType>();
            pathDotsSign = new Dictionary<PathDots, TileSignObj>();
            Tools.ForEachPathDots((x) => pathDots[x] = PathDotType.None);
            Tools.ForEachPathDots((x) => pathDotsSign[x] = new TileSignObj());
        }
        public void SetPathDots(Dictionary<PathDots, PathDotType> pathDots) => this.pathDots = pathDots;

        public void SetPathDotsSign(Dictionary<PathDots, TileSignObj> pathDotsSign) => this.pathDotsSign = pathDotsSign;
    }
}
