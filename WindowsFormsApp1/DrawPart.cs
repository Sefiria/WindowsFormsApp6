using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp1.Entities;
using WindowsFormsApp1.Plants;

namespace WindowsFormsApp1
{
    public static class DrawPart
    {
        public static void Draw(MainForm form)
        {
            SharedCore.g.Clear(Color.Black);

            //for (int x = 0; x < form.Width; x += SharedCore.TileSize)
            //    SharedCore.g.DrawLine(Pens.Cyan, x, 0, x, form.Height);
            //for (int y = 0; y < form.Height; y += SharedCore.TileSize)
            //    SharedCore.g.DrawLine(Pens.Cyan, 0, y, form.Width, y);

            var list = new List<DrawableEntity>(SharedData.Entities);
            foreach(var entity in list)
            {
                if (entity is Player)
                    continue;

                if(entity.Exists)
                    DrawOne(entity);
            }

            SharedData.Entities.First(x => x is Player && x.Exists).Draw();

            form.Render.Image = form.Img;
        }

        private static void DrawOne(DrawableEntity e)
        {
            e.Draw();
        }
        private static Point GetPoint(IDraw e) => new Point((int)e.X, (int)e.Y);
    }
}
