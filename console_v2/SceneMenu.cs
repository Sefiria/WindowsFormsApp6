using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Tooling;

namespace console_v2
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
                g.DrawString(Text, GraphicsManager.Font, new SolidBrush(color), Bounds);
            }
        }

        public List<EvtRect> Buttons = new List<EvtRect>();

        static Color buttonStartColor, buttonEndColor;

        int w;
        int h;
        Rectangle fullrect;
        Rectangle menurect;
        Rectangle mainrect;
        Rectangle gilsrect;
        Size menuButtonSize;
        Color buttonsColor;
        EvtRect selectedButton;

        static Brush makebrush(Rectangle _rect) => new LinearGradientBrush(_rect.Location.PlusF((0, _rect.Height).P()), _rect.Location.PlusF((_rect.Width, 0).P()), buttonStartColor, buttonEndColor);
        static Brush makeReversebrush(Rectangle _rect) => new LinearGradientBrush(_rect.Location.PlusF((0, _rect.Height).P()), _rect.Location.PlusF((_rect.Width, 0).P()), buttonEndColor, buttonStartColor);

        public SceneMenu()
        {
            Initialize();
        }

        public override void Initialize()
        {
            Buttons.Clear();
            w = Core.Instance.ScreenWidth;
            h = Core.Instance.ScreenHeight;
            buttonStartColor = Color.FromArgb(20, 20, 100);
            buttonEndColor = Color.FromArgb(0, 0, 40);
            buttonsColor = Color.FromArgb((buttonStartColor.R * 3).ByteCut(), (buttonStartColor.G * 3).ByteCut(), (buttonStartColor.B * 3).ByteCut());
            fullrect = new Rectangle(0, 0, w, h);
            menurect = new Rectangle(w - 300, 0, 300, h);
            mainrect = new Rectangle(0, 0, menurect.X, h);
            gilsrect = new Rectangle(w - 200, h - 50, 200, 50);
            menuButtonSize = new Size((int)(menurect.Width * 0.8), 40);

            Rectangle rect;
            EvtRect create_button(string name, bool selected = false)
            {
                var bt = new EvtRect { Text = name, Bounds = rect, ForeColor = buttonsColor, OnTrigger = () => OpenSubMenu(name) };
                if(selected)
                    selectedButton = bt;
                return bt;
            }
            for (int y = 0; y < 7; y++)
            {
                rect = new Rectangle(menurect.X + (int)(menurect.Width * 0.1), menuButtonSize.Height / 2 + y * (int)(menuButtonSize.Height * 1.5), menuButtonSize.Width, menuButtonSize.Height);
                switch (y)
                {
                    case 0: Buttons.Add(create_button("State", true)); break;
                    case 1: Buttons.Add(create_button("Items")); break;
                    case 2: Buttons.Add(create_button("Tools")); break;
                    case 3: Buttons.Add(create_button("Skills")); break;
                    case 4: Buttons.Add(create_button("Equip")); break;
                    case 5: Buttons.Add(create_button("Options")); break;
                    case 6: Buttons.Add(create_button("Save")); break;
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
                case "Items": SubMenu_Items_Update(); break;
                case "Tools": SubMenu_Tools_Update(); break;
                case "Skills": SubMenu_Skills_Update(); break;
                case "Equip": SubMenu_Equip_Update(); break;
                case "Options": SubMenu_Options_Update(); break;
                case "Save": SubMenu_Save_Update(); break;
            }
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(makebrush(mainrect), mainrect);
            g.FillRectangle(makebrush(menurect), menurect);
            g.FillRectangle(makebrush(gilsrect), gilsrect);

            Buttons.ForEach(b => b.Draw(g, selectedButton == b));

            g.DrawString($"{Core.Instance.TheGuy.Inventory.Gils} Gils", GraphicsManager.Font, new SolidBrush(buttonsColor), gilsrect);


            switch (selectedButton.Text)
            {
                case "State": SubMenu_State_Draw(g); break;
                case "Items": SubMenu_Items_Draw(g); break;
                case "Tools": SubMenu_Tools_Draw(g); break;
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
        #endregion

        #region Sub Menus Update
        private void SubMenu_State_Update()
        {
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
                        if (new Rectangle(x, y, 240, 20).Contains(ms))
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
                        if (new Rectangle(x, y, 240, 20).Contains(ms))
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
            var font = GraphicsManager.Font;
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
                g.DrawString(string.Concat((char)guy.CharToDisplay), font, guy.CharBrush, rect_guy.Location.Plus(50, rect_guy.Height / 2 - (int)GraphicsManager.CharSize.Height / 2));
                guy.Stats.List.ForEach(draw_stat);
            }

            draw(tg);
        }
        private void SubMenu_Items_Draw(Graphics g)
        {
            var gui = Core.Instance.gui;
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
                rect = new Rectangle(x, y, 240, h);
                hover = i == SubMenu_Items_selected_i || (SubMenu_Items_selected_i == -1 && rect.Contains(ms));
                g.DrawString(item.Name, font, hover ? Brushes.White : Brushes.Gray, new Rectangle(x, y, 140, 20));
                g.DrawString($"{item.Count,14}", font, hover ? Brushes.White : Brushes.Gray, new Rectangle(x+150, y, 100, 20));
                if(i == SubMenu_Items_selected_i)
                {
                    int j = 0;
                    void actions_remove() { guy.Inventory.Items.RemoveAt(guy.Inventory.Items.IndexOf(item)); }
                    void actions_remove_if_zero() { if (guy.Inventory.Items[guy.Inventory.Items.IndexOf(item)].Count == 0) actions_remove(); }
                    void actions_remove_one() { guy.Inventory.Items[guy.Inventory.Items.IndexOf(item)].Count--; actions_remove_if_zero(); }
                    Action Remove = () => actions_remove();
                    Action Consume = () => { item.Consume(); actions_remove_if_zero(); };
                    Action Drop1 = () => actions_remove_one();
                    Action DropAll = () => actions_remove();
                    var list_submenuitems = new Dictionary<string, Action> {
                        ["Consume"] = Consume,
                        ["Drop 1"] = Drop1,
                        ["Drop All"] = DropAll,
                        ["Remove"] = Remove,
                    };
                    var list_sz = list_submenuitems.Select(k => TextRenderer.MeasureText(k.Key, font));
                    int max_sz_w = list_sz.Max(sz => sz.Width);
                    int max_sz_h = list_sz.Max(sz => sz.Height);
                    void draw(string txt, Action action)
                    {
                        var r = new Rectangle(rect.X + 10, rect.Y + 10 + max_sz_h * j, max_sz_w, max_sz_h);
                        Brush brush = Brushes.Gray;
                        if (txt == "Consume" && !item.IsMenuConsommable) brush = new SolidBrush(Color.FromArgb(80, 80, 80));
                        else brush = r.Contains(ms) ? Brushes.White : Brushes.Gray;
                        gui.FillRectangle(makebrush(r), r);
                        gui.DrawString(txt, font, brush, r);
                        SubMenu_Items_dropdownitems[r] = action;
                        j++;
                    }
                    foreach(var submenuitem in list_submenuitems)
                        draw(submenuitem.Key, submenuitem.Key == "Consume" && !item.IsMenuConsommable ? null : submenuitem.Value);
                }
                i++;
            }
        }
        private void SubMenu_Tools_Draw(Graphics g)
        {
            var gui = Core.Instance.gui;
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
                rect = new Rectangle(x, y, 240, h);
                hover = i == SubMenu_Items_selected_i || (SubMenu_Items_selected_i == -1 && rect.Contains(ms));
                g.DrawString(tool.Name, font, hover ? Brushes.White : Brushes.Gray, new Rectangle(x, y, 140, 20));
                g.DrawString($"{tool.Count,14}", font, hover ? Brushes.White : Brushes.Gray, new Rectangle(x + 150, y, 100, 20));
                if (i == SubMenu_Items_selected_i)
                {
                    int j = 0;
                    Action Use = () => tool.Use(guy);
                    var list_submenuitems = new Dictionary<string, Action>
                    {
                        ["Use"] = Use,
                    };
                    var list_sz = list_submenuitems.Select(k => TextRenderer.MeasureText(k.Key, font));
                    int max_sz_w = list_sz.Max(sz => sz.Width);
                    int max_sz_h = list_sz.Max(sz => sz.Height);
                    void draw(string txt, Action action)
                    {
                        var r = new Rectangle(rect.X + 10, rect.Y + 10 + max_sz_h * j, max_sz_w, max_sz_h);
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
