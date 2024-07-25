using DOSBOX.Suggestions.fusion;
using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;

namespace DOSBOX.Suggestions
{
    public class Fusion : ISuggestion
    {
        public static Fusion Instance;
        public bool ShowHowToPlay { get; set; }
        public List<Bullet> bullets = new List<Bullet>();
        public List<Particule> particules = new List<Particule>();
        public Room room = null;
        public Samus samus;


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


            room.Update();
            samus.Update();
            bullets.Clone().ForEach(b =>b.Update());
            particules.Clone().ForEach(b => b.Update());

            room.Display(samus.vec);
            samus.Display(1, Core.Cam.i);
            room.DisplayFront(samus.vec);
            bullets.ForEach(b => b.Display(1, Core.Cam.i));
            particules.ForEach(b => b.Display(1, Core.Cam.i));


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
        public bool CollidesRoom(float x, float y) => ColliderRoom(x,y) != null;
        public object ColliderRoom(vecf v) => ColliderRoom(v.x, v.y);
        public object ColliderRoom(float x, float y)
        {
            if (Instance.room.isout(x, y))
                return true;
            
            // Doors

            var c_door = room.Doors.Clone().FirstOrDefault(d => new Rectangle((int)d.vec.x, (int)d.vec.y, d._w, d._h).IntersectsWith(new Rectangle((int)x, (int)y, samus._w, samus._h)));
            if (c_door != null)
                return c_door;
            
            // Tiles

            Tile tile = Room.RefTiles[room.Tiles[(int)x / Tile.TSZ, (int)y / Tile.TSZ]];
            var c_tile = tile.Type == Tile.TYPE.SOLID ? tile : null;
            if (c_tile != null)
                return c_tile;

            return null;
        }
    }
}