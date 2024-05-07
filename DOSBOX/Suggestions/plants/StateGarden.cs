using DOSBOX.Suggestions.plants;
using DOSBOX.Utilities;
using System.Collections.Generic;
using System.Linq;
using Tooling;

namespace DOSBOX.Suggestions
{
    public class Garden : IState
    {
        private static Garden m_Instance = null;
        public static Garden Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Garden();
                    m_Instance.Init();
                }
                return m_Instance;
            }
        }
        public static void KillInstance()
        {
            m_Instance = null;
        }

        public void Init()
        {
            Core.Layers.Clear();
            Core.Layers.Add(new byte[Data.Garden.MapWidth, 64]); // BG
            Core.Layers.Add(new byte[Data.Garden.MapWidth, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI

            Data.Garden.Init();

            DisplayBG();
        }
        public void InitActive()
        {
            Core.Layers.Clear();
            Core.Layers.Add(new byte[Data.Garden.MapWidth, 64]); // BG
            Core.Layers.Add(new byte[Data.Garden.MapWidth, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI

            for (int x = 0; x < Data.Garden.MapWidth; x++)
                for (int y = 0; y < 64 - Data.Garden.FloorLevel; y++)
                    if (x < Data.Garden.ActiveBG.GetLength(0))
                        Core.Layers[0][x, Data.Garden.FloorLevel + y] = Data.Garden.ActiveBG[x, y];
                    else
                        Core.Layers[0][x, Data.Garden.FloorLevel + y] = 2;
            Data.Garden.ActiveBG = null;
        }

        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                Data.Garden.ActiveBG = new byte[Data.Garden.MapWidth, Data.Garden.FloorLevel];
                for (int x = 0; x < Data.Garden.MapWidth; x++)
                    for (int y = 0; y < 64 - Data.Garden.FloorLevel; y++)
                        Data.Garden.ActiveBG[x, y] = Core.Layers[0][x, Data.Garden.FloorLevel + y];
                Plants.Instance.CurrentState = null;
                return;
            }

            Data.Garden.Meteo.Update();

            Data.Garden.User.Update();
            WaterDropsUpdate();
            new List<IPlant>(Data.Garden.ScenePlants).ForEach(p => p.Update());
            BGUpdate();
            TicksUpdate();


            WaterDropsDisplay();
            DisplayUI();
            new List<IPlant>(Data.Garden.ScenePlants).ForEach(p => p.Display(1));
            Data.Garden.Meteo.Display(2);
            Data.Garden.User.Display(2);
        }

        public void BackgroundWork()
        {
            Data.Garden.Meteo.Update();
            WaterDropsUpdate(Data.Garden.ActiveBG);
            new List<IPlant>(Data.Garden.ScenePlants).ForEach(p => p.Update(Data.Garden.ActiveBG));
            BGUpdate(Data.Garden.ActiveBG);
            TicksUpdate();
        }

        private void TicksUpdate()
        {
            Data.Garden.Ticks++;
            while (Data.Garden.Ticks > Data.Garden.TicksMax)
                Data.Garden.Ticks -= Data.Garden.TicksMax;
        }

        void WaterDropsUpdate(byte[,] ActiveBG = null)
        {
            new List<WaterDrop>(Data.Garden.WaterDrops).ForEach(drop =>
            {
                if(Data.Garden.ActiveBG == null)
                    drop.Update();
                else
                    drop.Update(Data.Garden.ActiveBG);

                if(drop.destroy)
                    Data.Garden.WaterDrops.Remove(drop);
            });
        }
        void WaterDropsDisplay()
        {
            new List<WaterDrop>(Data.Garden.WaterDrops).ForEach(drop =>
            {
                if(!Core.isout(drop.vec, 1))
                    Core.Layers[1][drop.vec.i.x, drop.vec.i.y] = 2;
            });
        }

        private void BGUpdate(byte[,] ActiveBG = null)
        {
            if(Data.Garden.Ticks == Data.Garden.TicksMax)
            {
                Data.Garden.Ticksimpact++;
                while (Data.Garden.Ticksimpact > Data.Garden.Ticksimpact_max)
                    Data.Garden.Ticksimpact -= Data.Garden.Ticksimpact_max;
            }
            if(Data.Garden.Ticksimpact == Data.Garden.Ticksimpact_max)
            {
                Data.Garden.Ticksimpact = 0;
                if(Data.Garden.ActiveBG == null)
                    Graphic.DisplayHorizontalInfiniteLine(Data.Garden.sunimpact_y, 2, 1, 0, false);
                else
                    Graphic.DisplayHorizontalInfiniteLine(Data.Garden.sunimpact_y, 2, 1, Data.Garden.ActiveBG, false);
                Data.Garden.sunimpact_y++;
                if (Data.Garden.sunimpact_y >= 64)
                    Data.Garden.sunimpact_y = Data.Garden.FloorLevel;
            }
        }
        private void DisplayBG()
        {
            Graphic.DisplayRect(0, Data.Garden.FloorLevel, Data.Garden.MapWidth, 64 - Data.Garden.FloorLevel, 2, 0);
        }

        private void DisplayUI()
        {
        }


        public Dictionary<Branch, List<Fruit>> GetPlantsFruits()
        {
            Dictionary<Branch, List<Fruit>> fruitsPerBranch = new Dictionary<Branch, List<Fruit>>();
            void subbranches(Branch b)
            {
                foreach (var fruit in b.fruits)
                {
                    if (!fruitsPerBranch.ContainsKey(b))
                        fruitsPerBranch[b] = new List<Fruit>();
                    fruitsPerBranch[b].Add(fruit);
                }
                foreach (var _b in b.tree_branches)
                    subbranches(_b);
            }
            var list = Data.Garden.ScenePlants.Where(p => p.vec.x >= Core.Cam.i.x && p.vec.x < Core.Cam.i.x + 64);
            foreach(var plant in Data.Garden.ScenePlants)
                if(plant.masterbranch != null)
                    subbranches(plant.masterbranch);
            return fruitsPerBranch;
        }
    }
}
