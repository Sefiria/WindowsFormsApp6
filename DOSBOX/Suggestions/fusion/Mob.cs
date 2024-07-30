using DOSBOX.Suggestions.fusion.jsondata;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace DOSBOX.Suggestions.fusion
{
    public class Mob : Hittable
    {
        public bool Exists = true;
        public string BaseHash = null, Hash = null;

        byte timershot = 0;
        float jump_look_y = 0F;
        bool is_on_ground = false;

        public sbyte ShieldMax = 0;
        public sbyte Shield = 0;
        public sbyte Lives = 1;

        public Mob(byte room_id, RoomData_mobs m)
        {
            vec = new vecf(m.vec.x * Tile.TSZ, m.vec.y * Tile.TSZ);
            CreateGraphics();
            DisplayCenterSprite = true;

            BaseHash = GenerateBaseHash(room_id, m.vec.x, m.vec.y);
            Hash = GenerateHash(room_id, m.vec.x, m.vec.y);
        }
        public Mob(byte room_id, vec v)
        {
            vec = new vecf(vec.x * Tile.TSZ, vec.y * Tile.TSZ);
            CreateGraphics();
            DisplayCenterSprite = true;

            BaseHash = GenerateBaseHash(room_id, v.x, v.y);
            Hash = GenerateHash(room_id, v.x, v.y);
        }
        public Mob(byte room_id, int x, int y)
        {
            vec = new vecf(x * Tile.TSZ, y * Tile.TSZ);
            CreateGraphics();
            DisplayCenterSprite = true;

            BaseHash = GenerateBaseHash(room_id, x, y);
            Hash = GenerateHash(room_id, x, y);
        }
        void CreateGraphics()
        {
            var human_readable = new byte[8, 8]
            {
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 1, 1, 0, 0, 1, 1, 0 },
                { 0, 0, 0, 1, 1, 0, 0, 0 },
                { 0, 3, 3, 2, 2, 3, 3, 0 },
                { 3, 3, 2, 2, 2, 2, 3, 3 },
                { 3, 2, 3, 3, 3, 3, 2, 3 },
                { 3, 3, 3, 3, 3, 3, 3, 3 },
            };

            var w = human_readable.GetLength(0);
            var h = human_readable.GetLength(1);
            g = new byte[w, h];

            foreach (var t in human_readable)
            {
                for (int x = 0; x < w; x++)
                    for (int y = 0; y < h; y++)
                        g[y, x] = human_readable[x, y];
            }

            scale = 1;
        }
        public void Update()
        {
            if (!Exists)
                return;

            var (T,L,B,R,Q,D,Space) = ai();

            vec last_tile = vec.i.tile(Tile.TSZ);

            is_on_ground = Fusion.Instance.CollidesRoom(new vecf(vec.x, vec.y + _h - 2), _w, _h);

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
                if (is_on_ground)
                    jump_look_y = 0F;
                else
                {
                    jump_look_y -= 0.2F;
                    move(new vecf(input_look_x, 1F), speed, new vecf(input_offset_x, (_h - 2) / 2F));
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


            if (Fusion.Instance.room.isout(vec.x, vec.y))
            {
                vec = last_tile.f * Tile.TSZ;
            }
        }

        private (bool T, bool L, bool B, bool R, bool Q, bool D, bool Space)  ai()
        {
            bool T, L, B, R, Q, D, Space;
            T = L = B = R = Q = D = Space = false;



            return (T, L, B, R, Q, D, Space);
        }

        /// <returns>false if colliding (then no move)</returns>
        private bool move(vecf look, float speed, vecf offset)
        {
            if (offset.x == 0F && offset.y == 0F) throw new Exception("jpeux pas faire mon lerp là comme ça");
            bool lerpOnH = offset.y != 0F;
            bool collides;

            // y
            if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x, vec.y + (lerpOnH ? offset.y : 0F) + look.y * speed), _w, _h)))
                vec.y += look.y;

            // x
            if (look.x != 0F)
            {
                if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x + offset.x + look.x * speed, vec.y), _w, _h)))
                    vec.x += look.x;
                else
                {
                    collides = false;
                    float n = -1F;
                    for (float i = 0.1F; i <= 1.05F && !collides; i += 0.1F)
                        if (!(collides = Fusion.Instance.CollidesRoom(new vecf(vec.x + offset.x + look.x * i * speed, vec.y), _w, _h)))
                            n = i;
                    if (collides && n != -1F)
                        vec.x += look.x * n * speed;
                }
            }
            return !collides;
        }
        void Shot(int direction)
        {
            float i = new List<int>() { 4, 5, 6 }.Contains(direction) ? 1F : (new List<int>() { 0, 1, 2 }.Contains(direction) ? -1F : 0F);
            float j = new List<int>() { 0, 6, 7 }.Contains(direction) ? 1F : (new List<int>() { 2, 3, 4 }.Contains(direction) ? -1F : 0F);
            if (i != 0 && j != 0)
            {
                i *= 0.5F;
                j *= 0.5F;
            }
            else if (i != 0F && j == 0F)
            {
                j = -0.2F;
            }
            Fusion.Instance.bullets.Add(new Bullet(vec.x + i * (_w + 2), vec.y + j * (_h + 2), new vecf(i, j)) { Owner = this });
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
            {
                Exists = false;
                var instance = Fusion.Instance;
                instance.particules.Add(new Particule(vec.x, vec.y, (-0.5F, -2F).Vf()));
                instance.particules.Add(new Particule(vec.x, vec.y, (-0.5F, -1F).Vf()));
                instance.particules.Add(new Particule(vec.x, vec.y, ( 0.5F, -1F).Vf()));
                instance.particules.Add(new Particule(vec.x, vec.y, ( 0.5F, -2F).Vf()));
            }
        }
        public static string GenerateBaseHash(byte room_id, int x, int y) => $"mob-{room_id}-{x}-{y}";
        public static string GenerateHash(byte room_id, int x, int y) => GenerateBaseHash(room_id, x, y)+"+"+RandomThings.GetCurrentTickDigits(4);
    }
}
