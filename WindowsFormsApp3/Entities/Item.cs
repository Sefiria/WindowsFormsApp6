using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp3.Properties;

namespace WindowsFormsApp3.Entities
{
    public abstract class Item : DrawableEntity
    {
        public Items ItemType = Items.Unknown;
        public Item(Items ItemType, float X, float Y)
        {
            this.ItemType = ItemType;
            this.X = X;
            this.Y = Y;
            this.SetImage();
        }
    }


    public static class ItemExt
    {
        public static Bitmap GetImageFromType(this Items type)
        {
            Bitmap img = null;
            switch (type)
            {
                case Items.Unknown: img = Resources.error_img; break;
                case Items.Coin: img = Resources.coin; break;
            }
            img?.MakeTransparent();
            return img;
        }
        public static void SetImage(this Item item)
        {
            item.Image = GetImageFromType(item.ItemType);
            item.Image?.MakeTransparent();
        }
    }
}