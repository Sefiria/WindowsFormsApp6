using DOSBOX.Properties;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using Tooling;

namespace DOSBOX.Suggestions
{
    public class Fusion : ISuggestion
    {
        public static Fusion Instance;
        public bool ShowHowToPlay { get; set; }
        List<Bullet> bullets = new List<Bullet>();
        List<Particule> particules = new List<Particule>();
        Room room = null;
        Samus samus;


        public void HowToPlay()
        {
            Text.DisplayText("press space", 2, 2, 0);
            Text.DisplayText("to continue", 2, 8, 0);

            if (KB.IsKeyDown(KB.Key.Space))
            {
                Graphic.Clear(0, 0);
                ShowHowToPlay = false;
            }
        }

        public void Init()
        {
            Instance = this;

            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI

            bullets = new List<Bullet>();

            room = Room.Load(0);
            samus = new Samus(32, 32);
        }

        public void Update()
        {
            if (room == null)
            {
                // Crash
                Core.CurrentSuggestion = null;
                return;
            }

            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                Core.CurrentSuggestion = null;
                return;
            }

            if (ShowHowToPlay)
            {
                HowToPlay();
                return;
            }


            samus.Update();
            new List<Bullet>(bullets).ForEach(b =>b.Update());
            new List<Particule>(particules).ForEach(b => b.Update());

            room.Display(samus.vec);
            samus.Display(1, Core.Cam.n.i);
            room.DisplayFront(samus.vec);
            bullets.ForEach(b => b.Display(1, Core.Cam.n.i));
            particules.ForEach(b => b.Display(1, Core.Cam.n.i));


            DisplayUI();


            Collisions();
        }

        private void DisplayUI()
        {
            for (int x = 0; x < 11; x++)
            {
                Core.Layers[2][x, 0] = 2;
                Core.Layers[2][x, 5] = 2;

                for (int y = 0; y < 6; y++)
                {
                    Core.Layers[2][0, y] = 2;
                    Core.Layers[2][10, y] = 2;

                    if(x > 0 && y > 0 && x < 10 && y < 5)
                        Core.Layers[2][x, y] = 4;
                }
            }
            Core.Layers[2][2, 2] = (byte)(KB.IsKeyDown(KB.Key.Q) ? 3 : 1);
            Core.Layers[2][4, 2] = (byte)(KB.IsKeyDown(KB.Key.D) ? 3 : 1);
            Core.Layers[2][2, 3] = (byte)(KB.IsKeyDown(KB.Key.Space) ? 3 : 1);
            Core.Layers[2][3, 3] = (byte)(KB.IsKeyDown(KB.Key.Space) ? 3 : 1);
            Core.Layers[2][4, 3] = (byte)(KB.IsKeyDown(KB.Key.Space) ? 3 : 1);
            Core.Layers[2][6, 3] = (byte)(KB.IsKeyDown(KB.Key.Left) ? 3 : 1);
            Core.Layers[2][8, 3] = (byte)(KB.IsKeyDown(KB.Key.Right) ? 3 : 1);
            Core.Layers[2][7, 2] = (byte)(KB.IsKeyDown(KB.Key.Up) ? 3 : 1);
            Core.Layers[2][7, 3] = (byte)(KB.IsKeyDown(KB.Key.Down) ? 3 : 1);
        }

        private void Collisions()
        {
        }

        public bool CollidesRoom(vecf v) => CollidesRoom(v.x, v.y);
        public bool CollidesRoom(float x, float y)
        {
            if (Instance.room.isout(x, y))
                return true;
            return Room.RefTiles[Instance.room.Tiles[(int)x / Tile.TSZ, (int)y / Tile.TSZ]].Type == Tile.TYPE.SOLID;
        }




        class Samus : Dispf
        {
            public bool SuperMorph = false;
            public bool PrevIsMorph, IsMorph = false;
            byte timershot = 0;
            float jump_look_y = 0F;

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

