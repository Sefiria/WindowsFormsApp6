using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp8
{
    public class Palette
    {
        public string FileName;
        public int TickValue;
        public List<Pixel> Pixels = new List<Pixel>();
        public Palette(string fileName)
        {
            FileName = fileName;
        }
    }
}
