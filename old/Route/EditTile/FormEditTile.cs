using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Core.Utils;
using System.Configuration;
using System.Linq;
using System.IO;
using Core;
using static Core.Definitions;
using static Core.TileResMngr;

namespace EditTile
{
    public partial class FormEditTile : Form
    {
        PenType penType;
        PathDotType pathDotType;

        Timer TimerUpdate = new Timer() { Interval = 50 };
        Timer TimerDraw = new Timer() { Interval = 50 };
        Graphics g;
        Bitmap RenderImage;
        public static readonly int PixelSize = 4;
        PictureBox Path = null;
        PictureBox PathSigns = null;
        string CurFileName;
        Dictionary<PathDots, PathDotType> pathDots = null;
        Dictionary<PathDots, TileSignObj> pathDotsSign = null;
        ImageList imgSignList = null;
        PathDots SelectedSign = PathDots.CA;

        Point MouseClient => Render.PointToClient(MousePosition);

        public FormEditTile()
        {
            InitializeComponent();

            TimerUpdate.Tick += Update;
            TimerUpdate.Start();
            TimerDraw.Tick += DrawTask;
            TimerDraw.Start();
        }
        protected override void OnLoad(EventArgs e)
        {
            Inputs.Initialize();
            TileResMngr.Instance.Initialize(Directory.GetCurrentDirectory());

            listPalettes.Items.Clear();
            if (pathDots == null)
                pathDots = new Dictionary<PathDots, PathDotType>();
            else
                pathDots.Clear();
            if (pathDotsSign == null)
                pathDotsSign = new Dictionary<PathDots, TileSignObj>();
            else
                pathDotsSign.Clear();
            penType = PenType.Pen;
            pathDotType = PathDotType.In;
            CurFileName = null;
            Tools.ForEachPathDots((x) => pathDots[x] = PathDotType.None);
            Tools.ForEachPathDots((x) => pathDotsSign[x] = new TileSignObj());
            nupIndex.Value = Tools.GetFilesCount(Directory.GetCurrentDirectory(), "*.tile");

            #region Image
            RenderImage = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(RenderImage);
            g.FillRectangle(Brushes.White, 0, 0, Render.Width, Render.Height);
            Render.Image = RenderImage;

            LoadPalettes();
            SetCurrentPalette("Custom");

            List<Button> ColorSaves = new List<Button>() { btColorSave1, btColorSave2, btColorSave3, btColorSave4, btColorSave5, btColorSave6, btColorSave7, btColorSave8, btColorSave9, btColorSave10 };
            foreach (var bt in ColorSaves)
            {
                bt.BackColor = Color.White;
                bt.MouseDown += delegate (object sender, MouseEventArgs me)
                {
                    if (me.Button == MouseButtons.Left)
                    {
                        bt.BackColor = btColor.BackColor;
                        return;
                    }

                    if (me.Button == MouseButtons.Right)
                    {
                        btColor.BackColor = bt.BackColor;
                        return;
                    }

                    if (me.Button == MouseButtons.Middle)
                    {
                        bt.BackColor = btColorTransparent.BackColor;
                        return;
                    }
                };
            }
            btColorSave1.BackColor = Color.Black;
            #endregion

            #region Path
            if (Path == null)
            {
                Path = new PictureBox();
                PathSigns = new PictureBox();
                Path.Size = RouteBackgroundPath.Size;
                PathSigns.Size = RouteBackgroundSigns.Size;
                Path.BackColor = Color.Transparent;
                PathSigns.BackColor = Color.Transparent;
            }
            DrawPathDots();
            Path.Parent = RouteBackgroundPath;
            PathSigns.Parent = RouteBackgroundSigns;
            Path.MouseClick += Path_MouseClick;
            #endregion

            #region Signs
            imgSignList = new ImageList();
            imgSignList.ImageSize = new Size(SharedData.TileSize, SharedData.TileSize);
            cbbSigns.Items.Clear();
            imgSignList.Images.Add(new Bitmap(32,32));
            cbbSigns.Items.Add("None");
            Dictionary<int, SignInfo> list = TileResMngr.Instance.ResourcesSignInfo;
            foreach (KeyValuePair<int, SignInfo> sign in list)
            {
                imgSignList.Images.Add(FromSmallToLarge(TileResMngr.Instance.ResourcesSignInfo[sign.Key].Image, SharedData.SignSize, 16));
                cbbSigns.Items.Add(sign.Value.Name);
            }
            cbbSigns.SelectedIndex = 0;
            PathSigns.MouseClick += PathSigns_MouseClick;
            SelectedSign = PathDots.CA;
            picSignLocation.Image = new Bitmap(picSignLocation.Width, picSignLocation.Height);
            #endregion
        }
        protected override void OnClosed(EventArgs e)
        {
            TimerUpdate.Stop();
            TimerDraw.Stop();
            g.Dispose();
            Path.MouseClick -= Path_MouseClick;
            PathSigns.MouseClick -= PathSigns_MouseClick;

            base.OnClosed(e);
        }

