using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Effects;

namespace DOSBOX.Suggestions
{
    public class Road : ISuggestion
    {
        public static Road Instance;
        public bool ShowHowToPlay { get; set; }
        public bg _bg;
        public car _car;
        public List<crate> crates;
        public List<fuelpuddle> fuelpuddles;
        public List<car> cars;
        public List<tree> trees;

        int Score;

        public void HowToPlay()
        {
            int x = 2, y = 2;
            Text.DisplayText("left", x, y, 0); y += 6;
            Text.DisplayText(" ►go left", x, y, 0); y += 8;
            Text.DisplayText("right", x, y, 0); y += 6;
            Text.DisplayText(" ►go right", x, y, 0); y += 8;
            Text.DisplayText("up", x, y, 0); y += 6;
            Text.DisplayText(" ►go up", x, y, 0); y += 8;
            Text.DisplayText("down", x, y, 0); y += 6;
            Text.DisplayText(" ►go down", x, y, 0);

            if (KB.IsKeyDown(KB.Key.Space))
            {
                Graphic.Clear(0, 0);
                ShowHowToPlay = false;
            }
        }

        public void Init()
        {
            Instance = this;

            Score = 0;

            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI

            _bg = new bg();
            _car = new car(32, 32);

            crates = new List<crate>();
            fuelpuddles = new List<fuelpuddle>();
            cars = new List<car>
            {
                new car(0, -48),
                new car(0, -128),
                new car(0, -256),
                new car(0, -272),
                new car(0, -Core.RND.Next(300, 500)),
                new car(0, -Core.RND.Next(520, 888))
            };
            trees = new List<tree>();
        }

        public void Update()
        {
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

            if (_car.crashed)
            {
                _bg.Display(0);
                crates.ForEach(c => c.Display(1, c.SrcLoc));
                trees.ForEach(c => c.Display(1, c.SrcLoc));
                cars.ForEach(c => c.Display(1));
                _car.DisplayCrash(1);
                DisplayUI();

                if (KB.IsKeyDown(KB.Key.Space))
                    Init();

                return;
            }

            Score++;
            if (Score > 9999) Score = 9999;

            if (KB.IsKeyDown(KB.Key.Left))
                _car.vec.x--;

            if (KB.IsKeyDown(KB.Key.Right))
                _car.vec.x++;

            if (KB.IsKeyDown(KB.Key.Up) && _car.vec.y > 0)
                _car.vec.y--;

            if (KB.IsKeyDown(KB.Key.Down) && _car.vec.y + _car._h < 64)
                _car.vec.y++;

            if (_car.spliping > 0.5F)
            {
                _car.vec.y += _car.spliping <= 3F ? (int)_car.spliping : 3;
                _car.spliping *= 0.75F;
            }


            if (Core.RND.Next(8 + 16 * CarPos) == 0)
                crates.Add(new crate(Core.RND.Next(1, _bg.rxs - 1), -4));

            if (CarPos < 4 && Core.RND.Next(8 + 8 * CarPos) == 0)
                fuelpuddles.Add(new fuelpuddle(Core.RND.Next(1, _bg.rxs - 1), -4));

            #region Add New Tree
            if (Core.RND.Next(32) == 16)
            {
                int x = -1;
                if (Core.RND.Next(2) == 0 && _bg.rx[0] > 4)
                {
                    x = Core.RND.Next(0, _bg.rx[0] - 4);
                }
                else
                {
                    int min = _bg.rx[0] + _bg.rxs;
                    int max = 59 - _bg.rx[0] + _bg.rxs;
                    if (min < max)
                    {
                        x = Core.RND.Next(min, max);
                    }
                }
                if (x > -1)
                    trees.Add(new tree(-_bg.rx[0] + x, 0));
            }
            #endregion


            _bg.Update();
            crates.Where(c => c.destroy).ToList().ForEach(c => crates.Remove(c));
            fuelpuddles.ForEach(c => c.Update());
            fuelpuddles.Where(c => c.destroy).ToList().ForEach(c => fuelpuddles.Remove(c));
            crates.ForEach(c => c.Update());
            trees.Where(c => c.destroy).ToList().ForEach(c => trees.Remove(c));
            trees.ForEach(c => c.Update());
            cars.Where(c => c.destroy).ToList().ForEach(c => cars.Remove(c));
            cars.ForEach(c => c.Update());

            _bg.Display(0);
            crates.ForEach(c => c.Display(1, c.SrcLoc));
            fuelpuddles.ForEach(c => c.Display(1, c.SrcLoc));
            trees.ForEach(c => c.Display(1, c.SrcLoc));
            cars.ForEach(c => c.Display(1));
            _car.Display(1);

            DisplayUI();


            Collisions();
        }

