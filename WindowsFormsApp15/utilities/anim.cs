using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using WindowsFormsApp15.utilities;

namespace WindowsFormsApp15.Utilities
{
    public class anim
    {
        public anim(float incr, List<Bitmap> g, bool pingpong = false)
        {
            this.g = g;
            this.incr = incr;
            this.pingpong = pingpong;
        }
        public anim(float incr, Bitmap tosplitvertically, bool pingpong = false)
        {
            g = tosplitvertically.SplitVertically(Core.TSZ).Resize(Core.TSZ);
            this.incr = incr;
            this.pingpong = pingpong;
        }
        public List<Bitmap> g = new List<Bitmap>();
        public int scale = 1;
        public bool pingpong = false;
        bool reverse = false;
        float frame = 0F;
        int framemax => g.Count;
        public float incr { get; set; } = 0.1F;
        public int w(int frame) => getlength(frame, true);
        public int h(int frame) => getlength(frame, false);
        int getlength(int frame, bool iswidth) => (frame < 0 || frame >= g.Count) ? 0 : (iswidth ? g[frame].Width : g[frame].Height);

        public void Tick()
        {
            if (pingpong)
            {
                frame += (reverse ? -1F : 1F) * incr;

                if ((reverse && (int)frame <= 0) || (!reverse && (int)frame >= framemax))
                {
                    frame = reverse ? 0 : framemax - 1;
                    reverse = !reverse;
                }
            }
            else
            {
                frame += incr;
                while ((int)frame >= framemax) frame -= framemax;
            }
        }

        public void Display(vecf vf)
        {
            Core.g.DrawImage(g[(int)frame], vf.x, vf.y);
        }
    }
}
