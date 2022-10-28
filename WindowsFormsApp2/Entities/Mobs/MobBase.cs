using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Interfaces;

namespace WindowsFormsApp2.Entities.Mobs
{
    public abstract class MobBase : DrawableEntity, IRP
    {
        public int HPMax { get; set; }
        public int HP { get; set; }
        public float Step = 0, MaxStep = 10;
        public float XBase, YBase;
        protected MobBase(Bitmap Image, int X = 0, int Y = 0) : base(X, Y)
        {
            this.Image = Image;
        }

        public override void Update()
        {
            int ts = SharedCore.TileSize;

            if (Step == 0)
            {
                Look = new VecF() { X = 0, Y = 0 };
                int rndMove = Tools.RND.Next(80);
                if (rndMove == 0 && !Tools.PositionTileIsOccupied(X - ts, Y + 0, this))
                    Look = new VecF() { X = -1, Y = 0 };
                else if (rndMove == 20 && !Tools.PositionTileIsOccupied(X + 0, Y - ts, this))
                    Look = new VecF() { X = 0, Y = -1 };
                else if (rndMove == 40 && !Tools.PositionTileIsOccupied(X + ts, Y + 0, this))
                    Look = new VecF() { X = 1, Y = 0 };
                else if (rndMove == 60 && !Tools.PositionTileIsOccupied(X + 0, Y + ts, this))
                    Look = new VecF() { X = 0, Y = 1 };

                if(Look.X != 0 || Look.Y != 0)
                {
                    Step++;
                    XBase = X;
                    YBase = Y;
                }
            }
            else
            {
                X = XBase + Look.X * ts * ((Step + 1) / MaxStep);
                Y = YBase + Look.Y * ts * ((Step + 1) / MaxStep);
                Step++;
                if (Step >= MaxStep)
                    Step = 0;
            }
        }
        public abstract void Hit(IDamager damager);

    }
}
