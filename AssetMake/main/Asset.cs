using AssetMake.Properties;
using AssetMake.utilities;
using AssetMake.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetMake.main
{
    internal class Asset
    {
        public Bitmap Texture;
        public Dictionary<string, List<Bitmap>> TexAnims;
        public static int TransparentColor = Color.FromArgb(159, 159, 179).ToArgb();

        public Asset()
        {
            Texture = Resources.missigno;
            LoadTexturesToAnims();
        }

        public Asset(Bitmap texture)
        {
            Texture = texture;
            LoadTexturesToAnims();
        }
        public void Display(Graphics g, string animname, int frameId, vecf coord)
        {
            if (!TexAnims.ContainsKey(animname))
                return;

            var anim = TexAnims[animname];
            var frame = anim[frameId % anim.Count];
            g.DrawImage(frame, coord.pt);
        }

        public void LoadTexturesToAnims()
        {
            AnimationMgr animmgr = Data.Instance.GlobalManager.AnimationMgr;

            TexAnims = new Dictionary<string, List<Bitmap>>()
            {
                ["walk"] = AppliedTex(animmgr.Animations["walk"].Frames),
                ["run"] = AppliedTex(animmgr.Animations["run"].Frames),
            };
        }
        private List<Bitmap> AppliedTex(List<Bitmap> frames)
        {
            var result = new List<Bitmap>();
            Bitmap applied;
            Color pixel; int debug = 0;
            foreach (Bitmap frame in frames)
            {
                applied = new Bitmap(frame.Width, frame.Height);
                for(int x = 0; x < frame.Width; x++)
                {
                    for (int y = 0; y < frame.Height; y++)
                    {
                        pixel = frame.GetPixel(x, y);
                        if (pixel.ToArgb() != TransparentColor)
                            applied.SetPixel(x, y, Texture.GetPixel(pixel.R, pixel.G));
                    }
                }
                applied.MakeTransparent(Color.FromArgb(TransparentColor));
                result.Add(applied.Resize((float)Core.PixelScale));
                debug++;
            }
            return result;
        }
    }
}
