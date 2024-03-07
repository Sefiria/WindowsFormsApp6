using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp25_vx
{
    public class Palette
    {
        public List<Voxels.Color> Colors;
        public int Count => Colors?.Count ?? 0;
        public Voxels.Color None => Colors.First();
        public Voxels.Color[] ToConformArray()
        {
            var array = new Voxels.Color[256];
            for (int i = 0; i < array.Length; i++)
                array[i] = i >= Colors.Count ? new Voxels.Color(0f,0f,0f,0f): Colors[i];
            return array;
        }

        public Voxels.Color this[int index] => Colors[index];

        public Palette()
        {
            Colors = new List<Voxels.Color>();
        }
        public Palette(List<Voxels.Color> colors, bool copy = false)
        {
            Colors = copy ? new List<Voxels.Color>(colors) : colors;
        }
        public Palette(Palette palette, bool copy = false)
        {
            Colors = copy ? new List<Voxels.Color>(palette.Colors) : palette.Colors;
        }
    }
}
