using System.Drawing;
using static Core.Utils.Tools;

namespace Core
{
    public class TileSignObj
    {
        public int Index;
        public Point Position;
        public bool ShowBar;
        public Rotation Angle;

        public TileSignObj()
        {
            Index = 0;
            Position = Point.Empty;
            ShowBar = true;
        }
        public TileSignObj(int Index, Point Position, Rotation Rotation = Rotation.None, bool ShowBar = true)
        {
            this.Index = Index;
            this.Position = new Point(Position.X / 8, Position.Y / 8);
            this.Angle = Rotation;
            this.ShowBar = ShowBar;
        }
    }
}
