using ComputeSharp;

namespace WindowsFormsApp5
{
    [AutoConstructor]
    public readonly partial struct shdr : IComputeShader
    {
        public readonly IReadWriteNormalizedTexture2D<Float4> Texture;

        public void Execute()
        {
            // Our image processing logic here. In this example, we are just
            // applying a naive grayscale effect to all pixels in the image.
            Float3 rgb = Texture[ThreadIds.XY].RGB;
            float avg = Hlsl.Dot(rgb, new Float3(0.0722f, 0.7152f, 0.2126f));

            Texture[ThreadIds.XY].RGB = avg;
        }
    }
}
