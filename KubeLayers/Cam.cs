using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Tooling;
using static Tooling.KB;

namespace KubeLayers
{
    public class Cam
    {
        GCore core => GCore.Instance;

        public Bitmap Image;
        public vecf Position = vecf.Zero;
        public float SpeedMove = 2F;

        public Cam()
        {
            Image = new Bitmap(Core.TSZ, Core.TSZ);
            using(Graphics g = Graphics.FromImage(Image))
            {
                g.Clear(Color.White);
            }
        }

        public void Update()
        {
            bool Z = IsKeyDown(Key.Z);
            bool S = IsKeyDown(Key.S);
            bool Q = IsKeyDown(Key.Q);
            bool D = IsKeyDown(Key.D);

            if (Z) Position.y -= SpeedMove;
            if (Q) Position.x -= SpeedMove;
            if (S) Position.y += SpeedMove;
            if (D) Position.x += SpeedMove;
        }
        public void Draw()
        {
            core.g.DrawImage(Image, core.CenterPoint);
        }

        public void Shot()
        {
            //shW = -1F;
            //new Bullet(Position.PlusF(FoWard.x(Bullet.Size)), FoWard);
        }
    }
}
