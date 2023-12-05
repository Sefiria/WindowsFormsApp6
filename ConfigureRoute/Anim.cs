using System.Collections.Generic;
using System.Drawing;
using Tooling;

namespace ConfigureRoute
{
    //public class Anim
    //{
    //    public float speed = 0.2F;
    //    public List<Bitmap> imgs = new List<Bitmap>();
    //    public RectangleF CollisionBounds;
    //    public float W => imgs[0].Width;
    //    public float H => imgs[0].Height;

    //    float fcur = 0F;
    //    private int v1;
    //    private int v2;

    //    int frames => imgs.Count;
    //    int cur => (int)fcur;


    //    public Anim() { }
    //    public Anim(string tex, int split_count_x = 0, int split_count_y = 0)
    //    {
    //        imgs = TexMgr.Load(tex).Split(split_count_x, split_count_y);
    //        CollisionBounds = new RectangleF(0F, 0F, W, H);
    //    }
    //    public Anim(string anim = "")
    //    {
    //        Anim a = AnimMgr.Load(anim);
    //        imgs = new List<Bitmap>(a.imgs);
    //        speed = a.speed;
    //    }

    //    public void Update()
    //    {
    //        fcur += speed;
    //        if (fcur >= frames) fcur = 0F;
    //    }
    //    public Bitmap CurrentTexture => imgs[cur];
    //}

}
