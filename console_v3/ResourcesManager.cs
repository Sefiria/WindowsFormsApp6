using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3
{
    internal class ResourcesManager
    {
        private static ResourcesManager m_Instance = null;
        public static ResourcesManager Instance => m_Instance ?? (m_Instance = new ResourcesManager());

        public static Dictionary<int, string> RawRessources = new Dictionary<int, string>()
        {
            [(int)Sols.Pierre] = @"
01001020
02201020
20020200
10002222
00020200
11220020
00201002
22001010",
            [(int)Sols.Herbe] = @"
11132121
31211111
01103312
12311121
21123110
10112321
21331113
12113112",
        };
        public Dictionary<int, vec[]> Ressources = new Dictionary<int, vec[]>()
        {
            [(int)Sols.Pierre] = RawRessources.ContainsKey((int)Sols.Pierre) ? TranslateRaw((int)Sols.Pierre) : new vec[] { /*(0, 0, 0).V()*/ },
        };
        public static vec[] TranslateRaw(int i)
        {
            string raw = RawRessources[i];
            List<vec> result = new List<vec>();
            int w = GraphicsManager.ConsoleCharSize.Width;
            int h = GraphicsManager.ConsoleCharSize.Height;
            var lines  = raw.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    if (int.TryParse("" + lines[y][x], out int v))
                        result.Add((x,y,v).V());
                }
            }
            return result.ToArray();
        }
        public ResourcesManager()
        {
        }
    }
}
