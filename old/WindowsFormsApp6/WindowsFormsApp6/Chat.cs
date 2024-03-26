using SharedClasses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Chat : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        Rectangle Header = Rectangle.Empty;
        Cursor CursorDot, CursorMove;
        Timer AskOnlineUsersServer = new Timer() { Enabled = false, Interval = 5000 };


        public Chat()
        {
            InitializeComponent();
            Header = new Rectangle(0, 0, Width, Height / 10);
            CursorDot = new Cursor(Properties.Resources.CursorDot.Handle);
            CursorMove = new Cursor(Properties.Resources.CursorMove.Handle);
            btMinimize.Cursor = CursorDot;
            tbUserName.Text = MAIN.Instance.textBox1.Text;

            AskOnlineUsersServer.Tick += delegate
            {
                Net.SendMsg("GetUsers");
            };
            AskOnlineUsersServer.Start();
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            Graphics g = e.Graphics;
            TransparencyKey = Color.FromArgb(1, 1, 1);
            g.FillRectangle(new SolidBrush(TransparencyKey), 0, 0, Width, Height);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            float tension = 0.1F;

            // body

            Point[] shape = new[] {
                new Point(0, 50),
                new Point(5, 5),
                new Point(50, 0),
                new Point(Width - 50, 0),
                new Point(Width - 5, 5),
                new Point(Width, 50),
                new Point(Width, Height - 50),
                new Point(Width - 5, Height - 5),
                new Point(Width - 50, Height),
                new Point(50, Height),
                new Point(5, Height - 5),
                new Point(0, Height - 50),
            };

            g.FillClosedCurve(new SolidBrush(Color.FromArgb(58,58,58)), shape, System.Drawing.Drawing2D.FillMode.Alternate, tension);
            g.DrawClosedCurve(new Pen(Color.FromArgb(100, 100, 100), 5F), shape, tension, System.Drawing.Drawing2D.FillMode.Alternate);

            // header

            Point[] headerShape = new[] {
                new Point(0, 50),
                new Point(5, 5),
                new Point(50, 0),
                new Point(Header.Width - 50, 0),
                new Point(Header.Width - 5, 5),
                new Point(Header.Width, 50),
                new Point(Header.Width, Header.Height),
                new Point(0, Header.Height),
            };

            g.FillClosedCurve(new SolidBrush(Color.FromArgb(72, 72, 72)), headerShape, System.Drawing.Drawing2D.FillMode.Alternate, tension);
            g.DrawClosedCurve(new Pen(Color.FromArgb(100, 100, 120), 2F), headerShape, tension, System.Drawing.Drawing2D.FillMode.Alternate);
            g.DrawCurve(new Pen(Color.FromArgb(110, 110, 110), 2F), new[] { new Point(10, 20), new Point(12, 12), new Point(20, 10) }, 0, 2, tension);
        }

        private void Chat_MouseDown(object sender, MouseEventArgs e)
        {
            if (Header.Contains(e.Location))
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btMinimize_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Chat_SizeChanged(object sender, EventArgs e)
        {
            Refresh();
        }

        private void ChatBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(ChatBoxInput.Text))
            {
                string msg = tbUserName.Text + " : " + ChatBoxInput.Text;
                ChatBoxInput.Text = "";
                Net.SendMsg($"ChatboxMsg|{msg}");
            }
        }

        private void Chat_MouseMove(object sender, MouseEventArgs e)
        {
            Point ClientMouse = PointToClient(MousePosition);
            Cursor = btMinimize.Bounds.Contains(ClientMouse) ? CursorDot : ( Header.Contains(ClientMouse) ? CursorMove : CursorDot);
        }
    }
}
