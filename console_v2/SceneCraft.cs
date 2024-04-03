using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using static console_v2.SceneCraft;

namespace console_v2
{
    public class SceneCraft : Scene
    {
        public class ListItem
        {
            public int Index;
            public static int sz = 50;
            public int x, y;
            public vec vec { get => (x, y).V(); set { x = value.x; y = value.y; } }
            public Guid content = Guid.Empty;
            public int Count = 0;
            public ListItem() { }
            public ListItem(int index)
            {
                Index = index;
            }
            public ListItem(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public virtual void Draw(Graphics g)
            {
                var inv = Core.Instance.TheGuy.Inventory;
                string name;
                int dbref, CharToDisplay, y;
                Bitmap DBResSpe;
                Font font = new Font("Segoe UI", 14f);
                var sz = TextRenderer.MeasureText("A", font);
                int w = sz.Width;
                int h = sz.Height;

                int i = Index - (int)scroll;
                if (i < 0f || i >= objs.Count) return;
                (name, dbref) = inv.GetObjectInfosByUniqueId(objs[i]);
                if (dbref == -1)
                    return;
                (CharToDisplay, DBResSpe) = DB.RetrieveDBResOrSpe(dbref);
                if (CharToDisplay == -1 && DBResSpe == null)
                    return;
                y = (int)(20 + (Index - scroll) * (h * 1.5f));// TODO    Index - scroll    adjust
                if (CharToDisplay > -1)
                    g.DrawString(string.Concat((char)CharToDisplay), font, Brushes.White, 20, y);
                else
                    g.DrawImage(DBResSpe, 20, y);
                g.DrawString(name, font, Brushes.White, 20 + w * 3, y);
            }
        }
        public class Slot : ListItem
        {
            public Rectangle Bounds => new Rectangle((mainrect.Location.vecf().i + mainrect.Size.V() / 2 - CraftSize * (sz + slot_margin) / 2 + vec * (sz + slot_margin)).ipt, new Size(sz, sz));
            public Slot() : base()
            {
            }
            public Slot(int x, int y) : base(x, y)
            {
            }
            public override void Draw(Graphics gui)
            {
                var r = Bounds;
                gui.FillRectangle(brushMid, r);
                r.Inflate(2, 2);
                float w = 4f;
                Pen p = new Pen(colorDark.Mod(30, 25, 20), w-1);
                gui.DrawLine(p, r.Left, r.Top, r.Right-w, r.Top);
                gui.DrawLine(p, r.Left, r.Top, r.Left, r.Bottom-w);
                p = new Pen(colorDark.Mod(0, -10, -15), w-1);
                gui.DrawLine(p, r.Right-w, r.Bottom-w, r.Left, r.Bottom-w);
                gui.DrawLine(p, r.Right-w, r.Bottom-w, r.Right-w, r.Top);   
            }
        }

        public List<Slot> Slots = new List<Slot>();

        int w;
        int h;
        public static int CraftSize;
        public static Rectangle mainrect;
        public static Rectangle listrect;
        public static int slot_margin = 2;
        public static Color colorDark = Color.FromArgb(40, 30, 20);
        public static Color colorMid = Color.FromArgb(60, 40, 25);
        public static Color colorLight = Color.FromArgb(80, 50, 30);
        public static SolidBrush brushDark, brushMid, brushLight;
        public static int hint_z_s;

        public static float scroll = 0f;
        private static List<Guid> objs;
        private static List<ListItem> listItems;

        public SceneCraft(int size)
        {
            CraftSize = size;
            Initialize();
        }

        public override void Initialize()
        {
            var m = 50;
            w = Core.Instance.ScreenWidth - m * 2;
            h = Core.Instance.ScreenHeight - m * 2;
            listrect = new Rectangle(m, m, 300, h);
            mainrect = new Rectangle(listrect.Right, m, w - listrect.Width, h);
            brushDark = new SolidBrush(colorDark);
            brushMid = new SolidBrush(colorMid);
            brushLight = new SolidBrush(colorLight);
            hint_z_s = byte.MaxValue;
            var inv = Core.Instance.TheGuy.Inventory;
            var items = inv.Items.Select(it => it.UniqueId);
            var tools = inv.Tools.Select(t => t.UniqueId);
            objs = tools.Concat(items).ToList();

            Slots.Clear();
            for (int y = 0; y < CraftSize; y++)
            {
                for (int x = 0; x < CraftSize; x++)
                {
                    Slots.Add(new Slot(x, y));
                }
            }

            listItems = objs.Select(obj => new ListItem(objs.IndexOf(obj))).ToList();
        }
        public override void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Tab) || KB.IsKeyPressed(KB.Key.Escape))
            {
                Core.Instance.SwitchScene(Core.Scenes.Adventure);
                return;
            }

