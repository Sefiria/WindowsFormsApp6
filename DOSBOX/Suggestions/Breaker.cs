using DOSBOX.Utilities;
using DOSBOX.Utilities.effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Security.RightsManagement;
using System.Windows.Media.Animation;
using static DOSBOX.Suggestions.Road;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace DOSBOX.Suggestions
{
    public class Breaker : ISuggestion
    {
        public static Breaker Instance;
        public bool ShowHowToPlay { get; set; }
        Bar bar;
        List<Ball> balls = new List<Ball>();
        List<Brick> bricks = new List<Brick>();
        List<Powerup> powerups = new List<Powerup>();
        List<Bullet> bullets = new List<Bullet>();
        int level, Score;
        List<List<Brick>> levels = new List<List<Brick>>();

        public void HowToPlay()
        {
            int x = 2, y = 2;
            Text.DisplayText("left", x, y, 0); y += 6;
            Text.DisplayText(" ►go left", x, y, 0); y += 8;
            Text.DisplayText("right", x, y, 0); y += 6;
            Text.DisplayText(" ►go right", x, y, 0); y += 8;
            Text.DisplayText("space", x, y, 0); y += 6;
            Text.DisplayText(" ►shot (when", x, y, 0); y += 6;
            Text.DisplayText("  powerup)", x, y, 0); y += 6;

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
            Core.Layers.Add(new byte[64, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI

            bar = new Bar(32 - 4, 56 - 1);
            Score = 0;

            balls.Clear();

            bullets = new List<Bullet>();
            balls = new List<Ball>();
            bricks = new List<Brick>();
            powerups = new List<Powerup>();

            DefineLevels();
            level = 0;
            LoadLevel(level);
        }

        private void DefineLevels()
        {
            int i = 0;
            levels = new List<List<Brick>>();

            levels.Add(new List<Brick>());
            for (int x = 0; x < 8; x++)
                for (int y = 2; y < 8; y++)
                    if (x != 4)
                        levels[i].Add(new Brick(x * 8, y * 4, 0));
            i++;

            levels.Add(new List<Brick>());
            for (int x = 0; x < 8; x++)
                for (int y = 2; y < 9; y++)
                    if (x != 4 && y != 5)
                        levels[i].Add(new Brick(x * 8, y * 4, 0));
            i++;

            levels.Add(new List<Brick>());
            Brick brick;
            for (int n = 2; n < 8; n++)
            {
                brick = new Brick((n - 2) * 8, n * 4, 0);
                brick.look = new vecf(1F, 0);
                levels[i].Add(brick);
                brick = new Brick(n * 8, n * 4, 0);
                brick.look = new vecf(1F, 0);
                levels[i].Add(brick);
            }
            i++;
        }

        private void LoadLevel(int _level)
        {
            bricks = new List<Brick>(levels[_level]);
            bricks.ForEach(b =>
            {
                if (Core.RND.Next(4) == 0)
                {
                    byte type = (byte) Core.RND.Next(Powerup.GetTypesCount());
                    b.powerup = new Powerup(type);
                }
            });
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


            if (bar.Lifes < 0)
            {
                bar.Display(1);
                bricks.ForEach(b => b.Display(1));
                DisplayUI();

                if (KB.IsKeyDown(KB.Key.Space))
                    Init();

                return;
            }

            if(bricks.Count == 0)
            {
                balls.Clear();
                bar.Lifes++;
                bar.vec = new vecf(32 - 4, 56 - 1);
                level++;
                Score += 50;
                if (Score > 9999) Score = 9999;
                if (level >= levels.Count)
                {
                    // WIN

                    // continue, but from the first level back (keep score)
                    level = 0;
                    LoadLevel(level);
                }
                else
                {
                    LoadLevel(level);
                }
            }

            if (balls.Count == 0)
            {
                if (bar.Lifes > 0)
                    balls.Add(new Ball());
                Instance.bar.Lifes--;
                Instance.bar.LosePowerups();
            }


            bar.Update();
            new List<Ball>(balls).ForEach(b => b.Update());
            new List<Brick>(bricks).ForEach(b => b.Update());
            new List<Powerup>(powerups).ForEach(b => b.Update());
            new List<Bullet>(bullets).ForEach(b => b.Update());

            //debug
            //if (balls.Count > 0)
            //    Core.Layers[0][(int)balls[0].vec.x, (int)balls[0].vec.y] = 3;

            bar.Display(1);
            powerups.ForEach(p => p.Display(1));
            bricks.ForEach(b => b.Display(1));
            bullets.ForEach(b => b.Display(1));
            balls.ForEach(b => b.Display(1));


            DisplayUI();


            Collisions();
        }

        private void DisplayUI()
        {
            // lifes

            Text.DisplaySingleNumber(bar.Lifes, new vec(1, 1), 2);

            // score

            string digits = Score.ToString();
            while (digits.Length > 0)
            {
                Text.DisplaySingleNumber(int.Parse("" + digits.Last()), new vec(60 - (Score.ToString().Length - digits.Length) * 4, 1), 2);
                digits = string.Concat(digits.Take(digits.Length - 1));
            }


            // press Space (no more life)

            if (bar.Lifes < 0)
            {
                string text = "press space";
                int w = text.Length * 5;
                int x = 32 - w / 2;
                int y = 16;
                Graphic.DisplayRectAndBounds(x - 2, y - 4, w + 4, 13, 4, 1, 2, 2);
                Text.DisplayText(text, x + 1, y, 2);
            }
        }

        private void Collisions()
        {
            List<Brick> _bricks;

            balls.ForEach(b =>
            {
                if (b.look != vecf.Zero)
                {
                    b.CalculateBounce(new Box(bar), true);
                    _bricks = new List<Brick>(bricks);
                    foreach (var brick in _bricks)
                    {
                        if (b.CalculateBounce(new Box(brick)))
                        {
                            brick.SetPowerupVecBeforeKilled();
                            bricks.Remove(brick);
                            Score++;
                            if (Score > 9999) Score = 9999;
                            break;
                        }
                    };
                }
            });

            var _powerups = new List<Powerup>(powerups);
            foreach (var powerup in _powerups)
            {
                if (Maths.CollisionBoxBox(new Box(bar), new Box(powerup)))
                {
                    bar.GivePowerup(powerup.type);
                    powerups.Remove(powerup);
                    break;
                }
            };

            _bricks = new List<Brick>(bricks);
            foreach (var brick in _bricks)
            {
                var _bullets = new List<Bullet>(bullets);
                foreach (var bullet in _bullets)
                {
                    if (Maths.CollisionBoxBox(new Box(brick), new Box(bullet)))
                    {
                        bullets.Remove(bullet);
                        brick.SetPowerupVecBeforeKilled();
                        bricks.Remove(brick);
                        Score++;
                        if (Score > 9999) Score = 9999;
                        break;
                    }
                }
            }
        }



        class Bar : Dispf
        {
            public bool prevSuperSized = false;
            public bool SuperSized = false, SuperSpeed = false, SuperShot = false;
            byte timershot = 0;

            public sbyte Lifes = 3;

            public Bar(int x, int y)
            {
                vec = new vecf(x, y);
                CreateGraphics();
            }

            void CreateGraphics()
            {
                g = new byte[SuperSized ? 16 : 8, 2];
                for (int x = 0; x < _w; x++)
                    for (int y = 0; y < _h; y++)
                        g[x, y] = 3;
                scale = 1;
            }

            public void Update()
            {
                if (prevSuperSized != SuperSized)
                {
                    CreateGraphics();
                    prevSuperSized = SuperSized;
                }

                float speed = SuperSpeed ? 4F : 2F;
                if (KB.IsKeyDown(KB.Key.Left))
                {
                    vec.x -= speed;
                    if (vec.x < 0) vec.x = 0;
                }
                if (KB.IsKeyDown(KB.Key.Right))
                {
                    vec.x += speed;
                    if (vec.x + _w > 63) vec.x = 64 - _w;
                }

                if (KB.IsKeyPressed(KB.Key.Space))
                {
                    Instance.balls.ForEach(b =>
                    {
                        if (b.look == vecf.Zero)
                            b.look = new vecf((b.vec.x + b._w / 2 - (vec.x + _w / 2)) / _w, -1);
                    });

                    if (SuperShot)
                    {
                        if (timershot == 0)
                        {
                            timershot = 10;
                            Shot();
                        }
                    }
                }
                if (timershot > 0)
                    timershot--;
            }

            void Shot()
            {
                Instance.bullets.Add(new Bullet(vec.x + _w / 2F - 1F, vec.y - 4F));
            }
            public void GivePowerup(byte type)
            {
                switch(type)
                {
                    case 0: SuperSized = true; break;
                    case 1: SuperSpeed = true; break;
                    case 2: new List<Ball>(Instance.balls).ForEach(b =>
                    {
                        // split each ball by 2
                        var newball = new Ball(b.vec.x + b._w / 2F, b.vec.y);
                        newball.look = new vecf(Core.RND.Next(21) / 10F - 1.0F, Core.RND.Next(21) / 10F - 1.0F);
                        Instance.balls.Add(newball);
                        b.vec = new vecf(b.vec.x - b._w / 2F, b.vec.y);
                        b.look = new vecf(Core.RND.Next(21) / 10F - 1.0F, Core.RND.Next(21) / 10F - 1.0F);
                    }); break;
                    case 3: SuperShot = true; break;
                }
            }
            public void LosePowerups()
            {
                SuperSized = SuperSpeed = SuperShot = false;
            }
        }

        class Ball : Dispf
        {
            public vecf prev_vec;
            public vecf look = vecf.Zero;
            public float speed = 1F;
            public float initofst = 0F;

            public Ball()
            {
                prev_vec = vec = vecf.Zero;
                CreateGraphics();
            }
            public Ball(float x, float y)
            {
                prev_vec = vec = new vecf(x, y);
                CreateGraphics();
            }

            void CreateGraphics()
            {
                g = new byte[2, 2]
                {
                    { 2, 2 },
                    { 2, 2 },
                };
                scale = 1;

                initofst = Core.RND.Next(2) == 0 ? _w : -_w;
            }

            public void Update()
            {
                if (look == vecf.Zero)
                    prev_vec = vec = new vecf(Instance.bar.vec.x + Instance.bar._w / 2F - _h / 2F + initofst, Instance.bar.vec.y - _h);

                vec.x += look.x * speed;
                vec.y += look.y * speed;

                if (vec.x < 0)
                {
                    vec.x = 0F;
                    look.x *= -1F;
                }

                if (vec.x + _w > 64)
                {
                    vec.x = 64 - _w;
                    look.x *= -1F;
                }

                if (vec.y <= 0)
                {
                    vec.y = 0F;
                    look.y *= -1F;
                }

                if (vec.y + _h > 64)
                    Instance.balls.Remove(this);
            }
            public bool CalculateBounce(Box box, bool relative_bounce = false)
            {
                bool inside = false;
                Circle C = new Circle(this);
                int minx = Math.Min(0, (int)(vec.x - prev_vec.x)), maxx = Math.Max(0, (int)(vec.x - prev_vec.x));
                int miny = Math.Min(0, (int)(vec.y - prev_vec.y)), maxy = Math.Max(0, (int)(vec.y - prev_vec.y));
                for (int x = minx; x <= maxx; x++)
                {
                    for (int y = miny; y <= maxy; y++)
                    {
                        C.x = (int)prev_vec.x + x; C.y = (int)prev_vec.y + y;
                        int c = Maths.CollisionCercleBox(C, box);
                        switch (c)
                        {
                            default:
                            case 0: continue;// no collision
                            case 1: look.x *= -1F; if (relative_bounce) look.y *= -1F; break;// corner top-left
                            case 2: look.x *= -1F; if (relative_bounce) look.y *= 1F; break;// corner bottom-left
                            case 3: look.x *= -1F; if (relative_bounce) look.y *= -1F; break;// corner top-right
                            case 4: look.x *= -1F; if (relative_bounce) look.y *= 1F; break;// corner bottom-right
                            case 5: inside = true; break;// one inside the other
                            case 6: look.y = -1F; if (relative_bounce) look.x = (vec.x + _w / 2 - (box.x + box.w / 2)) / box.w; break;// segment top
                            case 7: look.x *= -1F; break;// segment right
                            case 8: look.y *= -1F; break;// segment bottom
                            case 9: look.x *= -1F; break;// segment left
                        }
                        return true;// collision
                    }
                }
                if(inside)
                {
                    if(C.x < box.x + box.w / 2)
                    {
                        if (C.y < box.y + box.h / 2)
                        {
                            if (box.x + box.w / 2 - C.x > box.y + box.h / 2 - C.y)
                                look.x *= -1;
                            else
                                look.y *= -1;
                        }
                        else
                        {
                            if (box.x + box.w / 2 - C.x > C.y - (box.y + box.h / 2))
                                look.x *= -1;
                            else
                                look.y *= -1;
                        }
                    }
                    else
                    {
                        if (C.y < box.y + box.h / 2)
                        {
                            if (C.x - (box.x + box.w / 2) > box.y + box.h / 2 - C.y)
                                look.x *= -1;
                            else
                                look.y *= -1;
                        }
                        else
                        {
                            if (C.x - (box.x + box.w / 2) > C.y - (box.y + box.h / 2))
                                look.x *= -1;
                            else
                                look.y *= -1;
                        }
                    }
                }
                return false;// no collision
            }
        }
        class Brick : Dispf
        {
            byte type;
            public vecf look = vecf.Zero;
            public Powerup powerup = null;

            public Brick(int x, int y, byte type)
            {
                vec = new vecf(x, y);
                this.type = type;
                CreateGraphics();
            }

            void CreateGraphics()
            {
                scale = 1;
                switch (type)
                {
                    default:
                    case 0:
                        g = new byte[8, 4];

                        for (int x = 0; x < _w; x++)
                            for (int y = 0; y < _h; y++)
                                g[x, y] = 1;

                        for (int x = 0; x < _w; x++)
                        {
                            g[x, 0] = 2;
                            g[x, _h - 1] = 2;
                        }
                        for (int y = 0; y < _h; y++)
                        {
                            g[0, y] = 2;
                            g[_w - 1, y] = 2;
                        }
                        break;

                    case 1:
                        g = new byte[4, 32];

                        for (int x = 0; x < _w; x++)
                            for (int y = 0; y < _h; y++)
                                g[x, y] = 1;

                        for (int x = 0; x < _w; x++)
                        {
                            g[x, 0] = 2;
                            g[x, _h - 1] = 2;
                        }
                        for (int y = 0; y < _h; y++)
                        {
                            g[0, y] = 2;
                            g[_w - 1, y] = 2;
                        }
                        break;
                }
            }

            public void Update()
            {
                if (look == vecf.Zero)
                    return;

                vec.x += look.x;
                vec.y += look.y;

                if (vec.x < 0)
                {
                    vec.x = 0F;
                    look.x *= -1F;
                }

                if (vec.x + _w > 64)
                {
                    vec.x = 64 - _w;
                    look.x *= -1F;
                }
            }

            public void SetPowerupVecBeforeKilled()
            {
                if (powerup == null)
                    return;

                (int w, int h) = Powerup.GetSizeFromType(type);
                powerup.vec = new vecf(vec.x + _w / 2F - w / 2F, vec.y + _h / 2F - h / 2F);
                powerup.hidden = false;
                Instance.powerups.Add(powerup);
            }
        }
        class Powerup : Dispf
        {
            public byte type;
            public bool hidden = true;

            public Powerup(byte type)
            {
                this.type = type;
                CreateGraphics();
            }

            void CreateGraphics()
            {
                scale = 1;
                switch (type)
                {
                    default:
                    case 0: g = Graphic.GetGradientVertical(6, 3, new List<byte> { 1, 2, 1 }); break;
                    case 1: g = Graphic.GetGradientHorizontal(6, 3, new List<byte> { 1, 2, 3, 3, 2, 1 }); break;
                    case 2:
                        g = new byte[4, 4]
                        {
                            { 0, 1, 1, 0 },
                            { 1, 2, 2, 1 },
                            { 1, 2, 2, 1 },
                            { 0, 1, 1, 0 },
                        };
                        break;
                    case 3:
                        g = new byte[4, 4]
                        {
                            { 0, 1, 1, 0 },
                            { 1, 2, 2, 1 },
                            { 1, 2, 2, 1 },
                            { 2, 3, 3, 2 },
                        };
                        break;
                }
            }
            public static int GetTypesCount() => 4;
            public static (int w, int h) GetSizeFromType(byte _type)
            {
                switch (_type)
                {
                    default:
                    case 0: return (6, 3);
                    case 1: return (6, 3);
                    case 2: return (4, 4);
                    case 3: return (4, 4);
                }
            }

            public void Update()
            {
                if (hidden)
                    return;

                vec.y++;

                if (vec.y >= 64)
                    Instance.powerups.Remove(this);
            }
        }
        class Bullet : Dispf
        {
            public Bullet(float x, float y)
            {
                vec = new vecf(x, y);
                CreateGraphics();
            }

            void CreateGraphics()
            {
                g = new byte[2, 4]
                {
                    { 3, 3, 3, 3 },
                    { 3, 3, 3, 3 },
                };
                scale = 1;
            }
            public static int GetTypesCount() => 4;
            public static (int w, int h) GetSizeFromType(byte _type)
            {
                switch (_type)
                {
                    default:
                    case 0: return (6, 3);
                    case 1: return (6, 3);
                    case 2: return (4, 4);
                    case 3: return (4, 4);
                }
            }

            public void Update()
            {
                vec.y--;

                if (vec.y + _h < 0)
                    Instance.bullets.Remove(this);
            }
        }
    }
}
