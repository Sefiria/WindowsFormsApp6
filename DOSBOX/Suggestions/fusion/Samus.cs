using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using Tooling;

namespace DOSBOX.Suggestions.fusion
{
    public class Samus : Dispf
    {
        public bool SuperMorph = false;
        public bool PrevIsMorph, IsMorph = false;
        byte timershot = 0;
        float jump_look_y = 0F;
        bool is_on_ground = false;

        public sbyte ShieldMax = 5;
        public sbyte Shield = 5;
        public sbyte Lifes = 3;

        public Samus(int x, int y)
        {
            vec = new vecf(x, y);
            CreateGraphics();
            DisplayCenterSprite = true;
        }

        void CreateGraphics()
        {
            g = new byte[4, 4];
            for (int x = 0; x < _w; x++)
                for (int y = 0; y < _h; y++)
                    g[x, y] = 3;
            scale = 1;
        }

        public void Update()
        {
            if (PrevIsMorph != IsMorph)
            {
                CreateGraphics();
                PrevIsMorph = IsMorph;
            }

            is_on_ground = Fusion.Instance.CollidesRoom(new vecf(vec.x, vec.y + _h - 2));

            float speed = 1F;
            float input_look_x = (KB.IsKeyDown(KB.Key.Q) ? -1F : 0F) + (KB.IsKeyDown(KB.Key.D) ? 1F : 0F);
            float input_offset_x = input_look_x == 0 ? 0 : (_w / 2F * (input_look_x < 0F ? -1F : 1F));
            if (jump_look_y == 0F)
            {
                if (KB.IsKeyDown(KB.Key.Q))
                {
                    move(new vecf(-1F, 0F), speed, new vecf(-_w / 2F, 0F));
                }
                if (KB.IsKeyDown(KB.Key.D))
                {
                    move(new vecf(1F, 0F), speed, new vecf(_w / 2F, 0F));
                }
            }
            if (KB.IsKeyPressed(KB.Key.Space) && is_on_ground && jump_look_y == 0F)
            {
                jump_look_y = 2F;
            }
            if (jump_look_y > 0F)
            {
                move(new vecf(input_look_x, -1F), speed, new vecf(input_offset_x, -_h / 2F));
                jump_look_y -= 0.2F;
            }
            else
            {
                //if (!move(new vecf((KB.IsKeyDown(KB.Key.Q) ? -1F : 0F) + (KB.IsKeyDown(KB.Key.D) ? 1F : 0F), 1F), speed, new vecf(0F, _h / 2F)))
                if (is_on_ground)
                    jump_look_y = 0F;
                else
                {
                    jump_look_y -= 0.2F;
                    move(new vecf(input_look_x, 1F), speed, new vecf(input_offset_x, (_h-2) / 2F));
                }
            }

            bool T = KB.IsKeyDown(KB.Key.Up);
            bool L = KB.IsKeyDown(KB.Key.Left);
            bool B = KB.IsKeyDown(KB.Key.Down);
            bool R = KB.IsKeyDown(KB.Key.Right);
            if (T || L || B || R)
            {
                if (timershot == 0)
                {
                    timershot = 5;
                    int direction = -1;
                    if (!T && !B)
                    {
                        if (L) direction = 1;
                        if (R) direction = 5;
                    }
                    if (!L && !R)
                    {
                        if (T) direction = 3;
                        if (B) direction = 7;
                    }
                    if (B)
                    {
                        if (L) direction = 0;
                        if (R) direction = 6;
                    }
                    if (T)
                    {
                        if (L) direction = 2;
                        if (R) direction = 4;
                    }
                    if (direction != -1)
                        Shot(direction);
                }
            }
            if (timershot > 0)
                timershot--;
        }

        /// <returns>false if colliding (then no move)</returns>
        private bool move(vecf look, float speed, vecf offset)
        {
            if (offset.x == 0F && offset.y == 0F) throw new Exception("jpeux pas faire mon lerp là comme ça");
            bool lerpOnH = offset.y != 0F;
            bool collides;

            // y
            if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x, vec.y + (lerpOnH ? offset.y : 0F) + look.y * speed))))
                vec.y += look.y;

            // x
            if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x + offset.x + look.x * speed, vec.y))))
                vec.x += look.x;
            else
            {
                collides = false;
                float n = -1F;
                for (float i = 0.1F; i <= 1.05F && !collides; i += 0.1F)
                    if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x + offset.x + look.x * i * speed, vec.y))))
                        n = i;
                if (collides && n != -1F)
                    vec.x += look.x * n * speed;
            }
            //for (float i = 0F; i < speed && !collides; i++)
            //    for (float t = 0F; t <= 1F && !collides; t+=1F/ (lerpOnH?_h:_w))
            //        collides = Instance.CollidesRoom(Maths.Lerp(
            //            new vecf(vec.x + (lerpOnH ? offset.x : 0F) + look.x, vec.y + (lerpOnH ? offset.y : 0F) + look.y),
            //            new vecf(vec.x + (lerpOnH ? offset.x : _w) + look.x, vec.y + (lerpOnH ? offset.y : _h) + look.y),
            //            t));
            //if (!collides)
            //    vec += look;
            return !collides;
        }

        /// <summary>
        /// <para>0:bottom-left</para>
        /// <para>1:left</para>
        /// <para>2:top-left</para>
        /// <para>3:top</para>
        /// <para>4:top-right</para>
        /// <para>5:right</para>
        /// <para>6:bottom-right</para>
        /// <para>7:bottom</para>
        /// </summary>
        void Shot(int direction)
        {
            float i = new List<int>() { 4, 5, 6 }.Contains(direction) ? 1F : (new List<int>() { 0, 1, 2 }.Contains(direction) ? -1F : 0F);
            float j = new List<int>() { 0, 6, 7 }.Contains(direction) ? 1F : (new List<int>() { 2, 3, 4 }.Contains(direction) ? -1F : 0F);
            Fusion.Instance.bullets.Add(new Bullet(vec.x + _w / 2F + i * (_w + 2) / 2F, vec.y + _h / 4F + j * (_h + 2) / 2F, new vecf(i, j)));
        }
        public void GivePowerup(byte type)
        {
            switch (type)
            {
                case 0: SuperMorph = true; break;
            }
        }
    }
}
