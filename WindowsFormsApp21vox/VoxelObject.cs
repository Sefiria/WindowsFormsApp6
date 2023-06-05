using System.Collections.Generic;
using System.Numerics;

namespace Parser
{
    public struct VoxelObject
    {
        public Model[] Models;
        public uint[] ColorPalette;
    }

    public class Model
    {
        public Vector3 Size { get; set; }
        public Voxel[] Voxels;
    }

    public class Voxel
    {
        public byte X, Y, Z, ColorIndex;
    }
}