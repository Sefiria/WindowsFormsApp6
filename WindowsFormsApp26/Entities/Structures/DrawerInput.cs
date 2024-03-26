using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Tooling;
using WindowsFormsApp26.Properties;

namespace WindowsFormsApp26.Entities.Structures
{
    internal class DrawerInput : Entity
    {
        private Size m_Size;
        private int m_Margin = 10;

        public new Size Size
        {
            get => m_Size;
            set
            {
                m_Size = value;
                Resize();
            }
        }
        public Bitmap DrawerContentImage;
        public Color BoundsColor = Color.FromArgb(96, 66, 155);
        public Point ButtonPosition;
        public int ButtonRadius;
        public int ProcesState = 0, ProcessTime = 100;
        private Point old_ms = Point.Empty;

        public Rectangle ButtonRect => new Rectangle(ButtonPosition.X - ButtonRadius, ButtonPosition.Y - ButtonRadius, ButtonRadius * 2, ButtonRadius * 2);
        public Rectangle AbsoluteButtonRect
        {
            get
            {
                var rect = ButtonRect;
                rect.Location = rect.Location.Plus((int)Position.X - m_Margin * 2, (int)Position.Y - m_Margin * 2);
                return rect;
            }
        }

        public DrawerInput() : base()
        {
            Initialize();
        }
        public DrawerInput(PointF position, Size size) : base(position)
        {
            Initialize();
            Size = size;
        }
        private void Initialize()
        {
            IsKinetic = true;
        }

        private void Resize()
        {
            Size SizeExt = Size.Add(Size, new Size(m_Margin * 2, m_Margin * 2));
            Image = new Bitmap(SizeExt.Width, SizeExt.Height);
            using (Graphics g = Graphics.FromImage(Image))
            {
                var path = Common.CreateRoundedRect(m_Margin, m_Margin, Size.Width, Size.Height + m_Margin, 10);
                g.FillPath(Brushes.White, path);
                g.DrawPath(new Pen(BoundsColor, m_Margin / 2), path);
                g.FillRectangle(Brushes.White, 0, m_Margin + Size.Height - 2, SizeExt.Width, m_Margin * 2);
                ButtonRadius = m_Margin / 2 - 1;
                ButtonPosition = new Point(Size.Width + m_Margin / 2 + ButtonRadius, m_Margin / 2 + 2 + ButtonRadius);
            }
            Image.MakeTransparent(Color.White);
            if (DrawerContentImage != null)
            {
                var temp = new Bitmap(Size.Width, Size.Height);
                using (Graphics g = Graphics.FromImage(temp))
                    g.DrawImage(DrawerContentImage, 0f, 0f);
                DrawerContentImage = new Bitmap(temp);
                temp.Dispose();
            }
            else
            {
                DrawerContentImage = new Bitmap(Size.Width, Size.Height);
                using (Graphics g = Graphics.FromImage(DrawerContentImage))
                    g.Clear(Color.White);
            }
        }

        public override void Update()
        {
            base.Update();
            var rect = AbsoluteButtonRect;
            if (ProcesState > 0)
            {
                ProcesState--;
                if (ProcesState == 0)
                    IsCollisionable = true;
                old_ms = Point.Empty;
            }
            else if (ProcesState == 0)
            {
                var ms = MouseStates.Position.ToPoint();
                if (rect.Contains(ms) && MouseStates.IsButtonDown(MouseButtons.Left))
                {
                    ProcesState += ProcessTime;
                    IsCollisionable = false;
                    Create();
                    old_ms = Point.Empty;
                }
                else if(rect.Contains(ms) && MouseStates.IsButtonDown(MouseButtons.Left))
                {

                }
                else
                {
                    drawer_function();
                }
            }
        }

        private void drawer_function()
        {
            var ms = MouseStates.Position.ToPoint();
            bool ms_left = MouseStates.IsButtonDown(MouseButtons.Left);
            bool ms_right = MouseStates.IsButtonDown(MouseButtons.Right);
            if ((ms_left || ms_right) && Bounds.Contains(ms))
            {
                if (old_ms != Point.Empty)
                {
                    var bounds = new Rectangle(Position.Minus(m_Margin), DrawerContentImage.Size);
                    if (bounds.Contains(old_ms) && bounds.Contains(ms))
                    {
                        Color c = ms_left ? Color.Black : Color.White;
                        Point old = new Point(old_ms.X - (int)X + m_Margin, old_ms.Y - (int)Y + m_Margin);
                        Point cur = new Point(ms.X - (int)X + m_Margin, ms.Y - (int)Y + m_Margin);
                        Point lerp;
                        List<Point> treated = new List<Point>();
                        for (float t = 0f; t <= 1f; t += 1F / Maths.Distance(old_ms, ms))
                        {
                            lerp = Maths.Lerp(old, cur, t).ToPoint();
                            if (treated.Contains(lerp))
                                continue;
                            treated.Add(lerp);
                            DrawerContentImage.SetPixel(lerp.X, lerp.Y, c);
                            if (lerp.X - 1 >= 0)
                            {
                                DrawerContentImage.SetPixel(lerp.X - 1, lerp.Y, c);
                                if (lerp.Y - 1 >= 0)
                                    DrawerContentImage.SetPixel(lerp.X - 1, lerp.Y - 1, c);
                            }
                            if (lerp.Y - 1 >= 0)
                                DrawerContentImage.SetPixel(lerp.X, lerp.Y - 1, c);
                            if (ms_right)
                            {
                                if (lerp.X + 1 < DrawerContentImage.Width)
                                {
                                    DrawerContentImage.SetPixel(lerp.X + 1, lerp.Y, c);
                                    if (lerp.Y + 1 < DrawerContentImage.Height)
                                        DrawerContentImage.SetPixel(lerp.X + 1, lerp.Y + 1, c);
                                    if (lerp.Y - 1 >= 0)
                                        DrawerContentImage.SetPixel(lerp.X + 1, lerp.Y - 1, c);
                                }
                                if (lerp.Y + 1 < DrawerContentImage.Height)
                                {
                                    DrawerContentImage.SetPixel(lerp.X, lerp.Y + 1, c);
                                    if (lerp.X - 1 >= 0)
                                        DrawerContentImage.SetPixel(lerp.X - 1, lerp.Y + 1, c);
                                }
                            }
                        }
                    }
                }
                old_ms = MouseStates.Position.ToPoint();
            }
            else
                old_ms = Point.Empty;
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(DrawerContentImage, Position.Minus(m_Margin));
            g.DrawImage(Image, Position.Minus(m_Margin * 2 + 1));

            var rect = AbsoluteButtonRect;
            Brush brush;
            if (ProcesState > 0) brush = Brushes.Lime;
            else if(rect.Contains(MouseStates.Position.ToPoint()))
            {
                if (MouseStates.IsButtonDown(MouseButtons.Left)) brush = Brushes.Lime;
                else brush = Brushes.White;
            }
            else brush = Brushes.Red;
            g.FillEllipse(brush, rect);
            g.DrawEllipse(Pens.Black, rect);
        }

        private void Create()
        {
            var e = new Entity {
                Position = Position.Minus(m_Margin),
                Size = DrawerContentImage.Size,
                Image = new Bitmap(DrawerContentImage),
            };
            e.Collider = new Collider(e.ID) { LocalBoxes = new List<Rectangle>() { new Rectangle(Point.Empty, DrawerContentImage.Size) } };
        }
    }
}
