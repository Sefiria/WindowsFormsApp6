using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    public class Grid
    {
        public Block[][] grid;
        readonly public int DefaultBlockCount = 20;
        static public int BlockCount = 0;
        static public int BlockSize => MAIN.Instance.Mode == null ? 16 : 320 / BlockCount;
        public bool ShowPath = true;
        public bool SwitchMode = false;
        public ParticlesEngine ParticlesEngine = new ParticlesEngine();
        public bool CallModeEvent_BlockLevel = true;

        public Block GetBlock(int x, int y)
        {
            if (x < 0 || y < 0 || x >= BlockCount || y >= BlockCount)
                return null;
            return grid[x][y];
        }

        public void SetBlock(int x, int y, int value)
        {
            if (x < 0 || y < 0 || value < 0 || x >= BlockCount || y >= BlockCount)
                return;
            grid[x][y].V = value;
        }
        public Block ResetBlock(int x, int y, int SourceValue)
        {
            if (x < 0 || y < 0 || x >= BlockCount || y >= BlockCount)
                return null;
            grid[x][y].Destroy(SourceValue);
            return grid[x][y];
        }

        public Grid(int _BlockCount = 0, bool SwitchMode = false)
        {
            this.SwitchMode = SwitchMode;
            BlockCount = DefaultBlockCount;
            if (_BlockCount > 0)
                BlockCount = _BlockCount;

            if (SwitchMode)
            {
                grid = new Block[BlockCount][];
                for (int x = 0; x < BlockCount; x++)
                    grid[x] = new Block[BlockCount];
            }
            else
            {
                Random rnd = new Random((int)DateTime.Now.Ticks);
                grid = new Block[BlockCount][];
                for (int x = 0; x < BlockCount; x++)
                {
                    grid[x] = new Block[BlockCount];
                    for (int y = 0; y < BlockCount; y++)
                    {
                        grid[x][y] = new Block(x, y, rnd.Next(1, MAIN.Instance.Mode.BlockMaxValue + 1));
                        grid[x][y].SwitchMode = SwitchMode;
                        grid[x][y].RelatedGrid = this;
                    }
                }
            }
        }
        public void Initialize()
        {
            if (!SwitchMode)
                return;

            Random rnd = new Random((int)DateTime.Now.Ticks);
            for (int x = 0; x < BlockCount; x++)
            {
                for (int y = 0; y < BlockCount; y++)
                {
                    grid[x][y] = new Block(x, y, rnd.Next(1, MAIN.Instance.Mode.BlockMaxValue + 1));
                    if (grid[x][y].Path.Count > 2)
                    {
                        grid[x][y].V = 1;
                        while (grid[x][y].V < MAIN.Instance.Mode.BlockMaxValue && grid[x][y].Path.Count > 2)
                        {
                            if (grid[x][y].Path.Count > 2)
                                grid[x][y].V++;
                        }
                    }
                    grid[x][y].SwitchMode = SwitchMode;
                }
            }
        }
        public void Reset()
        {
            Random rdn = new Random((int)DateTime.Now.Ticks);
            for (int x = 0; x < BlockCount; x++)
            {
                for (int y = 0; y < BlockCount; y++)
                {
                    grid[x][y] = new Block(x, y, rdn.Next(1, MAIN.Instance.Mode.BlockMaxValue + 1));
                }
            }
            MAIN.Instance.Mode.Score = 0;
        }

        public void Draw(Graphics g, int OffsetX = 0, int OffsetY = 0)
        {
            for (int x = 0; x < BlockCount; x++)
                for (int y = 0; y < BlockCount; y++)
                    grid[x][y].Draw(g, OffsetX, OffsetY);

            ParticlesEngine.UpdateDraw(g, OffsetX, OffsetY);
        }

        public void Update(int OffsetX = 0, int OffsetY = 0)
        {
            if (MAIN.Instance.Mode == null)
                return;

            if (!MAIN.Instance.Mode.DestroyingAllBonuses)
            {
                for (int x = 0; x < BlockCount; x++)
                    for (int y = 0; y < BlockCount; y++)
                        grid[x][y].Hover = false;
            }

            for (int x = 0; x < BlockCount; x++)
            {
                for (int y = 0; y < BlockCount; y++)
                {
                    grid[x][y].Update(OffsetX, OffsetY);

                    if (grid[x][y].Falling)
                        continue;

                    if (grid[x][y].V > 0 && y < BlockCount - 1 && grid[x][y + 1].V == 0)
                        grid[x][y].Falling = true;
                }
            }

            if (MAIN.Instance.Mode.DestroyingAllBonuses && !MAIN.Instance.Mode.AllBonusesDestroyed)
            {
                bool NotDoneYet = false;
                for (int x = 0; x < BlockCount && !NotDoneYet; x++)
                {
                    for (int y = 0; y < BlockCount && !NotDoneYet; y++)
                    {
                        Block block = grid[x][y];
                        if (block.Falling || (GetBlock(x, y + 1) != null && GetBlock(x, y + 1).V == 0))
                        {
                            NotDoneYet = true;
                        }
                    }
                }

                if(!NotDoneYet)
                {
                    MAIN.Instance.Mode.AllBonusesDestroyed = true;
                }
            }
        }

        public void DestroyAllBonuses()
        {
            MAIN.Instance.Mode.DestroyingAllBonuses = true;

            Block block = null;
            for (int x = 0; x < BlockCount; x++)
            {
                for (int y = 0; y < BlockCount; y++)
                {
                    block = MAIN.Instance.Mode.Grid.GetBlock(x, y);
                    if (block.V > MAIN.Instance.Mode.BlockMaxValue)
                        block.Destroy(block.V);
                }
            }
        }
    }
}
