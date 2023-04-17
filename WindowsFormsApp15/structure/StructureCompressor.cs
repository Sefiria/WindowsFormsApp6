using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WindowsFormsApp15.items;
using WindowsFormsApp15.utilities.tiles;
using WindowsFormsApp15.Utilities;
using static WindowsFormsApp15.AnimRes;
using static WindowsFormsApp15.enums;
using static WindowsFormsApp15.structure.StructureCompressor;

namespace WindowsFormsApp15.structure
{
    internal class StructureCompressor : StructureTransformer
    {
        public static Dictionary<Type, Type> TransformPairs = new Dictionary<Type, Type>() { [typeof(Ore)] = typeof(Plate) };
        public static anim _anim = compressor;

        public StructureCompressor() : base(TransformPairs, _anim, vecf.Zero, Way.Up)
        {
        }
        public StructureCompressor(vecf vec, int way) : base(TransformPairs, _anim, vec, way)
        {
        }
        public StructureCompressor(vecf vec, Way way) : base(TransformPairs, _anim, vec, way)
        {
        }
    }
}