using DOSBOX.Utilities;
using System;
using Tooling;

namespace DOSBOX.Suggestions.fusion
{
    public class Door : Dispf
    {
        public static readonly float SPEED_OPENCLOSE = 0.05F;

        /// <summary>
        /// 1_closed  2_open 11_closing 22_opening
        /// </summary>
        public byte state = 1;
        public vec base_size;
        public vec size;
        public float interm_state = 0F, graphics_ext_timer = 0, graphics_ext_timer_incr = 0.1F;
        public Door(float x, float y, int w, int h, byte state = 1)
        {
            if (state == 11) state = 1;
            if (state == 22) state = 2;
            this.state = state;
            if (this.state == 1) interm_state = 1F;
            if (this.state == 2) interm_state = 0F;
            vec = new vecf(x * Tile.TSZ, y * Tile.TSZ);
            base_size = size = new vec(w * Tile.TSZ, h * Tile.TSZ);
            CreateGraphics();
            CreateGraphicsExt();
        }
        void CreateGraphics()
        {
            g = new byte[size.x, size.y];

            for (int i = 0; i < size.x; i++)
            {
                g[i, 0] = 3;
                g[i, size.y-1] = 3;
            }
            for (int j = 0; j < size.y; j++)
            {
                g[0, j] = 3;
                g[size.x-1, j] = 3;
            }
            for (int i = 1; i < size.x-1; i++)
            {
                g[i, 1] = 2;
                g[i, size.y - 2] = 2;
            }
            for (int j = 1; j < size.y-1; j++)
            {
                g[1, j] = 2;
                g[size.x - 2, j] = 2;
            }
            for (int i = 2; i < size.x -2; i++)
                for (int j = 2; j < size.y -2; j++)
                    g[i, j] = 1;

            scale = 1;
        }
        void CreateGraphicsExt()
        {
            if (state == 1)
            {
                vec s = size - (0, 4).V();
                for (int i = (int)(-s.x * 0.2F); i < (int)(s.x * 0.2F); i++)
                    for (int j = (int)(-s.y * 0.25F); j < (int)(s.y * 0.25F); j++)
                        if ((i % 2 == 0 && j % 2 != 0) || (i % 2 != 0 && j % 2 == 0))
                            g[size.x / 2 + i, size.y / 2 + j] = (byte)graphics_ext_timer;
            }

            if(state == 2)
            {
                for (int i = 2; i < size.x - 2; i++)
                    g[i, 2] = 1;
                g[size.x - 3 - (Core.TotalTicks % (size.x-4)), 2] = 0;
            }

            #region graphics_ext_timer_mgmt
            graphics_ext_timer += graphics_ext_timer_incr;
            if (graphics_ext_timer < 1)
            {
                graphics_ext_timer = 2;
                graphics_ext_timer_incr = 0.1F;
            }
            if (graphics_ext_timer > 4)
            {
                graphics_ext_timer = 3;
                graphics_ext_timer_incr = -0.1F;
            }
            #endregion
        }
        public void Update()
        {
            float old_interm = interm_state;
            
            if (state == 22) // opening
            {
                interm_state -= SPEED_OPENCLOSE;
                if (interm_state < 0F) interm_state = 0F;
                if (interm_state == 0F)
                    state = 2;
            }
            else if (state == 11) // closing
            {
                interm_state += SPEED_OPENCLOSE;
                if (interm_state > 1F) interm_state = 1F;
                if (interm_state == 1F)
                    state = 1;
            }

            if(old_interm != interm_state)
            {
                int min_open = 5;
                size = (base_size.x, min_open + (int)((base_size.y - min_open) * interm_state)).V();
                CreateGraphics();
            }

            CreateGraphicsExt();
        }
        public void Destroy()
        {
        }
        public void Hit(Bullet bullet)
        {
            if (state != 2)
                state = 22;
            else if (state != 1)
                state = 11;
        }
    }
}
