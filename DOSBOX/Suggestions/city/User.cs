using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DOSBOX.Suggestions.city.User;

namespace DOSBOX.Suggestions.city
{
    internal class User
    {
        public class Walk_anim : effect
        {
            protected override float incr { get; set; } = 0.5F;
            protected override List<byte[,]> g { get; set; } = new List<byte[,]>()
            {
                new byte[3,3]
                {
                    {0, 0, 0 },
                    {2, 3, 2},
                    {0, 0, 0 },
                },
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

        public float move_speed = 1F;
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
        Walk_anim walk_anim = new Walk_anim();

        public void Update()
        {
            UpdateMovements();
        }

        private void UpdateMovements()
        {
            bool up = KB.IsKeyDown(KB.Key.Up);
            bool down = KB.IsKeyDown(KB.Key.Down);
            bool left = KB.IsKeyDown(KB.Key.Left);
            bool right = KB.IsKeyDown(KB.Key.Right);

            if (up) Core.Cam.y -= move_speed;
            else if (down) Core.Cam.y += move_speed;
            if (left) Core.Cam.x -= move_speed;
            else if (right) Core.Cam.x += move_speed;

            if(up || down || left || right)
                angle = (up ?  90F + (left ? 45F : (right ? -45F : 0F)) : (down ? 270 +  (left ? -45F : (right ? 45F : 0F)) : 0F));
        }

        public void Display()
        {
            //walk_anim.g
            walk_anim.Display(1, Core.Cam + new vecf(32, 32));
        }
    }
}
