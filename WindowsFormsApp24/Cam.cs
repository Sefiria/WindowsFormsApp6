using System.Drawing;
using Tooling;

namespace WindowsFormsApp24
{
    internal class Cam
    {
        internal float X, Y;
        internal float Zoom = 1.25F;
        internal Point Position => (X, Y).iP();

        internal void Update()
        {
            var p = Core.MainCharacter;
            X = p.X + p.W / 2 / Core.Cam.Zoom - Core.Instance.Render.Width / 2 / Core.Cam.Zoom;
            Y = p.Y + p.H / 2 / Core.Cam.Zoom - Core.Instance.Render.Height / 2 / Core.Cam.Zoom;
        }
    }
}