        int text_press_space_bounce_value = 8, text_press_space_bounce_sign = 1;
        private void DisplayUI()
        {
            // car position

            Text.DisplaySingleNumber(CarPos, new vec(1, 1), 2);


            // score

            string digits = Score.ToString();
            while (digits.Length > 0)
            {
                Text.DisplaySingleNumber(int.Parse("" + digits.Last()), new vec(60 - (Score.ToString().Length - digits.Length) * 4, 1), 2);
                digits = string.Concat(digits.Take(digits.Length - 1));
            }

            // press Space (crashed)
            if (_car.crashed)
            {
                string text = "press space";
                int w = text.Length * 5;
                int x = 32 - w / 2;
                int y = text_press_space_bounce_value;
                Graphic.DisplayRectAndBounds(x - 2, y - 4, w + 4, 13, 4, 1, 2, 2);
                Text.DisplayText(text, x + 1, y, 2);

                text_press_space_bounce_value += text_press_space_bounce_sign;
                if (text_press_space_bounce_value == 8 || text_press_space_bounce_value == 48)
                    text_press_space_bounce_sign = text_press_space_bounce_sign == 1 ? -1 : 1;
            }
        }

        public int CarPos => cars.Where(c => c.vec.y < _car.vec.x).Count() + 1;

        private void Collisions()
        {
            if (_car.vec.y + _car._h >= 64)
            {
                Crash();
                return;
            }

            var carrec = new Rectangle(_car.vec.x, _car.vec.y, _car._w, _car._h);
            bool iscol(Disp d)
            {
                Rectangle other;
                if ((d is crate && (d as crate).SrcLoc != null) || (d is fuelpuddle && (d as fuelpuddle).SrcLoc != null))
                {
                    var src = (d as crate)?.SrcLoc ?? (d as fuelpuddle)?.SrcLoc;
                    var vec = new vec(src.x + d.vec.x, src.y = d.vec.y);
                    other = new Rectangle(vec.x, vec.y, d._w, d._h);
                }
                else
                    other = new Rectangle(d.vec.x, d.vec.y, d._w, d._h);
                return carrec.IntersectsWith(other);
            }

            int s = 64 / _bg.rxc;
            if (_car.vec.x <= _bg.rx[_car.vec.y / s] || _car.vec.x + _car._w >= _bg.rx[_car.vec.y / s] + _bg.rxs)
            {
                Crash();
                return;
            }

            foreach (var c in cars)
            {
                if (iscol(c))
                {
                    Crash(__car: c);
                    return;
                }
            }

            foreach (var c in crates)
            {
                if (iscol(c))
                {
                    Crash(_crate: c);
                    return;
                }
            }


            // not crash

            foreach (var f in fuelpuddles)
            {
                if (iscol(f))
                {
                    fuelpuddles.Remove(f);
                    _car.spliping = 10;
                    return;
                }
            }
        }

        void Crash(car __car = null, crate _crate = null)
        {
            if (_crate != null)
                crates.Remove(_crate);

            if (__car != null)
                cars.Remove(__car);

            _car.crashed = true;
        }

        public class car : Disp
        {
            public bool destroy = false, crashed = false;
            public float spliping = 0;
            public effect ef_crash = new effect.crash();
            public car(vec v) { CreateGraphics(); vec = v; }
            public car(int x, int y) { CreateGraphics(); vec = new vec(x, y); }

            private void CreateGraphics()
            {
                g = new byte[4, 6]
                {
                { 3, 3, 3, 3, 3, 3 },
                { 3, 2, 1, 1, 1, 3 },
                { 3, 2, 1, 1, 1, 3 },
                { 3, 3, 3, 3, 3, 3 },
                };
                scale = 1;
            }

            float ai_lookx = 0F;
            public void Update()
            {
                vec.y++;

                var rbg = Instance._bg;
                if (vec.y == -_h) vec.x = rbg.rx[0] + Core.RND.Next(1, rbg.rxs - 1);

                if (vec.y >= 0 && vec.y < 64)
                {
                    int s = 64 / rbg.rxc, ofst = 13;
                    float q = 1F, qdec = 0.1F;
                    if (vec.x - rbg.rx[vec.y / s] < ofst) ai_lookx = q;
                    else if (rbg.rx[vec.y / s] + rbg.rxs - vec.x < ofst) ai_lookx = -q;

                    if ((int)(ai_lookx * 10) != 0)
                    {
                        vec.x += (int)ai_lookx;
                        ai_lookx += ai_lookx < 0F ? qdec : -qdec;
                    }
                }
            }

