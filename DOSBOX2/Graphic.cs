﻿using System;
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

        public static readonly int resolution = 480, pixel_size = 4;
        public static readonly int screen_size = resolution / pixel_size;
        public static byte[,] pixels = new byte[resolution, resolution];
        public static Config config;
        public static Bitmap Bitmap = new Bitmap(resolution, resolution);
        public static Graphics g = Graphics.FromImage(Bitmap);

        private static List<vec> refresh_indexes = new List<vec>();

        public static bool isout(int x, int y) => x < 0 || y < 0 || x >= resolution || y >= resolution;
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
        public static void SetBatch(byte[,] batch, int x, int y, bool reverse = false)
        {
            int w = batch.GetLength(0);
            int h = batch.GetLength(1);
            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    Set(reverse ? x + w - i : x + i, y + j, batch[i, j]);
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
        public static void FillRect(byte color_index, int x, int y, int w, int h)
        {
            for (int i = x; i <= x + w; i++)
                for (int j = y; j <= y + h; j++)
                    Set(i, j, color_index);
        }
        public static void FillDrawRect(byte color_index_content, byte color_index_bounds, int x, int y, int w, int h)
        {
            FillRect(color_index_content, x, y, w, h);
            DrawRect(color_index_bounds, x, y, w, h);
        }
    }
}