            if (KB.IsKeyDown(KB.Key.Z)) scroll -= 0.2f;
            if (KB.IsKeyDown(KB.Key.S)) scroll += 0.2f;
            scroll = Math.Max(0, Math.Min(objs.Count - 1, scroll));

            //if (MouseStates.ButtonsDown[MouseButtons.Left])
            //{
            //    var ms = MouseStates.Position.ToPoint();
            //    var clicked = Slots.FirstOrDefault(bt => bt.Bounds.Contains(ms));
            //    if (clicked != null) { SubMenu_Items_selected_i = -1; selectedButton = clicked; }
            //}
        }

        public override void Draw(Graphics g, Graphics gui)
        {
            g.FillRectangle(brushDark, listrect);
            g.DrawRectangle(Pens.Black, listrect);
            g.FillRectangle(brushDark, mainrect);
            g.DrawRectangle(Pens.Black, mainrect);

            Slots.ForEach(slot => slot.Draw(gui));

            Bitmap listBitmap = new Bitmap(listrect.Width, listrect.Height);
            Graphics listgui = Graphics.FromImage(listBitmap);
            var inv = Core.Instance.TheGuy.Inventory;
            int y;
            Font font = new Font("Segoe UI", 14f);
            var sz = TextRenderer.MeasureText("A", font);
            int w = sz.Width;
            int h = sz.Height;

            listItems.ForEach(listitem => listitem.Draw(listgui));

            if (hint_z_s > 0)
            {
                listgui.FillRectangle(new SolidBrush(Color.FromArgb(hint_z_s, colorDark)), listrect.Width / 2 - GraphicsManager.CharSize.Width / 2, 0, GraphicsManager.CharSize.Width * 2, GraphicsManager.CharSize.Height * 3);
                listgui.DrawString("↑", GraphicsManager.Font, new SolidBrush(Color.FromArgb(hint_z_s, Color.Cyan)), listrect.Width / 2 - GraphicsManager.CharSize.Width / 2 + 5, 0);
                listgui.DrawString("Z", GraphicsManager.Font, new SolidBrush(Color.FromArgb(hint_z_s, Color.Cyan))  , listrect.Width / 2 - 5, GraphicsManager.CharSize.Height * 1.5f);
                listgui.FillRectangle(new SolidBrush(Color.FromArgb(hint_z_s, colorDark)), listrect.Width / 2 - GraphicsManager.CharSize.Width / 2, listrect.Height - GraphicsManager.CharSize.Height * 3, GraphicsManager.CharSize.Width * 2, GraphicsManager.CharSize.Height * 3);
                listgui.DrawString("S", GraphicsManager.Font, new SolidBrush(Color.FromArgb(hint_z_s, Color.Cyan)), listrect.Width / 2 - GraphicsManager.CharSize.Width / 2 + 5, listrect.Height - GraphicsManager.CharSize.Height * 2);
                listgui.DrawString("↓", GraphicsManager.Font, new SolidBrush(Color.FromArgb(hint_z_s, Color.Cyan)), listrect.Width / 2 - 5, listrect.Height - GraphicsManager.CharSize.Height * 2 + GraphicsManager.CharSize.Height);
                hint_z_s -= 3;
            }
            gui.DrawImage(listBitmap, listrect.Location);
        }

        //private void SubMenu_Map_Draw(Graphics g)
        //{
        //    // mode
        //    var _rect = new Rectangle(listrect.X + 50, listrect.Y + 50, 100, 25);
        //    g.FillRectangle(Brushes.DimGray, _rect);
        //    switch(SubMenu_Map_mode)
        //    {
        //        case 0: _rect = new Rectangle(listrect.X + 50, listrect.Y + 50, 40, 25); break;
        //        case 1: _rect = new Rectangle(listrect.X + 110, listrect.Y + 50, 40, 25); break;
        //        default: throw new Exception("SceneMenu.cs/SubMenu_Map_Draw: not set");
        //    }
        //    g.FillRectangle(Brushes.Gray, _rect);
        //    var _font = new Font("Segoe UI", 10);
        //    g.DrawString("Layers", _font, SubMenu_Map_mode == 0 ? Brushes.White : Brushes.Gray, listrect.X + 20, listrect.Y + 30);
        //    g.DrawString("Tiles", _font, SubMenu_Map_mode == 1 ? Brushes.White : Brushes.Gray, listrect.X + 140, listrect.Y + 30);
        //    g.DrawString("[Space]", _font, KB.IsKeyDown(KB.Key.Space) ? Brushes.White : Brushes.Gray, listrect.X + 80, listrect.Y + 85);
        //    // ----

        //    var sz = listrect.Size;
        //    var tg = Core.Instance.TheGuy;
        //    var world = Core.Instance.SceneAdventure.World;
        //    Rectangle rect;
        //    vec c = tg.CurChunk;
        //    var chunks = world.Dimensions[tg.CurDimension].Chunks.ToList();
        //    var szmini = 30;
        //    var g_x = listrect.X + listrect.Width / 2 - szmini / 2 * (c.x);
        //    var g_y = listrect.Y + listrect.Height / 2 - szmini / 2 * (c.y);
        //    Bitmap _img = new Bitmap(Chunk.ChunkSize.x, Chunk.ChunkSize.y);
        //    Graphics _g = Graphics.FromImage(_img);

        //    void draw_entities(KeyValuePair<vec, Chunk> chunk)
        //    {
        //        var entities = new List<Entity>(chunk.Value.Entities).Except(tg);
        //        foreach (var e in chunk.Value.Entities)
        //            g.DrawRectangle(Pens.White, rect.X + e.TileX, rect.Y + e.TileY, 1, 1);
        //        if (chunk.Key == tg.CurChunk)
        //        {
        //            g.DrawRectangle(new Pen(Color.FromArgb(150, Core.Instance.Ticks % 20 < 10 ? Color.Cyan : Color.Orange)), rect.X + tg.X - 1, rect.Y + tg.Y - 1, 3, 3);
        //            g.DrawRectangle(Pens.White, rect.X + tg.X, rect.Y + tg.Y, 1, 1);
        //            g.DrawRectangle(Pens.LightGray, rect);
        //        }
        //    }
        //    void draw_minichunk_mode_0(KeyValuePair<vec, Chunk> chunk)
        //    {
        //        rect = new Rectangle(g_x + szmini * chunk.Key.x, g_y + szmini * chunk.Key.y, szmini - 1, szmini - 1);
        //        g.FillRectangle(new SolidBrush(DB.ChunkLayerColor[chunk.Value.Layer]), g_x + szmini * chunk.Key.x, g_y + szmini * chunk.Key.y, szmini, szmini);
        //        draw_entities(chunk);
        //    }
        //    void draw_minichunk_mode_1(KeyValuePair<vec, Chunk> chunk)
        //    {
        //        _g.Clear(Color.Transparent);
        //        for (int i = 0; i < Chunk.ChunkSize.x; i++)
        //        {
        //            for (int j = 0; j < Chunk.ChunkSize.y; j++)
        //            {
        //                var tile = chunk.Value.Tiles[(i, j).V()];
        //                var color = DB.ResColor[tile.Sol != 0 ? (int)tile.Sol : (int)tile.Mur];
        //                _g.DrawRectangle(new Pen(color), i, j, 1, 1);
        //            }
        //        }
        //        g.DrawImage(_img.Resize(szmini), g_x + szmini * chunk.Key.x, g_y + szmini * chunk.Key.y);
        //        rect = new Rectangle(g_x + szmini * chunk.Key.x, g_y + szmini * chunk.Key.y, szmini - 1, szmini - 1);
        //        draw_entities(chunk);
        //    }

        //    chunks.ForEach(chunk =>
        //    {
        //        switch(SubMenu_Map_mode)
        //        {
        //            case 0: draw_minichunk_mode_0(chunk); break;
        //            case 1: draw_minichunk_mode_1(chunk); break;
        //        }
        //    });

        //    _g.Dispose();
        //    _img.Dispose();
        //}
        //private void SubMenu_Items_Draw(Graphics g, Graphics gui)
        //{
        //    var guy = Core.Instance.TheGuy;
        //    var items = new List<Item>(guy.Inventory.Items);
        //    var font = new Font("Segoe UI", 14f);
        //    int x, y, w = 240, h = 25, i = 0;
        //    var ms = MouseStates.Position.ToPoint();
        //    bool hover;
        //    Rectangle rect;
        //    foreach (var item in items)
        //    {
        //        x = i / (listrect.Height / h) * (w + 10);
        //        if (x >= listrect.Width) break;
        //        y = (i - x / w * (listrect.Height / h)) * h;
        //        rect = new Rectangle(listrect.X + x, listrect.Y + y, 240, h);
        //        hover = i == SubMenu_Items_selected_i || (SubMenu_Items_selected_i == -1 && rect.Contains(ms));
        //        g.DrawString(item.Name, font, hover ? Brushes.White : Brushes.Gray, new Rectangle(listrect.X + x, listrect.Y + y, 140, 20));
        //        g.DrawString($"{item.Count,14}", font, hover ? Brushes.White : Brushes.Gray, new Rectangle(listrect.X + x + 150, listrect.Y + y, 100, 20));
        //        if(i == SubMenu_Items_selected_i)
        //        {
        //            int j = 0;
        //            void actions_remove() { guy.Inventory.Items.RemoveAt(guy.Inventory.Items.IndexOf(item)); }
        //            void actions_remove_if_zero() { if (guy.Inventory.Items[guy.Inventory.Items.IndexOf(item)].Count == 0) actions_remove(); }
        //            void actions_remove_one() { guy.Inventory.Items[guy.Inventory.Items.IndexOf(item)].Count--; actions_remove_if_zero(); }
        //            void actions_drop_one() { NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"- {item.Name} x 1", Color.Red); Core.Instance.SceneAdventure.World.GetChunk(guy.CurChunk).Entities.Add(new Lootable(guy.TilePositionF.i + guy.DirectionPointed, false, new Item(item) { Count=1 })); actions_remove_one(); }
        //            void actions_drop_all() { NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"- {item.Name} x {item.Count}", Color.Red); Core.Instance.SceneAdventure.World.GetChunk(guy.CurChunk).Entities.Add(new Lootable(guy.TilePositionF.i + guy.DirectionPointed, false, new Item(item))); actions_remove(); }
        //            Action Remove = () => { NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"- {item.Name} x {item.Count}", Color.Red); actions_remove(); };
        //            Action Consume = () => { item.Consume(); actions_remove_if_zero(); };
        //            Action Drop1 = () => actions_drop_one();
        //            Action DropAll = () => actions_drop_all();
        //            var list_submenuitems = new Dictionary<string, Action> {
        //                ["Consume"] = Consume,
        //                ["Drop 1"] = Drop1,
        //                ["Drop All"] = DropAll,
        //                ["Remove"] = Remove,
        //            };
        //            var list_sz = list_submenuitems.Select(k => TextRenderer.MeasureText(k.Key, font));
        //            int max_sz_w = list_sz.Max(sz => sz.Width);
        //            int max_sz_h = list_sz.Max(sz => sz.Height);
        //            void draw(string txt, Action action)
        //            {
        //                var r = new Rectangle(rect.X + 40, rect.Y + 10 + max_sz_h * j, max_sz_w, max_sz_h);
        //                Brush brush = Brushes.Gray;
        //                if (txt == "Consume" && (!item.IsConsommable || !item.IsMenuConsommable)) brush = new SolidBrush(Color.FromArgb(80, 80, 80));
        //                else brush = r.Contains(ms) ? Brushes.White : Brushes.Gray;
        //                gui.FillRectangle(makebrush(r), r);
        //                gui.DrawString(txt, font, brush, r);
        //                SubMenu_Items_dropdownitems[r] = action;
        //                j++;
        //            }
        //            foreach(var submenuitem in list_submenuitems)
        //                draw(submenuitem.Key, submenuitem.Key == "Consume" && (!item.IsConsommable || !item.IsMenuConsommable) ? null : submenuitem.Value);
        //        }
        //        i++;
        //    }
        //}
    }
}
