using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp4
{
    public class Carte
    {
        public static readonly int W = Data.RenderW / Data.TileSz;
        public static readonly int H = Data.RenderH / Data.TileSz;
        public int[,] Map;
        public Card StartCard, EndCard;

        private Carte(int[,] Map)
        {
            this.Map = Map;
        }

        public static Carte Create(string map)
        {
            Carte carte;
            Card startCard = null, card = null;
            ArgumentException InvalidRowsCountException(int rows_count) => new ArgumentException($"In 'Carte Create(string map)' rows count expected : {H}, got {rows_count}.");
            ArgumentException InvalidRowsCharsCountException(int j, int chars_count) => new ArgumentException($"In 'Carte Create(string map)' at rows[{j}], chars count expected : {W}, got {chars_count}.");

            int[,] Map = new int[W, H];
            List<string> rows = map.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (rows.Count != H) throw InvalidRowsCountException(rows.Count);
            for (int j = 0; j < rows.Count; j++)
            {
                if (rows[j].Length != W) throw InvalidRowsCharsCountException(j, rows[j].Length);
                for (int i = 0; i < rows[j].Length; i++)
                {
                    if (rows[i][j] == '.')
                    {
                        Map[j, i] = 1;
                        startCard = new Card(new Point(i, j), null, null);
                    }
                    else if (int.Parse("" + rows[j][i]) > 0)
                    {
                        Map[j, i] = int.Parse("" + rows[j][i]);
                    }
                }
            }
            carte = new Carte(Map);
            carte.StartCard = startCard;

            card = startCard;
            do
            {
                bool Check(int x, int y)
                {
                    if (x < 0 || x >= H || y < 0 || y >= W)
                        return false;
                    if (rows[y][x] != '0' && card.Prev?.Coord != new Point(x, y))
                    {
                        card.Next = new Card(new Point(x, y), card, null);
                        card = card.Next;
                        return true;
                    }
                    return false;
                }

                if (!Check(card.Coord.X - 1, card.Coord.Y))
                    if (!Check(card.Coord.X, card.Coord.Y - 1))
                        if (!Check(card.Coord.X + 1, card.Coord.Y))
                            Check(card.Coord.X, card.Coord.Y + 1);
            }
            while (rows[card.Coord.X][card.Coord.Y] != '2');
            
            carte.EndCard = card;

            return carte;
        }

        public static Carte Default()
        {
            return Create(@"
0000000000000000000000000
0.11111111100000000000000
0000000000111111100000000
0000000000000000111100000
0000011111100000000110000
0001110000111000000010000
0011000000001100000010000
0010000000000100000010000
0010000000000100000110000
0010000000000100000100000
0011000000000100011100000
0001110000000111110000000
0000011110000000000000000
0000000011111100000000000
0000000000000111111100000
0000011111000000000111000
0000110001110000000001100
0001100000011110000000110
0001000000000011111000010
0011000000000000001110110
0110000000000000000011100
0100000000111100000000000
0110000001100111000000000
0011111111000001111200000
0000000000000000000000000");
        }
    }
}