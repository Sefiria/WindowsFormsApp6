using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using WindowsFormsApp27.entities;

namespace WindowsFormsApp27
{
    public partial class Form1 : Form
    {
        Timer TimerUpdate = new Timer() { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer() { Enabled = true, Interval = 10 };

        long one_second_ticks = 50, one_minute_ticks = 50 * 60;
        long[] sim_ticks_gen = new long[]{ 0L };
        Bitmap RenderImage;
        Graphics g;
        int w, h;
        bool IsReporting = false;
        Reporting Reporting;

        public Form1()
        {
            InitializeComponent();
            KB.Init();
            MouseStates.Initialize(Render);

            w = Render.Width;
            h = Render.Height;
            RenderImage = new Bitmap(w, h);
            g = Graphics.FromImage(RenderImage);

            proc_load();

            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;

            IsReporting = false;
            Common.IsSimRuning = true;
        }
        void proc_load()
        {
            Reporting = new Reporting();

            var tent = Tent.Create((int)(20 + RandomThings.rnd1() * (w - 40)), (int)(20 + RandomThings.rnd1() * (h - 40)), "");
            People.Create(tent.x, tent.y + tent.h / 2, "A04F3500A1DFCEC4A2B7D0EC").Home = tent;
        }
        private void Update(object sender, EventArgs e)
        {
            /* */if (KB.IsKeyDown(KB.Key.Up)) Global.SPEED_SCALE = 1.0F;
            else if (KB.IsKeyDown(KB.Key.Left)) Global.SPEED_SCALE = 5.0F;
            else if (KB.IsKeyDown(KB.Key.Down)) Global.SPEED_SCALE = 10.0F;
            else if (KB.IsKeyDown(KB.Key.Right)) Global.SPEED_SCALE = 30.0F;

            if (Common.IsSimRuning)
            {
                proc_update();
                if (Common.sim_ticks >= one_minute_ticks)
                {
                    Common.IsSimRuning = false;
                }
            }
            else
            {
                if (IsReporting)
                {
                    proc_report();
                }
                else
                {
                    proc_go_back_to_home();
                }
            }

            KB.Update();
            MouseStates.Update();
            Common.ticks++;
            if(Common.IsSimRuning)
                Common.sim_ticks += (int)Global.SPEED_SCALE;
        }
        private void Draw(object sender, EventArgs e)
        {
            g.Clear(Color.FromArgb(141, 186, 73));
            proc_draw();
            Render.Image = RenderImage;
        }

        void proc_draw()
        {
            if (!IsReporting)
            {
                var list = new List<Entity>(Entity.Entities);
                foreach (var e in list)
                {
                    e.Draw(g);
                }

                // UI

                g.DrawString($"{Common.sim_ticks}/{one_minute_ticks}  (x{Global.SPEED_SCALE})", new Font("Courrier New", 16F), Brushes.Black, 10, 10);
            }
            else
            {
                Reporting.GenerateDraw(g);
            }
        }
        void proc_update()
        {
            var list = new List<Entity>(Entity.Entities);
            foreach (var e in list)
            {
                e.PreUpdate();
                e.Update();
                e.PostUpdate();
                if (!e.exists)
                    Entity.Entities.Remove(e);
            }

            proc_generate_world();
        }
        void proc_go_back_to_home()
        {
            var list = new List<People>(Entity.Peoples.Where(e => !e.AtHome));
            if (list.Count == 0)
            {
                proc_end_day();
                IsReporting = true;
                Reporting.PreGenerate(w, h);
            }
            else
            {
                IsReporting = false;
                foreach (var e in list)
                    e.GoBackToHome();
            }
        }
        void proc_end_day()
        {
            foreach(var p in Entity.Peoples)
            {
                if(p.FruitsOwn >= 3)
                {
                    p.FruitsOwn -= 3;
                    var tent = Tent.Create((int)(20 + RandomThings.rnd1() * (w - 40)), (int)(20 + RandomThings.rnd1() * (h - 40)), "");
                    People.Create(tent.x, tent.y + tent.h / 2, p.dna_muted()).Home = tent;
                    Common.sim_births++;
                }
            }

            Reporting.AddDataPoint(new Reporting.DataPoint() { Population = Entity.Peoples.Count, Deaths = Common.sim_deaths, Births = Common.sim_births });
        }
        void proc_report()
        {
            if (KB.IsKeyDown(KB.Key.Space))
            {
                Reporting.PostGenerate();
                reset();
            }
            else
            {
            }
        }
        void reset()
        {
            Common.IsSimRuning = true;
            IsReporting = false;
            Common.sim_ticks = 0L;
            for (int i = 0; i < sim_ticks_gen.Length; i++)
                sim_ticks_gen[i] = 0L;
            Common.sim_births = 0;
            Common.sim_deaths = 0;
        }

        void proc_generate_world()
        {
            if (Entity.Fruits.Count < Global.MAX_FRUITS)
            {
                if ((Common.sim_ticks - sim_ticks_gen[0]) > 10 / Global.SPEED_GROW)
                {
                    sim_ticks_gen[0] = Common.sim_ticks;
                    //if (RandomThings.rnd_IsInRange(0.2F, 0.3F))
                        Fruit.Create(RandomThings.rnd_vec_in_screen(w, h), $"A0{RandomThings.rndByteHexString()}");
                }
            }
        }
    }
}
