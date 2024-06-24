using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Tooling;

namespace DOSBOX2
{
    public class Graphic
    {
        public class Config
        {
            public Palette palette;
        }

        public enum BatchMode
        {
            Raw=0, HumanReadable, Reset
        }

        public static readonly int screen_size = 480, pixel_size = 4;
        public static readonly int resolution = screen_size / pixel_size;
        public static byte[,] pixels = new byte[screen_size, screen_size];
        public static Config config;
        public static Bitmap Bitmap = new Bitmap(screen_size, screen_size);
        public static Graphics g = Graphics.FromImage(Bitmap);

        private static List<vec> refresh_indexes = new List<vec>();

        public static bool isout(int x, int y) => x < 0 || y < 0 || x >= screen_size || y >= screen_size;
        public static bool isin(int x, int y) => !isout(x,y);

        public static void Initialize(Config config)
        {
            Graphic.config = config;
        }

        public static void Clear(byte v)
        {
            pixels.SetEach((xy, px) => v);
            Refresh();
        }
        public static void Refresh()
        {
            for (int x = 0; x < resolution; x++)
                for (int y = 0; y < resolution; y++)
                    refresh_indexes.Add((x, y).V());
        }
        public static void Set(int x, int y, byte v)
        {
            if (isout(x, y) || pixels[x, y] == v) return;
            refresh_indexes.Add((x, y).V());
            pixels[x, y] = v;
        }
        public static void SetBatch(byte[,] batch, int x, int y, BatchMode mode = BatchMode.Raw, bool reverse = false)
        {
            int w, h, i, j;


            w = batch.GetLength(0);
            h = batch.GetLength(1);

            switch (mode)
            {

                case BatchMode.Raw:
                    for (i = 0; i < w; i++)
                        for (j = 0; j < h; j++)
                            Set(reverse ? x + w - i : x + i, y + j, batch[i, j]);
                    break;

                case BatchMode.HumanReadable:
                    for (i = 0; i < w; i++)
                        for (j = 0; j < h; j++)
                            Set(reverse ? x + w - i : x + i, y + j, batch[i, j]);
                    break;

                case BatchMode.Reset:
                    for (i = 0; i < w; i++)
                        for (j = 0; j < h; j++)
                            Set(reverse ? x + w - i : x + i, y + j, 3);
                    break;

            }
        }

        public static void Draw()
        {
            if (refresh_indexes.Count == 0) return;
            int sz = pixel_size;
            foreach (vec xy in refresh_indexes)
                g.FillRectangle(new SolidBrush(config.palette[pixels[xy.x, xy.y]]), xy.x * sz, xy.y * sz, sz, sz);
            refresh_indexes.Clear();
        }
        public static Bitmap GetBitmap() => Bitmap;

        public static void DrawRectAlt(byte color_index, int x, int y, int x2, int y2) => DrawRect(color_index, x, y, x2-x, y2-y);
        public static void DrawRect(byte color_index, int x, int y, int w, int h)
        {
            for (int i = x; i <= x + w; i++)
            {
                Set(i, y, color_index);
                Set(i, y+h, color_index);
            }
            for (int j = y; j <= y + h; j++)
            {
                Set(x, j, color_index);
                Set(x+w, j, color_index);
            }
        }
        public static void DrawCircle(byte color_index, int x, int y, int radius)
        {
            for (int angle = 0; angle < 360; angle++)
            {
                int i = (int)(radius * Math.Cos(angle * Math.PI / 180));
                int j = (int)(radius * Math.Sin(angle * Math.PI / 180));
                Set(x + i, y + j, color_index);
            }
        }


        public static void FillRect(byte color_index, int x, int y, int w, int h)
        {
            for (int i = x; i <= x + w; i++)
                for (int j = y; j <= y + h; j++)
                    Set(i, j, color_index);
        }
        public static void FillCircle(byte color_index, int x, int y, int radius)
        {
            for (int i = -radius; i <= radius; i++)
                for (int j = -radius; j <= radius; j++)
                    if (i * i + j * j <= radius * radius)
                        Set(x + i, y + j, color_index);
        }

        public static void FillDrawRect(byte color_index_content, byte color_index_bounds, int x, int y, int w, int h)
        {
            FillRect(color_index_content, x, y, w, h);
            DrawRect(color_index_bounds, x, y, w, h);
        }
    }
}
