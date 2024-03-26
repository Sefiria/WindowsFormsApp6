using Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Definitions;
using static Core.Utils.Tools;

namespace Core
{
    public class TileIndex
    {
        public int Index;
        public Rotation Angle;

        public TileIndex()
        {
        }
        public TileIndex(int Index = 0)
        {
            this.Index = Index;

            Angle = Rotation.None;
        }
        public TileIndex(TileIndex it)
        {
            this.Index = it.Index;
            this.Angle = it.Angle;
        }
        public Dictionary<PathDots, PathDotType> GetRotatedPathDots() => Tools.GetRotatedPathDots(TileResMngr.Instance.ResourcesTileInfo[Index].Tile.pathDots, Angle);
    }
}
