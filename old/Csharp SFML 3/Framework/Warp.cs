using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework
{
    [Serializable]
    public class Warp
    {
        public enum WarpType
        {
            Enter,
            Exit
        }

        public WarpType type;

        private Point tilePosition;
        public Point TilePosition { get { return tilePosition; } set { tilePosition.X = value.X; tilePosition.X = value.Y; } }
        public int TileX { get { return tilePosition.X; } set { tilePosition.X = value; } }
        public int TileY { get { return tilePosition.Y; } set { tilePosition.Y = value; } }

        public int Enter_WarpID;
        public string Exit_LevelName;
        public int Exit_LevelEnterID;

        public Warp(WarpType _type, int x, int y)
        {
            type = _type;
            TileX = x;
            TileY = y;
        }
        /*static public void SortID(List<Warp> list)
        {
            list.OrderBy(x => x.Enter_WarpID);

            var sorted = new List<Warp>();
            int id = 0;
            foreach(var warp in list)
            {
                if (warp.type == WarpType.Enter)
                {
                    var newWarp = new Warp(warp.type, warp.TileX, warp.TileY);
                    newWarp.Enter_WarpID = id++;
                    sorted.Add(newWarp);
                }
                else
                    sorted.Add(warp);
            }

            list = sorted;
        }*/
    }
}
