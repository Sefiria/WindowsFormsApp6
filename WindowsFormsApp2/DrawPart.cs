using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Entities;

namespace WindowsFormsApp2
{
    public static class DrawPart
    {
        static Timer TimerLogs = new Timer() { Enabled = true, Interval = 3000 };
        static bool Loaded = false;
        static bool DisplayLogs = false;

        public static void Draw(MainForm form)
        {
            if (!Loaded)
                Load();

            SharedCore.g.Clear(Color.Black);

            SharedData.World.Current.Draw();

            //for (int x = 0; x < form.Width; x += SharedCore.TileSize)
            //    SharedCore.g.DrawLine(Pens.Cyan, x, 0, x, form.Height);
            //for (int y = 0; y < form.Height; y += SharedCore.TileSize)
            //    SharedCore.g.DrawLine(Pens.Cyan, 0, y, form.Width, y);

            var list = new List<DrawableEntity>(SharedData.Entities);
            foreach(var entity in list)
            {
                if (entity is Player)
                    continue;

                if(!entity.Exists)
                {
                    SharedData.Entities.Remove(entity);
                }
                else
                {
                    DrawOne(entity);
                }
            }

            if(SharedData.Player.Exists)
                SharedData.Player.Draw();

            DoDisplayLogs();

            form.Render.Image = form.Img;
        }

        private static void DoDisplayLogs()
        {
            if(Logger.NewLogs)
            {
                DisplayLogs = true;
                TimerLogs.Stop();
                TimerLogs.Start();
                Logger.NewLogs = false;
            }

            if(DisplayLogs)
            {
                var logs = SharedData.Logs.Count > 5 ? SharedData.Logs.Skip(SharedData.Logs.Count - 5).ToList() : SharedData.Logs;

                var boxX = 0;
                var boxY = SharedCore.RenderH - 25 * logs.Count;
                var boxW = SharedCore.RenderW;
                var boxH = 25 * logs.Count;
                SharedCore.g.FillRectangle(new SolidBrush(Color.FromArgb(150, 50, 50, 50)), boxX, boxY, boxW, boxH);
                SharedCore.g.DrawRectangle(new Pen(Color.FromArgb(150, 80, 80, 80), 5F), boxX, boxY, boxW, boxH);

                string log;
                Brush b;
                for (int i = logs.Count; i > 0; i--)
                {
                    log = logs[logs.Count - i];
                    if (logs.Count == 1)
                        b = Brushes.White;
                    else
                        b = new SolidBrush(Color.FromArgb(255 - (int)((i - 1) / (float)logs.Count * 255), Color.White));
                    SharedCore.g.DrawString(log, SharedCore.Font, b, 0, SharedCore.RenderH - 25 * i);
                }
            }
        }

        private static void Load()
        {
            Loaded = true;
            TimerLogs.Tick += TimerLogs_Tick;
        }

        private static void TimerLogs_Tick(object sender, EventArgs e)
        {
            DisplayLogs = false;
            SharedData.Logs.Clear();
        }

        private static void DrawOne(DrawableEntity e)
        {
            e.Draw();
        }
        private static Point GetPoint(IDraw e) => new Point((int)e.X, (int)e.Y);
    }
}
