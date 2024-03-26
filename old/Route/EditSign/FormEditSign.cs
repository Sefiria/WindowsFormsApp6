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
using static Script.InputScript;
using Script;

namespace EditSign
{
    public partial class FormEditSign : Form
    {
        PenType penType;

        Timer TimerUpdate = new Timer() { Interval = 50 };
        Timer TimerDraw = new Timer() { Interval = 50 };
        Graphics g;
        Bitmap RenderImage;
        public static readonly int PixelSize = 16;
        string CurFileName;
        InputScript script = null;

        Point MouseClient => Render.PointToClient(MousePosition);

        public FormEditSign()
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

            listPalettes.Items.Clear();
            penType = PenType.Pen;
            CurFileName = null;
            nupIndex.Value = Tools.GetFilesCount(Directory.GetCurrentDirectory(), "*.sign");

            #region Image
            RenderImage = new Bitmap(Render.Width, Render.Height);
            g = Graphics.FromImage(RenderImage);
            btColorTransparent.BackColor = Color.LightGray;
            g.Clear(btColorTransparent.BackColor);
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

            script = new InputScript();
        }
        protected override void OnClosed(EventArgs e)
        {
            TimerUpdate.Stop();
            TimerDraw.Stop();
            g.Dispose();

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
            g.FillRectangle(brush, new Rectangle(Tools.Snap(position, PixelSize), new Size(PixelSize, PixelSize)));
            Render.Image = new Bitmap(RenderImage);
        }
        private void Fill(Point position, PenType type = PenType.Pen)
        {
            position = Tools.Snap(position, PixelSize);
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

        }
        private void btLoadPalette_Click(object sender, EventArgs e)
        {

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
            OnLoad(null);
        }
        private void MenuBtLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dial = new OpenFileDialog();
            dial.InitialDirectory = Directory.GetCurrentDirectory();
            dial.Filter = "SIGN files (*.sign)|*.sign";
            if (dial.ShowDialog() == DialogResult.OK)
            {
                CurFileName = dial.FileName;
                Sign sign = Tools.DeserializeJSONFromFile<Sign>(CurFileName);
                if (sign != null)
                {
                    Render.Image = FromSmallToLarge((Bitmap)Tools.ByteArrayToObject(sign.ImageBytes));
                    g.DrawImage(Render.Image, Point.Empty);
                    nupIndex.Value = sign.Index;
                    script = sign.script;
                }
            }
        }
        private void MenuBtSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurFileName))
            {
                MenuBtSaveAs_Click(sender, e);
            }
            else
            {
                Tools.Serialize(CreateSign(), CurFileName);
            }
        }
        private void MenuBtSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog dial = new SaveFileDialog();
            dial.InitialDirectory = Directory.GetCurrentDirectory();
            dial.Filter = "SIGN files (*.sign)|*.sign";
            if (dial.ShowDialog() == DialogResult.OK)
            {
                CurFileName = dial.FileName;
                MenuBtSave_Click(sender, e);
            }
        }

        private Sign CreateSign()
        {
            Sign sign = new Sign();
            sign.Index = (int)nupIndex.Value;
            Bitmap bmp = FromLargeToSmall((Bitmap)Render.Image);
            bmp.MakeTransparent(btColorTransparent.BackColor);
            sign.ImageBytes = Tools.ObjectToByteArray(bmp);
            sign.script = script;
            return sign;
        }
        private Bitmap FromLargeToSmall(Bitmap img)
        {
            int size = SharedData.SignSize;
            Bitmap result = new Bitmap(size, size);
            for (int x = 0; x < size; x++)
                for (int y = 0; y < size; y++)
                    result.SetPixel(x, y, img.GetPixel(x * PixelSize, y * PixelSize));
            return result;
        }
        private Bitmap FromSmallToLarge(Bitmap img)
        {
            int size = SharedData.SignSize;
            Bitmap result = new Bitmap(Render.Width, Render.Height);
            using (Graphics g = Graphics.FromImage(result))
                for (int x = 0; x < size; x++)
                    for (int y = 0; y < size; y++)
                        g.FillRectangle(new SolidBrush(img.GetPixel(x, y)), x * PixelSize, y * PixelSize, PixelSize, PixelSize);
            return result;
        }
        
        private void btRotate_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(Render.Image);
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            Render.Image = img;
            RenderImage = img;
            g.Dispose();
            g = Graphics.FromImage(RenderImage);
        }
        private void btFlip_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(Render.Image);
            img.RotateFlip(RotateFlipType.RotateNoneFlipX);
            Render.Image = img;
            RenderImage = img;
            g.Dispose();
            g = Graphics.FromImage(RenderImage);
        }

        private void btEditScript_Click(object sender, EventArgs e)
        {
            int X = 5, Y = 6;

            Cell Self = new Cell(X, Y, new Entity());


            Dictionary<string, PVarType> PublicVariables = new Dictionary<string, PVarType>()
            {
["A"] = new PVarType(new[] { X-1, Y-1, 1 }), ["Z"] = new PVarType(new[] { X, Y-1, 0 }), ["E"] = new PVarType(new[] { X+1, Y-1, 1 }),
["Q"] = new PVarType(new[] { X-1, Y,   0 }),                                            ["D"] = new PVarType(new[] { X+1, Y,   0 }),
["W"] = new PVarType(new[] { X-1, Y+1, 0 }), ["X"] = new PVarType(new[] { X, Y+1, 1 }), ["C"] = new PVarType(new[] { X+1, Y+1, 0 }),
            };
            List<Verb<DFunction>> VerbsFunctions = new List<Verb<DFunction>>()
            {
                new Verb<DFunction>("Set", Self.Set, "Set the values", new []{ $"Function:{string.Join(",", Enum.GetNames(typeof(Entity.RelativeCell)))}" })
            };


            InputScriptEditor editor = new InputScriptEditor(script, Self.GetType(), () => { });
            InputScript.Initialize(PublicVariables, VerbsFunctions);

            editor.ShowDialog();
            editor.Execute(Self);
        }
    }
}