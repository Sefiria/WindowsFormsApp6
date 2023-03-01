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
        public List<Plant> ScenePlants = new List<Plant>();
        public int Ticks, TicksMax;

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

        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                Plants.Instance.CurrentState = null;
                return;
            }


            User.Update();
            WaterDropsUpdate();
            new List<Plant>(ScenePlants).ForEach(p => p.Update());
            BGUpdate();
            TicksUpdate();


            WaterDropsDisplay();
            DisplayUI();
            User.Display(2);
            new List<Plant>(ScenePlants).ForEach(p => p.Display(1));
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
    }
}