        private void Update(object sender, EventArgs e)
        {
            Inputs.Update();
        }
        private void DrawTask(object sender, EventArgs e)
        {
        }

        private void Draw(int X, int Y, PenType type = PenType.Pen) => Draw(new Point(X, Y), type);
        private void Draw(Point position, PenType type = PenType.Pen)
        {
            if (position.X < 0 || position.Y < 0 || position.X >= Render.Width || position.Y >= Render.Height)
                return;

            SolidBrush brush = new SolidBrush((type == PenType.Eraser ? btColorTransparent : btColor).BackColor);
            g.FillRectangle(brush, new Rectangle(Tools.Snap(position, 4), new Size(PixelSize, PixelSize)));
            Render.Image = new Bitmap(RenderImage);
            RouteBackgroundPath.Image = Render.Image;
            RouteBackgroundSigns.Image = Render.Image;
            picSignLocation.Image = Render.Image;
            DrawSignsInRoute();
        }
        private void Fill(Point position, PenType type = PenType.Pen)
        {
            position = Tools.Snap(position, 4);
            void RecursiveDraw(int X, int Y, Color color)
            {
                if (X < 0 || Y < 0 || X >= Render.Width || Y >= Render.Height)
                    return;

                var c = Tools.GetPixel(RenderImage, X, Y);
                if (Tools.CompareColors(c, color))
                {
                    Draw(X, Y, type);
                    RecursiveDraw(X - PixelSize, Y, color);
                    RecursiveDraw(X + PixelSize, Y, color);
                    RecursiveDraw(X, Y - PixelSize, color);
                    RecursiveDraw(X, Y + PixelSize, color);
                }
            }

            var col = Tools.GetPixel(RenderImage, position);
            if (!Tools.CompareColors(col, (type == PenType.Eraser ? btColorTransparent : btColor).BackColor))
                RecursiveDraw(position.X, position.Y, col);
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.X < 0 || e.Y < 0 || e.X >= RenderImage.Width || e.Y >= RenderImage.Height)
                return;

