using DOSBOX.Suggestions.city;
using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DOSBOX.Suggestions
{
    public class CityEditor : ISuggestion
    {
        public static CityEditor Instance;

        public bool ShowHowToPlay { get; set; }
        public bool ShowTiles = false;
        public const int w = 128;
        public const int h = 128;
        public byte[,] Blocks = new byte[w, h];
        public vec seltile = vec.Zero;
        byte selcol = 0;


        public void HowToPlay()
        {
        }

        public void Init()
        {
            Instance = this;
            ShowHowToPlay = false;

            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // UI

            Data.Instance.ToString();// trigger Data Instance
        }

        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                Core.CurrentSuggestion = null;
                return;
            }

            if (KB.IsKeyDown(KB.Key.LeftCtrl))
            {
                if (KB.IsKeyPressed(KB.Key.S))
                    CitySave.Save();
                if (KB.IsKeyPressed(KB.Key.L))
                    CitySave.Load();
            }

            selcol++;
            if (selcol > 3) selcol = 0;


            UpdateUserInputs();


            DisplayUI();
        }

        private void UpdateUserInputs()
        {
            if (KB.IsKeyPressed(KB.Key.LeftCtrl))
            {
                ShowTiles = !ShowTiles;
                Core.Layers.Clear();
                Core.Layers.Add(new byte[64, 64]); // BG
                Core.Layers.Add(new byte[64, 64]); // UI
            }

            if (ShowTiles)
            {
                bool up = KB.IsKeyPressed(KB.Key.Up);
                bool down = KB.IsKeyPressed(KB.Key.Down);
                bool left = KB.IsKeyPressed(KB.Key.Left);
                bool right = KB.IsKeyPressed(KB.Key.Right);

                int sz = Tile.TSZ;
                int wc = 64 / sz - 1;
                int yamount = IndexedBlocks.RefTiles.Count / wc;

                if (up) seltile.y--;
                else if (down) seltile.y++;
                if (left) seltile.x--;
                else if (right) seltile.x++;

                if (seltile.x < 0) seltile.x = 0;
                if (seltile.y < 0) seltile.y = 0;
                if (seltile.y == yamount)
                {
                    if (seltile.x >= IndexedBlocks.RefTiles.Count % wc)
                        seltile.x = IndexedBlocks.RefTiles.Count % wc - 1;
                }
                else
                {
                    if (seltile.x >= wc)
                        seltile.x = wc - 1;
                }
                if (seltile.y >= yamount + 1) seltile.y = yamount;
            }
            else
            {
                float move_speed = 2F;

                bool up = KB.IsKeyDown(KB.Key.Z);
                bool down = KB.IsKeyDown(KB.Key.S);
                bool left = KB.IsKeyDown(KB.Key.Q);
                bool right = KB.IsKeyDown(KB.Key.D);

                if (up) Core.Cam.y -= move_speed;
                else if (down) Core.Cam.y += move_speed;
                if (left) Core.Cam.x -= move_speed;
                else if (right) Core.Cam.x += move_speed;

                if (KB.IsKeyDown(KB.Key.Space))
                {
                    int x = (int)(Core.Cam.x / Tile.TSZ);
                    int y = (int)(Core.Cam.y / Tile.TSZ);
                    if (x >= 0 && y >= 0 && x < w && y < h)
                    {
                        byte id = (byte)(seltile.y * (64 / Tile.TSZ - 1) + seltile.x);
                        Blocks[x, y] = id;
                    }
                }
            }
        }

        private void DisplayUI()
        {
            if (ShowTiles)
                Display_Tiles();
            else
                Display_Map();

            Text.DisplayText("                                ", 0, 0, 0);
            Text.DisplayText(Core.Cam.ToString(), 0, 0, 0);
        }

        private void Display_Map()
        {
            int _x, _y;
            for (int x = -32 / Tile.TSZ; x < 32 / Tile.TSZ; x++)
            {
                for (int y = -32 / Tile.TSZ; y < 32 / Tile.TSZ; y++)
                {
                    _x = (int)Core.Cam.x / Tile.TSZ + x;
                    _y = (int)Core.Cam.y / Tile.TSZ + y;
                    if (_x >= 0 && _y >= 0 && _x < w && _y < h)
                    {
                        for (int i = 0; i < Tile.TSZ; i++)
                        {
                            for (int j = 0; j < Tile.TSZ; j++)
                            {
                                int X = 32 + x * Tile.TSZ + i - (int)(Core.Cam.x % Tile.TSZ);
                                int Y = 32 + y * Tile.TSZ + j - (int)(Core.Cam.y % Tile.TSZ);
                                if (X >= 0 && X < 64 && Y >= 0 && Y < 64)
                                    Core.Layers[0][X, Y] = IndexedBlocks.RefTiles[Blocks[_x, _y]].Pixels[i, j];
                            }
                        }
                    }
                }
            }


            int sz = Tile.TSZ;
            vec v = Core.Cam.i;
            _x = 4 * sz - v.x % sz;
            _y = 4 * sz - v.y % sz;
            if (_x >= 0 && _y >= 0 && _x < w && _y < h)
            {
                for (int i = 0; i < Tile.TSZ; i++)
                {
                    Core.Layers[1][_x + i, _y] = selcol;
                    Core.Layers[1][_x + i, _y + sz - 1] = selcol;
                }
                for (int j = 0; j < Tile.TSZ; j++)
                {
                    Core.Layers[1][_x, _y + j] = selcol;
                    Core.Layers[1][(_x + sz) - 1, _y + j] = selcol;
                }
            }
        }

        private void Display_Tiles()
        {
            int sz = Tile.TSZ;
            int wc = 64 / sz - 1;
            int yamount = IndexedBlocks.RefTiles.Count / wc;
            for (int y = 0; y < yamount + 1; y++)
            {
                for (int x = 0; x < (y == yamount ? IndexedBlocks.RefTiles.Count % wc : wc); x++)
                {
                    for (int i = 0; i < Tile.TSZ; i++)
                    {
                        for (int j = 0; j < Tile.TSZ; j++)
                        {
                            if (x * sz + i >= 0 && x * sz + i < 64 && y * sz + j >= 0 && y * sz + j < 64)
                                Core.Layers[0][x * sz + i, y * sz + j] = IndexedBlocks.RefTiles[y * wc + x].Pixels[i, j];
                        }
                    }
                }
            }


            for (int i = 0; i < Tile.TSZ; i++)
            {
                Core.Layers[1][seltile.x * sz + i, seltile.y * sz] = selcol;
                Core.Layers[1][seltile.x * sz + i, (seltile.y + 1) * sz - 1] = selcol;
            }
            for (int j = 0; j < Tile.TSZ; j++)
            {
                Core.Layers[1][seltile.x * sz, seltile.y * sz + j] = selcol;
                Core.Layers[1][(seltile.x + 1) * sz - 1, seltile.y * sz + j] = selcol;
            }
        }


        public class CitySave
        {
            static string path = ".mem/.city/city.mem";

            static JsonSerializerSettings options = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                TypeNameHandling = TypeNameHandling.Objects,
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            };
            public class savefile
            {
                public Data data { get; set; }
            }

            public static void Save()
            {
                ChckPth();

                savefile savefile = new savefile();
                savefile.data = Data.Instance;

                string contents = JsonConvert.SerializeObject(savefile, options);

                File.WriteAllText(path, contents);
            }
            public static void Load()
            {
                ChckPth();

                string contents = File.ReadAllText(path);

                savefile savefile = JsonConvert.DeserializeObject<savefile>(contents, options);
                Data.LoadInstance(savefile.data);

                var _ = Data.Instance.ToString();// trigger Data Instance in case it's null
            }

            private static void ChckPth()
            {
                if (!Directory.Exists(path))
                {
                    string[] cells = path.Split('/');
                    cells = cells.Take(cells.Length - 1).ToArray();

                    string cumulpath = "";
                    for (int i = 0; i < cells.Length; i++)
                    {
                        cumulpath += cells[i];
                        if (!Directory.Exists(cumulpath))
                        {
                            Directory.CreateDirectory(cumulpath);
                        }
                        cumulpath += '/';
                    }
                }
            }
        }
    }
}
