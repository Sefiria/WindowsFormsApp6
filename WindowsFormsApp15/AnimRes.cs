using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using WindowsFormsApp15.Properties;
using WindowsFormsApp15.utilities;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.enums;

namespace WindowsFormsApp15
{
    internal class AnimRes
    {

        public static Dictionary<Way, anim> conveyors = new Dictionary<Way, anim>();
        public static Dictionary<Way, Bitmap> arrows = new Dictionary<Way, Bitmap>();
        public static anim drill, furnace;
        public static Dictionary<Ores, Bitmap> ores = new Dictionary<Ores, Bitmap>();
        public static Dictionary<Ores, Bitmap> plates = new Dictionary<Ores, Bitmap>();

        private static void InitConveyors()
        {
            var tileset = Resources.tileset;
            tileset.MakeTransparent(Color.White);
            var tileset_static = Resources.tileset_static;
            tileset_static.MakeTransparent(Color.White);

            var list = new List<Bitmap>();
            var vertical = new List<Bitmap>();
            var horizontal = new List<Bitmap>();
            int sz = 16, targetsz = Core.TSZ;
            int i, j;
            void add(List<Bitmap> to) => to.Add(tileset.Clone(new Rectangle(sz * (i++), sz * j, sz, sz), tileset.PixelFormat).Resize(targetsz));
            for (j = 0; j < 2; j++)
            {
                i = 0;
                add(horizontal);
                add(vertical);
            }
            conveyors[Way.Down] = new anim(0.1F, vertical);
            conveyors[Way.Up] = new anim(0.1F, vertical);
            conveyors[Way.Left] = new anim(0.1F, horizontal);
            conveyors[Way.Right] = new anim(0.1F, horizontal);


            arrows[Way.Up] = tileset_static.Clone(new Rectangle(sz * 0, sz * 0, sz, sz), tileset_static.PixelFormat).Resize(targetsz);
            arrows[Way.Down] = tileset_static.Clone(new Rectangle(sz * 0, sz * 1, sz, sz), tileset_static.PixelFormat).Resize(targetsz);
            arrows[Way.Left] = tileset_static.Clone(new Rectangle(sz * 1, sz * 0, sz, sz), tileset_static.PixelFormat).Resize(targetsz);
            arrows[Way.Right] = tileset_static.Clone(new Rectangle(sz * 1, sz * 1, sz, sz), tileset_static.PixelFormat).Resize(targetsz);



            void addAt(int x, int y) => list.Add(tileset.Clone(new Rectangle(sz * x, sz * y, sz, sz), tileset.PixelFormat).Resize(targetsz));
            void add2At(int x, int y) => list.Add(tileset.Clone(new Rectangle(sz * x, sz * 2 * y, sz, sz * 2), tileset.PixelFormat).Resize(targetsz, targetsz * 2));

            list = new List<Bitmap>();
            addAt(2, 0);
            addAt(2, 1);
            drill = new anim(0.1F, list);
            list = new List<Bitmap>();
            add2At(3, 0);
            add2At(3, 1);
            furnace = new anim(0.1F, list);

            Bitmap get(int x, int y) => tileset_static.Clone(new Rectangle(sz * x, sz * y, sz, sz), tileset_static.PixelFormat).Resize(targetsz);
            void set(ref Dictionary<Ores, Bitmap> dict, int x)
            {
                dict[Ores.Iron] = get(x, 0);
                dict[Ores.Copper] = get(x, 1);
                dict[Ores.Gold] = get(x + 1, 0);
                dict[Ores.Coal] = get(x + 1, 1);
                dict[Ores.Diamond] = get(x + 2, 0);
            }
            set(ref ores, 4);
            set(ref plates, 7);
        }
        public static void Init()
        {
            InitConveyors();
        }
        public static void Tick()
        {
            List<anim> list = new List<anim>(conveyors.Values)
            {
                drill,
                furnace,
            };
            foreach (var anim in list)
                anim.Tick();
        }

        internal static void SetSize(Items item, ref int w, ref int h)
        {
            switch(item)
            {
                case Items.Ore: w = h = 8; break;
            }
        }
    }
}