            if (e.Button == MouseButtons.Right)
            {
                btColor.BackColor = Tools.GetPixel(RenderImage, e.X, e.Y);
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                switch (penType)
                {
                    default:
                    case PenType.Pen:
                        if (Inputs.KeyDown(Inputs.Key.LAlt))
                            Draw(e.Location, PenType.Eraser);
                        else
                            Draw(e.Location);
                        break;
                    case PenType.Eraser:
                        if (Inputs.KeyDown(Inputs.Key.LAlt))
                            Draw(e.Location);
                        else
                            Draw(e.Location, PenType.Eraser);
                        break;
                    case PenType.Fill:
                        if (Inputs.KeyDown(Inputs.Key.LAlt))
                            Fill(e.Location, PenType.Eraser);
                        else
                            Fill(e.Location);
                        break;
                }
                return;
            }
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            Render_MouseDown(sender, e);
        }
        private void Path_MouseClick(object sender, MouseEventArgs e)
        {
            string[] listPathDotsNames = Enum.GetNames(typeof(PathDots));
            Rectangle rect = Rectangle.Empty;
            Tools.ForEachPathDots((pathDotValue) =>
            {
                rect = PathDotsRect[pathDotValue];
                if (rect.Contains(e.Location))
                {
                    if (Tools.IsPathDotCorner(pathDotValue))
                    {
                        pathDots[pathDotValue] = (pathDotType == PathDotType.None || pathDotType == PathDotType.Way) ? pathDotType : PathDotType.Way;
                    }
                    else
                    {
                        pathDots[pathDotValue] = pathDotType;
                    }
                }
            });
            DrawPathDots();
        }
        private void PathSigns_MouseClick(object sender, MouseEventArgs e)
        {
            string[] listPathDotsNames = Enum.GetNames(typeof(PathDots));
            Rectangle rect = Rectangle.Empty;
            Tools.ForEachPathDots((pathDotValue) =>
            {
                rect = PathDotsRect[pathDotValue];
                if (rect.Contains(e.Location))
                {
                    bool shouldDrawPathDots = true;
                    switch (e.Button)
                    {
                        default: shouldDrawPathDots = false; break;
                        case MouseButtons.Left: pathDotsSign[pathDotValue] = new TileSignObj(cbbSigns.SelectedIndex, PathDotsRect[pathDotValue].Location, 0, cbSignHasBar.Checked); SelectedSign = pathDotValue; break;
                        case MouseButtons.Middle: pathDotsSign[pathDotValue].Index = 0; DrawSignsInRoute(); break;
                        case MouseButtons.Right: cbbSigns.SelectedIndex = pathDotsSign[pathDotValue].Index; SelectedSign = pathDotValue; cbSignHasBar.Checked = pathDotsSign[pathDotValue].ShowBar; break;
                    }
                    if (shouldDrawPathDots)
                    {
                        DrawPathDots();
                    }
                }
            });
        }

