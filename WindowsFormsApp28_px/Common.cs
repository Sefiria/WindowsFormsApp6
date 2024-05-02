using System.Collections.Generic;
using System.Drawing;
using System.Text.Json;
using System.Windows.Forms;
using Tooling;
using WindowsFormsApp28_px.matters.non_organic;
using WindowsFormsApp28_px.matters.organic;

namespace WindowsFormsApp28_px
{
    internal class Common
    {
        public static List<Segment> Segments;
        public static List<IMatter> Matters;
        public static List<Bullet> Bullets;
        public static Controllable Controllable;
        public static PointF Cam => new PointF(ScreenWidth / 2F, ScreenHeight / 2F).MinusF(Controllable.Point);
        public static PointF Center;
        public static int ScreenWidth, ScreenHeight;

        public static void Initialize(int w, int h)
        {
            ScreenWidth = w;
            ScreenHeight = h;
            Center = (w / 2F, h / 2F).P();

            //Segments = new List<Segment>
            //{
            //    new Segment(100, 50, 50, 100),
            //    new Segment(50, 100, 100, 200),
            //    new Segment(100, 200, 500, 300),
            //};
            var data = "{\"X\":-196,\"Y\":-200}&{\"X\":-196,\"Y\":97}|{\"X\":-196,\"Y\":97}&{\"X\":-70,\"Y\":96}|{\"X\":-70,\"Y\":96}&{\"X\":-67,\"Y\":259}|{\"X\":-67,\"Y\":259}&{\"X\":-399,\"Y\":256}|{\"X\":-399,\"Y\":256}&{\"X\":-398,\"Y\":22}|{\"X\":-398,\"Y\":22}&{\"X\":-571,\"Y\":21}|{\"X\":-571,\"Y\":21}&{\"X\":-566,\"Y\":481}|{\"X\":-566,\"Y\":481}&{\"X\":-417,\"Y\":484}|{\"X\":-417,\"Y\":484}&{\"X\":-417,\"Y\":401}|{\"X\":-417,\"Y\":401}&{\"X\":-273,\"Y\":401}|{\"X\":-273,\"Y\":401}&{\"X\":-275,\"Y\":502}|{\"X\":-275,\"Y\":502}&{\"X\":-342,\"Y\":499}|{\"X\":-342,\"Y\":499}&{\"X\":-342,\"Y\":636}|{\"X\":-342,\"Y\":636}&{\"X\":-68,\"Y\":632}|{\"X\":-68,\"Y\":632}&{\"X\":-78,\"Y\":513}|{\"X\":-78,\"Y\":513}&{\"X\":-199,\"Y\":511}|{\"X\":-199,\"Y\":511}&{\"X\":-197,\"Y\":405}|{\"X\":-197,\"Y\":405}&{\"X\":47,\"Y\":403}|{\"X\":47,\"Y\":403}&{\"X\":162,\"Y\":293}|{\"X\":162,\"Y\":293}&{\"X\":257,\"Y\":293}|{\"X\":257,\"Y\":293}&{\"X\":257,\"Y\":-1}|{\"X\":257,\"Y\":-1}&{\"X\":107,\"Y\":-141}|{\"X\":107,\"Y\":-141}&{\"X\":-73,\"Y\":-141}|{\"X\":-73,\"Y\":-141}&{\"X\":-138,\"Y\":-199}|{\"X\":-138,\"Y\":-199}&{\"X\":-196,\"Y\":-200}";
            LoadSegments(data);
            Controllable = new Controllable(0, 0, 16, Color.Cyan.ToArgb());
            Matters = new List<IMatter>
            {
                new Chest(150, 150, 20, 12, Segments[0].Angle, Color.Yellow.ToArgb(), ( 0x46, 0x01 )),
            };
            Bullets = new List<Bullet>();
        }
        /*

         */
        private static void LoadSegments(string data)
        {
            Segments = new List<Segment>();
            string[] segs = data.Split('|');
            foreach(var seg in segs)
            {
                string[] pts = seg.Split('&');
                PointF A = JsonSerializer.Deserialize<PointF>(pts[0]);
                PointF B = JsonSerializer.Deserialize<PointF>(pts[1]);
                if (A == B)
                    continue;
                Segments.Add(new Segment(A, B));
            }
        }

