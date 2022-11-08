using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.RightsManagement;
using System.Windows;
using System.Windows.Forms;
using WindowsFormsApp6.Particules;
using WindowsFormsApp6.UI;
using WindowsFormsApp6.World.Blocs;
using WindowsFormsApp6.World.Entities;
using WindowsFormsApp6.World.Tower;
using FontStyle = System.Drawing.FontStyle;
using Point = System.Drawing.Point;

namespace WindowsFormsApp6.World
{
    public class StateTower : IState
    {
        public class TowerStats
        {
            public Font Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            public long Coins;
            public int HP, DMG, DFF, XPL, SPD;
            public TowerStats()
            {
            }
            public void ReInitialize()
            {
                Coins = 100;
                HP = 10;
                DMG = 1;
                DFF = 1;
                XPL = 0;
                SPD = 1;
            }

            internal void Draw()
            {
                int x = 10, y = Core.W - 60;
                Brush b = Brushes.White;
                void Write(string str)
                {
                    Core.g.DrawString(str, Font, b, x, y);
                    x += (int)Core.g.MeasureString(str, Font).Width + 10;
                }
                Write($"HP {HP}");
                Write($"DMG {DMG}");
                Write($"DFF {DFF}");
                Write($"XPL {XPL}");
                Write($"SPD {SPD}");
                Write($"Coins {Coins}");
            }
        }

        [JsonIgnore] public int TileSz => 48;
        [JsonIgnore] public int TimerAddUnit, TimerAddUnitMax;
        [JsonIgnore] public TowerStats Stats = new TowerStats();
        [JsonIgnore] public Font Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        [JsonIgnore] public Bitmap StaticImage { get; set; }
        [JsonIgnore] public List<Particule> Particules { get; set; } = new List<Particule>();
        [JsonIgnore] public IState PreviousState { get; set; } = null;
        [JsonIgnore] public static readonly int BlocCount = Core.W / 48;
        [JsonIgnore] public IBloc[,] Blocs { get; set; } = new IBloc[BlocCount, BlocCount];
        [JsonIgnore] public List<TowerUnit> Units { get; set; } = new List<TowerUnit>();
        [JsonIgnore] public List<IUI> UI { get; set; } = new List<IUI>();
        [JsonIgnore] public List<Point> Checkpoints { get; set; } = new List<Point>()
        {
            new Point(1, 1),
            new Point(15, 1),
            new Point(15, 13),
            new Point(1, 13),
            new Point(1, 3),
            new Point(13, 3),
            new Point(13, 11),
            new Point(3, 11),
            new Point(3, 5),
            new Point(11,5),
            new Point(11,8),
            new Point(10,8),
        };
        [JsonIgnore] public CardBase[,] Cards = new CardBase[5, 3];
        [JsonIgnore] public List<Bullet> Bullets { get; set; } = new List<Bullet>();

        public void RestartTower()
        {
            Units.Clear();
            Stats.ReInitialize();

            TimerAddUnit = 0;
            TimerAddUnitMax = 200;

            for (int i = 0; i < BlocCount; i++)
            {
                for (int j = 0; j < BlocCount; j++)
                {
                    Blocs[i, j] = new BlocGrass(i, j);
                }
            }

            Blocs[0, 3] = new BlocDirt(0, 3);
            var path = new List<(int x, int y)>(new[]
            {
                (0, 1), (15, 1), (15, 13), (1, 13), (1, 3), (13, 3), (13, 11), (3, 11), (3, 5), (11,5), (11,8), (10,8)
            });
            (int x, int y) c, n, min, max;
            for (int i = 0; i < path.Count - 1; i++)
            {
                c = path[i];
                n = path[i + 1];
                min = (Math.Min(c.x, n.x), Math.Min(c.y, n.y));
                max = (Math.Max(c.x, n.x), Math.Max(c.y, n.y));
                for (int x = min.x; x <= max.x; x++)
                    for (int y = min.y; y <= max.y; y++)
                        Blocs[x, y] = new BlocDirt(x, y);
            }
            for (int x = 5; x <= 9; x++)
                for (int y = 7; y <= 9; y++)
                    Blocs[x, y] = new BlocStone(x, y);
        }

