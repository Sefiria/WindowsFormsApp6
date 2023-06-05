using PishConverter.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace PishConverter.Common
{
    internal class User : Drawable, IHit
    {
        public static User _ = null;
        public StatsInfo StatsInfo;
        public List<Vector2> ShotPorts;

        public User() : base()
        {
            _ = this;

            ShotPorts = new List<Vector2>()
            {
                new Vector2(0, -H / 2F - 2F),
            };

            Position = new Vector2(Global.W / 2, Global.H * 0.82F);
            CreateTexture();

            StatsInfo = new StatsInfo()
            {
                HP = 10,
                MoveSpeed = 4F
            };
        }
        private void CreateTexture()
        {
            Texture = new byte[8, 8]
            {
                { 0, 0, 0, 1, 1, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 0, 0, 0 },
                { 0, 0, 1, 1, 1, 1, 0, 0 },
                { 0, 1, 1, 1, 1, 1, 1, 0 },
                { 1, 0, 1, 1, 1, 1, 0, 1 },
                { 1, 1, 1, 1, 1, 1, 1, 1 },
                { 0, 0, 0, 1, 1, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 0, 0, 0 },
            }.ToBitmap(2);
        }

        public override void Update()
        {
            Controls();
        }

        private void Controls()
        {
            if (KB.IsKeyDown(KB.Key.Z) && Y >= Global.H * 0.66F + StatsInfo.MoveSpeed)
                Position.Y -= StatsInfo.MoveSpeed;
            if (KB.IsKeyDown(KB.Key.S) && Y + H < Global.H - 1F - StatsInfo.MoveSpeed)
                Position.Y += StatsInfo.MoveSpeed;
            if (KB.IsKeyDown(KB.Key.Q) && X >= StatsInfo.MoveSpeed)
                Position.X -= StatsInfo.MoveSpeed;
            if (KB.IsKeyDown(KB.Key.D) && X + W < Global.W - 1F - StatsInfo.MoveSpeed)
                Position.X += StatsInfo.MoveSpeed;

            if (KB.IsKeyPressed(KB.Key.Space))
                Shot();
        }

        private void Shot()
        {
            foreach (var port in ShotPorts)
                new Bullet(this, Position + port, new Vector2(0, -1F));
        }

        public void Hit()
        {
            StatsInfo.HP--;
            if (StatsInfo.HP <= 0)
                Exists = false;
        }

        public void GivePowerUp(PowerUpTransformInfo info)
        {

        }
    }
}
