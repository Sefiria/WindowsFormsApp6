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

namespace WindowsFormsApp15.structure
{
    internal class StructureLiquefer : StructureTransformer
    {
        public static Dictionary<Type, Type> TransformPairs = new Dictionary<Type, Type>() { [typeof(Ore)] = typeof(Liquid) };
        public static anim _anim = liquefer;

        public StructureLiquefer() : base(TransformPairs, _anim, vecf.Zero, Way.Up)
        {
        }
        public StructureLiquefer(vecf vec, int way) : base(TransformPairs, _anim, vec, way)
        {
        }
        public StructureLiquefer(vecf vec, Way way) : base(TransformPairs, _anim, vec, way)
        {
        }
    }
}
