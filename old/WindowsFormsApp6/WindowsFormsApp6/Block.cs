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
        public int X, Y;
        private int m_V;
        public int V
        {
            get => m_V;
            set
            {
                if (m_V == 0 && value > 0 && SettingValueFallingBlockFrom == null)
                    MAIN.Instance.Mode?.BlockCreated();
                m_V = value;
            }
        }
        public Grid RelatedGrid = null;
        public bool Falling { get { return m_Falling; } set { m_Falling = value; if (value) { TargetFall = (X, Y + 1); fX = X; fY = Y; } } }
        public Rectangle Bounds => new Rectangle(X * Grid.BlockSize, Y * Grid.BlockSize, Grid.BlockSize, Grid.BlockSize);
        public RectangleF BoundsF => new RectangleF(fX * Grid.BlockSize, fY * Grid.BlockSize, Grid.BlockSize, Grid.BlockSize);
        public bool Contains(int X, int Y)
        {
            return Bounds.Contains(X, Y);
        }
        public bool Contains(Point point)
        {
            return Bounds.Contains(point);
        }
        public bool Hover = false;
        public bool SwitchMode = false;
        public bool Sealed = false;

        private bool m_Falling;
        private (int X, int Y) TargetFall = (-1, -1);
        private float fX, fY;

        static public int MaxValue => MAIN.Instance.Mode == null ? 0 : MAIN.Instance.Mode.BlockMaxValue;

        static public int TempVFrom = 0;
        static public bool GenerateParticles = true;
        static private Block SettingValueFallingBlockFrom = null;


        public Block(int X, int Y, int V)
        {
            this.X = X;
            this.Y = Y;
            this.V = V;
            Falling = false;
            fX = X;
            fY = Y;
        }

        public void Draw(Graphics g, int OffsetX = 0, int OffsetY = 0)
        {
            if (V > 0)
            {
                Color color = GetColor();

                if (Sealed)
                    color = Color.Gray;

                if (V > MaxValue)
                {
                    DrawCombo(g, OffsetX, OffsetY);
                    return;
                }

                var bounds = BoundsF;
                g.FillEllipse(new SolidBrush(color), OffsetX + bounds.X, OffsetY + bounds.Y, bounds.Width, bounds.Height);
            }
        }

        public bool Update(int OffsetX = 0, int OffsetY = 0)
        {
            if (m_Falling)
            {
                fY += (SwitchMode) ? 0.1F : 0.4F;
                if (X == TargetFall.X && (int)fY == TargetFall.Y)
                {
                    if (RelatedGrid.GetBlock(X, Y + 1)?.V > 0 || V == 0)
                        Falling = false;
                    else
                    {
                        SettingValueFallingBlockFrom = this;
                        RelatedGrid.SetBlock(X, Y + 1, V);
                        //bool OtherBlockSealed = RelatedGrid.grid[X][Y + 1].Sealed;
                        //RelatedGrid.grid[X][Y + 1].Sealed = Sealed;
                        RelatedGrid.SetBlock(X, Y, 0);
                        //Sealed = OtherBlockSealed;
                        MAIN.Instance.Mode.BlockFall(X, Y, X, Y + 1);
                        SettingValueFallingBlockFrom = null;
                    }
                    fY = Y;
                    return false;
                }
                return true;
            }

            if (RelatedGrid.ShowPath && Contains(MAIN.Instance.Mode.Render.PointToClient(new Point(Cursor.Position.X - OffsetX, Cursor.Position.Y - OffsetY))))
            {
                var path = Path;
                if (path.Count > 2)
                    path.ForEach(x => x.Hover = true);
            }


            if (SwitchMode && V > 0)
            {
                int PathCount = Path.Count;
                if (PathCount > 2 && !m_Falling && RelatedGrid.GetBlock(X, Y + 1)?.V > 0)
                {
                    OnClick(null, null);
                    if(RelatedGrid.CallModeEvent_BlockLevel)
                        MAIN.Instance.Mode.ComboMade(X, Y, V, PathCount);
                }
            }
            return false;
        }

        public void OnClick(object sender, MouseEventArgs e)
        {
            if (m_Falling || V == 0 || Sealed)
                return;

            var path = Path;
            if (path.Count > 2 && V <= MaxValueWithCombos)
            {
                int val = V;
                
                void SetVTo0(Block x)
                {
                    if(RelatedGrid.CallModeEvent_BlockLevel)
                        MAIN.Instance.Mode?.BlockDestroyed(TempVFrom, x.X, x.Y);
                    if (GenerateParticles && x.V > 0)
                    {
                        Color C = x.GetColor();
                        RelatedGrid.ParticlesEngine.Generate(x.X, x.Y, 3, C.R, C.G, C.B);
                    }
                    x.V = 0;
                }

                if (val <= MaxValue)
                {
                    path.ForEach(x => SetVTo0(x));
                }
                else
                {
                    path.Take(3).ToList().ForEach(x => SetVTo0(x));
                }

                SetVComboFromPathSize(path.Count, val);
                ComboCreationAddScore();
            }
            else
            {
                Destroy(V);
            }

            if(RelatedGrid.CallModeEvent_BlockLevel)
                MAIN.Instance.Mode.BlockClickEnded(X, Y);
        }
        public void Destroy(int SourceValue)
        {
            if (V == 0 || SourceValue <= MaxValue)
                return;

            Sealed = false;

            TempVFrom = SourceValue;

            if (GenerateParticles && V > 0)
            {
                Color C = GetColor();
                RelatedGrid.ParticlesEngine.Generate(X, Y, 3, C.R, C.G, C.B);
            }

            if(RelatedGrid.CallModeEvent_BlockLevel)
                MAIN.Instance.Mode?.BlockDestroyed(TempVFrom, X, Y);

            int val = V;
            V = 0;

            DestroyCombo(val);

            TempVFrom = 0;
        }

        public Color GetColor()
        {
            return GetColor(RelatedGrid, V, Hover);
        }
        static public Color GetColor(Grid RelatedGrid, int V, bool Hover = false)
        {
            Color color;
            int defaultValue = 150;
            
            switch (V)
            {
                default: return Color.White;
                case 1: color = Color.FromArgb(defaultValue, 0, 0); break;
                case 2: color = Color.FromArgb(0, defaultValue, 0); break;
                case 3: color = Color.FromArgb(0, 0, defaultValue); break;
                case 4: color = Color.FromArgb(defaultValue, 0, defaultValue); break;
                case 5: color = Color.FromArgb(defaultValue, defaultValue, 0); break;
                case 6: color = Color.FromArgb(0, defaultValue, defaultValue); break;
                case 7: color = Color.FromArgb(defaultValue, defaultValue / 3, defaultValue / 2); break;
                case 8: color = Color.FromArgb(defaultValue / 5, defaultValue / 2, defaultValue); break;
                case 9: color = Color.FromArgb(defaultValue / 3, defaultValue/2, defaultValue/3); break;
            }

            if (!Hover || !RelatedGrid.ShowPath)
                return color;

            int hoverValue = 255 - defaultValue;
            color = Color.FromArgb(color.R + hoverValue, color.G + hoverValue, color.B + hoverValue);
            return color;
        }

        public List<Block> Path
        {
            get
            {
                List<Block> Result = new List<Block>();
                Result.Add(this);

                void RecurAnaylseARound(int x, int y)
                {
                    List<Block> AroundBlocks = new List<Block>();
                    Block Z = RelatedGrid.GetBlock(x, y - 1);
                    Block Q = RelatedGrid.GetBlock(x - 1, y);
                    Block S = RelatedGrid.GetBlock(x, y + 1);
                    Block D = RelatedGrid.GetBlock(x + 1, y);
                    if (Z != null) AroundBlocks.Add(Z);
                    if (Q != null) AroundBlocks.Add(Q);
                    if (S != null) AroundBlocks.Add(S);
                    if (D != null) AroundBlocks.Add(D);

                    foreach(Block block in AroundBlocks)
                    {
                        if(Result.Contains(block) || block.Sealed)
                            continue;

                        if (block?.V == V)
                        {
                            Result.Add(block);
                            RecurAnaylseARound(block.X, block.Y);
                        }
                    }
                }
                RecurAnaylseARound(X, Y);

                return Result;
            }
        }
        public List<Block> GetPath(int V)
        {
            List<Block> Result = new List<Block>();
            Result.Add(this);

            void RecurAnaylseARound(int x, int y)
            {
                List<Block> AroundBlocks = new List<Block>();
                AroundBlocks.AddRange(new[]{
                    RelatedGrid.GetBlock(x, y - 1),
                    RelatedGrid.GetBlock(x - 1, y),
                    RelatedGrid.GetBlock(x, y + 1),
                    RelatedGrid.GetBlock(x + 1, y)
                });

                foreach (Block block in AroundBlocks)
                {
                    if (Result.Contains(block) || block.Sealed)
                        continue;

                    if (block?.V == V)
                    {
                        Result.Add(block);
                        RecurAnaylseARound(block.X, block.Y);
                    }
                }
            }
            RecurAnaylseARound(X, Y);

            return Result;
        }
    }
}
