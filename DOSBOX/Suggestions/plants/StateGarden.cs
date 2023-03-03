using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace DOSBOX.Suggestions
{
    public class Garden : IState
    {
        public static Garden Instance = null;

        public static int FloorLevel => 48;
        public User User;
        public List<WaterDrop> WaterDrops = new List<WaterDrop>();
        public List<IPlant> ScenePlants = new List<IPlant>();
        public int Ticks, TicksMax;
        public byte[,] ActiveBG = new byte[64, FloorLevel];

        public void Init()
        {
            if (Instance == null)
                Instance = this;

            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI

            WaterDrops.Clear();
            ScenePlants.Clear();
            Ticks = 0; TicksMax = 9;
            User = new User();

            DisplayBG();
        }
        public void InitActive()
        {
            for (int x = 0; x < 64; x++)
                for (int y = 0; y < 64 - FloorLevel; y++)
                    Core.Layers[0][x, FloorLevel + y] = ActiveBG[x, y];
        }

        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                for (int x = 0; x < 64; x++)
                    for (int y = 0; y < 64 - FloorLevel; y++)
                        ActiveBG[x, y] = Core.Layers[0][x, FloorLevel + y];
                Plants.Instance.CurrentState = null;
                return;
            }


            User.Update();
            WaterDropsUpdate();
            new List<IPlant>(ScenePlants).ForEach(p => p.Update());
            BGUpdate();
            TicksUpdate();


            WaterDropsDisplay();
            DisplayUI();
            new List<IPlant>(ScenePlants).ForEach(p => p.Display(1));
            User.Display(2);
        }

        private void TicksUpdate()
        {
            Ticks++;
            while (Ticks > TicksMax)
                Ticks -= TicksMax;
        }

        void WaterDropsUpdate()
        {
            new List<WaterDrop>(WaterDrops).ForEach(drop =>
            {
                drop.Update();

                if(drop.destroy)
                    WaterDrops.Remove(drop);
            });
        }
        void WaterDropsDisplay()
        {
            new List<WaterDrop>(WaterDrops).ForEach(drop =>
            {
                Core.Layers[1][drop.vec.i.x, drop.vec.i.y] = 2;
            });
        }

        int sunimpact_y = FloorLevel;
        int ticksimpact = 0, ticksimpact_max = 4;
        private void BGUpdate()
        {
            if(Ticks == TicksMax)
            {
                ticksimpact++;
                while (ticksimpact > ticksimpact_max)
                    ticksimpact -= ticksimpact_max;
            }
            if(ticksimpact == ticksimpact_max)
            {
                ticksimpact = 0;
                Graphic.DisplayHorizontalLine(0, 64, sunimpact_y, 2, 1, 0);
                sunimpact_y++;
                if (sunimpact_y >= 64)
                    sunimpact_y = FloorLevel;
            }
        }
        private void DisplayBG()
        {
            Graphic.DisplayRect(0, FloorLevel, 64, 64 - FloorLevel, 2, 0);
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
            foreach(var plant in ScenePlants)
                if(plant.masterbranch != null)
                    subbranches(plant.masterbranch);
            return fruitsPerBranch;
        }
    }
}
