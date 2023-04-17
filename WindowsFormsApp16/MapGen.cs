using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using WindowsFormsApp16.utilities;
using WindowsFormsApp16.Utilities;
using static WindowsFormsApp16.enums;

namespace WindowsFormsApp16
{
    internal class MapGen
    {
        public static void GenerateTiles()
        {
            var map = Data.Instance.map;
            int prevyh = 10, lvl_dirtygrass, lvl_dirt, lvl_dirtystone = 0, lvl_stone;
            for (int x = 0; x < map.w; x++)
            {
                lvl_dirtygrass = 3 + (Core.RND.Next(40) - 20) / 10;
                lvl_dirt = lvl_dirtygrass + 1;
                lvl_dirtystone += (lvl_dirtystone == 0 ? Core.RND.Next(40, 80) : (Core.RND.Next(41) - 20)) / 10;
                if (lvl_dirtystone < lvl_dirt + 4) lvl_dirtystone = lvl_dirt + 4;
                if (lvl_dirtystone > lvl_dirt + 10) lvl_dirtystone = lvl_dirt + 10;
                lvl_stone = lvl_dirtystone + 1;

                for (int y = prevyh; y < map.h; y++)
                {
                    if (y - prevyh < lvl_dirtygrass)
                    {
                        Data.Instance.map.set(x, y, (byte)TileName.Grass);
                        continue;
                    }
                    if (y - prevyh < lvl_dirt)
                    {
                        Data.Instance.map.set(x, y, (byte)TileName.DirtyGrass);
                        continue;
                    }
                    if (y - prevyh < lvl_dirtystone)
                    {
                        Data.Instance.map.set(x, y, (byte)TileName.Dirt);
                        continue;
                    }
                    if (y - prevyh < lvl_stone)
                    {
                        Data.Instance.map.set(x, y, (byte)TileName.DirtyStone);
                        continue;
                    }
                    Data.Instance.map.set(x, y, (byte)TileName.Stone);
                }

                prevyh = prevyh + (Core.RND.Next(40) - 20) / 10;
                if (prevyh < 2) prevyh = 2;
                if (prevyh > 18) prevyh = 18;
            }
            Data.Instance.cam = new vecf(map.w / 2, 10) * Core.TSZ;




            int ofst = 24;
            int w = map.w, h = map.h - ofst;
            int SEED = (int)DateTime.UtcNow.Ticks;
            int FREQ = 10;
            float SIZE = 0.25F;
            INoise noise = new ValueNoise(SEED, FREQ);
            float[,] arr = new float[w, h];

            //Sample the 2D noise and add it into a array.
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    float fx = x / (w - 1.0f);
                    float fy = y / (h - 1.0f);

                    arr[x, y] = noise.Sample2D(fx, fy);
                }
            }

            //Some of the noises range from -1-1 so normalize the data to 0-1 to make it easier to see.
            NormalizeArray(arr, w, h);



            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (arr[x, y] < SIZE)
                        map.reset(x, ofst + y);
                }
            }
        }

        private static void NormalizeArray(float[,] arr, int w, int h)
        {
            float min = float.PositiveInfinity;
            float max = float.NegativeInfinity;

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {

                    float v = arr[x, y];
                    if (v < min) min = v;
                    if (v > max) max = v;
                }
            }

            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    float v = arr[x, y];
                    arr[x, y] = (v - min) / (max - min);
                }
            }

        }
    }
}
