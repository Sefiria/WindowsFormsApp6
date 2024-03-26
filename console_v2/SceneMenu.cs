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
                if (clicked != null) selectedButton = clicked;
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
        #endregion

        #region Sub Menus Update
        private void SubMenu_State_Update()
        {
        }
        private void SubMenu_Items_Update()
        {
        }
        private void SubMenu_Tools_Update()
        {
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
            Rectangle rect_guy = new Rectangle(mainrect.X + 20, mainrect.Y + 20, mainrect.Width - 40, 150);

            void draw(Guy guy)
            {
                g.FillRectangle(makeReversebrush(rect_guy), rect_guy);
                g.DrawString(string.Concat((char)guy.CharToDisplay), font, guy.CharBrush, rect_guy.Location.Plus(50, rect_guy.Height / 2 - (int)GraphicsManager.CharSize.Height / 2));
            }

            draw(tg);
        }
        private void SubMenu_Items_Draw(Graphics g)
        {
            var guy = Core.Instance.TheGuy;
            var items = guy.Inventory.Items;
            var font = new Font("Segoe UI", 14f);
            int x, y, w = 50, h = 25, i = 0;
            foreach (var item in items)
            {
                x = (i / (mainrect.Height / h)) * (w + 10);
                y = i * h - (x / w) * mainrect.Height;
                g.DrawString(item.Name, font, Brushes.White, new Rectangle(x, y, 140, 20));
                g.DrawString($"{item.Count,1}", font, Brushes.White, new Rectangle(x+150, y, 50, 20));
                i++;
            }
        }
        private void SubMenu_Tools_Draw(Graphics g)
        {
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
