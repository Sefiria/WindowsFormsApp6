using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace WindowsFormsApp6.Particules
{
    public class Particule
    {
        public float X, Y, VecX, VecY;
        public bool Destroy = false;

        [JsonIgnore] private Bitmap Image;
        private int LifeTime = 30;

        public Particule(int x, int y, Bitmap image)
        {
            X = x;
            Y = y;
            VecX = Tools.RND.Next(-100, 100) / 1000F;
            VecY = Tools.RND.Next(-100, 100) / 1000F;
            Image = SmallImg(image);
            Data.Instance.State.Particules.Add(this);
        }
        public Particule(int x, int y, float vecX, float vecY, Bitmap image)
        {
            X = x;
            Y = y;
            VecX = vecX;
            VecY = vecY;
            Image = SmallImg(image);
            Data.Instance.State.Particules.Add(this);
        }

        private Bitmap SmallImg(Bitmap img)
        {
            Bitmap smallimg = new Bitmap(img.Width / 2, img.Height / 2);
            using (Graphics g = Graphics.FromImage(smallimg))
                g.DrawImage(img, Tools.RND.Next(img.Width / 2), Tools.RND.Next(img.Height / 2), img.Width / 2, img.Height / 2);
            return smallimg;
        }

        public void Update(float gravity = 0.2F)
        {
            X += VecX;
            Y += VecY;
            VecY += gravity;

            LifeTime--;
            if(LifeTime <= 0)
                Destroy = true;
        }

        public void Draw(Bitmap image)
        {
            using (Graphics g = Graphics.FromImage(image))
                g.DrawImage(Image, X, Y);
        }
        public void Draw()
        {
            Core.g.DrawImage(Image, X, Y);
        }

        public static List<Particule> RangeRND(int x, int y, int count, Bitmap img, int lifeTime = 30)
        {
            var result = new List<Particule>();
            Particule p;
            for (int i = 0; i < count; i++)
            {
                p = new Particule(x, y, img);
                p.LifeTime = lifeTime;
                result.Add(p);
            }
            return result;
        }
    }
}
