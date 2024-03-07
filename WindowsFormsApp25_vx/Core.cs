using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxels;

namespace WindowsFormsApp25_vx
{
    public static class Core
    {
        public static VoxelData Voxels;
        public static float yaw = 20f, pitch = -45f;
        public static float old_yaw = 0f, old_pitch = -20f;
        public static float spd = 2f;
        public static Renderer Renderer;
        public static Palette Palette;
        public static bool ms_down = false;
        public static XYZ msXYZ;
        public static int mode = 0;
        public static bool pause = false;
        public static XYZf Cam;
        public static int SelectedIndex = 1;
        public static Point ms_prev_loc = Point.Empty;
        public static bool ms_left_down = false;
        public static bool ms_middle_down = false;
        public static bool ms_right_down = false;
    }
}
