using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze
{
    public class MZ
    {
        public static readonly int MW = 32, MH = 48;
        public static Point Start, End;
        private static byte[,] Tiles;

        public static void Gen()
        {
            Tiles = new byte[MW, MH];
        }
    }


    [Flags]
    public enum Directions
    {
        N = 1,
        S = 2,
        E = 4,
        W = 8
    }

    public class Grid
    {
        private const int _rowDimension = 0;
        private const int _columnDimension = 1;

        public int MinSize { get; private set; }
        public int MaxSize { get; private set; }
        public int[,] Cells { get; private set; }

        public Grid() : this(3, 3)
        {

        }

        public Grid(int rows, int columns)
        {
            MinSize = 3;
            MaxSize = 64;
            Cells = Initialise(rows, columns);
        }

        public int[,] Initialise(int rows, int columns)
        {
            if (rows < MinSize)
                rows = MinSize;

            if (columns < MinSize)
                columns = MinSize;

            if (rows > MaxSize)
                rows = MaxSize;

            if (columns > MaxSize)
                columns = MaxSize;

            var cells = new int[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    cells[i, j] = 0;
                }
            }

            return cells;
        }

        private Dictionary<Directions, int> DirectionX = new Dictionary<Directions, int>
        {
            { Directions.N, 0 },
            { Directions.S, 0 },
            { Directions.E, 1 },
            { Directions.W, -1 }
        };

        private Dictionary<Directions, int> DirectionY = new Dictionary<Directions, int>
        {
            { Directions.N, -1 },
            { Directions.S, 1 },
            { Directions.E, 0 },
            { Directions.W, 0 }
        };

        private Dictionary<Directions, Directions> Opposite = new Dictionary<Directions, Directions>
        {
            { Directions.N, Directions.S },
            { Directions.S, Directions.N },
            { Directions.E, Directions.W },
            { Directions.W, Directions.E }
        };

        public void Generate()
        {
            var cells = Cells;
            CarvePassagesFrom(0, 0, ref cells);
            Cells = cells;
        }

        public void CarvePassagesFrom(int currentX, int currentY, ref int[,] grid)
        {
            var directions = new List<Directions>
            {
                Directions.N,
                Directions.S,
                Directions.E,
                Directions.W
            }
            .OrderBy(x => Guid.NewGuid());

            foreach (var direction in directions)
            {
                var nextX = currentX + DirectionX[direction];
                var nextY = currentY + DirectionY[direction];

                if (IsOutOfBounds(nextX, nextY, grid))
                    continue;

                if (grid[nextX, nextY] != 0) // has been visited
                    continue;

                grid[currentX, currentY] |= (int)direction;
                grid[nextX, nextY] |= (int)Opposite[direction];

                CarvePassagesFrom(nextX, nextY, ref grid);
            }
        }

        private bool IsOutOfBounds(int x, int y, int[,] grid)
        {
            if (x < 0 || x > grid.GetLength(_rowDimension) - 1)
                return true;

            if (y < 0 || y > grid.GetLength(_columnDimension) - 1)
                return true;

            return false;
        }

        public static void Print(int[,] grid)
        {
            var rows = grid.GetLength(_rowDimension);
            var columns = grid.GetLength(_columnDimension);

            // Top line
            Console.Write(" ");
            for (int i = 0; i < columns; i++)
                Console.Write(" _");
            Console.WriteLine();

            for (int y = 0; y < rows; y++)
            {
                Console.Write(" |");

                for (int x = 0; x < columns; x++)
                {
                    var directions = (Directions)grid[y, x];

                    var s = directions.HasFlag(Directions.S) ? " " : "_";

                    Console.Write(s);

                    s = directions.HasFlag(Directions.E) ? " " : "|";

                    Console.Write(s);
                }

                Console.WriteLine();
            }
        }

        public void Draw(Graphics g, int s)
        {
            var rows = Cells.GetLength(_rowDimension);
            var columns = Cells.GetLength(_columnDimension);

            g.Clear(Color.White);

            for (int i = 0; i < columns; i++)
            {
                g.DrawLine(Pens.Black, i * s, 0, (i + 1) * s, 0);
            }
            for (int j = 0; j < rows; j++)
            {
                g.DrawLine(Pens.Black, 0, j * s, 0, (j + 1) * s);
            }

            for (int y = 0; y < rows; y++)
            {
                g.DrawLine(Pens.Black, 0, y * s, 0, (y + 1) * s);

                for (int x = 0; x < columns; x++)
                {
                    if(((Directions)Cells[y, x]).HasFlag(Directions.E))
                        g.DrawLine(Pens.Black, x * s, y * s, (x + 1) * s, y * s);
                    else
                        g.DrawLine(Pens.Black, x * s, y * s, x * s, (y + 1) * s);
                }

            }
        }
    }
}
