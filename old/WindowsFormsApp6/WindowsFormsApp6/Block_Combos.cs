using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Block
    {
        static int MaxValueWithCombos => MaxValue + 4;
        static List<long> Score_Creation_Combo;
        static List<long> Score_Destruction_Combo;
        static List<int> MinPathSize_Combo;
        static List<int> BonusTime_Combo;

        public static void InitializeCombos()
        {
            Score_Creation_Combo = new List<long>();
            Score_Destruction_Combo = new List<long>();
            MinPathSize_Combo = new List<int>();
            BonusTime_Combo = new List<int>();

            Score_Creation_Combo.AddRange(new long[]
            {
                10, 25, 50, 150
            });
            Score_Destruction_Combo.AddRange(new long[]
            {
                10, 25, 50, 100
            });
            MinPathSize_Combo.AddRange(new[]
            {
                10, 20, 30, 50
            });
            BonusTime_Combo.AddRange(new[]
            {
                1, 4, 10, 30
            });
        }
        void ComboCreationAddScore()
        {
            if (V <= MaxValue)
                return;

            MAIN.Instance.Mode.Score += Score_Creation_Combo[V - MaxValue - 1];
        }
        void ComboDestructionAddScore()
        {
            if (V <= MaxValue)
                return;

            MAIN.Instance.Mode.Score += Score_Destruction_Combo[V - MaxValue - 1];
        }
        static public int GetBonusTime(int V)
        {
            if (V <= MaxValue)
                return 0;

            return BonusTime_Combo[V - MaxValue - 1];
        }

        void SetVComboFromPathSize(int PathCount, int val)
        {
            if (val >= MaxValueWithCombos)
                return;

            if (val <= MaxValue)
            {
                for (int i = MinPathSize_Combo.Count - 1; i >= 0; i--)
                {
                    if (PathCount >= MinPathSize_Combo[i])
                    {
                        V = MaxValue + i + 1;
                        break;
                    }
                }
            }
            else
            {
                V = val + 1;
            }
        }

        void DestroyCombo(int val)
        {
            List<Block> ExplodedBlocks = new List<Block>();
            int BlockDistanceToDestroy = 0;
            switch (val - MaxValue)
            {
                default: return;
                case 1:
                    BlockDistanceToDestroy = 4;
                    for (int d = 1; d <= BlockDistanceToDestroy; d++)
                    {
                        for (int a = 0; a < 360; a++)
                            ExplodedBlocks.Add(RelatedGrid.ResetBlock((int)(X + Math.Cos(a * Math.PI / 180D) * d), (int)(Y + Math.Sin(a * Math.PI / 180D) * d), val));
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(20);
                    }
                    break;

                case 2:
                    for (int x = X; x < Grid.BlockCount * 2; x++)
                    {
                        for (int y = Y - 2; y <= Y + 2; y++)
                        {
                            ExplodedBlocks.Add(RelatedGrid.ResetBlock(x, y, val));
                            ExplodedBlocks.Add(RelatedGrid.ResetBlock(X + X - x, y, val));
                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(20);
                    }
                    break;

                case 3:
                    for (int x = 0; x < Grid.BlockCount; x += 3)
                    {
                        for (int y = 0; y < Grid.BlockCount; y++)
                        {
                            ExplodedBlocks.Add(RelatedGrid.ResetBlock(x, y, val));
                            ExplodedBlocks.Add(RelatedGrid.ResetBlock(X + X - x, y, val));
                        }
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(20);
                    }
                    break;

                case 4:
                    BlockDistanceToDestroy = Grid.BlockCount / 2 - 1;
                    for (int d = 1; d <= BlockDistanceToDestroy; d++)
                    {
                        for (int a = 0; a < 360; a++)
                            ExplodedBlocks.Add(RelatedGrid.ResetBlock((int)(X + Math.Cos(a * Math.PI / 180D) * d), (int)(Y + Math.Sin(a * Math.PI / 180D) * d), val));
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(20);
                    }
                    MAIN.Instance.Mode.SlowMo();
                    break;

            }

            List<Block> list = new List<Block>(ExplodedBlocks);
            foreach (Block block in list)
                if (block == null)
                    ExplodedBlocks.Remove(block);
            MAIN.Instance.Mode.BlockComboDestroyed(val, X, Y, ExplodedBlocks);
            ComboDestructionAddScore();
        }
        void DrawCombo(Graphics g, int OffsetX = 0, int OffsetY = 0)
        {
            RectangleF bounds = BoundsF;
            bounds = new RectangleF(OffsetX + bounds.X, OffsetY + bounds.Y, bounds.Width, bounds.Height);
            switch(V - MaxValue)
            {

                case 1:
                    g.FillEllipse(Brushes.Red, bounds.X + bounds.Width / 4, bounds.Y, bounds.Width / 2, bounds.Height / 2);
                    g.FillEllipse(Brushes.LimeGreen, bounds.X, bounds.Y + bounds.Height / 2, bounds.Width / 2, bounds.Height / 2);
                    g.FillEllipse(Brushes.Blue, bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2, bounds.Width / 2, bounds.Height / 2);
                    break;

                case 2:
                    g.FillEllipse(Brushes.Red, bounds.X, bounds.Y, bounds.Width / 2, bounds.Height / 2);
                    g.FillEllipse(Brushes.LimeGreen, bounds.X + bounds.Width / 2, bounds.Y, bounds.Width / 2, bounds.Height / 2);
                    g.FillEllipse(Brushes.Blue, bounds.X, bounds.Y + bounds.Height / 2, bounds.Width / 2, bounds.Height / 2);
                    g.FillEllipse(Brushes.Yellow, bounds.X + bounds.Width / 2, bounds.Y + bounds.Height / 2, bounds.Width / 2, bounds.Height / 2);
                    g.FillEllipse(Brushes.White, bounds.X + bounds.Width / 4, bounds.Y + bounds.Height / 4, bounds.Width / 2, bounds.Height / 2);
                    g.DrawEllipse(Pens.Black, bounds.X + bounds.Width / 4, bounds.Y + bounds.Height / 4, bounds.Width / 2, bounds.Height / 2);
                    break;

                case 3:
                    g.FillEllipse(Brushes.Red, bounds.X + bounds.Width / 4 * 0, bounds.Y, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.LimeGreen, bounds.X + bounds.Width / 4 * 0, bounds.Y + bounds.Height / 4, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Blue, bounds.X + bounds.Width / 4 * 0, bounds.Y + bounds.Height / 2, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Yellow, bounds.X + bounds.Width / 4 * 0, (int)(bounds.Y + bounds.Height / 1.33), bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Cyan, bounds.X + bounds.Width / 4 * 1, bounds.Y, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Red, bounds.X + bounds.Width / 4 * 1, bounds.Y + bounds.Height / 4, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.LimeGreen, bounds.X + bounds.Width / 4 * 1, bounds.Y + bounds.Height / 2, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Blue, bounds.X + bounds.Width / 4 * 1, (int)(bounds.Y + bounds.Height / 1.33), bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Yellow, bounds.X + bounds.Width / 4 * 2, bounds.Y, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Cyan, bounds.X + bounds.Width / 4 * 2, bounds.Y + bounds.Height / 4, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Red, bounds.X + bounds.Width / 4 * 2, bounds.Y + bounds.Height / 2, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.LimeGreen, bounds.X + bounds.Width / 4 * 2, (int)(bounds.Y + bounds.Height / 1.33), bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Blue, bounds.X + bounds.Width / 4 * 3, bounds.Y, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Yellow, bounds.X + bounds.Width / 4 * 3, bounds.Y + bounds.Height / 4, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Cyan, bounds.X + bounds.Width / 4 * 3, bounds.Y + bounds.Height / 2, bounds.Width / 4, bounds.Height / 4);
                    g.FillEllipse(Brushes.Red, bounds.X + bounds.Width / 4 * 3, (int)(bounds.Y + bounds.Height / 1.33), bounds.Width / 4, bounds.Height / 4);
                    break;

                case 4:
                    Random rnd = new Random((int)DateTime.Now.Ticks);
                    Color[] colors = new[] { Color.Red, Color.Yellow, Color.Blue, Color.Magenta, Color.LimeGreen, Color.White };
                    for (int x = 0; x < 3; x++)
                        for (int y = 0; y < 3; y++)
                            g.FillEllipse(new SolidBrush(colors[rnd.Next(0, colors.Length)]), bounds.X + bounds.Width / 3 * x, bounds.Y + bounds.Height / 3 * y, bounds.Width / 3, bounds.Height / 3);
                    break;

            }
        }
    }
}
