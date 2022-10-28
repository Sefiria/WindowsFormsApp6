using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using WindowsFormsApp3.Entities.Weapons;
using WindowsFormsApp3.Interfaces;
using WindowsFormsApp3.Properties;

namespace WindowsFormsApp3.Entities
{
    public class Player : DrawableEntity, IRP
    {
        #region Variables
        public int STRMax { get; set; } = 5;
        public int STR { get; set; } = 5;
        public int HPMax { get; set; } = 10;
        public int HP { get; set; } = 10;
        public const float Speed = 2F;
        public bool MouseLeftDown = false;
        public Dictionary<Key, bool> KeysUp { get; set; } = new Dictionary<Key, bool>()
        {
            [Key.LeftCtrl] = true,
        };
        public Stack<Items> AddItemsAnimStack { get; set; } = new Stack<Items>();
        public List<Weapon> Weapons = new List<Weapon>();
        List<Timer> TimersShotLevels = new List<Timer>() {
            new Timer(){ Enabled = true, Interval = 2000 },
            new Timer(){ Enabled = true, Interval = 1700 },
            new Timer(){ Enabled = true, Interval = 1400 },
            new Timer(){ Enabled = true, Interval = 1100 },
            new Timer(){ Enabled = true, Interval =  800 }
        };

        bool ShowInventory = false;
        #endregion

        #region Ctor
        public Player(float X = 0, float Y = 0)
        {
            TimersShotLevels[0].Tick += ShotLevel1;

            this.X = X;
            this.Y = Y;
            Image = Resources.mainchar;
            Image.MakeTransparent();
            Weapons.Add(new Gun(this));
        }

        private void ShotLevel1(object sender, System.EventArgs e)
        {
            Weapons.Where(x => x.Level.Value == 1).ToList().ForEach(x => x.Action());
        }
        private void ShotLevel2(object sender, System.EventArgs e)
        {
            Weapons.Where(x => x.Level.Value == 2).ToList().ForEach(x => x.Action());
        }
        private void ShotLevel3(object sender, System.EventArgs e)
        {
            Weapons.Where(x => x.Level.Value == 3).ToList().ForEach(x => x.Action());
        }
        private void ShotLevel4(object sender, System.EventArgs e)
        {
            Weapons.Where(x => x.Level.Value == 4).ToList().ForEach(x => x.Action());
        }
        private void ShotLevel5(object sender, System.EventArgs e)
        {
            Weapons.Where(x => x.Level.Value == 5).ToList().ForEach(x => x.Action());
        }
        #endregion

        #region Draw
        public override void Draw()
        {
            SharedCore.g.DrawImage(Image, X - W / 2F, Y - H / 2F);

            DrawHPBar();

            DrawInventory();
        }
        private void DrawHPBar()
        {
            var g = SharedCore.g;
            float w = 20F, h = 5F;
            float x = X + W / 2F - w / 2F, y = Y + H + 5F;
            g.FillRectangle(Brushes.Red, x, y, w / (HP / (float)HPMax), h);
            g.DrawRectangle(Pens.White, x, y, w / (HP / (float)HPMax), h);
        }
        private void DrawInventory()
        {
            if (!ShowInventory)
                return;

            var g = SharedCore.g;
            var font = SharedCore.Font;
            var boxX = SharedCore.RenderW * 0.1F;
            var boxY = SharedCore.RenderH * 0.1F;
            var boxW = SharedCore.RenderW * 0.8F;
            var boxH = SharedCore.RenderH * 0.6F;
            g.FillRectangle(new SolidBrush(Color.FromArgb(150, 50, 50, 50)), boxX, boxY, boxW, boxH);
            g.DrawRectangle(new Pen(Color.FromArgb(150, 80, 80, 80), 5F), boxX, boxY, boxW, boxH);

            // TODO
        }
        #endregion

        #region Update
        public override void Update()
        {
            Inputs();

            if (MouseLeftDown)
            {
                PointF ms = SharedCore.MouseLocation;
                Look = Maths.GetLook(ms.X, ms.Y, X, Y);
            }
            if (Look != Vecf.Empty)
            {
                X += Look.X * Speed;
                Y += Look.Y * Speed;
            }
        }
        public void MouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            MouseLeftDown = true;
        }
        public void MouseMove(System.Windows.Forms.MouseEventArgs e)
        {
        }
        public void MouseUp()
        {
            Look = new Vecf();
            MouseLeftDown = false;
        }
        public void MouseLeave()
        {
            Look = new Vecf();
            MouseLeftDown = false;
        }
        private void Inputs()
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && KeysUp[Key.LeftCtrl])
            {
                ShowInventory = !ShowInventory;
            }

            var dict = new Dictionary<Key, bool>(KeysUp);
            foreach (var kv in dict)
                KeysUp[kv.Key] = !Keyboard.IsKeyDown(kv.Key);
        }
        public void Hit(IRP From)
        {
            HP -= From.STR;
            if (HP <= 0)
            {
                HP = 0;
                Exists = false;
            }
        }
        #endregion
    }
}
