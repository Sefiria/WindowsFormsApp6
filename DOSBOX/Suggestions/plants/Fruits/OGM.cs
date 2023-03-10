using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions.plants.Fruits
{
    public class OGM : Fruit
    {
        public OGM(vecf vec) : base(vec)
        {
            CreateGraphics();
        }

        private void CreateGraphics()
        {
            const int w = 1;
            const int h = 1;
            g = new byte[w, h];
            for(int x=0; x<w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                }
            }
        }

        public static string EncodeGenetic(List<string> dna)
        {
            sbyte qualityBranches = 0;
            sbyte qualityLeaves = 0;
            sbyte qualityFruits = 0;

            for(int i=0; i<dna.Count; i++)
            {
                switch(dna[i])
                {
                    default:
                    case "S":
                        qualityBranches += 32;
                        qualityLeaves -= 16;
                        qualityFruits += 0;
                        break;
                    case "X":
                        qualityLeaves -= 8;
                        qualityBranches -= 8;
                        qualityFruits += 32;
                        break;
                    case "0":
                        qualityLeaves += 64;
                        qualityBranches -= 16;
                        qualityFruits += 0;
                        break;
                    case "K":
                        qualityLeaves += 16;
                        qualityBranches += 16;
                        qualityFruits -= 4;
                        break;
                    case "235":
                        qualityLeaves = (sbyte)(qualityLeaves + 136);
                        qualityBranches += 104;
                        qualityFruits += 96;
                        break;
                }
            }
            return $"{qualityBranches},{qualityLeaves},{qualityFruits}";
        }
        public static Stats DecodeMutators(string dna)
        {
            Stats mutators = new Stats();
            string[] split = dna.Split(',');
            sbyte b = sbyte.Parse(split[0]);
            sbyte l = sbyte.Parse(split[1]);
            sbyte f = sbyte.Parse(split[2]);
            mutators.maxbranches = b < 0 ? 1 : b / 16;
            mutators.maxleaves = (l + 128) / 32;
            mutators.maxfruits = f + 55 < 0 ? 0 : (f + 55) / 25;
            return mutators;
        }

        public class Stats
        {
            public int maxbranches, maxleaves, maxfruits;
            public Stats() { }
            public Stats(string dna)
            {
                Stats decoded = DecodeMutators(dna);
                maxbranches = decoded.maxbranches;
                maxleaves = decoded.maxleaves;
                maxfruits = decoded.maxfruits;
            }
        }
    }
}
