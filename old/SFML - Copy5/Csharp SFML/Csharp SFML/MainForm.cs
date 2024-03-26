using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;

using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using sfColor = SFML.Graphics.Color;
using sfImage = SFML.Graphics.Image;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace Csharp_SFML
{
    public partial class MainForm : Form
    {
        private class InstantiatedItem
        {
            public string Name;
            public Sprite Value;
            public int index;

            public InstantiatedItem(InstantiatedItem instantiatedItem, bool reallocateSprite = false)
            {
                Name = instantiatedItem.Name;
                Value = reallocateSprite ? new Sprite(instantiatedItem.Value) : instantiatedItem.Value;
            }
            public InstantiatedItem(string name, Sprite value)
            {
                Name = name; Value = value;
            }
            public override string ToString()
            {
                return Name;
            }
        }
        private class InstantiatedItemCollection
        {
            public string Name;
            public List<InstantiatedItem> collection;

            public InstantiatedItemCollection(string name)
            {
                Name = name;
                collection = new List<InstantiatedItem>();
            }
            public InstantiatedItemCollection(string name, List<InstantiatedItem> collectionToCopy)
            {
                Name = name;
                collection = new List<InstantiatedItem>();
                InstantiatedItem newItem;
                foreach (var item in collectionToCopy)
                {
                    newItem = new InstantiatedItem(item, true);
                    newItem.index = item.index;
                    collection.Add(newItem);
                }
            }
            public int AddItem(InstantiatedItem item)
            {
                item.index = GetLastIndex() + 1;
                collection.Add(item);
                return collection.Count - 1;
            }
            public List<InstantiatedItem> GetCollectionCopy()
            {
                return new InstantiatedItemCollection("", collection).collection;
            }
            public int GetLastIndex()
            {
                int lastID = 0;
                foreach (var item in collection)
                    lastID = item.index > lastID ? item.index : lastID;
                return lastID;
            }
            public bool IndexExists(int index)
            {
                foreach (var item in collection)
                    if(index == item.index)
                        return true;
                return false;
            }
            public List<int> GetIndexes()
            {
                var list = new List<int>();
                foreach (var item in collection)
                    list.Add(item.index);
                return list;
            }
            public override string ToString()
            {
                return Name;
            }
        }
        private class TimelineKey
        {
            public DateTime timePosition;
            List<Sprite> parts;

            public TimelineKey()
            {
                timePosition = DateTime.Parse("00:00:00.0000");
                parts = new List<Sprite>();
            }
            public TimelineKey(TimelineKey key)
            {
                timePosition = key.timePosition;
                parts = new List<Sprite>(key.parts);
            }
            public TimelineKey(DateTime _timePosition, List<Sprite> _parts)
            {
                timePosition = _timePosition;
                parts = new List<Sprite>(_parts);
            }
            public TimelineKey(string timePositionFormatted, List<Sprite> _parts = null)
            {
                timePosition = DateTime.Parse(timePositionFormatted);
                parts = new List<Sprite>(_parts == null ? new List<Sprite>() : _parts);
            }
            public override string ToString()
            {
                return "{ Time:'"+timePosition+"' Parts Count:'"+parts.Count+"' }";
            }

            public void ParseTimePosition(string timePositionFormatted)
            {
                timePosition = DateTime.Parse(timePositionFormatted);
            }
        }

        #region Variables

        public RenderWindow renderwindow;
        public RenderWindow renderCreatePart, renderCreatePartWindowed;
        Point lastMousePosition;
        uint penSize = 1;
        Sprite spPenA = new Sprite(), spPenB = new Sprite();
        bool firstTick = true, shouldResetPivot = false;
        Bitmap RenderPartCursor;
        PictureBox draggingPart = null;
        Sprite draggingPartInRender = null;
        Sprite spRenderSelectionBox, spRenderPivot;
        Sprite spTarget = null;
        Point ShiftMouseAtFirstClick = Point.Empty;
        InstantiatedItem copiedItem = null;
        int lastPaletteColorIndex = 0;
        List<Button> palette = new List<Button>();
        sfImage keepPart;
        Form RenderForm = null, RenderLerpForm = null;
        RenderWindow RenderView = null, RenderViewLerp = null;
        int renderViewIndexCurrentFrame = 0, renderViewLerpIndexCurrentFrame = 0;
        int emulatedTimer = 0;
        List<InstantiatedItem> RenderLerpViewItems = new List<InstantiatedItem>();
        SFML.Graphics.View view = new SFML.Graphics.View(new FloatRect(0, 0, 100, 100));
        Size partSize;
        int snapSize;
        public static sfColor TransparentColor = new sfColor(255, 255, 255);
        Vector2f ratio;
        Color colorPicking = Color.Transparent;
        int buttonClickedWhilePicking = 0;

        #endregion

        public MainForm()
        {
            InitializeComponent();

            renderwindow = new RenderWindow(RenderPanel.Handle);
            renderCreatePart = new RenderWindow(RenderPart.Handle);
            renderCreatePartWindowed = null;

            newToolStripMenuItem_Click(0, null);
            snapSize = Math.Max(partSize.Width, partSize.Height);

            RenderPanel.MouseWheel += RenderPanel_MouseWheel;
            RenderPanel.MouseUp += RenderPanel_MouseUp;
            RenderPart.MouseWheel += RenderPart_OnMouseWheel;
            RenderPart.MouseUp += RenderPart_MouseUp;

            SetPen();
            panelPalette.BackColor = Color.FromArgb(220, 220, 220);
            AddPaletteColorFromA();
            AddPaletteColorFromB();

            //TimelineInitialize();

            timer1.Enabled = true;

            numPartPositionX.Controls[0].Visible = false;
            numPartPositionY.Controls[0].Visible = false;
            numPartAngle.Controls[0].Visible = false;
            numPartScaleX.Controls[0].Visible = false;
            numPartScaleY.Controls[0].Visible = false;
            numPartAreaSizeWidth.Controls[0].Visible = false;
            numPartAreaSizeHeight.Controls[0].Visible = false;
        }

        private void SetPen()
        {
            sfColor colA = new sfColor(btDrawPart_ColorA.BackColor.R, btDrawPart_ColorA.BackColor.G, btDrawPart_ColorA.BackColor.B, btDrawPart_ColorA.BackColor.A);
            sfColor colB = new sfColor(btDrawPart_ColorB.BackColor.R, btDrawPart_ColorB.BackColor.G, btDrawPart_ColorB.BackColor.B, btDrawPart_ColorB.BackColor.A);
            sfImage imgA = new sfImage(penSize, penSize, sfColor.Transparent);
            sfImage imgB = new sfImage(penSize, penSize, sfColor.Transparent);

            uint x, y;
            for (float a = 0F; a <= 360F; a += 0.1F)
            for (float p = 0F; p <= penSize / 2; p += 1F)
            {
                x = (uint)((penSize / 2) + (float)Math.Cos(a * (3.1456 / 180.0)) * p);
                y = (uint)((penSize / 2) + (float)Math.Sin(a * (3.1456 / 180.0)) * p);
                if (x >= 0 && x < penSize && y >= 0 && y < penSize)
                {
                    imgA.SetPixel(x, y, colA);
                    imgB.SetPixel(x, y, colB);
                }
            }

            Texture txA = new Texture(imgA);
            Texture txB = new Texture(imgB);
            spPenA.TextureRect = new IntRect(0, 0, (int)penSize, (int)penSize);
            spPenB.TextureRect = new IntRect(0, 0, (int)penSize, (int)penSize);
            spPenA.Texture = txA;
            spPenB.Texture = txB;
            spPenA.Origin = new Vector2f(penSize == 1 ? 0 : penSize / 2F, penSize == 1 ? 0 : penSize / 2F);
            spPenB.Origin = new Vector2f(penSize == 1 ? 0 : penSize / 2F, penSize == 1 ? 0 : penSize / 2F);

            int sizeX = (int)(ratio.X * penSize);
            int sizeY = (int)(ratio.Y * penSize);
            RenderPartCursor = new Bitmap(sizeX, sizeY, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(RenderPartCursor))
            {
                SolidBrush b = new SolidBrush(Color.FromArgb(200, 200, 200, 200));
                g.FillEllipse(b, 0F, 0F, sizeX, sizeY);
            }
            RenderPart.Cursor = new System.Windows.Forms.Cursor(RenderPartCursor.GetHicon());
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            CheckPickColor();

            Application.DoEvents();
            renderwindow.DispatchEvents();
            renderCreatePart.DispatchEvents();

            foreach (InstantiatedItem item in listPartsInstantiated.Items)
                renderwindow.Draw(item.Value);
            if (spRenderSelectionBox != null)
            {
                renderwindow.Draw(spRenderSelectionBox);
                renderwindow.Draw(spRenderPivot);



                Sprite sp = (listPartsInstantiated.Items[0] as InstantiatedItem).Value;
                Point spPosition = new Point((int)sp.Position.X - (int)sp.Origin.X, (int)sp.Position.Y - (int)sp.Origin.Y);
                Point RotatedMouse = RotatedPoint(GetWorldMouse(), new Point(spPosition.X, spPosition.Y), sp.Rotation);
                var rect = new IntRect(sp.TextureRect.Left - (int)sp.Origin.X, sp.TextureRect.Top - (int)sp.Origin.Y, (int)(sp.TextureRect.Width * sp.Scale.X), (int)(sp.TextureRect.Height * sp.Scale.Y));
                var mouse = new Vector2i(RotatedMouse.X - spPosition.X, spPosition.Y - RotatedMouse.Y);

                sfImage img = new sfImage(10, 10, sfColor.Red);
                var temp = new Sprite(new Texture(img));
                temp.Position = new Vector2f(mouse.X, mouse.Y);
                renderwindow.Draw(temp);

                

            }
            if (spTarget != null)
            {
                spTarget.Position = new Vector2f(renderwindow.Size.X / 2F + (float)nudTargetX.Value, renderwindow.Size.Y / 2F + (float)nudTargetY.Value);
                renderwindow.Draw(spTarget);
            }
            renderwindow.Display();
            renderwindow.Clear(TransparentColor);

            if (firstTick)
            {
                renderCreatePart.Display();
                renderCreatePart.Clear(TransparentColor);
                renderCreatePart.Display();
                renderCreatePart.Clear(TransparentColor);
                firstTick = false;
            }

            if(draggingPart != null && !Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                if (shouldResetPivot)
                {
                    shouldResetPivot = false;
                    btPartPivotReset_Click(null, null);
                }
                draggingPart.Parent = null;
                draggingPart = null;
                MouseMoveOnAll(false);
                renderCreatePart.Display();
                renderCreatePart.Display();
                SetPen();
                RenderPanel.Select();
            }
        }
        private double Distance(Point A, Point B)
        {
            return Math.Sqrt(Math.Pow(B.X - A.X, 2) + Math.Pow(B.Y - A.Y, 2));
        }
        private Point Lerp(Point A, Point B, double t)
        {
            return new Point((int)((1.0 - t) * A.X + t * B.X), (int)((1.0 - t) * A.Y + t * B.Y));
        }
        private void DrawPen(Sprite pen, Point position)
        {
            Point snappedPosition = GetSnappedPositionPart(position);
            pen.Position = new Vector2f(snappedPosition.X, snappedPosition.Y);
            pen.Scale = ratio;
            renderCreatePart.Draw(pen);
            renderCreatePart.Display();
            renderCreatePart.Draw(pen);
            renderCreatePart.Display();

            if(renderCreatePartWindowed != null)
            {
                position = GetSnappedPositionCustomRatio(position, new Size((int)renderCreatePartWindowed.Size.X, (int)renderCreatePartWindowed.Size.Y));
                pen.Position = new Vector2f(position.X, position.Y);
                var ratioWindowed = new Vector2f(renderCreatePartWindowed.Size.X / (float)partSize.Width, renderCreatePartWindowed.Size.Y / (float)partSize.Height);
                pen.Scale = ratioWindowed;

                renderCreatePartWindowed.Draw(pen);
                renderCreatePartWindowed.Display();
                renderCreatePartWindowed.Draw(pen);
                renderCreatePartWindowed.Display();
            }
        }
        private void RedrawTarget()
        {
            if (nudTargetWidth.Value <= 0 || nudTargetHeight.Value <= 0 || nudTargetBorderSize.Value == 0M)
            {
                spTarget = null;
                return;
            }

            var img = new sfImage((uint)nudTargetWidth.Value, (uint)nudTargetHeight.Value, sfColor.Transparent);
            var color = ToSFColor(btTargetColor.BackColor);
            for (uint x = 0; x < img.Size.X; x++)
            {
                for (uint b = 0; b < nudTargetBorderSize.Value; b++)
                {
                    img.SetPixel(x, b, color);
                    img.SetPixel(x, img.Size.Y - 1 - b, color);
                }
            }
            for (uint y = 0; y < img.Size.Y; y++)
            {
                for (uint b = 0; b < nudTargetBorderSize.Value; b++)
                {
                    img.SetPixel(b, y, color);
                    img.SetPixel(img.Size.X - 1 - b, y, color);
                }
            }
            spTarget = new Sprite();
            spTarget.Texture = new Texture(img);
        }

        private void AddPaletteColorFromA()
        {
            AddPaletteColorFromColor(btDrawPart_ColorA.BackColor);
        }
        private void AddPaletteColorFromB()
        {
            AddPaletteColorFromColor(btDrawPart_ColorB.BackColor);
        }
        private void AddPaletteColorFromColor(Color col)
        {
            if(!CheckPaletteColorExists(col))
            {
                Button bt = new Button();
                bt.BackColor = col;
                int border = 2;
                //int numColomnPerRow = 10;
                //int boxSize = (panelPalette.ClientSize.Width - border * 2 - SystemInformation.VerticalScrollBarWidth) / numColomnPerRow - border;
                int boxSize = 20;
                int numColomnPerRow = (panelPalette.ClientSize.Width - border * 2 - SystemInformation.VerticalScrollBarWidth) / (boxSize + border);
                bt.Size = new Size(boxSize, boxSize);
                bt.Location = new Point(border + lastPaletteColorIndex % numColomnPerRow * (boxSize + border), border + lastPaletteColorIndex / numColomnPerRow * (boxSize + border));
                bt.MouseDown += new MouseEventHandler((s, e) => {
                    /*-*/if (Mouse.IsButtonPressed(Mouse.Button.Left))   { btDrawPart_ColorA.BackColor = (s as Button).BackColor; SetPen(); }
                    else if (Mouse.IsButtonPressed(Mouse.Button.Right))  { btDrawPart_ColorB.BackColor = (s as Button).BackColor; SetPen(); }
                    else if (Mouse.IsButtonPressed(Mouse.Button.Middle)) { palette.Remove(s as Button); ResortPalette(); };
                });
                lastPaletteColorIndex++;
                palette.Add(bt);
                bt.Parent = panelPalette;
            }
        }
        private void ResortPalette()
        {
            var bckp = new List<Button>(palette);
            panelPalette.Controls.Clear();
            palette.Clear();
            lastPaletteColorIndex = 0;
            foreach (var bt in bckp)
                AddPaletteColorFromColor(bt.BackColor);
        }
        private bool CheckPaletteColorExists(Color col)
        {
            foreach (var pb in palette)
                if (pb.BackColor.ToArgb() == col.ToArgb())
                    return true;
            return false;
        }
        private void btDrawPart_ColorA_Click(object sender, EventArgs e)
        {
            ColorDialog diag = new ColorDialog();
            diag.Color = btDrawPart_ColorA.BackColor;
            diag.ShowDialog(this);
            btDrawPart_ColorA.BackColor = diag.Color;
            SetPen();
            AddPaletteColorFromA();
        }
        private void btDrawPart_ColorB_Click(object sender, EventArgs e)
        {
            ColorDialog diag = new ColorDialog();
            diag.Color = btDrawPart_ColorB.BackColor;
            diag.ShowDialog(this);
            btDrawPart_ColorB.BackColor = diag.Color;
            SetPen();
            AddPaletteColorFromB();
        }

        private void RenderPart_MouseDown(object sender, MouseEventArgs e)
        {
            OnCreatePartMouseDown(ratio);
        }
        private void renderCreatePartWindowed_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            OnCreatePartMouseDown(new Vector2f(renderCreatePartWindowed.Size.X / (float)partSize.Width, renderCreatePartWindowed.Size.Y / (float)partSize.Height));
        }
        private void OnCreatePartMouseDown(Vector2f currentRatio)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.LAlt))
                return;

            if (Mouse.IsButtonPressed(Mouse.Button.Left) && draggingPart == null)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
                {
                    listPartsInstantiated.ClearSelected();
                    draggingPart = new PictureBox();
                    sfImage sourceImg = GetRenderPartMinimalImage();
                    if (sourceImg == null)
                        return;
                    if (sourceImg.Size.X == 0 || sourceImg.Size.Y == 0)
                    {
                        draggingPart.Parent = null;
                        draggingPart = null;
                        MouseMoveOnAll(false);
                        return;
                    }
                    Bitmap img = new Bitmap((int)sourceImg.Size.X, (int)sourceImg.Size.Y);
                    sfColor pixel;
                    Color col;
                    for (int x = 0; x < img.Size.Width; x++)
                    {
                        for (int y = 0; y < img.Size.Height; y++)
                        {
                            pixel = sourceImg.GetPixel((uint)x, (uint)y);
                            col = Color.FromArgb(127, 255 - pixel.R, 255 - pixel.G, 255 - pixel.B);
                            img.SetPixel(x, y, pixel == TransparentColor ? Color.Transparent : col);
                        }
                    }
                    draggingPart.Size = new Size(50, 50);
                    draggingPart.BackgroundImageLayout = ImageLayout.Stretch;
                    draggingPart.BackgroundImage = img;
                    draggingPart.BackColor = Color.Transparent;
                    draggingPart.Parent = this;
                    draggingPart.BringToFront();
                    MouseMoveOnAll(true);

                    RenderPartCursor = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    RenderPart.Cursor = new System.Windows.Forms.Cursor(RenderPartCursor.GetHicon());
                }
                else
                    OnCreatePartMouseMove(currentRatio);
            }
            else
                if (Mouse.IsButtonPressed(Mouse.Button.Right) && !Keyboard.IsKeyPressed(Keyboard.Key.LControl))
                    OnCreatePartMouseMove(currentRatio);
        }
        private void MouseMoveOnAll(bool state)
        {
            foreach (Control child in Controls)
            {
                if (state)
                    child.MouseMove += MainForm_MouseMove;
                else
                    child.MouseMove -= MainForm_MouseMove;

                MouseMoveOnAll(state, child);
            }
        }
        private void MouseMoveOnAll(bool state, Control who)
        {
            foreach(Control child in who.Controls)
            {
                if(state)
                    child.MouseMove += MainForm_MouseMove;
                else
                    child.MouseMove -= MainForm_MouseMove;

                MouseMoveOnAll(state, child);
            }
        }
        private void RenderPart_MouseMove(object sender, MouseEventArgs e)
        {
            OnCreatePartMouseMove(ratio);
        }
        private void renderCreatePartWindowed_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            OnCreatePartMouseMove(new Vector2f(renderCreatePartWindowed.Size.X / (float)partSize.Width, renderCreatePartWindowed.Size.Y / (float)partSize.Height));
        }
        private void OnCreatePartMouseMove(Vector2f currentRatio)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.LAlt))
                return;

            bool LeftClick = Mouse.IsButtonPressed(Mouse.Button.Left);
            bool RightClick = Mouse.IsButtonPressed(Mouse.Button.Right);

            if ((LeftClick || RightClick) && !Keyboard.IsKeyPressed(Keyboard.Key.LControl) && draggingPart == null)
            {
                Point position = new Point();
                if (currentRatio == ratio)
                    position = RenderPart.PointToClient(MousePosition);
                else
                {
                    var pos = Mouse.GetPosition(renderCreatePartWindowed);
                    position = new Point(pos.X, pos.Y);
                }
                if (position.X / currentRatio.X >= 0 && position.X / currentRatio.X < partSize.Width && position.Y / currentRatio.Y >= 0 && position.Y / currentRatio.Y < partSize.Height)
                {
                    if (lastMousePosition == new Point(-1, -1))
                    {
                        if (LeftClick)
                            DrawPen(spPenA, position);
                        else if (RightClick)
                            DrawPen(spPenB, position);
                    }
                    else
                    {
                        Point lerpPos;
                        for (double t = 0.0; t < 1.0; t += 1.0 / Distance(lastMousePosition, position))
                        {
                            lerpPos = Lerp(lastMousePosition, position, t);
                            if (LeftClick)
                                DrawPen(spPenA, lerpPos);
                            else if (RightClick)
                                DrawPen(spPenB, lerpPos);
                        }
                    }
                    lastMousePosition = position;
                }
            }
            else
            {
                lastMousePosition = new Point(-1, -1);
            }
        }
        private void RenderPart_MouseUp(object sender, MouseEventArgs e)
        {
            OnCreatePartMouseUp();
        }
        private void RenderCreatePartWindowed_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            OnCreatePartMouseUp();
        }
        private void OnCreatePartMouseUp()
        {
            if (listPartsInstantiated.SelectedItem != null)
            {
                sfImage img = GetRenderPartMinimalImage();
                if (img == null)
                    return;
                Texture tx_render = new Texture(img);
                Sprite sp_render = new Sprite(tx_render);
                sp_render.Position = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value.Position - (listPartsInstantiated.SelectedItem as InstantiatedItem).Value.Origin;
                sp_render.Rotation = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value.Rotation;
                sp_render.Scale = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value.Scale;
                (listPartsInstantiated.SelectedItem as InstantiatedItem).Value = sp_render;
                SelectPart(listPartsInstantiated.SelectedIndex);
            }
        }
        private void RenderPart_OnMouseWheel(object sender, MouseEventArgs e)
        {
            OnCreatePartMouseWheel(e.Delta);
        }
        private void RenderCreatePartWindowed_MouseWheelMoved(object sender, MouseWheelEventArgs e)
        {
            OnCreatePartMouseWheel(e.Delta);
        }
        private void OnCreatePartMouseWheel(int delta)
        {
            if (delta > 0)
            {
                if (penSize * 2 >= 1 && penSize * 2 <= 128)
                {
                    penSize *= 2;
                    SetPen();
                }
            }
            else if (delta < 0)
            {
                if (penSize / 2 >= 1 && penSize / 2 <= 128)
                {
                    penSize /= 2;
                    SetPen();
                }
            }
        }
        private sfImage GetRenderPartMinimalImage()
        {
            sfImage result;
            sfImage img = renderCreatePart.Capture();

            int minX = -1, minY = -1, maxX = -1, maxY = -1;
            for (uint x = 0; x < img.Size.X; x++)
            {
                for (uint y = 0; y < img.Size.Y; y++)
                {
                    if (img.GetPixel(x, y) != TransparentColor)
                    {
                        if (minX == -1 || x < minX)
                            minX = (int)x;
                        if (minY == -1 || y < minY)
                            minY = (int)y;

                        if (maxX == -1 || x > maxX)
                            maxX = (int)x;
                        if (maxY == -1 || y > maxY)
                            maxY = (int)y;
                    }
                }
            }

            if (minX == -1 || minY == -1)
                return null;

            uint border = 2;
            uint deltaX = (uint)(maxX - minX + border * 2);
            uint deltaY = (uint)(maxY - minY + border * 2);
            if (deltaX > img.Size.X - 1) deltaX = img.Size.X - 1;
            if (deltaY > img.Size.Y - 1) deltaY = img.Size.Y - 1;
            result = new sfImage(deltaX, deltaY, sfColor.Transparent);
            bool atLeastOnePixelDraw = false;
            for (uint x = 0; x < result.Size.X; x++)
                for (uint y = 0; y < result.Size.Y; y++)
                    if (img.GetPixel((uint)minX + x, (uint)minY + y) != TransparentColor)
                    {
                        atLeastOnePixelDraw = true;
                        result.SetPixel(x, y, img.GetPixel((uint)minX + x, (uint)minY + y));
                    }

            return atLeastOnePixelDraw ? result : null;
        }
        private void CheckPickColor()
        {
            renderCreatePart.Display();
            renderCreatePart.Display();

            bool LeftClick = Mouse.IsButtonPressed(Mouse.Button.Left);
            bool RightClick = Mouse.IsButtonPressed(Mouse.Button.Right);
            if ((LeftClick || RightClick) && Keyboard.IsKeyPressed(Keyboard.Key.LAlt))
            {
                // color picker

                var pos = RenderPart.PointToClient(MousePosition);
                if (!RenderPart.ClientRectangle.Contains(pos))
                {
                    colorPicking = Color.Transparent;
                    btDrawPart_ColorPicker.BackColor = colorPicking;
                    return;
                }

                var sfc = renderCreatePart.Capture().GetPixel((uint)pos.X, (uint)pos.Y);
                btDrawPart_ColorPicker.BackColor = Color.FromArgb(sfc.A, sfc.R, sfc.G, sfc.B);
                colorPicking = btDrawPart_ColorPicker.BackColor;
                buttonClickedWhilePicking = LeftClick ? 1 : 2;

                return;
            }

            if (colorPicking != Color.Transparent)
            {
                AddPaletteColorFromColor(colorPicking);
                if (buttonClickedWhilePicking == 1)
                    btDrawPart_ColorA.BackColor = colorPicking;
                else
                    btDrawPart_ColorB.BackColor = colorPicking;
                colorPicking = Color.Transparent;
                btDrawPart_ColorPicker.BackColor = colorPicking;
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggingPart != null)
            {
                Point SnapMouse = GetSnappedRenderPanelClientMouse();

                if (RenderPanel.ClientRectangle.Contains(SnapMouse))
                {
                    draggingPart.Visible = false;
                    if (draggingPartInRender == null)
                    {
                        sfImage img = GetRenderPartMinimalImage();
                        if (img == null)
                            return;
                        if (img.Size.X == 0 || img.Size.Y == 0)
                        {
                            draggingPart.Parent = null;
                            draggingPart = null;
                            MouseMoveOnAll(false);
                            return;
                        }
                        Texture tx = new Texture(img);
                        draggingPartInRender = new Sprite(tx);
                        SelectPart(AddPart(new InstantiatedItem(tbPartName.Text, draggingPartInRender)));
                    }
                    Point SnappedPosition = GetSnappedRenderPanelClientPosition(new Point(MousePosition.X - draggingPart.Size.Width / 2, MousePosition.Y - draggingPart.Size.Height / 2));
                    var SnappedPosition2F = new Vector2f(SnappedPosition.X, SnappedPosition.Y);
                    draggingPartInRender.Origin = new Vector2f(draggingPartInRender.Texture.Size.X / 2F, draggingPartInRender.Texture.Size.Y / 2F);
                    draggingPartInRender.Position = SnappedPosition2F + draggingPartInRender.Origin;
                    if (spRenderSelectionBox != null)
                    {
                        spRenderSelectionBox.Position = draggingPartInRender.Position - draggingPartInRender.Origin;
                        spRenderPivot.Position = draggingPartInRender.Position;
                    }
                    shouldResetPivot = true;
                }
                else
                {
                    draggingPart.Visible = true;
                    draggingPart.Location = new Point(PointToClient(MousePosition).X - draggingPart.Size.Width / 2, PointToClient(MousePosition).Y - draggingPart.Size.Height / 2);
                    if (draggingPartInRender != null)
                    {
                        RemovePart(listPartsInstantiated.SelectedItem as InstantiatedItem);
                        listPartsInstantiated.ClearSelected();
                        draggingPartInRender = null;
                    }
                }
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (RenderPanel.ClientRectangle.Contains(RenderPanel.PointToClient(MousePosition)))
            {
                if (listPartsInstantiated.SelectedIndex > -1)
                {
                    if ((keyData == Keys.Back || keyData == Keys.Delete))
                    {
                        RemovePart(listPartsInstantiated.Items[listPartsInstantiated.SelectedIndex] as InstantiatedItem);
                        SelectPart(-1);
                        return true;
                    }

                    if (keyData == (Keys.Control | Keys.C))
                    {
                        copiedItem = new InstantiatedItem(listPartsInstantiated.SelectedItem as InstantiatedItem, true);
                        return true;
                    }
                }

                if (keyData == (Keys.Control | Keys.V) && copiedItem != null)
                {
                    int newItem = AddPart(copiedItem);
                    listPartsInstantiated_SelectItemIndex(newItem);
                    var sp = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value;
                    sp.Position = new Vector2f(RenderPanel.PointToClient(MousePosition).X, RenderPanel.PointToClient(MousePosition).Y);
                    copiedItem = new InstantiatedItem(listPartsInstantiated.SelectedItem as InstantiatedItem, true);
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void RenderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            timerMouseMovePart.Start();
            SelectPart(CheckWhatPartClicked());
        }
        private void RenderPanel_MouseUp(object sender, MouseEventArgs e)
        {
            timerMouseMovePart.Stop();
            if (ShiftMouseAtFirstClick != Point.Empty && e.Button == MouseButtons.Left)
                ShiftMouseAtFirstClick = Point.Empty;
        }
        private void timerMouseMovePartAndPivot_Tick(object sender, EventArgs e)
        {
            if (listPartsInstantiated.SelectedIndex != -1)
            {
                Point SnapMouse = GetSnappedRenderPanelClientPosition(GetWorldMouse());
                var sp = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value;

                if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
                {
                    // Pivot Point

                    Point MouseClient = GetWorldMouse();
                    Point MouseClientRotated = MouseClient;
                    double angle = -sp.Rotation * (Math.PI / 180);
                    MouseClientRotated.X = ((int)(MouseClientRotated.X * Math.Cos(angle) - MouseClientRotated.Y * Math.Sin(angle)));
                    MouseClientRotated.Y = ((int)(MouseClientRotated.Y * Math.Cos(angle) + MouseClient.X * Math.Sin(angle)));
                    if (ShiftMouseAtFirstClick == Point.Empty)
                        ShiftMouseAtFirstClick = new Point(MouseClientRotated.X - (int)sp.Origin.X, MouseClientRotated.Y - (int)sp.Origin.Y);
                    else if (MouseClientRotated != ShiftMouseAtFirstClick)
                    {
                        Vector2f newOrigin = new Vector2f(MouseClientRotated.X - ShiftMouseAtFirstClick.X, MouseClientRotated.Y - ShiftMouseAtFirstClick.Y);
                        Point RotatedNewOriginOnly = RotatedPoint(new Point((int)newOrigin.X, (int)newOrigin.Y), new Point((int)sp.Origin.X, (int)sp.Origin.Y), -sp.Rotation);
                        Point RotatedFinalOrigin = new Point((int)sp.Origin.X - RotatedNewOriginOnly.X, RotatedNewOriginOnly.Y - (int)sp.Origin.Y);
                        sp.Position -= new Vector2f(RotatedFinalOrigin.X, RotatedFinalOrigin.Y);
                        sp.Origin = newOrigin;
                        spRenderSelectionBox.Position = sp.Position;
                        spRenderSelectionBox.Origin = sp.Origin;
                        spRenderPivot.Position = spRenderSelectionBox.Position;

                        numPartPositionX.Value = (decimal)sp.Position.X;
                        numPartPositionY.Value = (decimal)sp.Position.Y;
                    }

                }
                else
                {
                    // Move Part

                    Point MouseClient = GetWorldMouse();
                    if (ShiftMouseAtFirstClick == Point.Empty)
                    {
                        ShiftMouseAtFirstClick = new Point(MouseClient.X - (int)sp.Position.X, MouseClient.Y - (int)sp.Position.Y);
                        if (cbSnap.Checked)
                            ShiftMouseAtFirstClick = GetSnappedPosition(ShiftMouseAtFirstClick);
                    }
                    else if(MouseClient != ShiftMouseAtFirstClick)
                    {
                        sp.Position = new Vector2f(MouseClient.X - ShiftMouseAtFirstClick.X, MouseClient.Y - ShiftMouseAtFirstClick.Y);
                        if (cbSnap.Checked)
                            sp.Position = GetSnappedPosition(sp.Position);
                        spRenderSelectionBox.Position = sp.Position;
                        spRenderSelectionBox.Origin = sp.Origin;
                        spRenderPivot.Position = spRenderSelectionBox.Position;

                        numPartPositionX.Value = (decimal)sp.Position.X;
                        numPartPositionY.Value = (decimal)sp.Position.Y;
                    }

                }
            }
        }
        private int CheckWhatPartClicked()
        {
            for (int i=0; i< listPartsInstantiated.Items.Count; i++)
                if (CheckPartClicked(i))
                    return i;
            return -1;
        }
        private bool CheckPartClicked(int indexPart)
        {
            Sprite sp = (listPartsInstantiated.Items[0] as InstantiatedItem).Value;
            Point spPosition = new Point((int)sp.Position.X - (int)sp.Origin.X, (int)sp.Position.Y - (int)sp.Origin.Y);
            Point RotatedMouse = RotatedPoint(GetWorldMouse(), new Point(spPosition.X, spPosition.Y), sp.Rotation);
            var rect = new IntRect(sp.TextureRect.Left, sp.TextureRect.Top - (int)sp.Origin.Y, (int)(sp.TextureRect.Width * sp.Scale.X), (int)(sp.TextureRect.Height * sp.Scale.Y));
            var mouse = new Vector2i(RotatedMouse.X - spPosition.X, spPosition.Y - RotatedMouse.Y);
            return rect.Contains(mouse.X, mouse.Y);
        }
        private void SelectPart(int partIndex)
        {
            listPartsInstantiated.ClearSelected();
            spRenderSelectionBox = null;
            spRenderPivot = null;
            btPartPivotReset.Enabled = false;
            nudPartID.Enabled = false;

            if (partIndex > -1)
            {
                btPartPivotReset.Enabled = true;
                nudPartID.Enabled = true;
                listPartsInstantiated.SelectedIndexChanged -= listPartsInstantiated_SelectedIndexChanged;
                listPartsInstantiated_SelectItemIndex(partIndex);

                this.nudPartID.ValueChanged -= new System.EventHandler(this.nudPartID_ValueChanged);
                nudPartID.Value = (listPartsInstantiated.SelectedItem as InstantiatedItem).index;
                this.nudPartID.ValueChanged += new System.EventHandler(this.nudPartID_ValueChanged);

                listPartsInstantiated.SelectedIndexChanged += listPartsInstantiated_SelectedIndexChanged;
                Sprite sp = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value;
                sfImage img = new sfImage(sp.Texture.Size.X, sp.Texture.Size.Y, sfColor.Transparent);
                for (uint x = 0; x < img.Size.X; x++)
                {
                    img.SetPixel(x, 0, sfColor.Cyan);
                    img.SetPixel(x, img.Size.Y - 1, sfColor.Cyan);
                }
                for (uint y = 0; y < img.Size.Y; y++)
                {
                    img.SetPixel(0, y, sfColor.Cyan);
                    img.SetPixel(img.Size.X - 1, y, sfColor.Cyan);
                }
                Texture tx = new Texture(img);
                spRenderSelectionBox = new Sprite(tx);
                spRenderSelectionBox.Position = sp.Position;
                spRenderSelectionBox.Rotation = sp.Rotation;
                spRenderSelectionBox.Scale = sp.Scale;
                spRenderSelectionBox.Origin = sp.Origin;

                float pivotRadius = 10F;
                {
                    uint x, y;
                    img = new sfImage((uint)pivotRadius * 2, (uint)pivotRadius * 2, sfColor.Transparent);
                    for (float a = 0F; a <= 360F; a += 0.1F)
                    {
                        x = (uint)((pivotRadius) + (float)Math.Cos(a * (3.1456 / 180.0)) * pivotRadius);
                        y = (uint)((pivotRadius) + (float)Math.Sin(a * (3.1456 / 180.0)) * pivotRadius);
                        if (x >= 0 && x < pivotRadius * 2 && y >= 0 && y < pivotRadius * 2)
                            img.SetPixel(x, y, sfColor.Cyan);

                        for (float p = 0F; p <= pivotRadius / 3; p += 1F)
                        {
                            x = (uint)((pivotRadius) + (float)Math.Cos(a * (3.1456 / 180.0)) * p);
                            y = (uint)((pivotRadius) + (float)Math.Sin(a * (3.1456 / 180.0)) * p);
                            if (x >= 0 && x < pivotRadius * 2 && y >= 0 && y < pivotRadius * 2)
                                img.SetPixel(x, y, new sfColor(0, 255, 255, 150));
                        }
                    }
                    tx = new Texture(img);
                    spRenderPivot = new Sprite(tx);
                    spRenderPivot.Position = spRenderSelectionBox.Position;
                    spRenderPivot.Origin = new Vector2f(tx.Size.X / 2, tx.Size.Y / 2);
                }

                if (draggingPart == null)
                {
                    btKeepPartClear_Click(null, null);
                    Sprite spPart = new Sprite(sp);
                    spPart.Position = GetSnappedPosition(new Vector2f( renderCreatePart.Size.X / 2 - spPart.TextureRect.Width  / 2,
                                                                       renderCreatePart.Size.Y / 2 - spPart.TextureRect.Height / 2));
                    spPart.Position += spPart.Origin;
                    spPart.Rotation = 0F;
                    spPart.Scale = new Vector2f(1F, 1F);
                    renderCreatePart.Draw(spPart);
                    renderCreatePart.Display();
                    renderCreatePart.Draw(spPart);
                    renderCreatePart.Display();
                }
                
                numPartPositionX.Enabled = true;
                numPartPositionY.Enabled = true;
                numPartAngle.Enabled = true;
                numPartScaleX.Enabled = true;
                numPartScaleY.Enabled = true;
                numPartPositionX.Value = (decimal)sp.Position.X;
                numPartPositionY.Value = (decimal)sp.Position.Y;
                numPartAngle.Value = (decimal)sp.Rotation;
                numPartScaleX.Value = (decimal)sp.Scale.X;
                numPartScaleY.Value = (decimal)sp.Scale.Y;
                if(listPartsInstantiated.SelectedIndex > -1)
                    tbPartName.Text = (listPartsInstantiated.SelectedItem as InstantiatedItem).Name;
            }
            else
            {
                // where nothing is clicked (unselect)

                btKeepPartClear_Click(null, null);
                numPartPositionX.Value = 0M;
                numPartPositionY.Value = 0M;
                numPartAngle.Value = 0M;
                numPartScaleX.Value = 1M;
                numPartScaleY.Value = 1M;
                numPartPositionX.Enabled = false;
                numPartPositionY.Enabled = false;
                numPartAngle.Enabled = false;
                numPartScaleX.Enabled = false;
                numPartScaleY.Enabled = false;
                tbPartName.Text = "Unnamed";
            }
        }

        static Point RotatedPoint(Point pointToRotate, Point centerPoint, double angleInDegrees)
        {
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            double cosTheta = Math.Cos(angleInRadians);
            double sinTheta = Math.Sin(angleInRadians);
            return new Point
            {
                X =
                    (int)
                    (cosTheta * (pointToRotate.X - centerPoint.X) +
                    sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                Y =
                    (int)
                    (sinTheta * (pointToRotate.X - centerPoint.X) -
                    cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            };
        }
        private void RenderPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            if(listPartsInstantiated.SelectedIndex != -1)
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.LAlt))
                {
                    // scale 

                    if (e.Delta > 0 && numPartScaleX.Value * 2 < numPartScaleX.Maximum)
                    {
                        numPartScaleX.Value *= 2;
                        numPartScaleY.Value *= 2;
                    }
                    else if (e.Delta < 0 && numPartScaleX.Value / 2 > numPartScaleX.Minimum)
                    {
                        numPartScaleX.Value /= 2;
                        numPartScaleY.Value /= 2;
                    }
                    else
                        return;

                    spRenderSelectionBox.Scale = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value.Scale = new Vector2f((float)numPartScaleX.Value, (float)numPartScaleY.Value);

                }
                else
                {
                    // pivot

                    float value = Math.Sign(e.Delta) * (Keyboard.IsKeyPressed(Keyboard.Key.LControl) ? 1 : 10);
                    (listPartsInstantiated.SelectedItem as InstantiatedItem).Value.Rotation += value;
                    spRenderSelectionBox.Rotation = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value.Rotation;

                    if ((float)numPartAngle.Value + value < 0F)
                         numPartAngle.Value = 360 - numPartAngle.Value + (decimal)value;
                    else if ((float)numPartAngle.Value + value >= 360F)
                         numPartAngle.Value += (decimal)value - 360;
                    else
                        numPartAngle.Value += (decimal)value;
                }
            }
        }
        
        private void tbPartName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbPartName.Text))
                tbPartName.Text = "Unnamed";

            if(listPartsInstantiated.SelectedIndex > -1)
                if ((listPartsInstantiated.SelectedItem as InstantiatedItem).Name != tbPartName.Text)
                    btPartNameApply.Enabled = true;
        }

        private void listPartsInstantiated_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete) && listPartsInstantiated.SelectedIndex > -1)
                RemovePart(listPartsInstantiated.Items[listPartsInstantiated.SelectedIndex] as InstantiatedItem);
        }
        private void listPartsInstantiated_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listPartsInstantiated.Items.Count > 0 && listPartsInstantiated.SelectedIndex >= 0 && listPartsInstantiated.SelectedIndex < listPartsInstantiated.Items.Count)
                SelectPart(listPartsInstantiated.Items.IndexOf(listPartsInstantiated.SelectedItem as InstantiatedItem));
        }
        private void listPartsInstantiated_SelectItemIndex(int index)
        {
            if (index < 0)
            {
                listPartsInstantiated.ClearSelected();
                return;
            }

            if (index >= listPartsInstantiated.Items.Count) index = listPartsInstantiated.Items.Count - 1;

            listPartsInstantiated.SelectedIndex = index;

            if (listPartsInstantiated.SelectedIndex == -1)
                listPartsInstantiated.ClearSelected();
        }
        private void btPartUnselect_Click(object sender, EventArgs e)
        {
            SelectPart(-1);
        }

        private void btKeepPartClear_Click(object sender, EventArgs e)
        {
            renderCreatePart.Clear(TransparentColor);
            renderCreatePart.Display();
            renderCreatePart.Clear(TransparentColor);
            renderCreatePart.Display();
        }
        private void btKeepPartSave_Click(object sender, EventArgs e)
        {
            keepPart = renderCreatePart.Capture();
            if (!btKeepPartLoad.Enabled)
            {
                btKeepPartLoad.Enabled = true;
                btKeepPartLoad.BackgroundImage = Properties.Resources.KeepPart_Load;
            }
        }
        private void btKeepPartLoad_Click(object sender, EventArgs e)
        {
            renderCreatePart.Clear();
            Texture tx = new Texture(keepPart);
            Sprite sp = new Sprite(tx);
            renderCreatePart.Draw(sp);
            renderCreatePart.Display();
            renderCreatePart.Draw(sp);
            renderCreatePart.Display();
        }

        private void TimelineInitialize()
        {
            Timeline.MouseWheel += Timeline_MouseWheel;

            Timeline.ChartAreas[0].AxisY.Minimum = 0;
            Timeline.ChartAreas[0].AxisY.Maximum = 1;
            Timeline.ChartAreas[0].AxisY.IsReversed = true;
            Timeline.ChartAreas[0].AxisX.IsMarksNextToAxis = false;
            Timeline.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = true;
            Timeline.ChartAreas[0].AxisX.LabelStyle.Format = "ss.fff";
            Timeline.ChartAreas[0].AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Milliseconds;
            Timeline.ChartAreas[0].AxisX.LabelStyle.Interval = 500;
            Timeline.ChartAreas[0].AxisX.Minimum = DateTime.Parse("00:00:00.000").ToOADate();
            Timeline.ChartAreas[0].AxisX.Maximum = DateTime.Parse("00:00:02.000").ToOADate();
            Timeline.Series[0].XValueType = ChartValueType.DateTime;
            Timeline.Series[0].Points.AddXY(DateTime.Parse("00:00:01.000"), 0.5);
            Timeline.Series[0].SmartLabelStyle.AllowOutsidePlotArea = LabelOutsidePlotAreaStyle.Yes;
            Timeline.Series[0].SmartLabelStyle.IsMarkerOverlappingAllowed = false;
            Timeline.Series[0].SmartLabelStyle.MovingDirection = LabelAlignmentStyles.Top;

            // Enable range selection and zooming end user interface
            Timeline.ChartAreas[0].CursorX.IsUserEnabled = true;
            Timeline.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            Timeline.ChartAreas[0].CursorX.Interval = 0;
            Timeline.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            Timeline.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            Timeline.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
            Timeline.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 0;
            Timeline.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSizeType = DateTimeIntervalType.Milliseconds;
            Timeline.ChartAreas[0].AxisX.ScaleView.SmallScrollSizeType = DateTimeIntervalType.Milliseconds;

            Timeline.MouseUp += Timeline_MouseUp;
        }
        private void Timeline_MouseUp(object sender, MouseEventArgs e)
        {/*
            HitTestResult result = Timeline.HitTest(e.X, e.Y);
            if (result.ChartElementType == ChartElementType.DataPoint)
                if (result.PointIndex != -1)
                    Console.WriteLine(DateTime.FromOADate(Timeline.Series[0].Points[result.PointIndex].XValue).ToString(Timeline.ChartAreas[0].AxisX.LabelStyle.Format));*/


            // Call Hit Test Method
            HitTestResult hitResult = Timeline.HitTest(e.X, e.Y);

            // Initialize currently selected data point
            DataPoint selectedDataPoint = null;
            if (hitResult.ChartElementType == ChartElementType.DataPoint)
            {
                if (hitResult.PointIndex != 1)
                {
                    selectedDataPoint = (DataPoint)hitResult.Object;
                }
                else
                {
                    //get the xValue of where the mouse is
                    double xValue = Timeline.ChartAreas["Area1"].AxisX.PixelPositionToValue(e.X);
                    // if the mouse is within a certain range from the first point in the series then 
                    // the selected point is the first point
                    if ((xValue - hitResult.Series.Points[0].XValue) < 0.25)
                        selectedDataPoint = hitResult.Series.Points[0];
                }
                // Show point value as label
                selectedDataPoint.IsValueShownAsLabel = true;

                // Set cursor shape
                Timeline.Cursor = Cursors.SizeNS;
            }
        }
        private void Timeline_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            HitTestResult result = Timeline.HitTest(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.DataPoint)
                if (result.PointIndex == -1)
                //if (result.PointIndex < 0 || result.PointIndex >= Timeline.Series[0].Points.Count)
                    if (Timeline.ChartAreas[0].CursorX.SelectionEnd - Timeline.ChartAreas[0].CursorX.SelectionStart == 0D)
                        Timeline.Series[0].Points.AddXY(Timeline.ChartAreas[0].CursorX.Position, 0.5);
        }
        private void Timeline_MouseWheel(object sender, MouseEventArgs e)
        {
            if(e.Delta < 0)
                Timeline.ChartAreas[0].AxisX.ScaleView.ZoomReset(1);
        }
        private void Timeline_SelectionRangeChanged(object sender, CursorEventArgs e)
        {
            double xMin = DateTime.FromOADate(Timeline.ChartAreas[0].AxisX.Minimum).Millisecond;
            double xMax = DateTime.FromOADate(Timeline.ChartAreas[0].AxisX.Maximum).Millisecond;
            Timeline.ChartAreas[0].AxisX.LabelStyle.Interval = (xMax - xMin) / 5;
        }

        private void listFrames_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listFrames.SelectedIndex != -1)
            {
                listPartsInstantiated.Items.Clear();
                foreach (var item in (listFrames.SelectedItem as InstantiatedItemCollection).collection)
                    listPartsInstantiated.Items.Add(item);
            }
        }
        private void btMoveUp_Click(object sender, EventArgs e)
        {
            if (listFrames.SelectedIndex > 0)
            {
                var itemToMove = listFrames.SelectedItem as InstantiatedItemCollection;
                var index = listFrames.Items.IndexOf(itemToMove);
                listFrames.Items.Remove(itemToMove);
                listFrames.Items.Insert(index - 1, itemToMove);
                listFrames.SelectedIndex = index - 1;
            }
        }
        private void btNewFrame_Click(object sender, EventArgs e)
        {
            int newItem = -1;
            if (listFrames.SelectedIndex != -1)
            {
                newItem = listFrames.SelectedIndex + 1;
                if (listFrames.Items.Count > 0)
                    listFrames.Items.Insert(newItem, new InstantiatedItemCollection(tbFrameName.Text, (listFrames.SelectedItem as InstantiatedItemCollection).collection));
                else
                    listFrames.Items.Insert(newItem, new InstantiatedItemCollection(tbFrameName.Text));
            }
            else
            {
                if (listFrames.Items.Count > 0)
                    newItem = listFrames.Items.Add(new InstantiatedItemCollection(tbFrameName.Text, (listFrames.SelectedItem as InstantiatedItemCollection).collection));
                else
                    newItem = listFrames.Items.Add(new InstantiatedItemCollection(tbFrameName.Text));

            }

                listFrames.SelectedItem = listFrames.Items[newItem];
            tbFrameName.Text = "Frame " + listFrames.Items.Count;
        }
        private void btMoveDown_Click(object sender, EventArgs e)
        {
            if (listFrames.SelectedIndex + 1 < listFrames.Items.Count)
            {
                var itemToMove = listFrames.SelectedItem as InstantiatedItemCollection;
                var index = listFrames.Items.IndexOf(itemToMove);
                listFrames.Items.Remove(itemToMove);
                listFrames.Items.Insert(index + 1, itemToMove);
                listFrames.SelectedIndex = index + 1;
            }
        }
        private void btRemoveFrame_Click(object sender, EventArgs e)
        {
            if (listFrames.Items.Count > 1)
            {
                var itemToRemove = listFrames.SelectedItem as InstantiatedItemCollection;
                var index = listFrames.Items.IndexOf(itemToRemove);
                listFrames.Items.Remove(itemToRemove);
                if (index < listFrames.Items.Count)
                    listFrames.SelectedIndex = index;
                else
                    listFrames.SelectedIndex = index - 1;
                tbFrameName.Text = "Frame " + listFrames.Items.Count;
            }
            else
            {
                (listFrames.Items[0] as InstantiatedItemCollection).Name = tbFrameName.Text;
                (listFrames.Items[0] as InstantiatedItemCollection).collection.Clear();
                listFrames.Items[0] = listFrames.Items[0];
            }
        }
        private void btRender_Click(object sender, EventArgs e)
        {
            if(RenderForm == null)
            {
                RenderForm = new Form();
                RenderForm.Disposed += delegate (object _s, EventArgs _e) {
                    timerRenderView.Stop();
                    RenderForm = null;
                    RenderView = null;
                };

                RenderForm.Text = "RenderView";
                RenderForm.Icon = Icon;
                RenderForm.ClientSize = RenderPanel.ClientSize;
                Panel panel = new Panel();
                panel.ClientSize = RenderForm.ClientSize;
                panel.Parent = RenderForm;
                RenderView = new RenderWindow(panel.Handle);
                var view = new SFML.Graphics.View(new FloatRect(RenderView.Size.X / 2F + (float)nudTargetX.Value,
                                                                RenderView.Size.Y / 2F + (float)nudTargetY.Value,
                                                                (float)nudTargetWidth.Value,
                                                                (float)nudTargetHeight.Value));
                RenderView.SetView(view);
                RenderView.Clear(TransparentColor);
                RenderView.Display();
                RenderView.Clear(TransparentColor);
                RenderView.Display();

                RenderForm.Show(this);
                timerRenderView.Start();
            }
            else
            {
                RenderForm.Dispose();
            }
        }
        private void btRenderLerp_Click(object sender, EventArgs e)
        {
            if (RenderLerpForm == null)
            {
                RenderLerpForm = new Form();
                RenderLerpForm.Disposed += delegate (object _s, EventArgs _e){
                    timerRenderViewLerp.Stop();
                    RenderLerpForm = null;
                    RenderViewLerp = null;
                };

                RenderLerpForm.Text = "RenderView Lerp";
                RenderLerpForm.Icon = Icon;
                RenderLerpForm.ClientSize = RenderPanel.ClientSize;
                Panel panel = new Panel();
                panel.ClientSize = RenderLerpForm.ClientSize;
                panel.Parent = RenderLerpForm;
                RenderViewLerp = new RenderWindow(panel.Handle);
                var view = new SFML.Graphics.View(new FloatRect(RenderViewLerp.Size.X / 2F + (float)nudTargetX.Value,
                                                                RenderViewLerp.Size.Y / 2F + (float)nudTargetY.Value,
                                                                (float)nudTargetWidth.Value,
                                                                (float)nudTargetHeight.Value));
                RenderViewLerp.SetView(view);
                RenderViewLerp.Clear(TransparentColor);
                RenderViewLerp.Display();
                RenderViewLerp.Clear(TransparentColor);
                RenderViewLerp.Display();

                RenderLerpForm.Show(this);
                timerRenderViewLerp.Start();
            }
            else
            {
                RenderLerpForm.Dispose();
            }
        }
        private void btRenderExport_Click(object sender, EventArgs e)
        {
            if (listFrames.Items.Count == 0)
                return;

            if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            int increment = 0;
            sfImage imgAnimation = null;
            if (saveFileDialog.FilterIndex == 1)
                imgAnimation = new sfImage((uint)RenderPanel.Width / (uint)partSize.Width * (uint)listFrames.Items.Count, (uint)RenderPanel.Height / (uint)partSize.Height, sfColor.Transparent);
            Color usedColorTransparent = Color.FromArgb(TransparentColor.A, TransparentColor.R, TransparentColor.G, TransparentColor.B);
            using (var render = new RenderWindow(new VideoMode((uint)RenderPanel.Width, (uint)RenderPanel.Height), ""))
            {
                var view = new SFML.Graphics.View(new FloatRect(render.Size.X / 2F + (float)nudTargetX.Value,
                                                                render.Size.Y / 2F + (float)nudTargetY.Value,
                                                                (float)nudTargetWidth.Value,
                                                                (float)nudTargetHeight.Value));
                render.SetView(view);
                render.SetVisible(false);
                for (int frameIndex = 0; frameIndex < listFrames.Items.Count; frameIndex++)
                {
                    render.Clear(TransparentColor);
                    foreach (var item in (listFrames.Items[frameIndex] as InstantiatedItemCollection).collection)
                        render.Draw(item.Value);
                    sfImage img = render.Capture();
                    var extension = Path.GetExtension(saveFileDialog.FileName);
                    sfColor sfc;
                    Color c;
                    if (saveFileDialog.FilterIndex > 1)
                    {
                        using (var b = new Bitmap((int)render.Size.X / partSize.Width, (int)render.Size.Y / partSize.Height))
                        {
                            using (var g = Graphics.FromImage(b))
                                g.Clear(Color.Transparent);
                            for (int x = 0; x < render.Size.X / partSize.Width; x++)
                                for (int y = 0; y < render.Size.Y / partSize.Height; y++)
                                {
                                    sfc = img.GetPixel((uint)x * (uint)partSize.Width, (uint)y * (uint)partSize.Height);
                                    c = Color.FromArgb(sfc.R, sfc.G, sfc.B, sfc.A);
                                    b.SetPixel(x, y, c == usedColorTransparent ? Color.Transparent : c);
                                }
                            char[] Ext = extension.Replace(".", "").ToCharArray();
                            Ext[0] = char.ToUpper(Ext[0]);
                            System.Reflection.PropertyInfo prop = typeof(ImageFormat).GetProperties().Where(p => p.Name.Equals(new string(Ext), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                            ImageFormat ImageFormatFromExtension = prop.GetValue(prop) as ImageFormat;
                            b.Save(saveFileDialog.FileName.Split('.')[0] + increment++ + "." + saveFileDialog.FileName.Split('.')[1], ImageFormatFromExtension);
                        }
                    }
                    else
                    {
                        for (int x = 0; x < render.Size.X / partSize.Width; x++)
                            for (int y = 0; y < render.Size.Y / partSize.Height; y++)
                            {
                                sfc = img.GetPixel((uint)x * (uint)partSize.Width, (uint)y * (uint)partSize.Height);
                                imgAnimation.SetPixel((uint)frameIndex * ((uint)RenderPanel.Width / (uint)partSize.Width) + (uint)x, (uint)y, sfc);
                            }
                    }
                }
            }
            if (saveFileDialog.FilterIndex == 1)
            {
                imgAnimation.CreateMaskFromColor(TransparentColor);
                imgAnimation.SaveToFile(saveFileDialog.FileName);
            }

            MessageBox.Show("Exportation ended !");
        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btRenderExport_Click(sender, e);
        }
        private void btRenderLerpExport_Click(object sender, EventArgs e)
        {
            if (listFrames.Items.Count == 0)
                return;

            if (saveFileDialog.ShowDialog(this) != DialogResult.OK)
                return;

            #region form
            bool options_abort = false;
            Form options = new Form();
            options.ClientSize = new Size(195, 200);
            options.StartPosition = FormStartPosition.Manual;
            options.Location = new Point(Left + ClientSize.Width / 2 - options.ClientSize.Width / 2, Top + ClientSize.Height / 2 - options.ClientSize.Height / 2);
            options.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            options.ControlBox = false;
            var lbFPL = new Label();
            lbFPL.Location = new Point(20, 20);
            lbFPL.Text = "Frames per lerp :";
            options.Controls.Add(lbFPL);
            var nupFPL = new NumericUpDown();
            nupFPL.Size = new Size(60, 25);
            nupFPL.Location = new Point(120, 20);
            nupFPL.Minimum = 1;
            nupFPL.Maximum = 60;
            nupFPL.Value = 5;
            options.Controls.Add(nupFPL);
            var cbReverse = new CheckBox();
            cbReverse.Size = new Size(100, 30);
            cbReverse.Location = new Point(20, 50);
            cbReverse.Checked = true;
            cbReverse.Text = "Reverse last lerp";
            options.Controls.Add(cbReverse);
            var lbTransparentColor = new Label();
            lbTransparentColor.Location = new Point(20, 95);
            lbTransparentColor.Text = "Transparent color :";
            options.Controls.Add(lbTransparentColor);
            var picTransparentColor = new PictureBox();
            picTransparentColor.Size = new Size(30, 30);
            picTransparentColor.Location = new Point(125, 87);
            picTransparentColor.BorderStyle = BorderStyle.Fixed3D;
            picTransparentColor.BackColor = Color.Transparent;
            picTransparentColor.Click += delegate (object _s, EventArgs _e) { var c = new ColorDialog(); c.Color = picTransparentColor.BackColor; c.ShowDialog(picTransparentColor); picTransparentColor.BackColor = c.Color; };
            options.Controls.Add(picTransparentColor);
            var lbAlpha = new Label();
            lbAlpha.Size = new Size(40, 20);
            lbAlpha.Location = new Point(20, 125);
            lbAlpha.Text = "Alpha :";
            options.Controls.Add(lbAlpha);
            var nupAlpha = new NumericUpDown();
            nupAlpha.Size = new Size(50, 25);
            nupAlpha.Location = new Point(60, 123);
            nupAlpha.Minimum = 0;
            nupAlpha.Maximum = 255;
            nupAlpha.Value = 255;
            nupAlpha.ValueChanged += delegate (object _s, EventArgs _e) { picTransparentColor.BackColor = Color.FromArgb((int)nupAlpha.Value, picTransparentColor.BackColor.R, picTransparentColor.BackColor.G, picTransparentColor.BackColor.B); };
            options.Controls.Add(nupAlpha);
            var btOK = new Button();
            btOK.Size = new Size(50, 30);
            btOK.Location = new Point(45, 160);
            btOK.Text = "Ok";
            btOK.Click += delegate (object _s, EventArgs _e) { options.Dispose(); };
            options.Controls.Add(btOK);
            var btAbort = new Button();
            btAbort.Size = new Size(50, 30);
            btAbort.Location = new Point(105, 160);
            btAbort.Text = "Abort";
            btAbort.Click += delegate (object _s, EventArgs _e) { options_abort = true; options.Dispose(); };
            options.Controls.Add(btAbort);
            options.ShowDialog(this);
            if (options_abort)
                return;
            #endregion

            int increment = 0;
            sfImage imgAnimation = null;
            int framesPerLerp = (int)nupFPL.Value;
            if (saveFileDialog.FilterIndex == 1)
                imgAnimation = new sfImage((uint)RenderPanel.Width / (uint)partSize.Width * (uint)listFrames.Items.Count * (uint)framesPerLerp, (uint)RenderPanel.Height / (uint)partSize.Height, sfColor.Transparent);
            bool reverseLastLerp = cbReverse.Checked;
            Color usedColorTransparent = picTransparentColor.BackColor;
            using (var render = new RenderWindow(new VideoMode((uint)RenderPanel.Width, (uint)RenderPanel.Height), ""))
            {
                var view = new SFML.Graphics.View(new FloatRect(render.Size.X / 2F + (float)nudTargetX.Value,
                                                                render.Size.Y / 2F + (float)nudTargetY.Value,
                                                                (float)nudTargetWidth.Value,
                                                                (float)nudTargetHeight.Value));
                render.SetView(view);
                render.SetVisible(false);
                for (int frameIndex = 0; frameIndex < listFrames.Items.Count; frameIndex++)
                {
                    for (int fpl = 0; fpl < framesPerLerp; fpl++)
                    {
                        render.Clear(TransparentColor);
                        int idA = frameIndex, idB = frameIndex + 1 >= listFrames.Items.Count ? 0 : frameIndex + 1;
                        List<InstantiatedItem> frameCol = (listFrames.Items[frameIndex] as InstantiatedItemCollection).GetCollectionCopy(), colA, colB;
                        Vector2f PA, PB;
                        float RA, RB;
                        foreach (var item in frameCol)
                        {
                            colA = (listFrames.Items[idA] as InstantiatedItemCollection).GetCollectionCopy();
                            colB = (listFrames.Items[idB] as InstantiatedItemCollection).GetCollectionCopy();
                            PA = colA[frameCol.IndexOf(item)].Value.Position;
                            PB = colB[frameCol.IndexOf(item)].Value.Position;
                            RA = colA[frameCol.IndexOf(item)].Value.Rotation;
                            RB = colB[frameCol.IndexOf(item)].Value.Rotation;
                            if (reverseLastLerp && idB == 0)
                                RA = -360F + RA;
                            item.Value.Position = Lerp(PA, PB, fpl / (float)framesPerLerp);
                            item.Value.Rotation = Lerp(RA, RB, fpl / (float)framesPerLerp);
                            render.Draw(item.Value);
                        }
                        sfImage img = render.Capture();
                        var extension = Path.GetExtension(saveFileDialog.FileName);
                        sfColor sfc;
                        Color c;
                        if (saveFileDialog.FilterIndex > 1)
                        {
                            using (var b = new Bitmap((int)render.Size.X, (int)render.Size.Y))
                            {
                                using (var g = Graphics.FromImage(b))
                                    g.Clear(usedColorTransparent);
                                for (int x = 0; x < render.Size.X / partSize.Width; x++)
                                    for (int y = 0; y < render.Size.Y / partSize.Height; y++)
                                    {
                                        sfc = img.GetPixel((uint)x * (uint)partSize.Width, (uint)y * (uint)partSize.Height);
                                        c = ToColor(sfc);
                                        b.SetPixel(x, y, ColorCompare(c, usedColorTransparent) ? picTransparentColor.BackColor : c);
                                    }
                                char[] Ext = extension.Replace(".", "").ToCharArray();
                                Ext[0] = char.ToUpper(Ext[0]);
                                System.Reflection.PropertyInfo prop = typeof(System.Drawing.Imaging.ImageFormat).GetProperties().Where(p => p.Name.Equals(new string(Ext), StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                                System.Drawing.Imaging.ImageFormat ImageFormatFromExtension = prop.GetValue(prop) as System.Drawing.Imaging.ImageFormat;
                                b.Save(saveFileDialog.FileName.Split('.')[0] + increment++ + "." + saveFileDialog.FileName.Split('.')[1], ImageFormatFromExtension);
                            }
                        }
                        else
                        {
                            for (int x = 0; x < render.Size.X / partSize.Width; x++)
                                for (int y = 0; y < render.Size.Y / partSize.Height; y++)
                                {
                                    sfc = img.GetPixel((uint)x * (uint)partSize.Width, (uint)y * (uint)partSize.Height);
                                    if (ColorCompare(sfc, TransparentColor))
                                        sfc = ToSFColor(picTransparentColor.BackColor);
                                    imgAnimation.SetPixel((uint)frameIndex * (uint)framesPerLerp * ((uint)RenderPanel.Width / (uint)partSize.Width) + (uint)fpl * ((uint)RenderPanel.Width / (uint)partSize.Width) + (uint)x, (uint)y, sfc);
                                }
                        }
                    }
                }
            }
            if (saveFileDialog.FilterIndex == 1)
            {
                imgAnimation.SaveToFile(saveFileDialog.FileName);
            }

            MessageBox.Show("Exportation ended !");
        }
        private void exportLerpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btRenderLerpExport_Click(sender, e);
        }
        private void timerRenderView_Tick(object sender, EventArgs e)
        {
            if (listFrames.Items.Count > 0)
            {
                for (int i = 0; i < 2; i++)// for the auto SFML.net double buffering rendering
                {
                    RenderView.Clear(TransparentColor);
                    foreach (InstantiatedItem item in (listFrames.Items[renderViewIndexCurrentFrame] as InstantiatedItemCollection).collection)
                        RenderView.Draw(item.Value);
                    RenderView.Display();
                }

                renderViewIndexCurrentFrame++;
                if (renderViewIndexCurrentFrame >= listFrames.Items.Count)
                    renderViewIndexCurrentFrame -= listFrames.Items.Count;
            }
        }
        private void timerRenderViewLerp_Tick(object sender, EventArgs e)
        {
            if (listFrames.Items.Count > 0)
            {
                // for the auto SFML.net double buffering rendering we double the clear + draw + display states
                if(emulatedTimer == 0)
                {
                    RenderLerpViewItems.Clear();
                    foreach (InstantiatedItem item in (listFrames.Items[renderViewLerpIndexCurrentFrame] as InstantiatedItemCollection).collection)
                        RenderLerpViewItems.Add(new InstantiatedItem(item, true));
                }

                RenderViewLerp.Clear(TransparentColor);
                int idA = renderViewLerpIndexCurrentFrame, idB = renderViewLerpIndexCurrentFrame + 1 >= listFrames.Items.Count ? 0 : renderViewLerpIndexCurrentFrame + 1;
                List<InstantiatedItem> colA, colB;
                Vector2f PA, PB;
                float RA, RB;
                foreach (var item in RenderLerpViewItems)
                {
                    colA = (listFrames.Items[idA] as InstantiatedItemCollection).collection;
                    colB = (listFrames.Items[idB] as InstantiatedItemCollection).collection;
                    PA = colA[RenderLerpViewItems.IndexOf(item)].Value.Position;
                    PB = colB[RenderLerpViewItems.IndexOf(item)].Value.Position;
                    RA = colA[RenderLerpViewItems.IndexOf(item)].Value.Rotation;
                    RB = colB[RenderLerpViewItems.IndexOf(item)].Value.Rotation;
                    if (cbReverseLastLerpRotation.Checked && idB == 0)
                        RA = - 360F + RA;
                    item.Value.Position = Lerp(PA, PB, emulatedTimer / (float)nupRenderViewInterval.Value);
                    item.Value.Rotation = Lerp(RA, RB, emulatedTimer / (float)nupRenderViewInterval.Value);
                    RenderViewLerp.Draw(item.Value);
                }
                RenderViewLerp.Display();
            }

            emulatedTimer += timerRenderViewLerp.Interval;
            if (emulatedTimer >= (int)nupRenderViewInterval.Value)
            {
                emulatedTimer = 0;
                renderViewLerpIndexCurrentFrame++;
                if (renderViewLerpIndexCurrentFrame >= listFrames.Items.Count)
                    renderViewLerpIndexCurrentFrame = 0;
            }
        }
        private void ctbRenderViewInterval_ValueChanged(object sender, EventArgs e)
        {
            timerRenderView.Interval = ctbRenderViewInterval.Value;
            nupRenderViewInterval.Value = ctbRenderViewInterval.Value;
        }
        private void nupRenderViewInterval_ValueChanged(object sender, EventArgs e)
        {
            timerRenderView.Interval = (int)nupRenderViewInterval.Value;
            ctbRenderViewInterval.Value = (int)nupRenderViewInterval.Value;
        }
        private void nupIntervalRenderLerp_ValueChanged(object sender, EventArgs e)
        {
            timerRenderViewLerp.Interval = (int)nupIntervalRenderLerp.Value;
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(sender is int))
                if (MessageBox.Show("Are you sure to create a new project ?\n The current proejct will be entirely removed.", "New Project", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;

            var form = new Form();
            form.ClientSize = new Size(170, 120);
            form.Location = new Point((Screen.FromControl(this).Bounds.Width - form.ClientSize.Width) / 2, (Screen.FromControl(this).Bounds.Height - form.ClientSize.Height) / 2);

            var lbW = new Label();
            lbW.Location = new Point(20, 20);
            lbW.Size = new Size(60, 15);
            lbW.Text = "Part Width";

            var nudW = new NumericUpDown();
            nudW.Location = new Point(lbW.Location.X + lbW.Size.Width + 10, lbW.Location.Y - 2);
            nudW.Size = new Size(60, 15);
            nudW.Minimum = 1;
            nudW.Maximum = 255;
            nudW.Value = 16;

            var lbH = new Label();
            lbH.Location = new Point(20, 40);
            lbH.Size = new Size(60, 15);
            lbH.Text = "Part Height";

            var nudH = new NumericUpDown();
            nudH.Location = new Point(lbW.Location.X + lbH.Size.Width + 10, lbH.Location.Y - 2);
            nudH.Size = new Size(60, 15);
            nudH.Minimum = 1;
            nudH.Maximum = 255;
            nudH.Value = 16;

            var btOk = new Button();
            btOk.Size = new Size(40, 30);
            btOk.Location = new Point(form.ClientSize.Width / 2 - btOk.Size.Width / 2, form.ClientSize.Height - btOk.Size.Height - 20);
            btOk.Text = "OK";
            btOk.Click += delegate { form.Close(); };

            if (!Keyboard.IsKeyPressed(Keyboard.Key.LControl))
            {
                form.Controls.AddRange(new Control[] { lbW, nudW, lbH, nudH, btOk });
                form.ShowDialog(this);
            }

            listFrames.Items.Clear();
            listFrames.ClearSelected();
            tbFrameName.Text = "Frame " + listFrames.Items.Count;
            btNewFrame_Click(null, null);
            listPartsInstantiated.Items.Clear();
            listPartsInstantiated.ClearSelected();
            btKeepPartClear_Click(null, null);
            clearPaletteToolStripMenuItem_Click(null, null);
            partSize = new Size((int)nudW.Value, (int)nudH.Value);
            numPartAreaSizeWidth.Value = partSize.Width;
            numPartAreaSizeHeight.Value = partSize.Height;

            btKeepPartClear_Click(null, null);

            ratio.X = renderCreatePart.Size.X / partSize.Width;
            ratio.Y = renderCreatePart.Size.Y / partSize.Height;

            nudTargetX.Value = - partSize.Width;
            nudTargetY.Value = - partSize.Height;
            nudTargetWidth.Value = 2M * partSize.Width;
            nudTargetHeight.Value = 2M * partSize.Width;
            nudTargetSize.Increment = 2;
        }

        private int AddPart(InstantiatedItem part)
        {
            (listFrames.SelectedItem as InstantiatedItemCollection).AddItem(part);

            listPartsInstantiated.Items.Clear();
            foreach (var i in (listFrames.SelectedItem as InstantiatedItemCollection).collection)
                listPartsInstantiated.Items.Add(i);
            listPartsInstantiated.Update();

            return (listFrames.SelectedItem as InstantiatedItemCollection).collection.Count - 1;
        }
        private void RemovePart(InstantiatedItem part)
        {
            int previousIndex = listPartsInstantiated.SelectedIndex;
            (listFrames.SelectedItem as InstantiatedItemCollection).collection.Remove(part);
            listPartsInstantiated.Items.Remove(part);
            listPartsInstantiated_SelectItemIndex(previousIndex);
        }
        
        private float Lerp(float firstFloat, float secondFloat, float by)
        {
            return (1F - by) * firstFloat + by * secondFloat;
        }
        private Vector2f Lerp(Vector2f firstVector, Vector2f secondVector, float by)
        {
            float retX = Lerp(firstVector.X, secondVector.X, by);
            float retY = Lerp(firstVector.Y, secondVector.Y, by);
            return new Vector2f(retX, retY);
        }
        private Point GetSnappedRenderPanelClientMouse()
        {
            return GetSnappedRenderPanelClientPosition(MousePosition);
        }
        private Point GetSnappedRenderPanelClientPosition(Point position)
        {
            var clientMouse = RenderPanel.PointToClient(position);
            return GetSnappedPosition(clientMouse);
        }
        private Point GetSnappedMouse()
        {
            return GetSnappedPosition(MousePosition);
        }
        private Point GetSnappedPosition(Point position)
        {
            return new Point((int)(position.X - position.X % (float)snapSize), (int)(position.Y - position.Y % (float)snapSize));
        }
        private Point GetSnappedPositionPart(Point position)
        {
            return new Point((int)(position.X / ratio.X) * (int)ratio.X,
                             (int)(position.Y / ratio.Y) * (int)ratio.Y);
        }
        private Point GetSnappedPositionCustom(Point position, Size from, Size to)
        {
            var pos = new Point((int)((position.X / (float)from.Width)  * to.Width),
                                (int)((position.Y / (float)from.Height) * to.Height));

            var ratioCustom = new Vector2f(to.Width / (float)partSize.Width, to.Height / (float)partSize.Height);

            return new Point((int)(pos.X / ratioCustom.X) * (int)ratioCustom.X,
                             (int)(pos.Y / ratioCustom.Y) * (int)ratioCustom.Y);
        }
        private Point GetSnappedPositionCustomRatio(Point position, Size to)
        {
            var ratioCustom = new Vector2f(to.Width / (float)partSize.Width, to.Height / (float)partSize.Height);

            return new Point((int)(position.X / ratioCustom.X) * (int)ratioCustom.X,
                             (int)(position.Y / ratioCustom.Y) * (int)ratioCustom.Y);
        }
        private Bitmap ToImage(sfImage img)
        {
            var bmp = new Bitmap((int)img.Size.X, (int)img.Size.Y);
            for (uint x = 0; x < img.Size.X; x++)
                for (uint y = 0; y < img.Size.Y; y++)
                    bmp.SetPixel((int)x, (int)y, ToColor(img.GetPixel(x, y)));
            return bmp;
        }
        private sfImage ToSFImage(Bitmap bmp)
        {
            var img = new sfImage((uint)bmp.Size.Width, (uint)bmp.Size.Height);
            for (int x = 0; x < bmp.Size.Width; x++)
                for (int y = 0; y < bmp.Size.Height; y++)
                    img.SetPixel((uint)x, (uint)y, ToSFColor(bmp.GetPixel(x, y)));
            return img;
        }

        private Vector2f GetSnappedPosition(Vector2f position)
        {
            var point = GetSnappedPosition(new Point((int)position.X, (int)position.Y));
            return new Vector2f(point.X, point.Y);
        }
        private Point GetWorldMouse()
        {
            var mouse2f = renderwindow.MapPixelToCoords(Mouse.GetPosition(renderwindow));
            return new Point((int)mouse2f.X, (int)mouse2f.Y);
        }
        private Point GetWorldPosition(Vector2f position)
        {
            var pos2f = renderwindow.MapPixelToCoords(new Vector2i((int)position.X, (int)position.Y));
            return new Point((int)pos2f.X, (int)pos2f.Y);
        }
        private static sfColor ToSFColor(Color c)
        {
            return new sfColor(c.R, c.G, c.B, c.A);
        }
        private static Color ToColor(sfColor c)
        {
            return Color.FromArgb(c.A, c.R, c.G, c.B);
        }
        private static bool ColorCompare(sfColor A, sfColor B, bool ignoreAlpha = false)
        {
            return A.R == B.R && A.G == B.G && A.B == B.B && (ignoreAlpha || A.A == B.A);
        }
        private static bool ColorCompare(sfColor A, Color B, bool ignoreAlpha = false)
        {
            return A.R == B.R && A.G == B.G && A.B == B.B && (ignoreAlpha || A.A == B.A);
        }
        private static bool ColorCompare(Color A, Color B, bool ignoreAlpha = false)
        {
            return ignoreAlpha ? A.R == B.R && A.G == B.G && A.B == B.B : A.ToArgb() == B.ToArgb();
        }

        private void savePaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "Palette file (.pal)|*.pal";
            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
                SavePalette(dialog.FileName);
        }
        private void SavePalette(string FileName)
        {
            using (var stream = new StreamWriter(FileName))
            {
                foreach (var bt in palette)
                    stream.WriteLine(ColorTranslator.ToHtml(bt.BackColor));
            }
        }
        private void loadPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "Palette file (.pal)|*.pal";
            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
                LoadPalette(dialog.FileName);
        }
        private void LoadPalette(string FileName)
        {
            using (var stream = new StreamReader(FileName))
            {
                ClearPalette(false);
                while (!stream.EndOfStream)
                    AddPaletteColorFromColor(ColorTranslator.FromHtml(stream.ReadLine()));
            }
        }
        private void clearPaletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearPalette();
        }
        private void ClearPalette(bool keepDefault = true)
        {
            palette.Clear();
            btDrawPart_ColorA.BackColor = Color.Black;
            btDrawPart_ColorB.BackColor = Color.White;
            if (keepDefault)
            {
                AddPaletteColorFromA();
                AddPaletteColorFromB();
            }
            ResortPalette();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            var view = renderwindow.GetView();
            var value = 100F / trackBar1.Value;
            view.Size = new Vector2f(value * renderwindow.Size.X, value * renderwindow.Size.Y);
            renderwindow.SetView(view);
        }
        private void btPartAreaSizeApply_Click(object sender, EventArgs e)
        {
            partSize = new Size((int)numPartAreaSizeWidth.Value, (int)numPartAreaSizeHeight.Value);
            btKeepPartClear_Click(null, null);
            ratio.X = (float)renderCreatePart.Size.X / partSize.Width;
            ratio.Y = (float)renderCreatePart.Size.Y / partSize.Height;
            btPartAreaSizeApply.Enabled = false;
            btPartAreaSizeCancel.Enabled = false;
            penSize = 1;
            SetPen();
        }
        private void btPartAreaSizeCancel_Click(object sender, EventArgs e)
        {
            numPartAreaSizeWidth.Value = partSize.Width;
            numPartAreaSizeHeight.Value = partSize.Height;
            btPartAreaSizeApply.Enabled = false;
            btPartAreaSizeCancel.Enabled = false;
        }
        private void numPartAreaSizeWidth_ValueChanged(object sender, EventArgs e)
        {
            var modified = Math.Round(numPartAreaSizeWidth.Value) != partSize.Width || Math.Round(numPartAreaSizeHeight.Value) != partSize.Height;
            btPartAreaSizeApply.Enabled = modified;
            btPartAreaSizeCancel.Enabled = modified;
        }
        private void numPartAreaSizeHeight_ValueChanged(object sender, EventArgs e)
        {
            var modified = Math.Round(numPartAreaSizeWidth.Value) != partSize.Width || Math.Round(numPartAreaSizeHeight.Value) != partSize.Height;
            btPartAreaSizeApply.Enabled = modified;
            btPartAreaSizeCancel.Enabled = modified;
        }
        private void btPartAreaSizeOpenSeparatedWindow_Click(object sender, EventArgs e)
        {
            var img = renderCreatePart.Capture();
            var sp = new Sprite(new Texture(img));

            uint sizeX = (uint)(Math.Max(ratio.X, ratio.Y) * partSize.Width);
            uint sizeY = (uint)(Math.Max(ratio.X, ratio.Y) * partSize.Height);
            sp.Scale = new Vector2f(sizeX / (float)sp.Texture.Size.X, sizeY / (float)sp.Texture.Size.Y);

            renderCreatePartWindowed = new RenderWindow(new VideoMode(sizeX, sizeY), "", Styles.Titlebar | Styles.Close);
            renderCreatePartWindowed.Draw(sp);
            renderCreatePartWindowed.Display();
            renderCreatePartWindowed.Draw(sp);
            renderCreatePartWindowed.Display();

            renderCreatePartWindowed.MouseButtonPressed += renderCreatePartWindowed_MouseButtonPressed;
            renderCreatePartWindowed.MouseMoved += renderCreatePartWindowed_MouseMoved;
            renderCreatePartWindowed.MouseButtonReleased += RenderCreatePartWindowed_MouseButtonReleased;
            renderCreatePartWindowed.MouseWheelMoved += RenderCreatePartWindowed_MouseWheelMoved;
            renderCreatePartWindowed.Closed += delegate {
                Invoke(new Action(() =>
                {
                    btPartAreaSizeOpenSeparatedWindow.Enabled = true;
                    numPartAreaSizeWidth.Enabled = true;
                    numPartAreaSizeHeight.Enabled = true;
                    renderCreatePartWindowed.Close();
                    renderCreatePartWindowed = null;
                }));
            };
            var windowedPollEventsThread = new Thread((ThreadStart)delegate
            {
                while(renderCreatePartWindowed != null)
                {
                    Invoke(new Action(() => renderCreatePartWindowed.DispatchEvents()));
                }
            });
            windowedPollEventsThread.IsBackground = true;
            windowedPollEventsThread.SetApartmentState(ApartmentState.STA);
            windowedPollEventsThread.Start();

            btPartAreaSizeOpenSeparatedWindow.Enabled = false;
            numPartAreaSizeWidth.Enabled = false;
            numPartAreaSizeHeight.Enabled = false;
        }
        private void btPartNameApply_Click(object sender, EventArgs e)
        {
            if (listPartsInstantiated.SelectedIndex > -1)
            {
                var item = listPartsInstantiated.SelectedItem;
                int index = listPartsInstantiated.Items.IndexOf(item);
                listPartsInstantiated.Items.RemoveAt(index);
                (item as InstantiatedItem).Name = tbPartName.Text;
                listPartsInstantiated.Items.Insert(index, item);
                btPartNameApply.Enabled = false;
            }
        }
        private void snapSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new Form();
            form.ClientSize = new Size(170, 120);
            form.Location = new Point((Screen.FromControl(this).Bounds.Width - form.ClientSize.Width) / 2, (Screen.FromControl(this).Bounds.Height - form.ClientSize.Height) / 2);

            var lbSnap = new Label();
            lbSnap.Location = new Point(20, 20);
            lbSnap.Size = new Size(60, 15);
            lbSnap.Text = "Snap Size";

            var nudSnap = new NumericUpDown();
            nudSnap.Location = new Point(lbSnap.Location.X + lbSnap.Size.Width + 10, lbSnap.Location.Y - 2);
            nudSnap.Size = new Size(60, 15);
            nudSnap.Minimum = 1;
            nudSnap.Maximum = 255;
            nudSnap.Value = snapSize;

            var btOk = new Button();
            btOk.Size = new Size(40, 30);
            btOk.Location = new Point(form.ClientSize.Width / 2 - btOk.Size.Width / 2, form.ClientSize.Height - btOk.Size.Height - 20);
            btOk.Text = "OK";
            btOk.Click += delegate { form.Close(); };

            form.Controls.AddRange(new Control[] { lbSnap, nudSnap, btOk });
            form.ShowDialog(this);

            snapSize = (int)nudSnap.Value;
        }
        private void btPartPivotReset_Click(object sender, EventArgs e)
        {
            var part = listPartsInstantiated.SelectedItem as InstantiatedItem;
            var newOrigin = new Vector2f(part.Value.Texture.Size.X / 2F, part.Value.Texture.Size.Y / 2F);
            part.Value.Position -= part.Value.Origin - newOrigin;
            part.Value.Origin = newOrigin;
            spRenderPivot.Position = part.Value.Position;
            SelectPart(listPartsInstantiated.Items.IndexOf(part));
        }
        private void snapAllPartsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure to proceed ?\nThat will snap all the used parts of the current frame.", "Snap All Parts", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                foreach (InstantiatedItem item in listPartsInstantiated.Items)
                    item.Value.Position -= GetSnappedPosition(item.Value.Position);
                Console.WriteLine();
            }
        }
        private void numPartPositionX_ValueChanged(object sender, EventArgs e)
        {
            if (listPartsInstantiated.SelectedIndex > -1)
            {
                var sp = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value;
                sp.Position = new Vector2f((float)numPartPositionX.Value, sp.Position.Y);
            }
        }
        private void numPartPositionY_ValueChanged(object sender, EventArgs e)
        {
            if (listPartsInstantiated.SelectedIndex > -1)
            {
                var sp = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value;
                sp.Position = new Vector2f(sp.Position.X, (float)numPartPositionY.Value);
            }
        }
        private void numPartAngle_ValueChanged(object sender, EventArgs e)
        {
            if (listPartsInstantiated.SelectedIndex > -1)
            {
                var sp = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value;
                sp.Rotation = (float)numPartAngle.Value;
            }
        }
        private void numPartScaleX_ValueChanged(object sender, EventArgs e)
        {
            if (listPartsInstantiated.SelectedIndex > -1)
            {
                var sp = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value;
                sp.Scale = new Vector2f((float)numPartScaleX.Value, sp.Scale.Y);
            }
        }
        private void numPartScaleY_ValueChanged(object sender, EventArgs e)
        {
            if (listPartsInstantiated.SelectedIndex > -1)
            {
                var sp = (listPartsInstantiated.SelectedItem as InstantiatedItem).Value;
                sp.Scale = new Vector2f(sp.Scale.X, (float)numPartScaleY.Value);
            }
        }
        private void renderColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dial = new ColorDialog();
            var result = dial.ShowDialog(this);
            if(result == DialogResult.OK)
            {
                var previousTransparentColor = TransparentColor;
                TransparentColor = new sfColor(dial.Color.R, dial.Color.G, dial.Color.B);

                sfImage img = renderCreatePart.Capture();
                for (uint x = 0; x < img.Size.X; x++)
                    for (uint y = 0; y < img.Size.Y; y++)
                        if (img.GetPixel(x, y) == previousTransparentColor)
                            img.SetPixel(x, y, TransparentColor);
                Sprite sp = new Sprite(new Texture(img));
                renderCreatePart.Clear();
                renderCreatePart.Draw(sp);
                renderCreatePart.Display();
                renderCreatePart.Clear();
                renderCreatePart.Draw(sp);
                renderCreatePart.Display();
            }
        }
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Filter = "2DAnim file (.2dam)|*.2dam";
            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                using (var stream = new FileStream(dialog.FileName, FileMode.Open))
                {
                    listFrames.Items.Clear();
                    listPartsInstantiated.Items.Clear();
                   
                    string folderstring = Path.GetDirectoryName(dialog.FileName) + @"\" + Path.GetFileNameWithoutExtension(dialog.FileName) + @"_parts\";
                    var files = Directory.EnumerateFiles(folderstring).Where(x => Path.GetExtension(x) == ".png").OrderByDescending(x => x).Reverse();
                    string fileNameWithoutExtension = "";
                    int frameNum = 0;
                    var frame = new InstantiatedItemCollection("Frame " + listFrames.Items.Count);
                    BinaryFormatter bf = new BinaryFormatter();
                    StreamReader file;
                    Sprite sp;
                    int partID = 0;
                    foreach (var fileFound in files)
                    {
                        fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileFound);
                        if (frameNum != int.Parse(fileNameWithoutExtension.Split('_')[1]))
                        {
                            listFrames.Items.Add(new InstantiatedItemCollection(frame.Name, frame.collection));
                            frame.collection.Clear();
                        }
                        frameNum = int.Parse(fileNameWithoutExtension.Split('_')[1]);
                        sp = new Sprite(new Texture(ToSFImage(new Bitmap(fileFound))));
                        using (file = new StreamReader(Path.GetDirectoryName(fileFound) + @"\" + Path.GetFileNameWithoutExtension(fileFound) + @".info"))
                        {
                            sp.Position = new Vector2f((float)bf.Deserialize(file.BaseStream), (float)bf.Deserialize(file.BaseStream));
                            sp.Rotation = (float) bf.Deserialize(file.BaseStream);
                            sp.Scale    = new Vector2f((float)bf.Deserialize(file.BaseStream), (float)bf.Deserialize(file.BaseStream));
                            sp.Scale    = new Vector2f((float)bf.Deserialize(file.BaseStream), (float)bf.Deserialize(file.BaseStream));
                            partID      = (int)bf.Deserialize(file.BaseStream);
                        }
                        frame.collection[frame.AddItem(new InstantiatedItem(fileNameWithoutExtension.Split('_')[0], sp))].index = partID;
                    }
                    if (frameNum >= listFrames.Items.Count)
                        listFrames.Items.Add(frame);

                    cbSnap.Checked = (bool) bf.Deserialize(stream);
                    trackBar1.Value = (int) bf.Deserialize(stream);
                    ctbRenderViewInterval.Value = (int)bf.Deserialize(stream);
                    cbReverseLastLerpRotation.Checked = (bool)bf.Deserialize(stream);
                    nupIntervalRenderLerp.Value = (decimal)bf.Deserialize(stream);
                    partSize.Width = (int)bf.Deserialize(stream);
                    partSize.Height = (int)bf.Deserialize(stream);
                    string palstring = folderstring + Path.GetFileNameWithoutExtension(dialog.FileName) + @".pal";
                    LoadPalette(palstring);
                    numPartAreaSizeWidth.Value = partSize.Width;
                    numPartAreaSizeHeight.Value = partSize.Width;

                    listFrames.SelectedIndex = 0;
                }
                MessageBox.Show("Load successfuly done.", "Load project");
            }
            else
                MessageBox.Show("Load failed.", "Load project");
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.Filter = "2DAnim file (.2dam)|*.2dam";
            var result = dialog.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                using (var stream = new FileStream(dialog.FileName, FileMode.Create))
                {
                    int frameNum=0,partNum=0;
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream file;
                    string folderstring = Path.GetDirectoryName(dialog.FileName) + @"\" + Path.GetFileNameWithoutExtension(dialog.FileName) + @"_parts\";
                    foreach (InstantiatedItemCollection frame in listFrames.Items)
                    {
                        foreach (InstantiatedItem part in frame.collection)
                        {
                            string filestring = folderstring + part.Name + @"_" + frameNum + @"_" + partNum;
                            string imgstring = filestring + @".png";
                            string infostring = filestring + @".info";
                            Directory.CreateDirectory(folderstring);
                            ToImage(part.Value.Texture.CopyToImage()).Save(imgstring, ImageFormat.Png);
                            using (file = new FileStream(infostring, FileMode.Create))
                            {
                                bf.Serialize(file, part.Value.Position.X);
                                bf.Serialize(file, part.Value.Position.Y);
                                bf.Serialize(file, part.Value.Rotation);
                                bf.Serialize(file, part.Value.Scale.X);
                                bf.Serialize(file, part.Value.Scale.Y);
                                bf.Serialize(file, part.index);
                            }
                            partNum++;
                        }
                        frameNum++;
                        partNum = 0;
                    }
                    
                    bf.Serialize(stream, cbSnap.Checked);
                    bf.Serialize(stream, trackBar1.Value);
                    bf.Serialize(stream, ctbRenderViewInterval.Value);
                    bf.Serialize(stream, cbReverseLastLerpRotation.Checked);
                    bf.Serialize(stream, nupIntervalRenderLerp.Value);
                    bf.Serialize(stream, partSize.Width);
                    bf.Serialize(stream, partSize.Height);
                    string palstring = folderstring + Path.GetFileNameWithoutExtension(dialog.FileName) + @".pal";
                    SavePalette(palstring);
                }

                MessageBox.Show("Save successfuly done.", "Save project");
            }
            else
                MessageBox.Show("Save failed.", "Save project");
        }
        private void cbSnap_CheckedChanged(object sender, EventArgs e)
        {
            if(cbSnap.Checked)
            {
                nudTargetX.Increment = partSize.Width;
                nudTargetY.Increment = partSize.Height;
                nudTargetWidth.Increment = partSize.Width;
                nudTargetHeight.Increment = partSize.Height;
            }
            else
            {
                nudTargetX.Increment = 1;
                nudTargetY.Increment = 1;
                nudTargetWidth.Increment = 1;
                nudTargetHeight.Increment = 1;
            }
        }
        private void nudTargetX_ValueChanged(object sender, EventArgs e)
        {
            if (cbSnap.Checked)
            {
                nudTargetX.ValueChanged -= nudTargetX_ValueChanged;
                nudTargetX.Value = Math.Round(nudTargetX.Value / partSize.Width) * partSize.Width;
                nudTargetX.ValueChanged += nudTargetX_ValueChanged;
            }
        }
        private void nudTargetY_ValueChanged(object sender, EventArgs e)
        {
            if (cbSnap.Checked)
            {
                nudTargetY.ValueChanged -= nudTargetY_ValueChanged;
                nudTargetY.Value = Math.Round(nudTargetY.Value / partSize.Height) * partSize.Height;
                nudTargetY.ValueChanged += nudTargetY_ValueChanged;
            }
        }
        private void nudTargetWidth_ValueChanged(object sender, EventArgs e)
        {
            if (cbSnap.Checked)
            {
                nudTargetWidth.ValueChanged -= nudTargetWidth_ValueChanged;
                nudTargetWidth.Value = Math.Round(nudTargetWidth.Value / partSize.Width) * partSize.Width;
                nudTargetWidth.ValueChanged += nudTargetWidth_ValueChanged;
            }

            if (nudTargetWidth.Value < 0)
                nudTargetWidth.Value = 0;

            RedrawTarget();
        }
        private void nudTargetHeight_ValueChanged(object sender, EventArgs e)
        {
            if (cbSnap.Checked)
            {
                nudTargetHeight.ValueChanged -= nudTargetHeight_ValueChanged;
                nudTargetHeight.Value = Math.Round(nudTargetHeight.Value / partSize.Height) * partSize.Height;
                nudTargetHeight.ValueChanged += nudTargetHeight_ValueChanged;
            }

            if (nudTargetHeight.Value < 0)
                nudTargetHeight.Value = 0;

            RedrawTarget();
        }
        private void nudTargetSize_ValueChanged(object sender, EventArgs e)
        {
            if (nudTargetSize.Value < 0)
                nudTargetSize.Value = 0;

            nudTargetWidth.Value = nudTargetSize.Value * partSize.Width;
            nudTargetHeight.Value = nudTargetSize.Value * partSize.Height;
            btCenterTarget_Click(null, null);
        }
        private void btCenterTarget_Click(object sender, EventArgs e)
        {
            nudTargetX.Value = -nudTargetWidth.Value / 2M;
            nudTargetY.Value = -nudTargetHeight.Value / 2M;
        }
        private void btTargetColor_Click(object sender, EventArgs e)
        {
            ColorDialog diag = new ColorDialog();
            diag.Color = btTargetColor.BackColor;
            diag.ShowDialog(this);
            btTargetColor.BackColor = diag.Color;
            RedrawTarget();
        }
        private void nudTargetBorderSize_ValueChanged(object sender, EventArgs e)
        {
            RedrawTarget();
        }
        private void nudPartID_ValueChanged(object sender, EventArgs e)
        {
            if (!(listFrames.SelectedItem as InstantiatedItemCollection).IndexExists((int)nudPartID.Value))
                (listPartsInstantiated.SelectedItem as InstantiatedItem).index = (int)nudPartID.Value;
            else
            {
                MessageBox.Show("The index already exists in the current frame.", "Set Part Index", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                nudPartID.ValueChanged -= nudPartID_ValueChanged;
                var part = listPartsInstantiated.SelectedItem as InstantiatedItem;
                while (part.index != (int)nudPartID.Value)
                {
                    nudPartID.Value++;
                    if (!(listFrames.SelectedItem as InstantiatedItemCollection).IndexExists((int)nudPartID.Value))
                        part.index = (int)nudPartID.Value;
                }
                nudPartID.ValueChanged += nudPartID_ValueChanged;
            }
        }
        private void btPartShowIndexes_Click(object sender, EventArgs e)
        {
            string msg = "";
            foreach (InstantiatedItem item in (listFrames.SelectedItem as InstantiatedItemCollection).collection)
                msg += item.Name + " (" + item.index + ")" + Environment.NewLine;

            MessageBox.Show(msg == "" ? "There's no part currently instantiated." : msg, "Parts Indexes", MessageBoxButtons.OK);
        }
        private void btFrameShowAllIndexes_Click(object sender, EventArgs e)
        {
            var indexes = new List<int>();
            foreach (InstantiatedItemCollection frame in listFrames.Items)
                foreach (InstantiatedItem item in frame.collection)
                    if (!indexes.Contains(item.index))
                        indexes.Add(item.index);
            if (indexes.Count > 0)
            {
                var dgv = new DataGridView();
                dgv.ReadOnly = true;
                dgv.AllowUserToAddRows = false;
                dgv.AllowUserToDeleteRows = false;
                dgv.AllowUserToOrderColumns = false;
                dgv.AllowUserToResizeColumns = false;
                dgv.AllowUserToResizeRows = false;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                dgv.DefaultCellStyle.SelectionBackColor = Color.White;
                dgv.SelectionChanged += delegate { dgv.ClearSelection(); };
                dgv.RowHeadersVisible = false;
                dgv.SortCompare += delegate (object o, DataGridViewSortCompareEventArgs ea) { ea.SortResult = (int.Parse(ea.CellValue1.ToString())).CompareTo(int.Parse(ea.CellValue2.ToString())); ea.Handled = true; };

                dgv.Columns.Add("HEADER", "ID");
                foreach (InstantiatedItemCollection frame in listFrames.Items)
                    dgv.Columns[dgv.Columns.Add(listFrames.Items.IndexOf(frame).ToString(), frame.Name)].Resizable = DataGridViewTriState.False;
                DataGridViewRow row;
                int rowIndex = 0;
                for(int index = 0; index < indexes.Count; index++)
                {
                    row = new DataGridViewRow();
                    row.CreateCells(dgv);
                    rowIndex = dgv.Rows.Add(row);
                    dgv.Rows[rowIndex].Cells["HEADER"].Value = indexes[index].ToString();
                    for (int i = 1; i < dgv.ColumnCount; i++)
                        dgv.Rows[rowIndex].Cells[i].Style.BackColor = (listFrames.Items[i-1] as InstantiatedItemCollection).IndexExists(indexes[index]) ? Color.Gray : Color.White;
                }

                dgv.Sort(dgv.Columns["HEADER"], ListSortDirection.Ascending);

                DataGridViewElementStates states = DataGridViewElementStates.None;
                dgv.ScrollBars = ScrollBars.None;
                var totalHeight = dgv.Rows.GetRowsHeight(states) + dgv.ColumnHeadersHeight;
                var totalWidth = dgv.Columns.GetColumnsWidth(states) + dgv.RowHeadersWidth;
                dgv.ClientSize = new Size(totalWidth, totalHeight);

                var form = new Form();
                form.Text = btFrameShowAllIndexes.Text;
                form.ClientSize = dgv.ClientSize;
                form.Controls.Add(dgv);
                form.ShowDialog(this);
            }
        }
    }
}
