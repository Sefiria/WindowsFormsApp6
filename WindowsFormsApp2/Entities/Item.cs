using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Properties;

namespace WindowsFormsApp2.Entities
{
    public class Item : DrawableEntity
    {
        public Items ItemType = Items.Unknown;
        public bool Drawable;
        public Item(Items ItemType, Point? Coord = null, bool Drawable = false)
        {
            this.ItemType = ItemType;
            TX = Coord?.X ?? 0;
            TY = Coord?.Y ?? 0;
            this.Drawable = Drawable;
            this.SetImage();
        }
        public Item(Items ItemType, int TX, int TY, bool Drawable = false)
        {
            this.ItemType = ItemType;
            this.TX = TX;
            this.TY = TY;
            this.Drawable = Drawable;
            this.SetImage();
        }
        public override void Draw()
        {
            if (Drawable)
                base.Draw();
        }

        public override void Update()
        {
        }

        public static List<Item> Repeat(int count, Items ItemType, Point? Coord = null, bool Drawable = false) => Enumerable.Repeat(new Item(ItemType, Coord, Drawable), count).ToList();
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
                case Items.PlantAquarus: img = Resources.plant_aquarus; break;
                case Items.PlantSelanium: img = Resources.plant_selanium; break;
                case Items.Knife: img = Resources.knife; break;
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