using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Tooling;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace ToolingGranularity
{
    public partial class ToolRects : Form
    {
        public class EdgeCornerInfo
        {
            public int RectangleId;
            public ContentAlignment Kind;
            public Point Corner;
            public Line Edge = new Line();
            public EdgeCornerInfo() { }
            public EdgeCornerInfo(EdgeCornerInfo copy) { RectangleId = copy.RectangleId; Kind = copy.Kind; Corner = copy.Corner; }
            public EdgeCornerInfo(int id, ContentAlignment edgeCorner, Point cornerLocation) { RectangleId = id; Kind = edgeCorner; Corner = cornerLocation; }
            public EdgeCornerInfo(int id, ContentAlignment edgeCorner, Point edgeA, Point edgeB) { RectangleId = id; Kind = edgeCorner; Edge.A = edgeA; Edge.B = edgeB; }
            public EdgeCornerInfo(int id, ContentAlignment edgeCorner, Line edgeLocation) { RectangleId = id; Kind = edgeCorner; Edge = edgeLocation; }
            public EdgeCornerInfo Copy() => new EdgeCornerInfo(this);
            public bool SameAs(EdgeCornerInfo other) => RectangleId == other.RectangleId && ((Corner != Point.Empty && Corner == other.Corner) || (Edge.A != Point.Empty && Edge.B != Point.Empty && Edge.A == other.Edge.A && Edge.B == other.Edge.B));
            public bool IsEdge() => (Kind & (ContentAlignment.TopCenter | ContentAlignment.BottomCenter | ContentAlignment.MiddleLeft | ContentAlignment.MiddleRight)) != 0;
            public bool IsCorner() => (Kind & (ContentAlignment.TopLeft| ContentAlignment.BottomLeft | ContentAlignment.TopRight | ContentAlignment.BottomRight)) != 0;
        }
        public class SelectionInfo
        {
            public List<EdgeCornerInfo> EdgesCorners = new List<EdgeCornerInfo>();
            public IEnumerable<EdgeCornerInfo> Edges => EdgesCorners.Where(x => x.IsEdge());
            public IEnumerable<EdgeCornerInfo> COrners => EdgesCorners.Where(x => x.IsCorner());
            public SelectionInfo() { }
            public void Clear()
            {
                EdgesCorners.Clear();
            }
            public void Add(EdgeCornerInfo corner)
            {
                if (!EdgesCorners.Any(c => c.SameAs(corner)))
                    EdgesCorners.Add(corner);
            }
        }

        Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };

        public Bitmap RenderImage, UIImage;
        public Graphics g, gui;

        public long Ticks = 0;
        public List<Rectangle> LocalBoxes = new List<Rectangle>() { new Rectangle(100, 100, 100, 100) };
        public Color RainbowColor;
        public Pen RainbowPen;
        public SolidBrush RainbowBrush;
        float CornerRadius = 4f;
        SelectionInfo Selection = new SelectionInfo();
        bool MouseHold = false;
        int lastContainingBoxId = -1;
        const float SNAP_RANGE = 10F;
        Image Image = null;
        string Filename = null;

        #region Init
        public ToolRects(string filename)
        {
            Filename = filename;
            InitializeComponent();
            KB.Init();
            MouseStates.Initialize();
            update();
            TimerUpdate.Tick += Update;
            TimerDraw.Tick += Draw;
            Render.MouseUp += Render_MouseUp;
            Render.MouseDown += Render_MouseDown;
            Render.MouseMove += Render_MouseMove;

            if (!string.IsNullOrWhiteSpace(filename))
                LoadData();
        }

        private void Update(object sender, EventArgs e)
        {
            update();

            KB.Update();
            MouseStates.Update();
        }

        public void ResetGraphics()
        {
            RenderImage = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(RenderImage);
            UIImage = new Bitmap(Render.Width, Render.Height);
            gui = Graphics.FromImage(UIImage);
        }
        private void Draw(object sender, EventArgs e)
        {
            ResetGraphics();

            draw();

            g.DrawImage(UIImage, 0f, 0f);

            Render.Image = RenderImage;
        }


        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonsDown[e.Button] = false;
        }
        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            MouseStates.ButtonsDown[e.Button] = true;
            MouseDown(e);
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            MouseStates.OldPosition = MouseStates.Position;
            MouseStates.Position = e.Location;
            move();
        }
        #endregion

        private void update()
        {
            if (KB.LeftCtrl && KB.IsKeyPressed(KB.Key.S))
                SaveData();

            if (MouseStates.IsButtonPressed(MouseButtons.Middle) && LocalBoxes.Count > 1 && lastContainingBoxId != -1)
            {
                Rectangle box = new Rectangle(LocalBoxes[lastContainingBoxId].Location, LocalBoxes[lastContainingBoxId].Size);
                LocalBoxes.RemoveAt(lastContainingBoxId);
                LocalBoxes.Add(box);
                lastContainingBoxId = -1;
            }
                
            if (MouseHold && MouseStates.ReleasedButtons[MouseButtons.Left])
            {
                MouseHold = false;
                Selection.Clear();
                FixRectangles();
            }

            Ticks = Ticks == long.MaxValue ? Ticks = long.MinValue : Ticks + 1;
            RainbowColor = ColorExt.HSL2RGB((Ticks % 100D) / 100F, 0.5, 0.5);
            RainbowPen = new Pen(RainbowColor, 2f);
            RainbowBrush = new SolidBrush(RainbowColor);
        }
        /// <summary>
        /// When moving Left after Right side or Top after Bottom side, the width / height are negatives, let's fix that
        /// </summary>        
        private void FixRectangles()
        {
            int id;
            var boxes = new List<Rectangle>(LocalBoxes);
            foreach (Rectangle box in boxes)
            {
                id = LocalBoxes.IndexOf(box);
                if (box.Width < 0)
                    LocalBoxes[id] = new Rectangle(box.X + box.Width, box.Y, -box.Width, box.Height);
                if (box.Height < 0)
                    LocalBoxes[id] = new Rectangle(box.X, box.Y + box.Height, box.Width, -box.Height);
            }
        }
        private void move()
        {
            if (MouseStates.ButtonsDown[MouseButtons.Left])
            {
                MouseHold = true;
                Point delta = MouseStates.Position.MinusF(MouseStates.OldPosition).ToPoint();
                var ignore = false;
                new List<EdgeCornerInfo>(Selection.EdgesCorners).ForEach(info =>
                {
                    Rectangle box = LocalBoxes[info.RectangleId];
                    if (KB.LeftCtrl) ignore = Snap(info);
                    if(!ignore)
                    {
                        switch (info.Kind)
                        {
                            // Edges
                            case ContentAlignment.TopCenter:        LocalBoxes[info.RectangleId] = new Rectangle(box.X,                box.Y + delta.Y,  box.Width,               box.Height -  delta.Y); break;
                            case ContentAlignment.BottomCenter:   LocalBoxes[info.RectangleId] = new Rectangle(box.X,                box.Y,                box.Width,               box.Height + delta.Y); break;
                            case ContentAlignment.MiddleLeft:        LocalBoxes[info.RectangleId] = new Rectangle(box.X + delta.X, box.Y,                box.Width -  delta.X, box.Height);               break;
                            case ContentAlignment.MiddleRight:      LocalBoxes[info.RectangleId] = new Rectangle(box.X,               box.Y,                box.Width + delta.X, box.Height);               break;
                            // Corners
                            case ContentAlignment.TopLeft:            LocalBoxes[info.RectangleId] = new Rectangle(box.X + delta.X,  box.Y + delta.Y, box.Width -  delta.X, box.Height -  delta.Y); break;
                            case ContentAlignment.TopRight:          LocalBoxes[info.RectangleId] = new Rectangle(box.X,                box.Y + delta.Y, box.Width + delta.X, box.Height -  delta.Y); break;
                            case ContentAlignment.BottomLeft:       LocalBoxes[info.RectangleId] = new Rectangle(box.X + delta.X, box.Y,                box.Width -  delta.X, box.Height + delta.Y); break;
                            case ContentAlignment.BottomRight:     LocalBoxes[info.RectangleId] = new Rectangle(box.X,               box.Y,                box.Width + delta.X, box.Height + delta.Y); break;
                        }
                    }
                });
                if (lastContainingBoxId != -1)
                {
                    LocalBoxes[lastContainingBoxId] = LocalBoxes[lastContainingBoxId].WithOffset(delta);
                    if (KB.LeftCtrl) Snap(lastContainingBoxId);
                }
            }
        }
        private bool Snap(EdgeCornerInfo info)
        {
            var ms = MouseStates.Position;
            var box = LocalBoxes[info.RectangleId];
            float d = float.MaxValue;
            EdgeCornerInfo closest_edgecorner = null;
            float td;
            IEnumerable<EdgeCornerInfo> list;
            List<ContentAlignment> edgesVerticalAligns = new List<ContentAlignment> { ContentAlignment.MiddleLeft, ContentAlignment.MiddleRight };
            List<ContentAlignment> edgesHOrizontalAlign = new List<ContentAlignment> { ContentAlignment.TopCenter, ContentAlignment.BottomCenter };
            Line infoEdge = null;

            if (info.IsEdge())
            {
                infoEdge = box.GetEdge(info.Kind);
                List<ContentAlignment> edgesAlign;
                if (info.Kind == ContentAlignment.TopCenter || info.Kind == ContentAlignment.BottomCenter)
                    edgesAlign = edgesHOrizontalAlign;
                else //if (info.Kind == ContentAlignment.MiddleLeft || info.Kind == ContentAlignment.MiddleRight)
                    edgesAlign = edgesVerticalAligns;
                list = LocalBoxes.SelectMany(_box => edgesAlign.Select(c => new EdgeCornerInfo(LocalBoxes.IndexOf(_box), c, _box.GetEdge(c))));
            }
            else //if (info.IsCorner())
                list = LocalBoxes.SelectMany(_box => Ext.GetCornersAlign().Select(c => new EdgeCornerInfo(LocalBoxes.IndexOf(_box), c, _box.GetCorner(c))));

            foreach(var item in list)
            {
                if (item.RectangleId == info.RectangleId)
                    continue;
                Line itemEdge = null;
                if (info.IsEdge())
                {
                    itemEdge = LocalBoxes[item.RectangleId].GetEdge(item.Kind);
                    td = Maths.Abs(edgesVerticalAligns.Contains(info.Kind) ? ms.X - itemEdge.A.X : ms.Y - itemEdge.A.Y);
                }
                else
                    td = Maths.Distance(ms, item.Corner);
                if (td < d)
                {
                    d = td;
                    closest_edgecorner = item;
                }
            }
            if (d < SNAP_RANGE)
            {
                int x = info.IsEdge() ? closest_edgecorner.Edge.A.X : closest_edgecorner.Corner.X;
                int y = info.IsEdge() ? closest_edgecorner.Edge.A.Y : closest_edgecorner.Corner.Y;
                switch (info.Kind)
                {
                    // Edges
                    case ContentAlignment.TopCenter: LocalBoxes[info.RectangleId] = new Rectangle(box.X, y, box.Width, box.Height + (box.Y - y)); break;
                    case ContentAlignment.BottomCenter: LocalBoxes[info.RectangleId] = new Rectangle(box.X, box.Y, box.Width, y - box.Y); break;
                    case ContentAlignment.MiddleLeft: LocalBoxes[info.RectangleId] = new Rectangle(x, box.Y, box.Width + (box.X - x), box.Height); break;
                    case ContentAlignment.MiddleRight: LocalBoxes[info.RectangleId] = new Rectangle(box.X, box.Y, x - box.X, box.Height); break;
                    // Corners
                    case ContentAlignment.TopLeft: LocalBoxes[info.RectangleId] = new Rectangle(closest_edgecorner.Corner, box.Size); break;
                    case ContentAlignment.TopRight: LocalBoxes[info.RectangleId] = new Rectangle(closest_edgecorner.Corner.Minus(box.Width, 0), box.Size); break;
                    case ContentAlignment.BottomLeft: LocalBoxes[info.RectangleId] = new Rectangle(x, box.Y, box.Width, box.Height + (y - box.Bottom)); break;
                    case ContentAlignment.BottomRight: LocalBoxes[info.RectangleId] = new Rectangle(box.X, box.Y, box.Width + (x - box.Right), box.Height + (y - box.Bottom)); break;
                }
                return true;
            }
            else
            {
                switch (info.Kind)
                {
                    case ContentAlignment.TopCenter:        LocalBoxes[info.RectangleId] = new Rectangle((int)ms.X, box.Y, box.Width, box.Height); break;
                    case ContentAlignment.BottomCenter:   LocalBoxes[info.RectangleId] = new Rectangle(box.X, box.Y, (int)ms.X - box.X, box.Height); break;
                    case ContentAlignment.MiddleLeft:        LocalBoxes[info.RectangleId] = new Rectangle((int)ms.X, box.Y, box.Width, box.Height); break;
                    case ContentAlignment.MiddleRight:      LocalBoxes[info.RectangleId] = new Rectangle(box.X, box.Y, (int)ms.X - box.X, box.Height); break;
                }
            }
                return false;
        }
        private void Snap(int id)
        {
        }
        private void draw()
        {
            if (Image != null)
                g.DrawImage(Image, Point.Empty);

            bool mouse_hover;
            Rectangle rect;
            void draw_line(Rectangle box, ContentAlignment kind)
            {
                Line edge = box.GetEdge(kind);
                var look = edge.B.Minus(edge.A);
                if (look.X > look.Y) rect = new Rectangle(edge.A.X, edge.A.Y - (int)CornerRadius, look.X, (int)(CornerRadius * 2f));
                else rect = new Rectangle(edge.A.X - (int)CornerRadius, edge.A.Y, (int)(CornerRadius * 2f), look.Y);
                mouse_hover = rect.Contains(MouseStates.Position.ToPoint());
                var edgeinfo = new EdgeCornerInfo(LocalBoxes.IndexOf(box), kind, edge.A, edge.B);
                if (mouse_hover && Selection.EdgesCorners.All(ec => ec.IsEdge()) && lastContainingBoxId == -1)
                {
                    g.DrawLine(new Pen(RainbowPen.Color, 4f), edge.A, edge.B);
                    if (!MouseHold)
                        Selection.Add(edgeinfo);
                }
                else
                {
                    g.DrawLine(Pens.Black, edge.A, edge.B);
                    if (!MouseHold)
                        Selection.EdgesCorners.RemoveAll(edgecorner => edgecorner.SameAs(edgeinfo));
                }
            }

            // Corners
            var cornersAlignments = new List<ContentAlignment>() { ContentAlignment.TopLeft, ContentAlignment.TopRight, ContentAlignment.BottomLeft, ContentAlignment.BottomRight };
            var corners = LocalBoxes.SelectMany(box => cornersAlignments.Select(align => new EdgeCornerInfo(LocalBoxes.IndexOf(box), align, box.GetCorner(align))));
            foreach (EdgeCornerInfo info in corners)
            {
                rect = new RectangleF(info.Corner.X - CornerRadius, info.Corner.Y - CornerRadius, CornerRadius * 2, CornerRadius * 2).ToIntRect();
                var rectext = new RectangleF(rect.Location, rect.Size);
                rectext.Inflate(CornerRadius, CornerRadius);
                if (rectext.Contains(MouseStates.Position) && !MouseHold && lastContainingBoxId == -1)
                {
                    g.FillEllipse(RainbowBrush, rectext);
                    if (!MouseHold)
                        Selection.Add(info.Copy());
                    Selection.EdgesCorners.RemoveAll(ec => ec.IsEdge());
                }
                else if (!MouseHold)
                {
                    g.FillEllipse(Brushes.Black, rect);
                    if (!MouseHold)
                        Selection.EdgesCorners.RemoveAll(edgecorner => edgecorner.SameAs(info));
                }
            }

            // Edges
            var edges = new List<ContentAlignment>() { ContentAlignment.TopCenter, ContentAlignment.BottomCenter, ContentAlignment.MiddleLeft, ContentAlignment.MiddleRight };
            LocalBoxes.ForEach(box => edges.ForEach(edge => draw_line(box, edge)));

            // Rectangle
            if (Selection.EdgesCorners.Count == 0)
            {
                var ms = MouseStates.Position.ToPoint();
                var ids = LocalBoxes.Where(b => b.Contains(ms)).Select(_b =>LocalBoxes.IndexOf(_b));
                if (ids.Count() == 0) lastContainingBoxId = -1;
                else if (!ids.Contains(lastContainingBoxId)) lastContainingBoxId = ids.ElementAt(0);
                if (lastContainingBoxId != -1)
                    g.FillRectangle(RainbowBrush, LocalBoxes[lastContainingBoxId]);
                else
                    LocalBoxes.ForEach(box => g.FillRectangle(new SolidBrush(Color.FromArgb(100, RainbowBrush.Color)), box));
            }
        }

        private void MouseDown(MouseEventArgs _e)
        {
            if(_e.Button == MouseButtons.Right)
            {
                var ms = _e.Location;
                int id = LocalBoxes.IndexOf(LocalBoxes.FirstOrDefault(b => b.Contains(ms)));
                if (id != -1)
                {
                    var menu = new ContextMenuStrip();
                    menu.Items.Add("Split 4").Click += (_, e) => Split("4", id);
                    menu.Items.Add(new ToolStripSeparator());
                    menu.Items.Add("Remove").Click += (_, e) => { LocalBoxes.RemoveAt(id); Selection.Clear(); MouseHold = false; };
                    menu.Show(MousePosition);
                }
                else
                {
                    var menu = new ContextMenuStrip();
                    menu.Items.Add("Create any rectangle").Click += (_, e) => LocalBoxes.Add(new Rectangle(ms.X - 50, ms.Y - 50, 100, 100));
                    menu.Show(MousePosition);
                }
            }
        }
        private void Split(string mode,  int id)
        {
            var box = LocalBoxes[id];
            int w = box.Width;
            int h = box.Height;
            int hw = w / 2;
            int hh = h / 2;
            switch (mode)
            {
                default: return;
                case "4":
                    LocalBoxes.RemoveAt(id);
                    LocalBoxes.Add(new Rectangle(box.X, box.Y, hw, hh));
                    LocalBoxes.Add(new Rectangle(box.X + hw, box.Y, hw, hh));
                    LocalBoxes.Add(new Rectangle(box.X, box.Y + hh, hw, hh));
                    LocalBoxes.Add(new Rectangle(box.X + hw, box.Y + hh, hw, hh));
                    break;
            }
        }

        #region File
        public void LoadData()
        {
            if (string.IsNullOrWhiteSpace(Filename))
                return;
            var img = Filename;
            var cld = $"{Path.GetFileNameWithoutExtension(Filename)}.{Common.COLLIDER_FILE_EXTENSION}";
            if (File.Exists(img))
                Image = Image.FromFile(img);
            if (!File.Exists(cld))
                return;
            byte[] data = File.ReadAllBytes(cld);

            LocalBoxes.Clear();

            const uint MINIMUM_VERSION = 0;

            const uint CLDR = ('C') + ('L' << 8) + ('D' << 16) + ('R' << 24);

            using (var ms = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(ms))
                {
                    if (reader.ReadUInt32() != CLDR) return;

                    uint version = reader.ReadUInt32();
                    if (version < MINIMUM_VERSION) return;

                    uint count = reader.ReadUInt32();
                    for (int i = 0; i < count; i++)
                    {
                        uint x = reader.ReadUInt32();
                        uint y = reader.ReadUInt32();
                        uint w = reader.ReadUInt32();
                        uint h = reader.ReadUInt32();
                        LocalBoxes.Add(new Rectangle((int)x, (int)y, (int)w, (int)h));
                    }
                }
            }
        }
        public void SaveData()
        {
            const uint MINIMUM_VERSION = 0;

            const uint CLDR = ('C') + ('L' << 8) + ('D' << 16) + ('R' << 24);

            if (string.IsNullOrWhiteSpace(Filename))
            {
                var dialog = new SaveFileDialog();
                dialog.InitialDirectory = Directory.GetCurrentDirectory();
                if (dialog.ShowDialog(this) == DialogResult.OK)
                    Filename = dialog.FileName;
                else
                    return;
            }

            var cld = $"{Path.GetFileNameWithoutExtension(Filename)}.{Common.COLLIDER_FILE_EXTENSION}";

            using (var stream = File.OpenWrite(cld))
            {
                using (var writer = new BinaryWriter(stream))
                {
                    writer.Write(CLDR);
                    writer.Write(MINIMUM_VERSION);
                    writer.Write(LocalBoxes.Count);
                    foreach(var box in LocalBoxes)
                    {
                        writer.Write(box.X);
                        writer.Write(box.Y);
                        writer.Write(box.Width);
                        writer.Write(box.Height);
                    }
                }
            }
        }
        #endregion
    }
}
