using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using static DOSBOX.Suggestions.city.User;

namespace DOSBOX.Suggestions.city
{
    public class User
    {
        public class IdleH_anim : effect
        {
            protected override List<byte[,]> g { get; set; } = new List<byte[,]>()
            {
                new byte[3,3]
                {
                    {0, 0, 0},
                    {2, 3, 2},
                    {0, 0, 0},
                },
            };
        }
        public class IdleV_anim : effect
        {
            protected override List<byte[,]> g { get; set; } = new List<byte[,]>()
            {
                new byte[3,3]
                {
                    {0, 2, 0},
                    {0, 3, 0},
                    {0, 2, 0},
                },
            };
        }
        public class WalkH_anim : effect
        {
            protected override bool pingpong { get; set; } = true;
            protected override float incr { get; set; } = 0.4F;
            protected override List<byte[,]> g { get; set; } = new List<byte[,]>()
            {
                new byte[3,3]
                {
                    {0, 0, 2 },
                    {2, 3, 2},
                    {2, 0, 0 },
                },
                new byte[3,3]
                {
                    {0, 0, 0 },
                    {2, 3, 2},
                    {0, 0, 0 },
                },
                new byte[3,3]
                {
                    {2, 0, 0 },
                    {2, 3, 2},
                    {0, 0, 2 },
                },
            };
        }
        public class WalkV_anim : effect
        {
            protected override bool pingpong { get; set; } = true;
            protected override float incr { get; set; } = 0.4F;
            protected override List<byte[,]> g { get; set; } = new List<byte[,]>()
            {
                new byte[3,3]
                {
                    {2, 2, 0},
                    {0, 3, 0},
                    {0, 2, 2},
                },
                new byte[3,3]
                {
                    {0, 2, 0},
                    {0, 3, 0},
                    {0, 2, 0},
                },
                new byte[3,3]
                {
                    {0, 2, 2},
                    {0, 3, 0},
                    {2, 2, 0},
                },
            };
        }

        public float move_speed = 0.75F;
        private float m_angle = 270F;
        public float angle
        {
            get => m_angle;
            set
            {
                m_angle = (float) Math.Truncate(value);

                while (m_angle < 0F) m_angle += 360F;
                while (m_angle >= 360F) m_angle -= 360F;
            }
        }
        IdleH_anim idleH_anim = new IdleH_anim();
        IdleV_anim idleV_anim = new IdleV_anim();
        WalkH_anim walkH_anim = new WalkH_anim();
        WalkV_anim walkV_anim = new WalkV_anim();
        bool lastpositionwasvertical = true;

        public User()
        {
            Core.KeepCamCoords = true;
            Core.Cam = new vecf(Data.Instance.map.Blocks.GetLength(0) / 2F * Tile.TSZ, Data.Instance.map.Blocks.GetLength(1) / 2F * Tile.TSZ);
        }

        public void Update()
        {
            UpdateMovements();
        }

        private void UpdateMovements()
        {
            bool up = KB.IsKeyDown(KB.Key.Z);
            bool down = KB.IsKeyDown(KB.Key.S);
            bool left = KB.IsKeyDown(KB.Key.Q);
            bool right = KB.IsKeyDown(KB.Key.D);
            if ((up || down) && (left || right)) up = down = left = right = false;

            if (up) Core.Cam.y -= move_speed;
            else if (down) Core.Cam.y += move_speed;
            if (left) Core.Cam.x -= move_speed;
            else if (right) Core.Cam.x += move_speed;

            if(up || down || left || right)
                angle = (up ?  90F + (left ? 45F : (right ? -45F : 0F)) : (down ? 270 +  (left ? -45F : (right ? 45F : 0F)) : 0F));

            if ((up || down) && !left && !right)
                lastpositionwasvertical = true;
            else if ((left || right) && !up && !down)
                lastpositionwasvertical = false;
        }

        public void Display()
        {
            bool up = KB.IsKeyDown(KB.Key.Z);
            bool down = KB.IsKeyDown(KB.Key.S);
            bool left = KB.IsKeyDown(KB.Key.Q);
            bool right = KB.IsKeyDown(KB.Key.D);

            effect anim = null;
            if (lastpositionwasvertical)
                anim = idleV_anim;
            else
                anim = idleH_anim;
            if ((up || down) && !left && !right)
                anim = walkV_anim;
            if ((left || right) && !up && !down)
                anim = walkH_anim;
            anim.Display(1, new vecf(32 - anim.w() / 2F, 32 - anim.h() / 2F));
        }
    }
}