        public StateTower()
        {
            StaticImage = new Bitmap(Core.W, Core.H);
            RestartTower();

            var b = new UIButton("ARROW (10)", Core.W - 120, Core.H - 60);
            b.OnClick += (s, e) => { if (Stats.Coins >= 10 && GetEmptyCardSlot() != (-1, -1)) { Stats.Coins -= 10; AddArrow(); } };
            UI.Add(b);
        }

        private void AddArrow()
        {
            var slot = GetEmptyCardSlot();
            Cards[slot.x, slot.y] = new CardArrow(slot.x, slot.y);
        }
        private (int x, int y) GetEmptyCardSlot()
        {
            for (int x = 0; x < 5; x++)
                for (int y = 0; y < 3; y++)
                    if (Cards[x, y] == null)
                        return (x, y);
            return (-1, -1);
        }

        public TowerUnit GetClosestTowerUnit(int x, int y)
        {
            float d = 0F, closest_d = float.MaxValue;
            TowerUnit target = null;
            foreach(var unit in Units)
            {
                d = Maths.Distance(x, y, unit.X, unit.Y);
                if (d < closest_d)
                {
                    closest_d = d;
                    target = unit;
                }
            }
            return target;
        }
        public void AddBullet(Bullet bullet)
        {
            Bullets.Add(bullet);
        }

        public void Update()
        {
            // Units

            if (TimerAddUnit >= TimerAddUnitMax)
            {
                TimerAddUnit = 0;
                if(TimerAddUnitMax >= 50)
                    TimerAddUnitMax -= 15;
                if (Tools.RND.Next(5) == 2)
                    Units.Add(new TowerUnit(0, 3, gain: 15, hp: 1, dmg: 1, speed: 0.01F, 4));
                else
                    Units.Add(new TowerUnit(0, 1, gain: 10, hp: 1, dmg: 1, speed: 0.01F));
            }
            else TimerAddUnit++;

            var units = new List<TowerUnit>(Units);
            foreach (var unit in units)
            {
                if (unit.HP <= 0)
                {
                    Stats.Coins += unit.Gain;
                    Units.Remove(unit);
                    continue;
                }
                Stats.HP -= unit.Update(Checkpoints);
                if(Stats.HP <= 0)
                {
                    Data.Instance.State = PreviousState;
                    RestartTower();
                }
            }

            // Cards

            foreach (var card in Cards)
                card?.Update(Stats, GetClosestTowerUnit, AddBullet);

            // Bullets

            var bullets = new List<Bullet>(Bullets);
            foreach (var bullet in bullets)
            {
                bullet.Update();
                if (bullet.Destroy)
                    Bullets.Remove(bullet);
            }
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

            // Cards

            foreach (var card in Cards)
                card?.Draw();

            // Bullets

            foreach (var bullet in Bullets)
            {
                bullet.Draw();
            }

            // PARTICULES

            foreach (Particule p in Particules)
                p.Draw();

            // UI

            foreach (var ui in UI)
                ui.Draw();

            //Core.g.DrawString(Core.MousePosition.X.ToUnit().ToString(), new Font("Arial", 15F), Brushes.Black, 10, Core.W - 50);
            //Core.g.DrawString(Core.MousePosition.Y.ToUnit().ToString(), new Font("Arial", 15F), Brushes.Black, 50, Core.W - 50);

            Stats.Draw();
        }

        public void MouseDown(MouseEventArgs e)
        {
            var clickedUI = UI.Where(i => e.X >= i.X && e.X < i.X + i.W && e.Y >= i.Y && e.Y < i.Y + i.H).ToList();
            if (clickedUI.Count > 0)
                clickedUI.ForEach(i => i.Clicked());
        }

        public void ReturnToPreviousState()
        {
            Data.Instance.State = PreviousState;
            PreviousState = null;
        }
    }
}
