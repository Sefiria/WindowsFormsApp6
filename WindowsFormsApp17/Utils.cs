using System.Drawing;
using Tooling;

namespace WindowsFormsApp17
{
    internal class Utils
    {
        public static PointF DrawPoint(PointF world_pos) => Core.CenterPoint.PlusF(world_pos.X, world_pos.Y).MinusF(Data.Cam);
        public static PointF DrawPoint(float world_x, float world_y) => Core.CenterPoint.PlusF(world_x, world_y).MinusF(Data.Cam);
    }
}
