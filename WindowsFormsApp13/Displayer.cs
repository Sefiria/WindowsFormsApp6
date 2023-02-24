using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace WindowsFormsApp13
{
    public class LightPx
    {
        public vec px;
        public float fade = 255F;
        public int c;
    }

    internal class Displayer
    {
        static List<LightPx> lightFading = new List<LightPx>();
        static int lightFadingMax = 500;

        public static void Display(Graphics g, Map m, Cam c, vecf _look = null)
        {
            Bitmap mimg = new Bitmap(m.Image);
            using(Graphics _g = Graphics.FromImage(mimg))
                Core.CamHide.Draw(_g, Color.Red);

            float ofst = Core.hw - m.w / 2F;
            var look = _look ?? new vecf(c.look);

            if(_look == null)
                g.FillRectangle(Brushes.Black, ofst, ofst, m.w, m.h);

            vecf s = new vecf(c.vec);
            for (float fov = -c.fov; fov <= c.fov; fov += 2)
            {
                vecf fovlook = look.Rotate(fov);
                vecf hit = Maths.Raycast(s, fovlook, mimg, Color.Black);
                vecf e = Maths.GetRaycastLine(s, fovlook, 0, 0, m.w, m.h);
                g.DrawLine(new Pen(Color.FromArgb(8, Color.White), 24F), ofst + s.x, ofst + s.y, ofst + (hit != vecf.Zero ? hit.x : e.x), ofst + (hit != vecf.Zero ? hit.y : e.y));

                if (hit != vecf.Zero)
                    Light(g, mimg, hit);
            }

            Bitmap img = new Bitmap((int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height);
            List<LightPx> list = new List<LightPx>(lightFading);
            foreach (LightPx light in list)
            {
                if (light.c == Color.Red.ToArgb())
                {
                    if (Core.CamHide.IsSuperHidden())
                    {
                        lightFading.Remove(light);
                        continue;
                    }
                    else
                        Core.CamHide.found = true;
                }
                img.SetPixel(light.px.x, light.px.y, Color.FromArgb((byte)light.fade, Color.FromArgb(light.c)));
                light.fade -= 16F;
                if (light.fade <= 0.5F) lightFading.Remove(light);
            }

            Core.Cam.Draw(g, Color.Yellow);

            img.MakeTransparent(Color.Black);
            g.DrawImage(img, ofst, ofst);
        }
        private static void Light(Graphics g, Bitmap img, vecf hit)
        {
            int w = (int)g.VisibleClipBounds.Width;
            int h = (int)g.VisibleClipBounds.Height;
            int c = Color.Black.ToArgb();
            int max = 8;
            List<vecf> justadded = new List<vecf>();
            void check(int x, int y, int far)
            {
                if (justadded.Any(px => px.x == x && px.y == y))
                    return;

                if(x < 0 || y < 0 || x >= w || y >= h)
                    return;

                var mc = img.GetPixel(x, y).ToArgb();
                if (c != mc)
                {
                    var already = lightFading.FirstOrDefault(l => l.px.x == x && l.px.y == y);
                    if (already != null)
                        already.fade = 255F;
                    else
                    {
                        if (lightFading.Count == lightFadingMax)
                            lightFading.RemoveAt(0);
                        lightFading.Add(new LightPx { px = new vec(x, y), c = mc });
                    }
                    justadded.Add(new vecf(x, y));

                    if (far + 1 == max)
                        return;

                    check(x - 1, y, far + 1);
                    check(x + 1, y, far + 1);
                    check(x, y - 1, far + 1);
                    check(x, y + 1, far + 1);
                    check(x - 1, y - 1, far + 1);
                    check(x - 1, y + 1, far + 1);
                    check(x + 1, y - 1, far + 1);
                    check(x + 1, y + 1, far + 1);
                }
            }
            check((int)hit.x, (int)hit.y, 0);
        }

        public static void Display_V1(Graphics g, Map m, Cam c)
        {
            float ofst = Core.iRW - m.w / 2F;
            var look = new vecf(c.look);

            //var sl= look.Rotate(-c.fov);
            //var el= look.Rotate(c.fov);
            //vecf s = new vecf(c.vec.x + sl.x * m.hw, c.vec.y + sl.y * m.hh);
            //vecf e = new vecf(c.vec.x + el.x * m.hw, c.vec.y + el.y * m.hh);
            //vecf vfov = new vecf(e.x - s.x, e.y - s.y);

            g.DrawImage(m.Image, ofst, ofst);

            vecf s = new vecf(c.vec);
            vecf hit = vecf.Zero;
            for (float fov = -c.fov; fov < c.fov; fov++)
            {
                vecf fovlook = look.Rotate(fov);
                vecf e = Maths.GetRaycastLine(s, fovlook, 0, 0, m.w, m.h);
                hit = Maths.Raycast(s, fovlook, m.Image, Color.Black);
                g.DrawLine(Pens.Cyan, ofst + s.x, ofst + s.y, ofst + (hit != vecf.Zero ? hit.x : e.x), ofst + (hit != vecf.Zero ? hit.y : e.y));
            }
        }
    }
}
