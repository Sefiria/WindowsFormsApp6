using Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Definitions;
using static Core.Utils.Tools;

namespace Core
{
    public class Car
    {
        public static int W = 8, H = 5;
        public Bitmap Image = null;

        public TileMap tileMap = null;
        public bool Exists = true;
        // TX/TY are TileX/TileY, X/Y location of the Car in the current TileMap
        public int TX = 0, TY = 0;
        // X/Y location of the Car in the current Tile of the TileMap
        public int X = 0, Y = 0;
        public PointF Origin = new PointF(0.5F, 0.5F);
        private int m_Angle;
        public int Angle
        {
            get => m_Angle;
            set
            {
                m_Angle += value;
                if (m_Angle < 0)
                    m_Angle += 360;
                if (m_Angle > 360)
                    m_Angle -= 360;
            }
        }
        public CarPath path;

        public Point TilePosition => new Point(TX, TY);
        public Point Position => new Point((int)(W * Origin.X) + X, (int)(H * Origin.Y) + Y);
        // Absolute X
        public int AX => TX * SharedData.TileSize + X;
        // Absolute Y
        public int AY => TY * SharedData.TileSize + Y;
        public Point AbsolutePosition => new Point((int)(W * Origin.X) + AX, (int)(W * Origin.X) + AY);


        public Car(TileMap tileMap, Point TilePosition, CarPath path)
        {
            Initialize(tileMap, TilePosition.X, TilePosition.Y, path);
        }
        public Car(TileMap tileMap, int TX, int TY, CarPath path)
        {
            Initialize(tileMap, TX, TY, path);
        }
        public void Initialize(TileMap tileMap, int TX, int TY, CarPath path)
        {
            CreateImage();

            this.tileMap = tileMap;
            this.TX = TX;
            this.TY = TY;
            Point loc = PathDotsLoc(path.DS) ?? default(Point);
            this.X = loc.X;
            this.Y = loc.Y;
            if (PathDotsLinkedCell[path.DS].Y != 0)
                Angle = (int)(PathDotsLinkedCell[path.DS].Y == -1 ? Rotation.D270 : Rotation.D90);
            this.path = path;
        }

        private void CreateImage()
        {
            Image = new Bitmap(W, H);
            using (Graphics g = Graphics.FromImage(Image))
            {
                Color color;
                switch(Tools.rnd.Next(5))
                {
                    case 0: color = Color.DarkRed;  break;
                    case 1: color = Color.DarkBlue; break;
                    case 2: color = Color.Yellow;   break;
                    case 3: color = Color.Green;    break;
           default: case 4: color = Color.DimGray;  break;
                }
                g.FillRectangle(new SolidBrush(color), 0, 0, W, H);
                g.DrawRectangle(Pens.Black, 0, 0, W - 1, H - 1);
            }
        }

        public void Draw(Graphics g, float zoom = 1F)
        {
            Bitmap img = null;
            Point pt = Point.Empty;

            if (zoom != 1F)
            {
                img = new Bitmap(Image, (int)(W * zoom), (int)(H * zoom));
                img = Rotated(img, Angle);
                pt = new Point((int)(AX * zoom), (int)(AY * zoom));
            }
            else
            {
                img = Rotated(Image, Angle);
                pt = AbsolutePosition;
            }

            g.DrawImage(img, pt);
        }

        public void Update()
        {
            if (path == null)
                return;

            int a = Angle;
            path.Next(ref X, ref Y, ref a);
            Angle = a;

            if (path.Ended)
            {
                (int lX, int lY, PathDots lD) = PathDotsLinkedCell[path.DE];
                TX += lX;
                TY += lY;
                path = FindAnyPath(TX, TY, tileMap, lD);
                if(path == null)
                    return;
                a = Angle;
                path.Next(ref X, ref Y, ref a);
                Angle = a;
                path.t = 0F;
                return;
            }
        }
    }
}
