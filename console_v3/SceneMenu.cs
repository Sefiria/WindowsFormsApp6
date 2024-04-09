using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tooling;

namespace console_v3
{
    public class SceneMenu: Scene
    {
        public class EvtRect
        {
            public bool Visible;
            public string Text;
            public Rectangle Bounds;
            public Color ForeColor;
            public Action OnTrigger;
            public void Draw(Graphics g, bool IsSelected)
            {
                g.FillRectangle(makebrush(Bounds), Bounds);
                var ms = MouseStates.Position.ToPoint();
                var dtms = DateTime.UtcNow.Millisecond;
                var cv = (int)(255 - (Bounds.Contains(ms) ? (dtms % 1000 < 500 ? 200 - (dtms / 1000f * 200) : dtms / 1000f * 200) : 0));
                var color = Color.FromArgb(cv, IsSelected ? ForeColor.Reversed() : ForeColor);
                g.DrawString(Text, GraphicsManager.FontSQ, new SolidBrush(color), Bounds);
            }
        }

        public List<EvtRect> Buttons = new List<EvtRect>();

        static Color menuStartColor = Color.FromArgb(0, 0, 50), menuEndColor = Color.FromArgb(0, 0, 100);
        static Color buttonStartColor = Color.FromArgb(0, 50, 0), buttonEndColor = Color.FromArgb(0, 100, 0);

        int w;
        int h;
        Rectangle fullrect;
        Rectangle menurect;
        Rectangle mainrect;
        Rectangle gilsrect;
        Size menuButtonSize;
        Color buttonsColor;
        EvtRect selectedButton;

        static Brush makebrush(Rectangle _rect) => new LinearGradientBrush(_rect.Location.PlusF((0, _rect.Height).P()), _rect.Location.PlusF((_rect.Width, 0).P()), menuStartColor, menuEndColor);
        static Brush makebuttonbrush(Rectangle _rect) => new LinearGradientBrush(_rect.Location.PlusF((0, _rect.Height).P()), _rect.Location.PlusF((_rect.Width, 0).P()), buttonStartColor, buttonEndColor);
        static Brush makeReversebrush(Rectangle _rect) => new LinearGradientBrush(_rect.Location.PlusF((0, _rect.Height).P()), _rect.Location.PlusF((_rect.Width, 0).P()), menuEndColor, menuStartColor);

        public SceneMenu()
        {
            Initialize();
        }

        public override void Initialize()
        {
            Buttons.Clear();
            var m = 50;
            w = Core.Instance.ScreenWidth - m * 2;
            h = Core.Instance.ScreenHeight - m * 2;
            buttonsColor = Color.FromArgb((buttonStartColor.R * 3).ByteCut(), (buttonStartColor.G * 3).ByteCut(), (buttonStartColor.B * 3).ByteCut());
            fullrect = new Rectangle(m, m, w, h);
            menurect = new Rectangle(fullrect.X + fullrect.Width - 300, fullrect.Y, 300, fullrect.Height);
            mainrect = new Rectangle(fullrect.X, fullrect.Y, menurect.X - fullrect.X, fullrect.Height);
            gilsrect = new Rectangle(fullrect.X + fullrect.Width - 200, fullrect.Y + fullrect.Height - 50, 200, 50);
            menuButtonSize = new Size((int)(menurect.Width * 0.8), 40);

            Rectangle rect;
            EvtRect create_button(string name, bool selected = false)
            {
                var bt = new EvtRect { Text = name, Bounds = rect, ForeColor = buttonsColor, OnTrigger = () => OpenSubMenu(name) };
                if(selected)
                    selectedButton = bt;
                return bt;
            }
            for (int y = 0; y < 8; y++)
            {
                rect = new Rectangle(menurect.X + (int)(menurect.Width * 0.1), menurect.Y + menuButtonSize.Height / 2 + y * (int)(menuButtonSize.Height * 1.5), menuButtonSize.Width, menuButtonSize.Height);
                switch (y)
                {
                    case 0: Buttons.Add(create_button("State", true)); break;
                    case 1: Buttons.Add(create_button("Map")); break;
                    case 2: Buttons.Add(create_button("Items")); break;
                    case 3: Buttons.Add(create_button("Tools")); break;
                    case 4: Buttons.Add(create_button("Skills")); break;
                    case 5: Buttons.Add(create_button("Equip")); break;
                    case 6: Buttons.Add(create_button("Options")); break;
                    case 7: Buttons.Add(create_button("Save")); break;
                }
            }
        }

