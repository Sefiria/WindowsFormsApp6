using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using Voxels;
using Color = Voxels.Color;

namespace Tooling
{
    public static class VoxParser
    {
        public static Color[] ToConformPalette(this System.Drawing.Color[] palette)
        {
            var conform_palette = new Color[256];
            for (int i = 0; i < 256; i++)
            {
                System.Drawing.Color c = i < palette.Length ? palette[i] : System.Drawing.Color.White;
                conform_palette[i] = new Color(c.R, c.G, c.B, c.A);
            }
            return conform_palette;
        }
        public static System.Drawing.Color[] ToDrawingPalette(this Color[] conform_palette)
        {
            var drawing_palette = new System.Drawing.Color[256];
            for (int i = 0; i < 256; i++)
            {
                Color c = conform_palette[i];
                drawing_palette[i] = i < conform_palette.Length ? System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B) : System.Drawing.Color.White;
            }
            return drawing_palette;
        }
        public static VoxelDataBytes ToVox(this DATA data, Color[] palette)
        {
            var size = new XYZ(data.W, data.H, data.LAYERS);
            var result = new VoxelDataBytes(size, palette);
            for (int z = 0; z < result.Size.Z; z++)
                for (int x = 0; x < result.Size.X; x++)
                    for (int y = 0; y < result.Size.Y; y++)
                        result[new XYZ(x, y, z)] = new Voxel(data[z, x, y]);
            return result;
        }
        public static DATA ToDATA(this VoxelDataBytes vox)
        {
            var result = new DATA(vox.Size.Z, vox.Size.X, vox.Size.Y);
            for (int x = 0; x < vox.Size.X; x++)
                for (int y = 0; y < vox.Size.Y; y++)
                    for (int z = 0; z < vox.Size.Z; z++)
                        result[z, x, y] = (byte)vox[new XYZ(x, y, z)].Index;
            return result;
        }
        public static void SaveVox(VoxelDataBytes voxels, string filename)
        {
            if (voxels != null)
            {
                using (var stream = File.Create(filename))
                {
                    new MagicaVoxel(voxels).Write(stream);
                }
                MessageBox.Show("Saved successfully.", "Save to .vox (MagicaVoxel)", MessageBoxButtons.OK);
            }
        }
        public static VoxelDataBytes LoadVox(string filename)
        {
            using (var stream = File.OpenRead(filename))
            {
                var magicaVoxel = new MagicaVoxel();
                magicaVoxel.Read(stream);
                return magicaVoxel.Flatten();
            }
        }
    }
}
