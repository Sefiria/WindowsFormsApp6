using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Entities;
using WindowsFormsApp2.Entities.Mobs;
using WindowsFormsApp2.Plants;

namespace WindowsFormsApp2
{
    public static class MapGen
    {
        private static readonly int LuckChest = 100;
        private static readonly int LuckMob = 100;

        public static void SetFromStrings(this Map map, List<string> lines)
        {
            for (int x = 0; x < Map.W; x++)
            {
                for (int y = 0; y < Map.H; y++)
                {
                    if (lines.Count > y && lines[y].Count() > x)
                    {
                        map.Tiles[x, y] = int.TryParse($"{lines[y][x]}", out int v) ? v + 1 : 0;
                    }
                }
            }
        }
        public static void SetFromStrings(this Map map, char[,] lines)
        {
            for (int x = 0; x < Map.W; x++)
            {
                for (int y = 0; y < Map.H; y++)
                {
                    if (lines.GetLength(0) > x && lines.GetLength(1) > y)
                    {
                        map.Tiles[x, y] = int.TryParse($"{lines[x, y]}", out int v) ? v + 1 : 0;
                    }
                }
            }
        }
        public static Map Generate1(this Map map)
        {
            char[,] lines = new char[Map.W, Map.H];
            for (int x = 0; x < Map.W; x++)
                for (int y = 0; y < Map.H; y++)
                    lines[x, y] = ' ';
            var box1 = GenBox(Map.W, Map.H);
            map.Boxes.Add(new System.Drawing.Rectangle(box1.x + 1, box1.y + 2, box1.w - 2, box1.h - 4));
            lines = Merge(lines, box1);
            map.SetFromStrings(lines);
            AddEntities(map);
            return map;
        }
        private static void AddEntities(Map map)
        {
            AddChests(map);
            AddMobs(map);
        }
        private static void AddChests(Map map)
        {
            if (Tools.RND.Next(0, 100) >= LuckChest)
                return;
            var rect = map.Boxes[0];
            int x = Tools.RND.Next(rect.X + 1, rect.X + rect.Width - 1);
            int y = Tools.RND.Next(rect.Y + 1, rect.Y + rect.Height - 1);
            map.Entities.Add(new Chest(GenerateRandomChestContent(), x, y));
        }
        private static void AddMobs(Map map)
        {
            if (Tools.RND.Next(0, 100) >= LuckMob)
                return;
            var rect = map.Boxes[0];
            int x = Tools.RND.Next(rect.X + 1, rect.X + rect.Width - 1);
            int y = Tools.RND.Next(rect.Y + 1, rect.Y + rect.Height - 1);
            map.Entities.Add(new MobDummy(x, y));
        }
        private static Dictionary<Item, int> GenerateRandomChestContent()
        {
            var content = new Dictionary<Item, int>();

            content[new Knife(MaterialQuality.Wood)] = Tools.RND.Next(10, 20);

            //content[new Item(Items.Coin)] = Tools.RND.Next(1, 28);

            //switch(Tools.RND.Next(0, 3))
            //{
            //    case 0:
            //        content[new Item(Items.Coin)] = Tools.RND.Next(1, 100);
            //        content[new Knife(MaterialQuality.Wood)] = Tools.RND.Next(10, 20);
            //        break;
            //    case 1:
            //        break;
            //    case 2:
            //        content[new Item(Items.Coin)] = Tools.RND.Next(1, 100);
            //        content[new PlantAquarus()] = Tools.RND.Next(1, 4);
            //        content[new PlantSelanium()] = Tools.RND.Next(1, 10);
            //        break;
            //}

            return content;
        }

        private static (int x, int y, char[,] t, int w, int h) GenBox(int maxW, int maxH)
        {
            int xs = Tools.RND.Next(5, maxW);
            int xp = Tools.RND.Next(maxW - xs);
            int ys = Tools.RND.Next(7, maxH);
            int yp = Tools.RND.Next(maxH - ys);

            var t = new char[xs,ys];
            for (int x = 0; x < xs; x++)
            {
                for (int y = 0; y < ys; y++)
                {
                    t[x,y] = '0';
                }
            }

            for (int x = 0; x < xs; x++)
            {
                t[x, 0] = '1';
                t[x, 1] = '2';
                t[x, ys - 2] = '1';
                t[x, ys - 1] = '2';
            }
            for (int y = 0; y < ys - 1; y++)
            {
                t[0, y] = '1';
                t[xs - 1, y] = '1';
            }

            return GenWater(xp, yp, t, xs, ys);
        }

        private static (int x, int y, char[,] t, int w, int h) GenWater(int xp, int yp, char[,] t, int xs, int ys)
        {
            (int x, int y, char[,] t, int w, int h) result = (xp, yp, t, xs, ys);
            if (Tools.RND.Next(0, 100) >= 15) return result;
            List<(int x, int y)> grounds = new List<(int x, int y)>();
            for (int x = 0; x < xs; x++)
                for (int y = 0; y < ys; y++)
                    if(t[x, y] == '0') grounds.Add((x, y));
            if (grounds.Count > 6)
            {
                int rnd = Tools.RND.Next(0, grounds.Count);
                int x = grounds[rnd].x;
                int y = grounds[rnd].y;
                result.t[x, y] = '3';
                result.t[x + Tools.RND.Next(-1, 2), y] = '3';
                result.t[x, y + Tools.RND.Next(-1, 2)] = '3';
                result.t[x + Tools.RND.Next(-1, 2), y + Tools.RND.Next(-1, 2)] = '3';
            }
            return result;
        }

        private static char[,] Merge(char[,] Base, (int x, int y, char[,] t, int w, int h) ToMerge)
        {
            for (int x = 0; x < ToMerge.w; x++)
            {
                for (int y = 0; y < ToMerge.h; y++)
                {
                    Base[ToMerge.x + x, ToMerge.y + y] = ToMerge.t[x, y];
                }
            }
            return Base;
        }
    }
}
