using SkiaSharp;
using System;
using System.Drawing;
using Voxels;

namespace WindowsFormsApp25_vx
{
    public class Renderer : BaseRenderer, IDisposable
    {
        public SKBitmap Bitmap;
        public SKCanvas Canvas;
        public RenderSettings Settings;
        public Palette Palette;

        public Renderer(int size, Palette palette)
        {
            Settings = new RenderSettings(size, 0f, 0f);
            Bitmap = new SKBitmap(size, size, false);
            Canvas = new SKCanvas(Bitmap);
            Palette = palette;
        }

        public void RenderVoxels(VoxelData voxelData, float yaw, float pitch)
        {
            Settings.Yaw = yaw;
            Settings.Pitch = pitch;
            Bitmap.Erase(SKColors.Transparent);
            RenderTriangles(voxelData);
        }

        private void RenderTriangles(VoxelData voxelData)
        {
            RenderTriangles(voxelData, Canvas, new MeshSettings
            {
                Yaw = Settings.Yaw,
                Pitch = Settings.Pitch,
                FakeLighting = true,
                FloorShadow = true,
                MeshType = MeshType.Triangles,
            }, Settings);
        }

        public XYZ GetCursorVoxOLD(VoxelData voxelData, Point ms)
        {
            int x = voxelData.Size.X / 2 + (int)((ms.X - Bitmap.Width / 2) / 30F);
            int y = voxelData.Size.Y / 2 + (int)((Bitmap.Height / 2 - ms.Y) / 30F);
            int z = 0;
            return new XYZ(x, y, z);
        }
        public XYZ GetCursorVoxNEW(VoxelData vox, Point ms)
        {
            int x = vox.Size.X / 2 + (int)((ms.X - Bitmap.Width / 2) / 30F);
            int y = 0;
            int z = (Bitmap.Height - ms.Y) / (Bitmap.Height / vox.Size.MaxDimension);
            return new XYZ(x, y, z);
        }
        public XYZ GetCursorVox(int mode, VoxelData vox, Point ms)
        {
            if (mode == 0)
            {
                return GetCursorVoxOLD(vox, ms);
            }
            
            return GetCursorVoxNEW(vox, ms);
        }
        public void RenderCursor3D(VoxelData voxelData, XYZ msXYZ)
        {
            if (!voxelData.BoundsContains(msXYZ))
                return;

            var vox = new VoxelDataBytes(voxelData.Size, new Voxels.Color[] { Voxels.Color.Transparent, new Voxels.Color(Core.Palette[Core.SelectedIndex], (byte)(0.25f * byte.MaxValue)) });
            vox[msXYZ] = new Voxel(1);
            RenderTriangles(vox);
        }

        public void Dispose()
        {
            Canvas.Dispose();
            Bitmap.Dispose();
        }
    }
}
