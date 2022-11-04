using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsFormsApp6.Particules;
using WindowsFormsApp6.UI;
using WindowsFormsApp6.World.Blocs;
using WindowsFormsApp6.World.Entities;

namespace WindowsFormsApp6.World
{
    public class StateTower : IState
    {
        public int TileSz => 48;
        [JsonIgnore] public Bitmap StaticImage { get; set; }
        [JsonIgnore] public List<Particule> Particules { get; set; } = new List<Particule>();
        [JsonIgnore] public IState PreviousState { get; set; } = null;
        public static readonly int BlocCount = Core.W / 48;
        [JsonIgnore] public IBloc[,] Blocs { get; set; } = new IBloc[BlocCount, BlocCount];
        [JsonIgnore] public List<TowerUnit> Units { get; set; } = new List<TowerUnit>();
        [JsonIgnore] public List<IUI> UI { get; set; } = new List<IUI>();
        [JsonIgnore] public List<(Point Pt, Point NextLook)> Checkpoints { get; set; } = new List<(Point Pt, Point NextLook)>()
        {
            (new Point(1, 1), new Point(1, 0)),
            (new Point(15, 1), new Point(0, 1)),
            (new Point(15, 13), new Point(-1, 0)),
            (new Point(1, 13), new Point(0, -1)),
            (new Point(1, 3), new Point(1, 0)),
            (new Point(13, 3), new Point(0, 1)),
            (new Point(13, 11), new Point(-1, 0)),
            (new Point(3, 11), new Point(0, -1)),
            (new Point(3, 5), new Point(1, 0)),
            (new Point(11,5), new Point(0, 1)),
            (new Point(11,8), new Point(-1, 0)),
            (new Point(10,8), new Point(0, 0))
        };

        public StateTower()
        {
            StaticImage = new Bitmap(Core.W, Core.H);

            for (int i = 0; i < BlocCount; i++)
            {
                for (int j = 0; j < BlocCount; j++)
                {
                    Blocs[i, j] = new BlocGrass(i, j);
                }
            }

            Blocs[0, 3] = new BlocDirt(0, 3);
            var path = new List<(int x, int y)>(new []
            {
                (0, 1), (15, 1), (15, 13), (1, 13), (1, 3), (13, 3), (13, 11), (3, 11), (3, 5), (11,5), (11,8), (10,8)
            });
            (int x, int y) c, n, min, max;
            for(int i=0; i<path.Count - 1; i++)
            {
                c = path[i];
                n = path[i + 1];
                min = (Math.Min(c.x, n.x), Math.Min(c.y, n.y));
                max = (Math.Max(c.x, n.x), Math.Max(c.y, n.y));
                for (int x = min.x; x <= max.x; x++)
                    for(int y = min.y; y <= max.y; y++)
                        Blocs[x, y] = new BlocDirt(x, y);
            }
            for (int x = 5; x <= 9; x++)
                for (int y = 7; y <= 9; y++)
                        Blocs[x, y] = new BlocStone(x, y);

        }

        public void Update()
        {
        }

        public void DrawStatic()
        {
            foreach (IBloc bloc in Blocs)
                bloc.Draw(StaticImage, TileSz);
        }

        public void Draw()
        {
            // STATIC

            Core.g.DrawImage(StaticImage, 0, 0);

            // ENTITIES

            foreach (TowerUnit e in Units)
                e.Draw();

            // PARTICULES

            foreach (Particule p in Particules)
                p.Draw();

            // UI

            foreach (var ui in UI)
                ui.Draw();

            //Core.g.DrawString(Core.MousePosition.X.ToUnit().ToString(), new Font("Arial", 15F), Brushes.Black, 10, Core.W - 50);
            //Core.g.DrawString(Core.MousePosition.Y.ToUnit().ToString(), new Font("Arial", 15F), Brushes.Black, 50, Core.W - 50);
        }

        public void MouseDown(MouseEventArgs e)
        {
        }

        public void ReturnToPreviousState()
        {
            Data.Instance.State = PreviousState;
            PreviousState = null;
        }
    }
}
