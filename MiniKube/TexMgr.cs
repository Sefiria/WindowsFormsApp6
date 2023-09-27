using MiniKube.Properties;
using System.Drawing;
using System.Resources;

namespace MiniKube
{
    internal class TexMgr
    {
        public static Bitmap Load(string tex)
        {
            var result = Resources.ResourceManager.GetObject(tex) as Bitmap;
            result.MakeTransparent();
            return result;
        }
    }
}
