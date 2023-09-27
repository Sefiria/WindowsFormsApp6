using System.Drawing;

namespace MiniKube
{
    internal partial class Map
    {
        void draw_grid()
        {
            Pen pen = new Pen(Color.FromArgb(180, 190, 210));
            float modx = Core.Cam.X % Core.Cube;
            float mody = Core.Cam.Y % Core.Cube;
            for (float y= -1F; y < Core.RH/Core.Cube + 2; y++)
            {
                Core.g.DrawLine(pen, - Core.Cube - modx, y * Core.Cube - mody, Core.RW - modx, y * Core.Cube - mody);
            }
            for (float x = -1F; x < Core.RW / Core.Cube + 2; x++)
            {
                Core.g.DrawLine(pen, x * Core.Cube - modx, -Core.Cube - mody, x * Core.Cube - modx, Core.RH - mody);
            }
        }
    }
}
