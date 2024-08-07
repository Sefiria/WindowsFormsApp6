﻿using DOSBOX.Suggestions.fusion.Triggerables;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Tooling;

namespace DOSBOX.Suggestions.fusion
{
    public class Samus : Hittable
    {
        public bool Exists = true;

        public bool SuperMorph = false;
        public bool PrevIsMorph, IsMorph = false;
        public long afk_duration = 0;

        byte timershot = 0;
        float jump_look_y = 0F;
        bool is_on_ground = false;

        public sbyte ShieldMax = 5;
        public sbyte Shield = 5;
        public sbyte Lives = 3;

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
            if (!Exists)
                return;

            bool T = KB.IsKeyDown(KB.Key.Up);
            bool L = KB.IsKeyDown(KB.Key.Left);
            bool B = KB.IsKeyDown(KB.Key.Down);
            bool R = KB.IsKeyDown(KB.Key.Right);
            bool Q = KB.IsKeyDown(KB.Key.Q);
            bool Z = KB.IsKeyDown(KB.Key.Z);
            bool D = KB.IsKeyDown(KB.Key.D);
            bool Space = KB.IsKeyDown(KB.Key.Space);

            vecf last_vec = new vecf(vec);
            vec last_tile = vec.i.tile(Tile.TSZ);

            if (PrevIsMorph != IsMorph)
            {
                CreateGraphics();
                PrevIsMorph = IsMorph;
            }

            is_on_ground = Fusion.Instance.CollidesRoom(new vecf(vec.x, vec.y + _h / 2), _w, _h, this);

            float speed = 1F;
            float input_look_x = (Q ? -1F : 0F) + (D ? 1F : 0F);
            float input_offset_x = input_look_x == 0 ? 0 : (_w / 2F * (input_look_x < 0F ? -1F : 1F));
            if (jump_look_y == 0F)
            {
                if (Q)
                {
                    move(new vecf(-1F, 0F), speed, new vecf(-_w / 2F, 0F));
                }
                if (D)
                {
                    move(new vecf(1F, 0F), speed, new vecf(_w / 2F, 0F));
                }
            }
            if (Space && is_on_ground && jump_look_y == 0F)
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

            if(Z)
                (Fusion.Instance.room.PhysicalObjects.FirstOrDefault(po => po is ITriggerable && (vec / Tile.TSZ).i == (po.vec / Tile.TSZ).i) as ITriggerable)?.Trigger(this);


            if (Fusion.Instance.room.isout(vec.x, vec.y))
            {
                var warp = Fusion.Instance.room.Warps.FirstOrDefault(w => w.Tiles.From.Contains(last_tile));
                if (warp != null)
                {
                    var next_room = Room.Load((byte)warp.DestinationRoom);
                    var next_vec = warp.Tiles.To[warp.Tiles.From.IndexOf(last_tile)].f * Tile.TSZ + _h % Tile.TSZ;
                    var door = next_room.Doors.FirstOrDefault(d => d.vec.tile(Tile.TSZ) == next_vec.tile(Tile.TSZ) || d.vec.tile(Tile.TSZ) == next_vec.tile(Tile.TSZ) - (0, 1).Vf());
                    bool blocked_by_locked_door = false;
                    if (door != null)
                    {
                        if (door.Locked)
                            blocked_by_locked_door = true;
                        else
                            door.state = 22;
                    }
                    if (!blocked_by_locked_door)
                    {
                        Fusion.Instance.room = next_room;
                        vec = next_vec;
                    }
                    else
                    {
                        vec = last_vec;
                    }
                }
                else
                {
                    vec = last_tile.f * Tile.TSZ;
                }
            }

            if(!T && !L && !B && !R && !Q && !D && !Space)
            {
                afk_duration++;
            }
            else
            {
                afk_duration = 0;
            }
        }

        /// <returns>false if colliding (then no move)</returns>
        private bool move(vecf look, float speed, vecf offset)
        {
            if (offset.x == 0F && offset.y == 0F) throw new Exception("jpeux pas faire mon lerp là comme ça");
            bool lerpOnH = offset.y != 0F;
            bool collides;

            // y
            if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x, vec.y + (lerpOnH ? offset.y : 0F) + look.y * speed), _w, _h, this)))
                vec.y += look.y;

            // x
            if (look.x != 0F)
            {
                if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x + offset.x + look.x * speed, vec.y), _w, _h, this)))
                    vec.x += look.x;
                else
                {
                    collides = false;
                    float n = -1F;
                    for (float i = 0.1F; i <= 1.05F && !collides; i += 0.1F)
                        if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x + offset.x + look.x * i * speed, vec.y), _w, _h, this)))
                            n = i;
                    if (collides && n != -1F)
                        vec.x += look.x * n * speed;
                }
            }
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
            if (i!=0&&j!=0)
            {
                i *= 0.5F;
                j *= 0.5F;
            }
            else if(i!=0F&&j==0F)
            {
                j = -0.2F;
            }
            Fusion.Instance.bullets.Add(new Bullet(vec.x + i * _w / 2F, vec.y + j * _h / 2F, new vecf(i, j)) { Owner = this });
        }
        public void GivePowerup(byte type)
        {
            switch (type)
            {
                case 0: SuperMorph = true; break;
            }
        }
        public override void Hit(Harmful by)
        {
            if (by.Owner == this)
                return;

            Shield--;
            if (Shield < 0)
            {
                Lives += Shield;
                Shield = 0;
            }

            if (Lives <= 0)
                Exists = false;
        }
    }
}
