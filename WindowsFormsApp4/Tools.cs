using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp4.Properties;

namespace WindowsFormsApp4
{
    public static class Tools
    {
        public static string RNDID()
        {
            string ID = "";
            for(int i=0; i<8; i++)
            {
                ID += ""+Data.RND.Next(256);
            }
            return ID;
        }

        public static Bitmap GetDiceResFromValue(int v)
        {
            switch(v)
            {
                default:
                case 1: return Resources.dice1;
                case 2: return Resources.dice2;
                case 3: return Resources.dice3;
                case 4: return Resources.dice4;
                case 5: return Resources.dice5;
                case 6: return Resources.dice6;
            }
        }

        public static float Lerp(float v0, float v1, float t)
        {
            return (1F - t) * v0 + t * v1;
        }
    }
}
