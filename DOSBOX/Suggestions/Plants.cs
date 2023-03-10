using DOSBOX.Suggestions.plants;
using DOSBOX.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DOSBOX.Suggestions
{
    public class Plants : ISuggestion
    {
        public static Plants Instance;
        public bool ShowHowToPlay { get; set; }
        public IState CurrentState, NextState;
        public static List<List<(string Name, IState Instance)>> PagesStates = new List<List<(string, IState)>>()
        {
            new List<(string Name, IState Instance)>{
                ("Garden", Garden.Instance),
                ("Storage", new Storage()),
                ("Seeds", new Seeds()),
                ("Shop", new Shop()),
                ("Sell", new Sell()),
            },
            new List<(string Name, IState Instance)>{
                ("Save", null),
                ("Load", null),
                ("New", null),
            },
            new List<(string Name, IState Instance)>{
                ("Labs", new Labs()),
                ("LabsShop", new LabsShop()),
            },
        };
        public static int pagemax => PagesStates.Count - 1;
        public static int page = 0;


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
            Core.Layers.Add(new byte[64, 64]); // UI
            Graphic.Clear(0, 0);
            Graphic.Clear(0, 1);
        }

        public void Update()
        {
            if (ShowHowToPlay)
            {
                HowToPlay();
                return;
            }

            if (NextState != null)
            {
                CurrentState = NextState;
                NextState = null;
                if (CurrentState != Garden.Instance)
                    CurrentState.Init();
                else
                    Garden.Instance.InitActive();
            }

            if (CurrentState == null)
            {
                Garden.Instance?.BackgroundWork();
                Core.Cam = vecf.Zero;
                UpdateMenu();

                if (KB.IsKeyPressed(KB.Key.Escape))
                {
                    Garden.KillInstance();
                    Data.KillInstance();
                    Core.CurrentSuggestion = null;
                    return;
                }
            }
            else
            {
                CurrentState.Update();
            }
        }


        int menu_selection = 0;
        private void UpdateMenu()
        {
            if (KB.IsKeyPressed(KB.Key.Left) && page > 0)
            {
                page--;
                menu_selection = 0;
            }
            if (KB.IsKeyPressed(KB.Key.Right) && page < pagemax)
            {
                page++;
                menu_selection = 0;
            }

            var States = PagesStates[page];

            if (KB.IsKeyPressed(KB.Key.Up) && menu_selection > 0)
                menu_selection--;
            if (KB.IsKeyPressed(KB.Key.Down) && menu_selection < States.Count - 1)
                menu_selection++;

            Graphic.Clear(2, 0);
            Graphic.Clear(0, 1);
            Graphic.DisplayRectAndBounds(2, 2, 60, 60, 0, 1, 1, 0);
            int x = 6, y = 6, w = 56, h = 9;
            Graphic.DisplayRectAndBounds(x - 2, y - 2 + menu_selection * (h + 1), w, h, 0, 1, 1, 1);
            for (int i = 0; i < States.Count; i++)
            {
                Text.DisplayText(States[i].Name, x, y, 1);
                y += h + 1;
            }

            if (KB.IsKeyPressed(KB.Key.Enter))
            {
                if (States[menu_selection].Instance != null && States[menu_selection].Name == "Garden")
                    States[menu_selection] = ("Garden", Garden.Instance);
                (string name, IState state) = States[menu_selection];
                if (state != null)
                {
                    NextState = state;
                }
                else
                {
                    if (name == "Save")
                        PlantsSave.Save();
                    else if (name == "Load")
                        PlantsSave.Load();
                    else if (name == "New")
                    {
                        Garden.KillInstance();
                        Data.KillInstance();
                    }
                }
            }
        }
    }

    public class PlantsSave
    {
        static string path = ".mem/.plts/plts.mem";

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
            public List<vec> wetdirt = new List<vec>();

            public void SaveWetDirt()
            {
                if (data == null) return;
                int w = data.m_Garden.ActiveBG.GetLength(0);
                int h = data.m_Garden.ActiveBG.GetLength(1);
                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++)
                        if(data.m_Garden.ActiveBG[x, y] == 3)
                            wetdirt.Add(new vec(x, y));
            }
            public void LoadWetDirt()
            {
                if(data.m_Garden.ActiveBG == null)
                    data.m_Garden.ActiveBG = new byte[data.m_Garden.MapWidth, data.m_Garden.FloorLevel];
                int w = data.m_Garden.ActiveBG.GetLength(0);
                int h = data.m_Garden.ActiveBG.GetLength(1);
                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++)
                        data.m_Garden.ActiveBG[x, y] = (byte)(wetdirt.FirstOrDefault(wd => wd.x == x && wd.y == y) != null ? 3 : 2);
            }
        }

        public static void Save()
        {
            ChckPth();

            savefile savefile = new savefile();
            savefile.data = Data.Instance;
            savefile.SaveWetDirt();

            string contents = JsonConvert.SerializeObject(savefile, options);

            File.WriteAllText(path, contents);
        }
        public static void Load()
        {
            ChckPth();

            string contents = File.ReadAllText(path);

            savefile savefile = JsonConvert.DeserializeObject<savefile>(contents, options);
            savefile.LoadWetDirt();
            Data.LoadInstance(savefile.data);

            var _ = Data.Instance.ToString();// trigger Data Instance in case it's null
        }

        private static void ChckPth()
        {
            if(!Directory.Exists(path))
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
