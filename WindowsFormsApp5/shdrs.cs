using ComputeSharp;
using ComputeSharp.__Internals;
using System;

namespace WindowsFormsApp5
{
    [AutoConstructor]
    public readonly partial struct shdr : IComputeShader
    {
        public readonly IReadWriteNormalizedTexture2D<Float4> Texture;

        public void BuildHlslSource(out ArrayPoolStringBuilder builder, int threadsX, int threadsY, int threadsZ)
        {
            throw new NotImplementedException();
        }

        public void Execute()
        {
            // Our image processing logic here. In this example, we are just
            // applying a naive grayscale effect to all pixels in the image.
            Float3 rgb = Texture[ThreadIds.XY].RGB;
            float avg = Hlsl.Dot(rgb, new Float3(0.0722f, 0.7152f, 0.2126f));

            Texture[ThreadIds.XY].RGB = avg;
        }

        public int GetDispatchId()
        {
            throw new NotImplementedException();
        }

        public void LoadBytecode<TLoader>(ref TLoader loader, int threadsX, int threadsY, int threadsZ) where TLoader : struct, IBytecodeLoader
        {
            throw new NotImplementedException();
        }

        public void LoadDispatchData<TLoader>(ref TLoader loader, GraphicsDevice device, int x, int y, int z) where TLoader : struct, IDispatchDataLoader
        {
            throw new NotImplementedException();
        }

        public void LoadDispatchMetadata<TLoader>(ref TLoader loader, out IntPtr result) where TLoader : struct, IDispatchMetadataLoader
        {
            throw new NotImplementedException();
        }
    }
}
