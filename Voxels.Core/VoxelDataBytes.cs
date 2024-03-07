using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxels {
    public class VoxelDataBytes : VoxelData<byte> {
        public Color[] Colors { get; set;  }

        public VoxelDataBytes(XYZ size, Color[] colors) : base(size) {
            this.Colors = colors;
        }
        public VoxelDataBytes(VoxelDataBytes src) : base(src.Size)
        {
            src.Colors.CopyTo(Colors, 0);
        }

        public sealed override Voxel this[XYZ p] {
            get => new Voxel(Get(p));
            set => Set(p, (byte)value.Index);
        }

        protected sealed override Color ColorOf(Voxel voxel) {
            return Colors[voxel.Index];
        }
    }
}
