using System.Drawing;
using System.Drawing.Drawing2D;
using Tooling;

namespace KubeLayers.entities.minable
{
    public class Rock : Entity
    {
        public Rock()
            : base()
        {
            //TexMgt.Tex[""];

            Image = new Bitmap(tsz, tsz);
            using (Graphics g = Graphics.FromImage(Image))
            {
                float s = tsz, hs = tsz / 2F;
                var brush = new SolidBrush(Color.Peru);
                GraphicsPath path = new GraphicsPath();
                path.AddPolygon(new PointF[] {
                    (hs, 0F).P(),
                    (s, s).P(),
                    (0F, s).P(),
                });
                g.FillPath(brush, path);
                g.DrawPath(new Pen(brush), path);
            }
        }

        public override void Dispose()
        {
            Image.Dispose();
        }
    }
}
