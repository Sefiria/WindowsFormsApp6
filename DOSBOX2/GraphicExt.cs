using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Tooling;

namespace DOSBOX2
{
    public static class GraphicExt
    {
        public static void DisplayControlsRectInfo(int x, int y, bool rect = false)
        {
            var (z, q, s, d) = KB.ZQSD();
            var select = KB.IsKeyDown(KB.Key.Back);
            var start = KB.IsKeyDown(KB.Key.Space);
            var a = KB.IsKeyDown(KB.Key.Left);
            var b = KB.IsKeyDown(KB.Key.Right);
            if (rect)
            {
                Graphic.FillDrawRect(3, 1, x, y, 9, 3);
                x++;y++;
            }
            Graphic.Set(x + 0, y + 1, (byte)(q ? 0 : 2));
            Graphic.Set(x + 1, y + 0, (byte)(z ? 0 : 2));
            Graphic.Set(x + 1, y + 1, (byte)(s ? 0 : 2));
            Graphic.Set(x + 2, y + 1, (byte)(d ? 0 : 2));
            Graphic.Set(x + 3, y + 1, (byte)(select ? 0 : 2));
            Graphic.Set(x + 4, y + 1, (byte)(start ? 0 : 2));
            Graphic.Set(x + 5, y + 0, (byte)(a ? 0 : 2));
            Graphic.Set(x + 6, y + 0, (byte)(b ? 0 : 2));
        }

        public static byte[,] CreateBatch(string to_parse)
        {
            var lines = to_parse.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            var w = lines[0].Length;
            var h = lines.Length;
            byte[,] result = new byte[w, h];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                    result[x, y] = (byte) int.Parse(""+lines[y][x]);
            return result;
        }
    }
}
