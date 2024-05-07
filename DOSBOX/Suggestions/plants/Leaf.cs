using System.Collections.Generic;
using System.Linq;
using Tooling;

namespace DOSBOX.Suggestions
{
    public class Leaf
    {
        public Branch Owner { get; set; }
        public vecf vec { get; set; }

        List<vec> px;

        public Leaf(Branch owner)
        {
            Owner = owner;
            vec = Owner.endvec;
            px = new List<vec>() { new vec(vec.i) };

            CreateGraphics();
        }

        private void CreateGraphics()
        {
            int count = Core.RND.Next(4, 10);

            List<vec> _px = new List<vec>(px);
            while (count > 0)
            {
                _px = px.Except(_px).ToList();
                _px.ForEach(s =>
                {
                    if (Core.RND.Next(1) == 0)
                    {
                        if (!Core.isout(s.x - 1, s.y, 1, Core.Cam) && !px.Any(px => px.x == s.x - 1 && px.y == s.y) && Core.RND.Next(4) == 0) px.Add(new vec(s.x - 1, s.y));
                        if (!Core.isout(s.x + 1, s.y, 1, Core.Cam) && !px.Any(px => px.x == s.x + 1 && px.y == s.y) && Core.RND.Next(4) == 0) px.Add(new vec(s.x + 1, s.y));
                        if (!Core.isout(s.x, s.y - 1, 1, Core.Cam) && !px.Any(px => px.x == s.x && px.y == s.y - 1) && Core.RND.Next(4) == 0) px.Add(new vec(s.x, s.y - 1));
                        if (!Core.isout(s.x, s.y + 1, 1, Core.Cam) && !px.Any(px => px.x == s.x && px.y == s.y + 1) && Core.RND.Next(4) == 0) px.Add(new vec(s.x, s.y + 1));
                    }
                });
                count--;
            }
        }

        public void Update()
        {
        }
        public void Display(int layer)
        {
            foreach (var _px in px)
                if(!Core.isout(_px.x, _px.y, 1))
                    Core.Layers[layer][_px.x, _px.y] = 2;
        }
    }
}
