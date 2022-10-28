using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public static class Data
    {
        public static Random RND = new Random((int)DateTime.Now.Ticks);
        public static int RenderW, RenderH;
        public static readonly int TileSz = 32;
        public static Graphics g;
        public static Carte Carte;
        public static List<Pion> Pions = new List<Pion>();
        public static int Turn = 0;
        public static bool RollingDices = false, EndRollingDices = false;
        public static List<int> Dices = new List<int>() { 1 };
        public static float DiceRollTime = 0F, DiceRollTimeMax = 1F;
        public static Form1 MainForm;
        public static bool MovingPion = false;
        public static float t = 0F;
        public static Card TargetCardForMovePion;

        public static int ResultDices => Dices.Sum();
    }
}