        public override void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                Core.Instance.SwitchScene(Core.Scenes.Adventure);
                return;
            }

            if (MouseStates.ButtonsDown[MouseButtons.Left])
            {
                var ms = MouseStates.Position.ToPoint();
                var clicked = Buttons.FirstOrDefault(bt => bt.Bounds.Contains(ms));
                if (clicked != null) { SubMenu_Items_selected_i = -1; selectedButton = clicked; }
            }

            switch(selectedButton.Text)
            {
                case "State":  SubMenu_State_Update(); break;
                case "Map": SubMenu_Map_Update(); break;
                case "Items": SubMenu_Items_Update(); break;
                case "Tools": SubMenu_Tools_Update(); break;
                case "Skills": SubMenu_Skills_Update(); break;
                case "Equip": SubMenu_Equip_Update(); break;
                case "Options": SubMenu_Options_Update(); break;
                case "Save": SubMenu_Save_Update(); break;
            }
        }

        public override void Draw(Graphics g, Graphics gui)
        {
            g.FillRectangle(makebrush(mainrect), mainrect);
            g.FillRectangle(makebrush(menurect), menurect);
            g.FillRectangle(makebrush(gilsrect), gilsrect);

            Buttons.ForEach(b => b.Draw(g, selectedButton == b));

            g.DrawString($"{Core.Instance.TheGuy.Inventory.Gils} Gils", GraphicsManager.FontSQ, new SolidBrush(buttonsColor), gilsrect);


            switch (selectedButton.Text)
            {
                case "State": SubMenu_State_Draw(g); break;
                case "Map": SubMenu_Map_Draw(g); break;
                case "Items": SubMenu_Items_Draw(g, gui); break;
                case "Tools": SubMenu_Tools_Draw(g, gui); break;
                case "Skills": SubMenu_Skills_Draw(g); break;
                case "Equip": SubMenu_Equip_Draw(g); break;
                case "Options": SubMenu_Options_Draw(g); break;
                case "Save": SubMenu_Save_Draw(g); break;
            }
        }

        public void OpenSubMenu(string submenuname)
        {
            selectedButton = Buttons.FirstOrDefault(bt => bt.Text == submenuname) ?? Buttons.First(bt => bt.Text == "State");
        }

        #region Sub Menus Vars
        int SubMenu_Items_selected_i = -1;
        Dictionary<Rectangle, Action> SubMenu_Items_dropdownitems = new Dictionary<Rectangle, Action>();
        int SubMenu_Map_mode = 0;
        #endregion

        #region Sub Menus Update
        private void SubMenu_State_Update()
        {
        }
        private void SubMenu_Map_Update()
        {
            if (KB.IsKeyPressed(KB.Key.Space))
            {
                SubMenu_Map_mode++;
                while (SubMenu_Map_mode > 1) { SubMenu_Map_mode -= 2; }
            }
        }
        private void SubMenu_Items_Update()
        {
            var ms = MouseStates.Position.ToPoint();
            if (MouseStates.IsButtonPressed(MouseButtons.Left) && SubMenu_Items_selected_i != -1)
            {
                if (SubMenu_Items_selected_i != -1)
                {
                    foreach (var dropdownitem in SubMenu_Items_dropdownitems)
                    {
                        if (dropdownitem.Key.Contains(ms))
                        {
                            if (dropdownitem.Value == null)
                                return;
                            dropdownitem.Value();
                            break;
                        }
                    }
                }
                SubMenu_Items_selected_i = -1;
            }
            if (MouseStates.IsButtonPressed(MouseButtons.Right))
            {
                if (SubMenu_Items_selected_i == -1)
                {
                    var guy = Core.Instance.TheGuy;
                    var items = guy.Inventory.Items;
                    int x, y, w = 240, h = 25, i = 0;
                    foreach (var item in items)
                    {
                        x = i / (mainrect.Height / h) * (w + 10);
                        if (x >= mainrect.Width) break;
                        y = (i - x / w * (mainrect.Height / h)) * h;
                        if (new Rectangle(mainrect.X + x, mainrect.Y + y, 240, 20).Contains(ms))
                        {
                            SubMenu_Items_selected_i = i;
                            break;
                        }
                        i++;
                    }
                }
                else
                {
                    SubMenu_Items_selected_i = -1;
                }
            }
        }
        private void SubMenu_Tools_Update()
        {
            var ms = MouseStates.Position.ToPoint();
            if (MouseStates.IsButtonPressed(MouseButtons.Left) && SubMenu_Items_selected_i != -1)
            {
                if (SubMenu_Items_selected_i != -1)
                {
                    foreach (var dropdownitem in SubMenu_Items_dropdownitems)
                    {
                        if (dropdownitem.Key.Contains(ms))
                        {
                            if (dropdownitem.Value == null)
                                return;
                            dropdownitem.Value();
                            break;
                        }
                    }
                }
                SubMenu_Items_selected_i = -1;
            }
            if (MouseStates.IsButtonPressed(MouseButtons.Right))
            {
                if (SubMenu_Items_selected_i == -1)
                {
                    var guy = Core.Instance.TheGuy;
                    var tools = guy.Inventory.Tools;
                    int x, y, w = 240, h = 25, i = 0;
                    foreach (var tool in tools)
                    {
                        x = i / (mainrect.Height / h) * (w + 10);
                        if (x >= mainrect.Width) break;
                        y = (i - x / w * (mainrect.Height / h)) * h;
                        if (new Rectangle(mainrect.X + x, mainrect.Y + y, 240, 20).Contains(ms))
                        {
                            SubMenu_Items_selected_i = i;
                            break;
                        }
                        i++;
                    }
                }
                else
                {
                    SubMenu_Items_selected_i = -1;
                }
            }
        }
        private void SubMenu_Skills_Update()
        {
        }
        private void SubMenu_Equip_Update()
        {
        }
        private void SubMenu_Options_Update()
        {
        }
        private void SubMenu_Save_Update()
        {
        }
        #endregion

        #region Sub Menus Draw
        private void SubMenu_State_Draw(Graphics g)
        {
            var tg = Core.Instance.TheGuy;
            var font = GraphicsManager.FontSQ;
            var statfont = new Font("Segoe UI", 12);
            Rectangle rect, rect_guy = new Rectangle(mainrect.X + 20, mainrect.Y + 20, mainrect.Width - 40, 150);
            int x, y, w = 170, h = 25, i = 0, max_h = rect_guy.Height - 20;

            void draw_stat(KeyValuePair<Statistics.Stat, int> stat)
            {
                x = i / (max_h / h) * (w + 10);
                y = (i - x / w * (max_h / h)) * h;
                rect = new Rectangle(rect_guy.X + 120 + x, rect_guy.Y + 10 + y, 240, h);
                g.DrawString($"{Enum.GetName(typeof(Statistics.Stat), stat.Key), -5}", statfont, Brushes.White, rect);
                g.DrawString($"{stat.Value,10}", statfont, Brushes.White, rect.WithOffset((80, 0).P()));
                i++;
            }
            void draw(Guy guy)
            {
                g.FillRectangle(makeReversebrush(rect_guy), rect_guy);
                g.DrawImage(guy.Texture, rect_guy.Location.Plus(50, rect_guy.Height / 2 - GraphicsManager.TileSize / 2));
                guy.Stats.List.ForEach(draw_stat);
            }

            draw(tg);
        }
        private void SubMenu_Map_Draw(Graphics g)
        {
            // mode
            var _rect = new Rectangle(mainrect.X + 50, mainrect.Y + 50, 100, 25);
            g.FillRectangle(Brushes.DimGray, _rect);
            switch(SubMenu_Map_mode)
            {
                case 0: _rect = new Rectangle(mainrect.X + 50, mainrect.Y + 50, 40, 25); break;
                case 1: _rect = new Rectangle(mainrect.X + 110, mainrect.Y + 50, 40, 25); break;
                default: throw new Exception("SceneMenu.cs/SubMenu_Map_Draw: not set");
            }
            g.FillRectangle(Brushes.Gray, _rect);
            var _font = new Font("Segoe UI", 10);
            g.DrawString("Layers", _font, SubMenu_Map_mode == 0 ? Brushes.White : Brushes.Gray, mainrect.X + 20, mainrect.Y + 30);
            g.DrawString("Tiles", _font, SubMenu_Map_mode == 1 ? Brushes.White : Brushes.Gray, mainrect.X + 140, mainrect.Y + 30);
            g.DrawString("[Space]", _font, KB.IsKeyDown(KB.Key.Space) ? Brushes.White : Brushes.Gray, mainrect.X + 80, mainrect.Y + 85);
            // ----

            var sz = mainrect.Size;
            var tg = Core.Instance.TheGuy;
            var world = Core.Instance.SceneAdventure.World;
            Rectangle rect;
            vec c = tg.CurChunk;
            var chunks = world.Dimensions[tg.CurDimension].Chunks.ToList();
            var szmini = 30;
            var g_x = mainrect.X + mainrect.Width / 2 - szmini / 2 * (c.x);
            var g_y = mainrect.Y + mainrect.Height / 2 - szmini / 2 * (c.y);
            Bitmap _img = new Bitmap(Chunk.ChunkSize.x, Chunk.ChunkSize.y);
            Graphics _g = Graphics.FromImage(_img);

            void draw_entities(KeyValuePair<vec, Chunk> chunk)
            {
                var entities = new List<Entity>(chunk.Value.Entities).Except(tg);
                foreach (var e in chunk.Value.Entities)
                    g.DrawRectangle(Pens.White, rect.X + e.TileX, rect.Y + e.TileY, 1, 1);
                if (chunk.Key == tg.CurChunk)
                {
                    g.DrawRectangle(new Pen(Color.FromArgb(150, Core.Instance.Ticks % 20 < 10 ? Color.Cyan : Color.Orange)), rect.X + tg.TilePositionF.x - 1, rect.Y + tg.TilePositionF.y - 1, 3, 3);
                    g.DrawRectangle(Pens.White, rect.X + tg.TilePositionF.x, rect.Y + tg.TilePositionF.y, 1, 1);
                    g.DrawRectangle(Pens.LightGray, rect);
                }
            }
            void draw_minichunk_mode_0(KeyValuePair<vec, Chunk> chunk)
            {
                rect = new Rectangle(g_x + szmini * chunk.Key.x, g_y + szmini * chunk.Key.y, szmini - 1, szmini - 1);
                g.FillRectangle(new SolidBrush(DB.ChunkLayerColor[chunk.Value.Layer]), g_x + szmini * chunk.Key.x, g_y + szmini * chunk.Key.y, szmini, szmini);
                draw_entities(chunk);
            }
            void draw_minichunk_mode_1(KeyValuePair<vec, Chunk> chunk)
            {
                _g.Clear(Color.Transparent);
                for (int i = 0; i < Chunk.ChunkSize.x; i++)
                {
                    for (int j = 0; j < Chunk.ChunkSize.y; j++)
                    {
                        var tile = chunk.Value.Tiles[(i, j).V()];
                        var color = DB.GetPxColor(tile.Value);
                        _g.DrawRectangle(new Pen(Color.FromArgb(color)), i, j, 1, 1);
                    }
                }
                g.DrawImage(_img.Resize(szmini), g_x + szmini * chunk.Key.x, g_y + szmini * chunk.Key.y);
                rect = new Rectangle(g_x + szmini * chunk.Key.x, g_y + szmini * chunk.Key.y, szmini - 1, szmini - 1);
                draw_entities(chunk);
            }

            chunks.ForEach(chunk =>
            {
                switch(SubMenu_Map_mode)
                {
                    case 0: draw_minichunk_mode_0(chunk); break;
                    case 1: draw_minichunk_mode_1(chunk); break;
                }
            });

            _g.Dispose();
            _img.Dispose();
        }
        private void SubMenu_Items_Draw(Graphics g, Graphics gui)
        {
            var guy = Core.Instance.TheGuy;
            var items = new List<Item>(guy.Inventory.Items);
            var font = new Font("Segoe UI", 14f);
            int x, y, w = 240, h = 25, i = 0;
            var ms = MouseStates.Position.ToPoint();
            bool hover;
            Rectangle rect;
            foreach (var item in items)
            {
                x = i / (mainrect.Height / h) * (w + 10);
                if (x >= mainrect.Width) break;
                y = (i - x / w * (mainrect.Height / h)) * h;
                rect = new Rectangle(mainrect.X + x, mainrect.Y + y, 272, h);
                hover = i == SubMenu_Items_selected_i || (SubMenu_Items_selected_i == -1 && rect.Contains(ms));
                g.DrawImage(DB.GetTexture(item.DBRef, 24), new Rectangle(mainrect.X + x, mainrect.Y + y, 32, 24));
                g.DrawString(item.Name, font, hover ? Brushes.White : Brushes.Gray, new Rectangle(mainrect.X + 32 + x, mainrect.Y + y, 140, 24));
                g.DrawString($"{item.Count,14}", font, hover ? Brushes.White : Brushes.Gray, new Rectangle(mainrect.X + 32 + x + 150, mainrect.Y + y, 100, 24));
                if(i == SubMenu_Items_selected_i)
                {
                    int j = 0;
                    void actions_remove() { guy.Inventory.Items.RemoveAt(guy.Inventory.Items.IndexOf(item)); }
                    void actions_remove_if_zero() { if (guy.Inventory.Items[guy.Inventory.Items.IndexOf(item)].Count == 0) actions_remove(); }
                    void actions_remove_one() { guy.Inventory.Items[guy.Inventory.Items.IndexOf(item)].Count--; actions_remove_if_zero(); }
                    void actions_drop_one() { NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"- {item.Name} x 1", Color.Red); Core.Instance.SceneAdventure.World.GetChunk(guy.CurChunk).Entities.Add(new Lootable(guy.TilePositionF.i + guy.DirectionPointed, false, new Item(item) { Count=1 })); actions_remove_one(); }
                    void actions_drop_all() { NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"- {item.Name} x {item.Count}", Color.Red); Core.Instance.SceneAdventure.World.GetChunk(guy.CurChunk).Entities.Add(new Lootable(guy.TilePositionF.i + guy.DirectionPointed, false, new Item(item))); actions_remove(); }
                    Action Remove = () => { NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"- {item.Name} x {item.Count}", Color.Red); actions_remove(); };
                    Action Use = () => { item.Use(); actions_remove_if_zero(); };
                    Action Drop1 = () => actions_drop_one();
                    Action DropAll = () => actions_drop_all();
                    var list_submenuitems = new Dictionary<string, Action>();
                    string use_name = "";
                    if (item.DBRef.IsConsumable()) use_name = "Consume";
                    else if (item.DBRef.IsStructure()) use_name = "Place";
                    if(!string.IsNullOrWhiteSpace(use_name))
                        list_submenuitems[use_name] = Use;
                    list_submenuitems["Drop 1"] = Drop1;
                    list_submenuitems["Drop All"] = DropAll;
                    list_submenuitems["Remove"] = Remove;
                    list_submenuitems["---------------------"] = () => { };
                    list_submenuitems["Assign shortcut 1 [C]"] = () => Core.Instance.Shortcuts.Add(new Shortcut(0, KB.Key.C, item)); // TODO REPLACE SHORTCUT WHEN ALREADY EXIST (actual : add new one with same index
                    list_submenuitems["Assign shortcut 2 [V]"] = () => Core.Instance.Shortcuts.Add(new Shortcut(1, KB.Key.V, item)); // TODO Shortcut.Index ??
                    list_submenuitems["Assign shortcut 3 [B]"] = () => Core.Instance.Shortcuts.Add(new Shortcut(2, KB.Key.B, item));
                    list_submenuitems["Assign shortcut 4 [N]"] = () => Core.Instance.Shortcuts.Add(new Shortcut(3, KB.Key.N, item));
                    var list_sz = list_submenuitems.Select(k => TextRenderer.MeasureText(k.Key, font));
                    int max_sz_w = list_sz.Max(sz => sz.Width);
                    int max_sz_h = list_sz.Max(sz => sz.Height);
                    void draw(string txt, Action action)
                    {
                        var r = new Rectangle(rect.X + 40, rect.Y + 10 + max_sz_h * j, max_sz_w, max_sz_h);
                        Brush brush = Brushes.Gray;
                        if (txt == "Consume" && (!item.IsConsommable || !item.IsMenuConsommable)) brush = new SolidBrush(Color.FromArgb(80, 80, 80));
                        else brush = r.Contains(ms) ? Brushes.White : Brushes.Gray;
                        gui.FillRectangle(makebrush(r), r);
                        gui.DrawString(txt, font, brush, r);
                        SubMenu_Items_dropdownitems[r] = action;
                        j++;
                    }
                    foreach(var submenuitem in list_submenuitems)
                        draw(submenuitem.Key, submenuitem.Key == "Consume" && (!item.IsConsommable || !item.IsMenuConsommable) ? null : submenuitem.Value);
                }
                i++;
            }
        }
        private void SubMenu_Tools_Draw(Graphics g, Graphics gui)
        {
            var guy = Core.Instance.TheGuy;
            var tools = new List<Tool>(guy.Inventory.Tools);
            var font = new Font("Segoe UI", 14f);
            int x, y, w = 240, h = 25, i = 0;
            var ms = MouseStates.Position.ToPoint();
            bool hover;
            Rectangle rect;
            foreach (var tool in tools)
            {
                x = i / (mainrect.Height / h) * (w + 10);
                if (x >= mainrect.Width) break;
                y = (i - x / w * (mainrect.Height / h)) * h;
                rect = new Rectangle(mainrect.X + x, mainrect.Y + y, 272, h);
                hover = i == SubMenu_Items_selected_i || (SubMenu_Items_selected_i == -1 && rect.Contains(ms));
                g.DrawImage(tool.Image.Resize(24), new Rectangle(mainrect.X + x, mainrect.Y + y, 32, 24));
                g.DrawString(tool.Name, font, hover ? Brushes.White : Brushes.Gray, new Rectangle(mainrect.X + 32 + x, mainrect.Y + y, 140, 24));
                g.DrawString($"{tool.Count,14}", font, hover ? Brushes.White : Brushes.Gray, new Rectangle(mainrect.X + 32 + x + 150, mainrect.Y + y, 100, 24));
                if (i == SubMenu_Items_selected_i)
                {
                    int j = 0;
                    void actions_remove() { guy.Inventory.Tools.RemoveAt(guy.Inventory.Tools.IndexOf(tool)); }
                    void actions_remove_if_zero() { if (guy.Inventory.Tools[guy.Inventory.Tools.IndexOf(tool)].Count == 0) actions_remove(); }
                    void actions_remove_one() { guy.Inventory.Tools[guy.Inventory.Tools.IndexOf(tool)].Count--; actions_remove_if_zero(); }
                    void actions_drop_one() { NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"- {tool.Name} x 1", Color.Red); Core.Instance.SceneAdventure.World.GetChunk(guy.CurChunk).Entities.Add(new Lootable(guy.TilePositionF.i + guy.DirectionPointed, false, new Tool(tool) { Count = 1 })); actions_remove_one(); }
                    void actions_drop_all() { NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"- {tool.Name} x {tool.Count}", Color.Red); Core.Instance.SceneAdventure.World.GetChunk(guy.CurChunk).Entities.Add(new Lootable(guy.TilePositionF.i + guy.DirectionPointed, false, new Tool(tool))); actions_remove(); }
                    Action Remove = () => { NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"- {tool.Name} x {tool.Count}", Color.Red); actions_remove(); };
                    Action Use = () => tool.Use(guy);
                    Action Drop1 = () => actions_drop_one();
                    Action DropAll = () => actions_drop_all();
                    var list_submenuitems = new Dictionary<string, Action>
                    {
                        ["Use"] = Use,
                        ["Drop 1"] = Drop1,
                        ["Drop All"] = DropAll,
                        ["---------------------"] = () => {},
                        ["Remove"] = Remove,
                        ["---------------------"] = () => {},
                        ["Assign shortcut 1 [C]"] = () => Core.Instance.Shortcuts.Add(new Shortcut(0, KB.Key.C, tool)),
                        ["Assign shortcut 2 [V]"] = () => Core.Instance.Shortcuts.Add(new Shortcut(1, KB.Key.V, tool)),
                        ["Assign shortcut 3 [B]"] = () => Core.Instance.Shortcuts.Add(new Shortcut(2, KB.Key.B, tool)),
                        ["Assign shortcut 4 [N]"] = () => Core.Instance.Shortcuts.Add(new Shortcut(3, KB.Key.N, tool)),
                    };
                    var list_sz = list_submenuitems.Select(k => TextRenderer.MeasureText(k.Key, font));
                    int max_sz_w = list_sz.Max(sz => sz.Width);
                    int max_sz_h = list_sz.Max(sz => sz.Height);
                    void draw(string txt, Action action)
                    {
                        var r = new Rectangle(rect.X + 40, rect.Y + 10 + max_sz_h * j, max_sz_w, max_sz_h);
                        Brush brush = r.Contains(ms) ? Brushes.White : Brushes.Gray;
                        gui.FillRectangle(makebrush(r), r);
                        gui.DrawString(txt, font, brush, r);
                        SubMenu_Items_dropdownitems[r] = action;
                        j++;
                    }
                    foreach (var submenuitem in list_submenuitems)
                        draw(submenuitem.Key, submenuitem.Value);
                }
                i++;
            }
        }
        private void SubMenu_Skills_Draw(Graphics g)
        {
        }
        private void SubMenu_Equip_Draw(Graphics g)
        {
        }
        private void SubMenu_Options_Draw(Graphics g)
        {
        }
        private void SubMenu_Save_Draw(Graphics g)
        {
        }
        #endregion
    }
}