                float speed = 1F;
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
                if(KB.IsKeyPressed(KB.Key.Space) && jump_look_y == 0F)
                {
                    jump_look_y = 2F;
                }
                if (jump_look_y > 0F)
                {
                    move(new vecf((KB.IsKeyDown(KB.Key.Q) ? -1F : 0F) + (KB.IsKeyDown(KB.Key.D) ? 1F : 0F), -1F), speed, new vecf(0F, -_h / 2F));
                    jump_look_y -= 0.2F;
                }
                else
                {
                    if(!move(new vecf((KB.IsKeyDown(KB.Key.Q) ? -1F : 0F) + (KB.IsKeyDown(KB.Key.D) ? 1F : 0F), 1F), speed, new vecf(0F, _h / 2F)))
                        jump_look_y = 0F;
                    else
                        jump_look_y -= 0.2F;
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
                        if(direction != -1)
                            Shot(direction);
                    }
                }
                if (timershot > 0)
                    timershot--;
            }

            /// <returns>false if colliding (then no move)</returns>
            private bool move(vecf look, float speed, vecf offset)
            {
                offset *= 0.1F;
                if (offset.x == 0F && offset.y == 0F) throw new Exception("jpeux pas faire mon lerp là comme ça");
                bool lerpOnH = offset.y != 0F;
                bool collides = false;
                for (float i = 0F; i < speed && !collides; i++)
                    if(!(collides = Instance.CollidesRoom(new vecf(vec.x + offset.x + look.x, vec.y + (lerpOnH ? offset.y : 0F) + look.y))))
                        vec += look;
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
                float i = new List<int>() {4,5,6}.Contains(direction) ? 1F : (new List<int>() {0,1,2}.Contains(direction) ? -1F : 0F);
                float j = new List<int>() {0,6,7}.Contains(direction) ? 1F : (new List<int>() {2,3,4}.Contains(direction) ? -1F : 0F);
                Instance.bullets.Add(new Bullet(vec.x + _w / 2F + i * (_w + 2) / 2F, vec.y + _h / 4F + j * (_h + 2) / 2F, new vecf(i,j)));
            }
            public void GivePowerup(byte type)
            {
                switch (type)
                {
                    case 0: SuperMorph = true; break;
                }
            }
        }

        class Bullet : Dispf
        {
            vecf look;

            public Bullet(float x, float y, vecf look)
            {
                vec = new vecf(x, y);
                CreateGraphics();
                this.look = look;
            }

            void CreateGraphics()
            {
                g = new byte[2, 2]
                {
                    { 3, 3 },
                    { 3, 3 },
                };
                scale = 1;
            }

            public void Update()
            {
                float speed = 2F;
                if(!Instance.CollidesRoom(vec + look * speed))
                {
                    vec += look * speed;
                    look.y += 0.08F;
                }
                else
                {
                    Destroy();
                }

                if (Instance.room.isout(vec.x, vec.y))
                    Destroy();
            }
            public void Destroy()
            {
                Instance.bullets.Remove(this);
                Instance.particules.Add(new Particule(vec.x, vec.y, look.Rotate(110F)));
                Instance.particules.Add(new Particule(vec.x, vec.y, look.Rotate(-110F)));
            }
        }

        class Particule : Dispf
        {
            vecf look;

            public Particule(float x, float y, vecf look)
            {
                vec = new vecf(x, y);
                CreateGraphics();
                this.look = look;
            }

            void CreateGraphics()
            {
                g = new byte[1, 1]
                {
                    { 2 },
                };
                scale = 1;
            }

            public void Update()
            {
                float speed = 2F;
                if (!Instance.CollidesRoom(vec + look * speed))
                {
                    vec += look * speed;
                    look += new vecf(0F, 0.5F);
                }
                else
                {
                    Instance.particules.Remove(this);
                }

                if (Instance.room.isout(vec.x, vec.y))
                    Instance.particules.Remove(this);
            }
        }

        class Room
        {
            public byte ID;
            public byte[,] Tiles;
            public byte[,] Pixels, PixelsFront;
            public int w => Pixels.GetLength(0);
            public int h => Pixels.GetLength(1);

            public static Room Load(byte ID)
            {
                if (RefTiles == null)
                    DefineTiles();

                Room room = new Room();
                if (room.LoadPixels(ID))
                {
                    room.ID = ID;
                    room.LoadData();
                    return room;
                }
                return null;
            }

            public void Display(vecf cam)
            {
                (int w, int h) screen = (64, 64);
                (int x, int y) chunk = ((int)cam.x / screen.w, (int)cam.y / screen.h);
                Core.Cam = new vecf(chunk.x * 64, chunk.y * 64);

                for (int x = 0; x < screen.w; x++)
                {
                    for (int y = 0; y < screen.h; y++)
                    {
                        Core.Layers[0][x, y] = Pixels[chunk.x * screen.w + x, chunk.y * screen.h + y];
                    }
                }
            }
            public void DisplayFront(vecf cam)
            {
                (int w, int h) screen = (64, 64);
                (int x, int y) chunk = ((int)cam.x / screen.w, (int)cam.y / screen.h);
                byte px;

                for (int x = 0; x < screen.w; x++)
                {
                    for (int y = 0; y < screen.h; y++)
                    {
                        px = PixelsFront[chunk.x * screen.w + x, chunk.y * screen.h + y];
                        if (px != 0)
                            Core.Layers[1][x, y] = px;
                    }
                }
            }
            bool LoadPixels(byte ID)
            {
                Bitmap img = Resources.ResourceManager.GetObject($"room_{ID}") as Bitmap;
                if (img == null)
                    return false;

                Pixels = new byte[img.Width * TSZ, img.Height * TSZ];
                PixelsFront = new byte[img.Width * TSZ, img.Height * TSZ];
                Tiles = new byte[img.Width, img.Height];
                byte b;
                for (int x = 0; x < w / TSZ; x++)
                    for (int y = 0; y < h / TSZ; y++)
                    {
                        b = GetByteFromColor(img.GetPixel(x, y).R);
                        Tiles[x, y] = b;
                        SetPixelsFromTileId(b, x, y);
                    }
                return true;
            }
            byte GetByteFromColor(int r) => (byte)(r == 255 ? 0 : r + 1);
            void SetPixelsFromTileId(byte id, int _x, int _y)
            {
                for (int x = 0; x < TSZ; x++)
                    for (int y = 0; y < TSZ; y++)
                    {
                        if (RefTiles[id].Type == Tile.TYPE.FRONT)
                            PixelsFront[_x * TSZ + x, _y * TSZ + y] = RefTiles[id][x, y];
                        else if (RefTiles[id][x, y] != 4)
                            Pixels[_x * TSZ + x, _y * TSZ + y] = RefTiles[id][x, y];
                    }
            }
            void LoadData()
            {
                object obj = Resources.ResourceManager.GetObject($"room_{ID}_meta");
                if (obj == null)
                    return;

                string meta = BitConverter.ToString((byte[])obj);

                if (string.IsNullOrWhiteSpace(meta))
                    return;

                RoomData data = JsonSerializer.Deserialize<RoomData>(meta);

            }
            public bool isout(vecf v) => isout(v.i.x, v.i.y);
            public bool isout(float x, float y) => isout((int)x, (int)y);
            public bool isout(int x, int y) => x < 0 || y < 0 || x >= w || y >= h;
            public bool isout(byte[,] g, float x, float y)
            {
                int w = g.GetLength(0);
                int h = g.GetLength(1);
                return x < 0 || y < 0 || x + w >= this.w || y + h >= this.h;
            }

            class RoomData
            {
            }

            public static int TSZ => Tile.TSZ;
            public static List<Tile> RefTiles = null;
            private static void DefineTiles()
            {
                var humanreadableTiles = new List<Tile>
                {
                    new Tile() { Type = Tile.TYPE.EMPTY, Pixels = new byte[8, 8] },
                    new Tile() { Type = Tile.TYPE.SOLID, Pixels = new byte[8, 8]
                    {
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 3, 0, 0, 0, 0, 0, 0, 0 },
                        { 3, 1, 1, 1, 1, 1, 1, 0 },
                        { 3, 1, 1, 1, 1, 1, 1, 0 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 0, 0, 0, 0, 3, 0, 0, 0 },
                        { 1, 1, 1, 0, 3, 1, 1, 1 },
                        { 1, 1, 1, 0, 3, 1, 1, 1 },
                    } },
                    new Tile() { Type = Tile.TYPE.SOLID, Pixels = new byte[8, 8]
                    {
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                        { 3, 2, 0, 0, 0, 0, 2, 3 },
                        { 3, 0, 2, 0, 0, 2, 0, 3 },
                        { 3, 0, 0, 2, 2, 0, 0, 3 },
                        { 3, 0, 0, 2, 2, 0, 0, 3 },
                        { 3, 0, 2, 0, 0, 2, 0, 3 },
                        { 3, 2, 0, 0, 0, 0, 2, 3 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                    } },
                    new Tile() { Type = Tile.TYPE.FRONT, Pixels = new byte[8, 8]
                    {
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 0, 0, 0, 0, 0 },
                        { 0, 0, 0, 3, 3, 0, 0, 0 },
                        { 0, 3, 3, 3, 3, 3, 3, 0 },
                        { 3, 3, 4, 4, 4, 4, 3, 3 },
                        { 3, 4, 3, 3, 3, 3, 4, 3 },
                        { 3, 3, 3, 3, 3, 3, 3, 3 },
                    } },
                };
                
                RefTiles = new List<Tile>();
                foreach(var t in humanreadableTiles)
                {
                    var tile = new Tile() { Type = t.Type, Pixels = new byte[8, 8] };
                    for (int x = 0; x < 8; x++)
                        for (int y = 0; y < 8; y++)
                            tile.Pixels[x, y] = t[y, x];
                    RefTiles.Add(tile);
                }
            }
        }

        class Tile
        {
            public static int TSZ => 8;
            public enum TYPE
            {
                EMPTY = 0,
                SOLID,
                FRONT
            }

            public byte[,] Pixels;
            public TYPE Type;

            public byte this[int x, int y] => x<0||y<0||x>7||y>7 ? (byte)0 : Pixels[x, y];
        }
    }
}