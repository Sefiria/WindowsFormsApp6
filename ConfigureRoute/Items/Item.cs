using ConfigureRoute.Entities;
using ConfigureRoute.Structures;
using System;
using System.Drawing;
using Tooling;

namespace ConfigureRoute.Items
{
    //public class Item : Entity
    //{
    //    public Item() { }
    //    public Item(string resource, int split_x=0, int split_y=0)
    //    {
    //        if (Properties.Resources.ResourceManager.GetObject(resource) == null)
    //            throw new Exception($"ConfigureRoute.Items.Item/ Resource does not exist : '{resource}'");
    //        tex = Properties.Resources.ResourceManager.GetObject(resource) as Bitmap;
    //        if (split_x > 0 && split_y > 0)
    //            tex = tex.Split(split_x, split_y)[0];
    //        tex.MakeTransparent(Color.White);
    //    }

    //    public override void Draw(PointF? offset = null)
    //    {
    //        if (offset != null)
    //            Core.g.DrawImage(tex, Pos.Minus(Anim.W - Anim.CollisionBounds.X, Anim.H - Anim.CollisionBounds.Y).PlusF(offset.Value));
    //        else
    //            Core.g.DrawImage(tex, Pos.Minus(Anim.W - Anim.CollisionBounds.X, Anim.H - Anim.CollisionBounds.Y));
    //    }

    //    public static Type GetItemTypeById(int id)
    //    {
    //        switch(id)
    //        {
    //            default: return null;
    //            case 0: return typeof(ItemIronPlate);
    //            case 1: return typeof(ItemIronRod);
    //            case 2: return typeof(ItemIronScrew);
    //            case 3: return typeof(Input<>);
    //        }
    //    }
    //    public static int GetIdByItemType<T>() where T:Item
    //    {
    //        if (typeof(T) == typeof(ItemIronPlate)) return 0;
    //        if (typeof(T) == typeof(ItemIronRod)) return 1;
    //        if (typeof(T) == typeof(ItemIronScrew)) return 2;
    //        if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Input<>)) return 3;
    //        return -1;
    //    }
    //    internal static Bitmap GetTexById(int id)
    //    {
    //        switch (id)
    //        {
    //            default: return null;
    //            case 0: return TexMgr.Load("item_iron_plate");
    //            case 1: return TexMgr.Load("item_iron_rod");
    //            case 2: return TexMgr.Load("item_iron_screw");
    //            case 3: return TexMgr.Load("item_struct_input");
    //        }
    //    }
    //}
    //public class ItemIronPlate : Item { public ItemIronPlate() : base("item_iron_plate") {}}
    //public class ItemIronRod : Item { public ItemIronRod() : base("item_iron_rod"){} }
    //public class ItemIronScrew : Item { public ItemIronScrew() : base("item_iron_screw") {} }
}
