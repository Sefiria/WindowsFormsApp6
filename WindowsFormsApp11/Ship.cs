using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml;
using WindowsFormsApp11.Bullets;
using WindowsFormsApp11.Items;

namespace WindowsFormsApp11
{
    public class Ship : Entity
    {
        public float SpeedMove = 5F;
        public float CooldownShot = 0F, CooldownShotThreshold = 50;

        public int hW => W / 2;
        public int hH => H / 2;

        public ModStats ModStats;

        //public ItemPackage ItemPackage = new ItemPackage(new List<Item>());
        public ItemPackage ItemPackage = new ItemPackage(new List<Item>()
        {
        new ItemBlue(10),
        new ItemGreen(10),
        new ItemRed(10),
        new ItemYellow(10),
        new ItemBlack(10),
        new ItemWhite(10)
        });// temp

        public Ship()
        {
            W = 20;
            H = 10;
            X = Var.W / 2 - hW;
            Y = Var.H - 30 - hH;
            ModStats = new ModStats();
        }

        public new void Update()
        {
            ManageKeys();
        }

        public new void Draw()
        {
            Var.g.DrawRectangle(Pens.White, X - hW, Y - hW, W, H);
        }


        private void ManageKeys()
        {
            if (Keyboard.IsKeyDown(Key.Q) && X - hW > 1)
            {
                X -= SpeedMove;
                if (X - hW < 1) X = 1 + hW;
            }

            if (Keyboard.IsKeyDown(Key.D) && X + hW < Var.W - 1)
            {
                X += SpeedMove;
                if (X + hW > Var.W - 1) X = Var.W - 1 - hW;
            }

            if (Keyboard.IsKeyDown(Key.Space))
            {
                if(CooldownShot >= CooldownShotThreshold)
                {
                    CooldownShot = 0F;
                    Shot();
                }
            }
            if(CooldownShot < CooldownShotThreshold)
                CooldownShot++;
        }

        public void Shot()
        {
            for (int i = 0; i < ModStats.X; i++)
            {
                new BulletTiny(X, Y - hH - 1)
                {
                    WaveForce = ModStats.B,
                    WaveAmp = ModStats.C,
                    SpeedMove = ModStats.A,
                    LookX = ModStats.Y <= 0F ? 0F : (ModStats.Y - Var.Rnd.Next((int)(ModStats.Y * 2))) / (ModStats.Y * 2F),
                    LookY = ModStats.Z <= 0F ? -1F : -1F + (ModStats.Z - Var.Rnd.Next((int)(ModStats.Z * 2))) / (ModStats.Z * 2F)
                };
            }
        }

        public void AddLoot(ItemPackage package)
        {
            ItemPackage.AddPackage(package);
        }
        internal void Mod(List<Item> table)
        {   
            int blue = table.FirstOrDefault(x => x.GetType().IsAssignableFrom(typeof(ItemBlue)))?.Count ?? 0;
            int red = table.FirstOrDefault(x => x.GetType().IsAssignableFrom(typeof(ItemRed)))?.Count ?? 0;
            int green = table.FirstOrDefault(x => x.GetType().IsAssignableFrom(typeof(ItemGreen)))?.Count ?? 0;
            int white = table.FirstOrDefault(x => x.GetType().IsAssignableFrom(typeof(ItemWhite)))?.Count ?? 0;
            int yellow = table.FirstOrDefault(x => x.GetType().IsAssignableFrom(typeof(ItemYellow)))?.Count ?? 0;
            int black = table.FirstOrDefault(x => x.GetType().IsAssignableFrom(typeof(ItemBlack)))?.Count ?? 0;

            if (blue > 0) ModStats.ModA(blue);
            if (red > 0) ModStats.ModB(red);
            if (green > 0) ModStats.ModC(green);
            if (white > 0) ModStats.ModX(white);
            if (yellow > 0) ModStats.ModY(yellow);
            if (black > 0) ModStats.ModZ(black);
        }
    }
}
