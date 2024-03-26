using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core
{
    public class Room
    {
        // Tiles layers : 0=Floor, 1=Items, 2=Event
        public int[,] Tiles = null;
        public int Size = 0;

        public Room() { }
        public Room(string data)
        {
            Initialize(data.Split(new[] { Environment.NewLine }, StringSplitOptions.None).Length);

            LoadData(data, ref Tiles);
        }
        public void LoadData(string data, ref int[,] targetLayer)
        {
            string[] lines = data.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            string[] words = null;

            for (int j = 0; j < Size; j++) // foreach line in lines
            {
                words = lines[j].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < words.Length; i++) // foreach word in words
                {
                    targetLayer[i, j] = int.Parse(words[i], System.Globalization.NumberStyles.HexNumber);
                }
            }
        }
        public Room(int Size)
        {
            Initialize(Size);
        }
        public void Initialize(int Size)
        {
            this.Size = Size;
            Tiles = new int[Size, Size];
        }

        public int this[int i, int j] { get => (i < Size && j < Size) ? Tiles [i, j] : 0; set { if (i < Size && j < Size) Tiles[i, j] = value; } }
        public int this[Vec Loc] { get => Tiles[Loc.x, Loc.y]; set => Tiles[Loc.x, Loc.y] = value; }
        /// <summary>
        /// Get converted Tiles int[,] into char[]
        /// </summary>
        /// <returns></returns>
        public string GetBuffer(int OffsetX, int OffsetY)
        {
            string buffer = "";

            for (int j = 0; j < Size; j++)
            {
                for (int k = 0; k < OffsetX; k++)
                    buffer += ' ';

                for (int i = 0; i < Size; i++)
                {
                    buffer += HelpBlock.GetBlockFromId(Tiles[i, j]).RenderChar;
                }
            }

            return buffer;
        }
        public void DrawBuffer(int OffsetX, int OffsetY)
        {
            for (int j = 0; j < Size; j++)
            {
                for (int k = 0; k < OffsetX; k++)
                    Console.Write(' ');

                for (int i = 0; i < Size; i++)
                {
                    var block = HelpBlock.GetBlockFromId(Tiles[i, j]);
                    Console.ForegroundColor = block.RenderColor;
                    Console.Write(block.RenderChar);
                }
            }
        }
    }
}
