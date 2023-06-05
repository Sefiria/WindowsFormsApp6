using PishConverter.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PishConverter.Common
{
    internal class Shpe : Drawable, IHit
    {
        public StatsInfo StatsInfo;
        public List<Vector2> ShotPorts;
        public int RndDest = -1;
        public byte ShpeType;

        public Shpe(byte type, Vector2 position, StatsInfo statsInfo) : base()
        {
            ShotPorts = new List<Vector2>()
            {
                new Vector2(0, H / 2F - 2F),
            };

            ShpeType = type;

            Position = position;
            CreateTexture();

            StatsInfo = statsInfo;
        }
        private void CreateTexture()
        {
            if (ShpeType == 0)
                Texture = new byte[5, 5]
                {
                    { 0, 0, 1, 0, 0 },
                    { 1, 0, 1, 0, 1 },
                    { 0, 1, 1, 1, 0 },
                    { 0, 0, 1, 0, 0 },
                    { 0, 0, 1, 0, 0 },
                }.ToBitmap(2);

            if (ShpeType == 1)
                Texture = new byte[5, 5]
                {
                    { 0, 0, 1, 0, 0 },
                    { 1, 1, 1, 1, 1 },
                    { 0, 1, 0, 1, 0 },
                    { 0, 0, 1, 0, 0 },
                    { 0, 0, 1, 0, 0 },
                }.ToBitmap(4);
        }

        public override void Update()
        {
            Controls();
        }

        private void Controls()
        {
            int rnd;
            Position.Y += 0.2F;

            if (RndDest == -1)
            {
                rnd = Global.RND.Next(100);
                if (rnd == 0)
                {
                    RndDest = Global.RND.Next(3, Global.W - 3);
                }
            }
            else
            {
                Position.X += (RndDest - Position.X < 0 ? -1 : 1) * StatsInfo.MoveSpeed;
                if ((Position.X - RndDest)*(Position.X - RndDest) < 5F)
                    RndDest = -1;
            }

            rnd = Global.RND.Next(400);
            if (rnd == 0)
                Shot();
        }

        private void Shot()
        {
            foreach (var port in ShotPorts)
                new Bullet(this, Position + port, new Vector2(0, 1F));
        }

        public void Hit()
        {
            if (StatsInfo.HP <= 0)
                return;
            StatsInfo.HP--;
            if (StatsInfo.HP <= 0)
                Destroy();
        }

        public void Destroy()
        {
            if (Exists == false)
                return;
            Exists = false;
            if (Global.RND.Next(10) == 0)
                DropPowerUp();
        }

        private void DropPowerUp()
        {
        }
    }
}
