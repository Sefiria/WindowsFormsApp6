using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp5
{
    public class Entity
    {
        public int X, Y;
        public Bitmap Image { get; private set; }

        public Entity(int x, int y, string imgbytes)
        {
            X = x;
            Y = y;
            ConvertToImage(imgbytes);
        }

        private void ConvertToImage(string imgbytes)
        {
            Image = new Bitmap(8, 8);
            List<string> rows = imgbytes.Split(new [] { Environment.NewLine }, StringSplitOptions.None).Skip(1).ToList();
            if (rows.Count != 8) throw new ArgumentException("Height should be 16 px");
            for (int j = 0; j < rows.Count; j++)
            {
                string row = rows[j];
                if (row.Length != 8) throw new ArgumentException("Width should be 16 px");
                for (int i = 0; i < row.Length; i++)
                {
                    Image.SetPixel(i, j, row[i] == '.' ? Color.White : Color.Black);
                }
            }
        }
    }
}
