﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;

namespace console_v2
{
    internal class GraphicsManager
    {
        public struct RGB
        {
            public byte R, G, B;
            public RGB(int rgb)
            {
                var bytes = BitConverter.GetBytes(rgb);
                R = bytes[0];
                G = bytes[1];
                B = bytes[2];
            }
            public static implicit operator RGB(int d) => new RGB(d);
        }

        public static Font Font = new Font("Courrier New", 24f, FontStyle.Regular);
        public static Size ConsoleCharSize = new Size(8, 17);//new Size(8, 8);
        public static RGB[] Palette = Defaults.Palette;
        public static SolidBrush[] PaletteBrushes = Palette.Select(x => new SolidBrush(Color.FromArgb(x.R, x.G, x.B))).ToArray();
        public static SizeF CharSize = new SizeF(1,1);
        public static int TileWidth = (int)CharSize.Width;
        public static int TileHeight = (int)CharSize.Height;

        public static void DrawTile(Graphics g, Tile tile, vec coordinates)
        {
            var rw = Core.Instance.ScreenWidth;
            var rh = Core.Instance.ScreenHeight;
            var tw = TileWidth;
            var th = TileHeight;
            var bounds = new Rectangle(- rw / 2 - tw, - rh / 2 - th, rw + tw * 2, rh + th * 2);
            if (bounds.Contains(coordinates.ipt))
            {
                DrawString(g, string.Concat((char)DB.Resources[(int)tile.Sol]), Brushes.DimGray, coordinates);
            }
        }
        public static void DrawString(Graphics g, string text, Brush brush, vec v) => DrawString(g, text, brush, v.f);
        public static void DrawString(Graphics g, string text, Brush brush, vecf v)
        {
            for (int i = 0; i < text.Length; i++)
            {
                int y = i / Chunk.ChunkSize.x;
                float x = i * CharSize.Width - y * CharSize.Width * Chunk.ChunkSize.x;
                vecf vf = (x, y * CharSize.Height).Vf();
                g.DrawString("" + text[i], Font, brush, SceneAdventure.DrawingRect.Location.Plus((v + vf).pt.MinusF(5, 0)));
            }
        }
        //public static void DrawTile(Graphics g, Tile tile, vec coordinates)
        //{
        //    var sol = (int)tile.Sol;
        //    var mur = (int)tile.Mur;
        //    var ents = tile.Entities;
        //    if(sol != 0) DrawRes(g, ResourcesManager.Instance.Ressources[sol], coordinates);
        //    if(mur != 0) DrawRes(g, ResourcesManager.Instance.Ressources[mur], coordinates);
        //    ents.ToList().ForEach(e => DrawRes(g, ResourcesManager.Instance.Ressources[e], coordinates));
        //}
        //public static void DrawRes(Graphics g, vec[] metacoords, vec coordinates)
        //{
        //    var scale = Core.Instance.GlobalScale;
        //    var coords = ((coordinates - Core.Instance.Cam) * scale).pt;
        //    var w = ConsoleCharSize.Width;
        //    var h = ConsoleCharSize.Height;
        //    foreach (var v in metacoords)
        //        g.FillRectangle(PaletteBrushes[v.UserData], coords.X * w + v.x * scale, coords.Y * h + v.y * scale, scale, scale);
        //}
    }
}