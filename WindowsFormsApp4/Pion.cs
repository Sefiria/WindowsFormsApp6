using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsFormsApp4.Properties;

namespace WindowsFormsApp4
{
    public class Pion
    {
        public string ID { get;  private set; }
        public Bitmap Image;
        public int X, Y;
        public Point CoordTile => new Point(X / Data.TileSz, Y / Data.TileSz);
        public Point Coord => new Point(X, Y);
        public Card Card = null;

        public Pion(Point spawn)
        {
            ID = Tools.RNDID();
            X = spawn.X * Data.TileSz;
            Y = spawn.Y * Data.TileSz;
            Image = Resources.pionA;
            Image.MakeTransparent();
            Card = Data.Carte.StartCard;
        }

        public void Draw()
        {
            Data.g.DrawImage(Image, Coord);
        }

        internal void Update()
        {
            if(Keyboard.IsKeyDown(Key.Space))
            {
                Data.RollingDices = true;
            }
        }
    }
}