        public static void Update()
        {
            Controllable.Update();
            Matters.ForEach(e => e.Update());
            var bullets = new List<Bullet>(Bullets);
            bullets.ForEach(b => { if (!b.Exists) Bullets.Remove(b); else b.Update(); });
        }
        public static void Draw(Graphics g)
        {
            Controllable.Draw(g);
            Segments.ForEach(s => s.Draw(g, Cam));
            Matters.ForEach(e => e.Draw(g, Cam));
            Bullets.ForEach(b => b.Draw(g, Cam));
        }
        public static void DrawUI(Graphics g)
        {
            Matters.ForEach(e => e.DrawUI(g));

            DrawUI_Keys(g);

            var p = new Pen(Color.FromArgb(150, Color.Cyan));
            g.DrawLine(p, MouseStates.Position.PlusF(-10, 0), MouseStates.Position.PlusF(-4, 0));
            g.DrawLine(p, MouseStates.Position.PlusF(4, 0), MouseStates.Position.PlusF(10, 0));
            g.DrawLine(p, MouseStates.Position.PlusF(0, -10), MouseStates.Position.PlusF(0, -4));
            g.DrawLine(p, MouseStates.Position.PlusF(0, 4), MouseStates.Position.PlusF(0, 10));
        }

        private static void DrawUI_Keys(Graphics g)
        {
            Font fontMini = new Font("Courrier New", 8F);
            var h = ScreenHeight;

            Size sz = g.MeasureString("O", fontMini).ToSize();
            PointF text_loc;
            var (z, q, s, d) = KB.ZQSD();
            var ctrl = KB.IsKeyDown(KB.Key.LeftCtrl);
            var alt = KB.IsKeyDown(KB.Key.LeftAlt);
            var lc = MouseStates.ButtonsDown[MouseButtons.Left];
            var rc = MouseStates.ButtonsDown[MouseButtons.Right];

            var ui_ctrl = new { rect = new Rectangle(10, h - sz.Height - 50, sz.Width + 30, sz.Height + 10), text = "CTRL", pressed = ctrl };
            var ui_z = new { rect = new Rectangle(60, h - sz.Height - 50, sz.Width + 10, sz.Height + 10), text = "Z", pressed = z };
            var ui_alt = new { rect = new Rectangle(90, h - sz.Height - 50, sz.Width + 30, sz.Height + 10), text = "ALT", pressed = alt };
            var ui_q = new { rect = new Rectangle(30, h - sz.Height - 20, sz.Width + 10, sz.Height + 10), text = "Q", pressed = q };
            var ui_s = new { rect = new Rectangle(60, h - sz.Height - 20, sz.Width + 10, sz.Height + 10), text = "S", pressed = s };
            var ui_d = new { rect = new Rectangle(90, h - sz.Height - 20, sz.Width + 10, sz.Height + 10), text = "D", pressed = d };
            var ui_lc = new { rect = new Rectangle(140, h - sz.Height - 20, sz.Width + 30, sz.Height + 10), text = "Left", pressed = lc };
            var ui_rc = new { rect = new Rectangle(190, h - sz.Height - 20, sz.Width + 30, sz.Height + 10), text = "Right", pressed = rc };

            var ui_list = new List<dynamic> { ui_ctrl, ui_z, ui_alt, ui_q, ui_s, ui_d, ui_lc, ui_rc };

            foreach (var ui_item in ui_list)
            {
                text_loc = new PointF(ui_item.rect.X + 5, ui_item.rect.Y + 5);

                g.DrawRectangle(ui_item.pressed ? Pens.LightGray : Pens.DimGray, ui_item.rect);
                g.DrawString(ui_item.text, fontMini, ui_item.pressed ? Brushes.LightGray : Brushes.DimGray, text_loc);
            }
        }
    }
}