        private void btPen_Click(object sender, EventArgs e)
        {
            penType = PenType.Pen;

            btPen.FlatStyle = FlatStyle.Standard;
            btEraser.FlatStyle = FlatStyle.Popup;
            btFill.FlatStyle = FlatStyle.Popup;
        }
        private void btEraser_Click(object sender, EventArgs e)
        {
            penType = PenType.Eraser;

            btPen.FlatStyle = FlatStyle.Popup;
            btEraser.FlatStyle = FlatStyle.Standard;
            btFill.FlatStyle = FlatStyle.Popup;
        }
        private void btFill_Click(object sender, EventArgs e)
        {
            penType = PenType.Fill;

            btPen.FlatStyle = FlatStyle.Popup;
            btEraser.FlatStyle = FlatStyle.Popup;
            btFill.FlatStyle = FlatStyle.Standard;
        }
        private void btColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = btColor.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                btColor.BackColor = colorDialog.Color;
            }
        }
        private void btColorTransparent_Click(object sender, EventArgs e)
        {
            colorDialog.Color = btColorTransparent.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
                btColorTransparent.BackColor = colorDialog.Color;
        }

        private void LoadPalettes()
        {
            foreach (SettingsProperty prop in Properties.Settings.Default.Properties)
            {
                if (((string)Properties.Settings.Default[prop.Name]).Split(';').Length == 10)
                    listPalettes.Items.Add(prop.Name);
            }
        }
        private void SaveSettingPalette(string name, string value)
        {
            Properties.Settings.Default[name] = value;
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }
        private string GetCurrentPalette()
        {
            return string.Concat(
                   btColorSave1.BackColor.ToArgb().ToString(), ';',
                   btColorSave2.BackColor.ToArgb().ToString(), ';',
                   btColorSave3.BackColor.ToArgb().ToString(), ';',
                   btColorSave4.BackColor.ToArgb().ToString(), ';',
                   btColorSave5.BackColor.ToArgb().ToString(), ';',
                   btColorSave6.BackColor.ToArgb().ToString(), ';',
                   btColorSave7.BackColor.ToArgb().ToString(), ';',
                   btColorSave8.BackColor.ToArgb().ToString(), ';',
                   btColorSave9.BackColor.ToArgb().ToString(), ';',
                   btColorSave10.BackColor.ToArgb().ToString());
        }
        private void SetCurrentPalette(string PaletteName)
        {
            SetCurrentPaletteWithValues((string)Properties.Settings.Default[PaletteName]);
        }
        private void SetCurrentPaletteWithValues(string PaletteValues)
        {
            List<string> paletteValues = PaletteValues.Split(';').ToList();
            foreach (string val in paletteValues)
                ((Button)GetType().GetField($"btColorSave{paletteValues.IndexOf(val) + 1}", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(this)).BackColor = Color.FromArgb(int.Parse(val));
        }
        private void listPalettes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btRemovePalette.Enabled = btSavePalette.Enabled = listPalettes.SelectedIndex > -1 && listPalettes.SelectedItem?.ToString() != "Custom" && listPalettes.SelectedItem?.ToString() != "Palette1";
        }
        private void btNewPalette_Click(object sender, EventArgs e)
        {
            /*Clipboard.SetText(GetCurrentPalette());
            return;
            */
            if (string.IsNullOrWhiteSpace(tbPaletteName.Text))
                return;

            listPalettes.Items.Add(tbPaletteName.Text);

            SettingsProperty property = new SettingsProperty(tbPaletteName.Text);
            property.DefaultValue = "-1;-1;-1;-1;-1;-1;-1;-1;-1;-1";
            property.IsReadOnly = false;
            property.PropertyType = typeof(string);
            property.Provider = Properties.Settings.Default.Providers["LocalFileSettingsProvider"];
            property.Attributes.Add(typeof(UserScopedSettingAttribute), new UserScopedSettingAttribute());
            Properties.Settings.Default.Properties.Add(property);

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            SetCurrentPaletteWithValues((string)property.DefaultValue);
        }
        private void btSavePalette_Click(object sender, EventArgs e)
        {
            if (listPalettes.SelectedIndex == -1)
                return;

            SaveSettingPalette((string)listPalettes.SelectedItem, GetCurrentPalette());
        }
        private void btLoadPalette_Click(object sender, EventArgs e)
        {
            if (listPalettes.SelectedItem == null)
                return;

            SetCurrentPalette((string)listPalettes.SelectedItem);
        }
        private void btRemovePalette_Click(object sender, EventArgs e)
        {
            listPalettes.Items.Remove(tbPaletteName.Text);
            Properties.Settings.Default.Properties.Remove(tbPaletteName.Text);
            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
        }
        private void listPalettes_DoubleClick(object sender, EventArgs e)
        {
            if (listPalettes.SelectedItem == null)
                return;

            SetCurrentPalette((string)listPalettes.SelectedItem);
        }

        private void MenuBtNew_Click(object sender, EventArgs e)
        {
            Render.Image = new Bitmap(Render.Width, Render.Height);
            RouteBackgroundPath.Image = Render.Image;
            RouteBackgroundSigns.Image = Render.Image;
            picSignLocation.Image = Render.Image;
            DrawSignsInRoute();
            Path.Image = new Bitmap(Path.Width, Path.Height);
            PathSigns.Image = new Bitmap(PathSigns.Width, PathSigns.Height);
            picSignLocation.Image = new Bitmap(picSignLocation.Width, picSignLocation.Height);
            OnLoad(null);
        }
        private void MenuBtLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dial = new OpenFileDialog();
            dial.InitialDirectory = Directory.GetCurrentDirectory();
            dial.Filter = "TILE files (*.tile)|*.tile";
            if (dial.ShowDialog() == DialogResult.OK)
            {
                CurFileName = dial.FileName;
                Tile tile = Tools.DeserializeJSONFromFile<Tile>(CurFileName);
                if (tile != null)
                {
                    Render.Image = FromSmallToLarge((Bitmap)Tools.ByteArrayToObject(tile.ImageBytes));
                    g.DrawImage(Render.Image, Point.Empty);
                    RouteBackgroundPath.Image = Render.Image;
                    RouteBackgroundSigns.Image = Render.Image;
                    picSignLocation.Image = Render.Image;
                    DrawSignsInRoute();
                    Tools.ForEachPathDots((x) => pathDots[x] = tile.pathDots[x]);
                    Tools.ForEachPathDots((x) => pathDotsSign[x] = tile.pathDotsSign[x]);
                    nupIndex.Value = tile.Index;

                    DrawPathDots();
                }
            }
        }
        private void MenuBtSave_Click(object sender, EventArgs e)
        {
            Tile tile;

            if (string.IsNullOrWhiteSpace(CurFileName))
            {
                SaveFileDialog dial = new SaveFileDialog();
                dial.InitialDirectory = Directory.GetCurrentDirectory();
                dial.Filter = "TILE files (*.tile)|*.tile";
                if (dial.ShowDialog() == DialogResult.OK)
                {
                    CurFileName = dial.FileName;
                }
                else
                {
                    return;
                }
            }

            tile = CreateTile(System.IO.Path.GetFileName(CurFileName));
            Tools.Serialize(tile, CurFileName);
        }
        private void MenuBtSaveAs_Click(object sender, EventArgs e)
        {
            Tile tile;
            SaveFileDialog dial = new SaveFileDialog();
            dial.InitialDirectory = Directory.GetCurrentDirectory();
            dial.Filter = "TILE files (*.tile)|*.tile";
            if (dial.ShowDialog() == DialogResult.OK)
            {
                CurFileName = dial.FileName;
                tile = CreateTile(System.IO.Path.GetFileName(CurFileName));
            }
            else
            {
                return;
            }
            Tools.Serialize(tile, CurFileName);
        }

        private Tile CreateTile(string Name = "")
        {
            Tile tile = new Tile();
            tile.Name = Name;
            tile.Index = (int)nupIndex.Value;
            Bitmap bmp = FromLargeToSmall((Bitmap)Render.Image);
            bmp.MakeTransparent(btColorTransparent.BackColor);
            tile.ImageBytes = Tools.ObjectToByteArray(bmp);
            tile.SetPathDots(pathDots);
            tile.SetPathDotsSign(pathDotsSign);
            return tile;
        }
        private Bitmap FromLargeToSmall(Bitmap img)
        {
            int size = SharedData.TileSize;
            Bitmap result = new Bitmap(size, size);
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    result.SetPixel(x, y, img.GetPixel(x * PixelSize, y * PixelSize));
            return result;
        }
        private Bitmap FromSmallToLarge(Bitmap img, int size = -1, int newSize = -1)
        {
            if (size == -1)
                size = SharedData.TileSize;

            if (newSize == -1)
                newSize = PixelSize;

            Bitmap result = new Bitmap(Render.Width, Render.Height);
            using (Graphics g = Graphics.FromImage(result))
                for (int x = 0; x < size; x++)
                    for (int y = 0; y < size; y++)
                        g.FillRectangle(new SolidBrush(img.GetPixel(x, y)), x * newSize, y * newSize, newSize, newSize);
            return result;
        }

        private void DrawPathDots()
        {
            Brush GetBrushFromPathDotType(PathDotType type)
            {
                switch (type)
                {
                    default:
                    case PathDotType.None: return Brushes.White;
                    case PathDotType.In: return new SolidBrush(In.BackColor);
                    case PathDotType.Out: return new SolidBrush(Out.BackColor);
                    case PathDotType.Way: return new SolidBrush(Way.BackColor);
                }
            }

            Bitmap img = new Bitmap(Path.Width, Path.Height);
            Bitmap imgPathSigns = new Bitmap(PathSigns.Width, PathSigns.Height);
            Graphics gSigns = Graphics.FromImage(imgPathSigns);
            using (Graphics g = Graphics.FromImage(img))
            {
                string[] listPathDotsNames = Enum.GetNames(typeof(PathDots));
                Rectangle rect = Rectangle.Empty;
                foreach (string PathDotsName in listPathDotsNames)
                {
                    PathDots PathDotsValue = Tools.EnumParser<PathDots>(PathDotsName);
                    rect = PathDotsRect[PathDotsValue];

                    g.FillEllipse(GetBrushFromPathDotType(pathDots[PathDotsValue]), rect);
                    g.DrawEllipse(new Pen(Color.Black, 2F), rect);

                    gSigns.FillEllipse(new SolidBrush(pathDotsSign[PathDotsValue].Index == 0 ? Color.White : Color.DarkGray), rect);
                    gSigns.DrawEllipse(new Pen(SelectedSign == PathDotsValue ? Color.Blue : Color.Black, 2F), rect);

                    if (pathDotsSign[PathDotsValue].Index != 0)
                        DrawSignsInRoute();
                }
            }
            gSigns.Dispose();

            Path.Image = img;
            PathSigns.Image = imgPathSigns;
        }
        private void DrawSignsInRoute()
        {
            picSignLocation.Image = Render.Image;
            Bitmap img = new Bitmap(Render.Image);
            Graphics g = Graphics.FromImage(img);

            foreach (TileSignObj sign in pathDotsSign.Select(x => x.Value))
            {
                if (sign.Index == 0)
                    continue;

                Bitmap signImg = FromSmallToLarge(Tools.ApplyRotation(Instance.ResourcesSignInfo[sign.Index].Image, sign.Angle), SharedData.SignSize, 2);
                Point pt = new Point(sign.Position.X * PixelSize, sign.Position.Y * PixelSize);
                g.DrawImage(signImg, pt);
                if (sign.ShowBar)
                {
                    g.DrawLine(new Pen(Color.LightGray, 3F), new Point(pt.X + 6, pt.Y + 16), new Point(pt.X + 6, pt.Y + 24));
                    g.DrawLine(new Pen(Color.DimGray, 3F), new Point(pt.X + 8, pt.Y + 16), new Point(pt.X + 8, pt.Y + 24));
                }
            }

            g.Dispose();
            picSignLocation.Image = img;
        }

        private void None_Click(object sender, EventArgs e)
        {
            pathDotType = PathDotType.None;
        }
        private void In_Click(object sender, EventArgs e)
        {
            pathDotType = PathDotType.In;
        }
        private void Way_Click(object sender, EventArgs e)
        {
            pathDotType = PathDotType.Way;
        }
        private void Out_Click(object sender, EventArgs e)
        {
            pathDotType = PathDotType.Out;
        }

        private void btRotate_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(Render.Image);
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            Render.Image = img;
            RenderImage = img;
            RouteBackgroundPath.Image = img;
            RouteBackgroundSigns.Image = img;
            picSignLocation.Image = img;
            DrawSignsInRoute();
            g.Dispose();
            g = Graphics.FromImage(RenderImage);
        }
        private void btFlip_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(Render.Image);
            img.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Render.Image = img;
            RenderImage = img;
            RouteBackgroundPath.Image = img;
            RouteBackgroundSigns.Image = img;
            picSignLocation.Image = img;
            DrawSignsInRoute();
            g.Dispose();
            g = Graphics.FromImage(RenderImage);
        }

        private void cbbSigns_SelectedIndexChanged(object sender, EventArgs e)
        {
            picSelectedSign.Image = imgSignList.Images[cbbSigns.SelectedIndex];
        }

        private void btSignLocLeft_Click(object sender, EventArgs e)
        {
            pathDotsSign[SelectedSign].Position.X--;
            DrawSignsInRoute();
        }
        private void btSignLocRight_Click(object sender, EventArgs e)
        {
            pathDotsSign[SelectedSign].Position.X++;
            DrawSignsInRoute();
        }
        private void btSignLocUp_Click(object sender, EventArgs e)
        {
            pathDotsSign[SelectedSign].Position.Y--;
            DrawSignsInRoute();
        }
        private void btSignLocDown_Click(object sender, EventArgs e)
        {
            pathDotsSign[SelectedSign].Position.Y++;
            DrawSignsInRoute();
        }
        private void cbSignHasBar_CheckedChanged(object sender, EventArgs e)
        {
            pathDotsSign[SelectedSign].ShowBar = cbSignHasBar.Checked;
            DrawSignsInRoute();
        }
        private void btSignRotate_Click(object sender, EventArgs e)
        {
            Tools.Rotate(ref pathDotsSign[SelectedSign].Angle);
            DrawSignsInRoute();
        }
    }
}