            public void DisplayCrash(int layer)
            {
                ef_crash.Display(layer, d: this);
            }
        }

        public class bg : Disp
        {
            public List<int> rx = new List<int>();
            public int rxc = 16, rxs = 32;
            public int timer_route_change = 0;
            int prevsign = Core.RND.Next(2) == 0 ? -2 : 2;

            public bg() { Init(); CreateGraphics(); }

            private void CreateGraphics()
            {
                g = new byte[64, 64];
                scale = 1;

                int s = 64 / rxc;
                for (int j = 0; j < rxc; j++)
                {
                    for (int n = 0; n < s; n++)
                    {
                        g[63 - rx[j], j * s + n] = 3;
                        if (j % 2 == 0)
                        {
                            g[63 - (rx[j] + rxs / 3), j * s + n] = 1;
                            g[63 - (rx[j] + rxs / 3 * 2), j * s + n] = 1;
                        }
                        g[63 - (rx[j] + rxs), j * s + n] = 3;
                    }
                }
            }

            private void Init()
            {
                for (int i = 0; i < rxc; i++)
                    rx.Add(16);
            }

            public void Update()
            {
                for (int i = rxc - 1; i > 0; i--)
                    rx[i] = rx[i - 1];

                if (timer_route_change >= Core.RND.Next(2, 8) + Road.Instance.CarPos)
                {
                    timer_route_change = 0;
                    int sign;
                    if (Road.Instance.CarPos == 1) sign = Core.RND.Next(2) == 0 ? -4 : 4;
                    if (Road.Instance.CarPos > 3 && Core.RND.Next(10) <= 4) sign = prevsign;
                    else sign = Core.RND.Next(2) == 0 ? -2 : 2;
                    prevsign = sign;

                    rx[0] = rx[0] + sign;
                    if (rx[0] < 1) rx[0] = 1;
                    if (rx[0] + rxs > 62) rx[0] = 62 - rxs;
                }
                else timer_route_change++;

                CreateGraphics();
            }
        }

        public class crate : Disp
        {
            public bool destroy = false;
            public crate(vec v) { CreateGraphics(); vec = v; }
            public crate(int x, int y) { CreateGraphics(); vec = new vec(x, y); }

            public vec SrcLoc => (vec.y / (64 / Road.Instance._bg.rxc) < 0 || vec.y / (64 / Road.Instance._bg.rxc) >= 16) ? null : new vec(Road.Instance._bg.rx[vec.y / (64 / Road.Instance._bg.rxc)], 0);

            private void CreateGraphics()
            {
                g = new byte[4, 4]
                {
                { 3, 2, 2, 3 },
                { 2, 1, 2, 2 },
                { 2, 2, 1, 2 },
                { 3, 2, 2, 3 },
                };
                scale = 1;
            }

            public void Update()
            {
                vec.y++;
                if (vec.y >= 64)
                    destroy = true;
            }
        }

        public class tree : Disp
        {
            public bool destroy = false;
            public tree(vec v) { CreateGraphics(); vec = v; }
            public tree(int x, int y) { CreateGraphics(); vec = new vec(x, y); }

            public vec SrcLoc => (vec.y < 0 || vec.y >= 64) ? null : new vec(Road.Instance._bg.rx[vec.y / (64 / Road.Instance._bg.rxc)], 0);

            private void CreateGraphics()
            {
                g = new byte[4, 6]
                {
                { 0, 1, 1, 0, 0, 0 },
                { 1, 2, 2, 1, 1, 1 },
                { 1, 2, 2, 1, 1, 1 },
                { 0, 1, 1, 0, 0, 0 },
                };
                scale = 1;
            }

            public void Update()
            {
                vec.y++;
                if (vec.y >= 64)
                    destroy = true;
            }
        }

        public class fuelpuddle : Disp
        {
            public bool destroy = false;
            public fuelpuddle(vec v) { CreateGraphics(); vec = v; }
            public fuelpuddle(int x, int y) { CreateGraphics(); vec = new vec(x, y); }

            public vec SrcLoc => (vec.y / (64 / Road.Instance._bg.rxc) < 0 || vec.y / (64 / Road.Instance._bg.rxc) >= 16) ? null : new vec(Road.Instance._bg.rx[vec.y / (64 / Road.Instance._bg.rxc)], 0);

            private void CreateGraphics()
            {
                g = new byte[4, 4]
                {
                { 0, 1, 1, 0 },
                { 2, 2, 2, 1 },
                { 3, 2, 2, 2 },
                { 0, 3, 0, 0 },
                };
                scale = 1;
            }

            public void Update()
            {
                vec.y++;
                if (vec.y >= 64)
                    destroy = true;
            }
        }

    }
}
