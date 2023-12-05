using ConfigureRoute.Properties;
using System.Drawing;
using System.Resources;

namespace ConfigureRoute
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
