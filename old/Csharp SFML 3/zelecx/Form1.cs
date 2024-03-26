using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static zelecx.StructuresAndCells;

namespace zelecx
{
    public partial class _Main : Form
    {
        static private class DebugInfos
        {
            static public bool ShowDebugInfos = false;
            static public bool MapIds => ShowDebugInfos && false;
            static public bool InfosUnderMouse => ShowDebugInfos && true;

            public static Font DebugFont = new Font("Arial", 5F);
        }

        public static _Main m_instance = null;
        public static _Main Instance { get { if (m_instance == null) m_instance = new _Main(false, new Size(480, 480)); return m_instance; } }

        public enum Tools { Wire = 1, Generator, MergeTool, AndGate, XorGate, Switch, Potentiometer, ExternalOutput_1x1, ExternalOutput_3x3, ExternalInput }

        #region Timers
        public Timer TimerHold = new Timer() { Enabled = false, Interval = 5 };
        public Timer TimerDraw = new Timer() { Enabled = false, Interval = 5 };
        public Timer TimerUpdate = new Timer() { Enabled = false, Interval = 5 };
        #endregion
        #region Const Members
        public Size WinSize = new Size(480, 480);
        static public int TileSize = 8;
        public Rectangle Camera = Rectangle.Empty;
        private Size _TileCount = new Size(8, 8);
        public Size TileCount => Camera != Rectangle.Empty ? Camera.Size : _TileCount;
        public Point CameraPosition => Camera != Rectangle.Empty ? Camera.Location : Point.Empty;
        static private Image BackgroundAndGridImage = null;
        #endregion
        #region Common Members
        public Cell[][] map;
        public Tools tool = Tools.Wire;
        public List<Structure> structures = new List<Structure>();
        public int Rotation = 0;
        #endregion
        #region Palette Colors
        public Color ColorBackground, ColorGrid, ColorWire, ColorOn;
        public void LoadPalette(string PaletteName)
        {
            string PAL = Properties.Resources.ResourceManager.GetString(PaletteName);
            var PALColorStrings = PAL.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);

            ColorBackground = (Color)new ColorConverter().ConvertFromString("#" + PALColorStrings[0]);
            ColorGrid = (Color)new ColorConverter().ConvertFromString("#" + PALColorStrings[1]);
            ColorWire = (Color)new ColorConverter().ConvertFromString("#" + PALColorStrings[2]);
            ColorOn = (Color)new ColorConverter().ConvertFromString("#" + PALColorStrings[3]);
        }
        #endregion
        #region Temporary Variables
        public Point MousePositionAtDown = Point.Empty;
        public Point MousePositionPreviousTick = Point.Empty;
        public int MapIdToSet = 0;
        public Point PreviousMouse = Point.Empty;
        public float DebugToolTipTension = 0F;
        public bool CtrlHolding = false;
        #endregion
        #region Tools Properties / Methods
        #region Mouse
        //public Point CameraMouse => new Point(MousePosition.X + CameraPosition.X, MousePosition.Y + CameraPosition.Y);
        public Point ClientMouse => Render.IsDisposed ? MousePosition : Render.PointToClient(MousePosition);
        public Point MouseTile => Maths.PositionTile(ClientMouse);
        public Point MouseSnap => Maths.PositionSnap(ClientMouse);
        public Point CameraMouse => new Point(ClientMouse.X + CameraPosition.X, ClientMouse.Y + CameraPosition.Y);
        #endregion
        #region Map
        public Rectangle MapBounds => new Rectangle(0, 0, WinSize.Width, WinSize.Height);
        public bool CellUnderMouseHasStructure => GetCellUnderMouse().HasContainer;
        public Structure GetStructureUnderMouse => GetCellUnderMouse().Container;
        public Cell GetCellUnderMouse() { var MouseTile = this.MouseTile; return MapBounds.Contains(MouseTile) ? map[MouseTile.X][MouseTile.Y] : new Cell(MouseTile.X, MouseTile.Y); }
        public Cell GetCellUnderPosition(int x, int y) => MapBounds.Contains(x, y) ? map[x][y] : new Cell(x, y);
        public int GetMapIdUnderMouse => GetCellUnderMouse().Id;
        public int GetMapIdUnderPosition(int x, int y) => GetCellUnderPosition(x, y).Id;
        public bool MapIdUnderMouseIs0 => GetCellUnderMouse().Id == 0;
        public bool MapIdUnderPositionIs0(int x, int y) => GetCellUnderPosition(x, y).Id == 0;
        public void SetMapIdUnderMouse(int id) { var MouseTile = this.MouseTile; if (MapBounds.Contains(MouseTile)) map[MouseTile.X][MouseTile.Y].Id = id; }
        public void ResetMapIdUnderMouse()
        {
            var MouseTile = this.MouseTile;
            if (MapBounds.Contains(MouseTile))
                map[MouseTile.X][MouseTile.Y].Reset();
        }
        public void FloodFillResetMapIdUnderMouse()
        {
            var MouseTile = this.MouseTile;
            if (MapBounds.Contains(MouseTile))
            {
                if(GetMapIdUnderMouse == 1)
                {
                    void RecursiveFloodFill(int x, int y)
                    {
                        map[x][y].Reset();

                        if (GetMapIdUnderPosition(x, y-1) == 1)
                            RecursiveFloodFill(x, y-1);
                        if (GetMapIdUnderPosition(x-1, y) == 1)
                            RecursiveFloodFill(x-1, y);
                        if (GetMapIdUnderPosition(x, y+1) == 1)
                            RecursiveFloodFill(x, y+1);
                        if (GetMapIdUnderPosition(x+1, y) == 1)
                            RecursiveFloodFill(x+1, y);
                    }
                    RecursiveFloodFill(MouseTile.X, MouseTile.Y);
                }
            }
        }
        public void SetMapId(int id, int x, int y)
        {
            if (!MapBounds.Contains(x, y))
                return;

            if (id < 2)
            {
                if (id == 0)
                    map[x][y].Reset();
                else
                    map[x][y].Id = id;
                return;
            }

            Structure.CreateStructure(tool, x, y);
        }
        #endregion
        #region Misc
        public Image SetImageOpacity(Image image, float opacity)
        {
            //create a Bitmap the size of the image provided  
            Bitmap bmp = new Bitmap(image.Width, image.Height);

            //create a graphics object from the image  
            using (Graphics gfx = Graphics.FromImage(bmp))
            {

                //create a color matrix object  
                ColorMatrix matrix = new ColorMatrix();

                //set the opacity  
                matrix.Matrix33 = opacity;

                //create image attributes  
                ImageAttributes attributes = new ImageAttributes();

                //set the color(opacity) of the image  
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //now draw the image  
                gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
            }
            return bmp;
        }
        public Image RotateImage(Image _img, int rotation = -1)
        {
            if (rotation == -1)
                rotation = Rotation;

            Image img = (Image)_img.Clone();
            if (rotation > 0)
            {
                if (rotation == 90)
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                if (rotation == 180)
                    img.RotateFlip(RotateFlipType.RotateNoneFlipY);
                if (rotation == 270)
                    img.RotateFlip(RotateFlipType.Rotate90FlipX);
            }
            return img;
        }
        #endregion
        #endregion

        public _Main(bool CallFromMainRun, Size _WinSize, int _TileSize = 8, Panel panelAttached = null, Image RenderBackground = null, Rectangle? _Camera = null)
        {
            InitializeComponent();

            if (CallFromMainRun)
                m_instance = this;

            if (panelAttached != null)
            {
                if (RenderBackground != null)
                {
                    Render.Parent = panelAttached;
                    Render.Location = Point.Empty;
                }
                this.Visible = false;
                this.ShowInTaskbar = false;
                if (_Camera != null)
                    Camera = _Camera.Value;
                else
                    Camera = new Rectangle(0, 0, panelAttached.Width / TileSize, panelAttached.Height / TileSize);
            }

            WinSize = _WinSize;
            TileSize = _TileSize;
            Size = new Size(WinSize.Width + 16, WinSize.Height + 39);
            Render.Size = new Size(Camera.Width * TileSize, Camera.Height* TileSize);
            _TileCount = new Size(WinSize.Width / TileSize, WinSize.Height / TileSize);

            ImagesManager.Initialize(TileSize);
            LoadPalette("PAL_DEFAULT");

            TimerHold.Tick += TimerHold_Tick;
            TimerDraw.Tick += TimerDraw_Tick;
            TimerUpdate.Tick += TimerUpdate_Tick;

            map = new Cell[WinSize.Width][];
            for(int x=0; x< WinSize.Width; x++)
            {
                map[x] = new Cell[WinSize.Height];
                for (int y = 0; y < WinSize.Height; y++)
                    map[x][y] = new Cell(x, y);
            }

            if (RenderBackground != null && Camera != Rectangle.Empty)
            {
                BackgroundAndGridImage = new Bitmap(Camera.Width * TileSize, Camera.Height * TileSize);
                Graphics g = Graphics.FromImage(BackgroundAndGridImage);
                if (RenderBackground != null)
                    g.DrawImage(RenderBackground, 0, 0);
                Color color = Color.FromArgb(128, ColorBackground.R, ColorBackground.G, ColorBackground.B);
                g.FillRectangle(new SolidBrush(color), 0, 0, Camera.Width * TileSize, Camera.Height * TileSize);
                for (int X = 0; X < TileCount.Width; X++)
                    for (int Y = 0; Y < TileCount.Height; Y++)
                        g.DrawRectangle(new Pen(ColorGrid, 1), X * TileSize, Y * TileSize, TileSize, TileSize);
                g.Dispose();
            }

            if(!(CallFromMainRun && RenderBackground == null))
                TimerDraw.Start();
            TimerUpdate.Start();
        }
        public void EndModuleTask()
        {
            if (m_instance != null)
            {
                Render.Parent = this;
                Close();
            }
        }
        public void UpdateCamera(int CameraX, int CameraY, Image background = null)
        {
            Camera.X = CameraX;
            Camera.Y = CameraY;

            if (background != null)
            {
                BackgroundAndGridImage = new Bitmap(Camera.Width * TileSize, Camera.Height * TileSize);
                Graphics g = Graphics.FromImage(BackgroundAndGridImage);
                g.DrawImage(background, 0, 0);
                Color color = Color.FromArgb(128, ColorBackground.R, ColorBackground.G, ColorBackground.B);
                g.FillRectangle(new SolidBrush(color), 0, 0, Camera.Width * TileSize, Camera.Height * TileSize);
                for (int X = 0; X < TileCount.Width; X++)
                    for (int Y = 0; Y < TileCount.Height; Y++)
                        g.DrawRectangle(new Pen(ColorGrid, 1), X * TileSize, Y * TileSize, TileSize, TileSize);
                g.Dispose();
            }
        }

        public void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)// Add
            {
                var cell = GetCellUnderMouse();
                if (tool == Tools.Wire && !CellUnderMouseHasStructure)
                {
                    MousePositionAtDown = ClientMouse;
                    MapIdToSet = GetMapIdUnderMouse == (int)tool ? 0 : (int)tool;
                    TimerHold.Start();
                }
                else
                {
                    if(MapIdUnderMouseIs0)
                        SetMapId((int)tool, MouseTile.X, MouseTile.Y);
                }
            }
            else if (e.Button == MouseButtons.Right)// Remove
            {
                if (CtrlHolding)
                    FloodFillResetMapIdUnderMouse();
                else
                    ResetMapIdUnderMouse();
            }
            else if (e.Button == MouseButtons.Middle)// Action
            {
                if (CellUnderMouseHasStructure)
                    GetCellUnderMouse().Container.DoAction();
            }

            structures.ForEach(
                s =>
                {
                    if (s is StructureUserControl)
                        (s as StructureUserControl).UpdateOnClick(e);
                });
        }
        public void Render_MouseUp(object sender, MouseEventArgs e)
        {
            TimerHold.Stop();
            MousePositionAtDown = Point.Empty;
            MousePositionPreviousTick = Point.Empty;
        }
        public void _Main_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1)
                tool = Tools.Wire;
            if (e.KeyCode == Keys.D2)
                tool = tool == Tools.Generator ? Tools.MergeTool : Tools.Generator;
            if (e.KeyCode == Keys.D3)
                tool = tool == Tools.AndGate ? Tools.XorGate : Tools.AndGate;
            if (e.KeyCode == Keys.D4)
                tool = tool == Tools.Switch ? Tools.Potentiometer : Tools.Switch;
            if (e.KeyCode == Keys.D5)
                tool = tool == Tools.ExternalOutput_1x1 ? Tools.ExternalOutput_3x3 : Tools.ExternalOutput_1x1;
            if (e.KeyCode == Keys.D6)
                tool = Tools.ExternalInput;

            if (e.KeyCode == Keys.F12)
                DebugInfos.ShowDebugInfos = !DebugInfos.ShowDebugInfos;

            if (e.KeyCode == Keys.C)
                ClearMap();
            if (e.KeyCode == Keys.S)
                SaveMap("map.MAP");
            if (e.KeyCode == Keys.L)
                LoadMap("map.MAP");

            if (e.KeyCode == Keys.R)
            {
                if (e.Control)
                    Rotation = Rotation == 0 ? 270 : (Rotation - 90);
                else
                    Rotation = Rotation == 270 ? 0 : (Rotation + 90);
            }

            CtrlHolding = e.Control;
        }

        private void ClearMap()
        {
            for (int X = 0; X < WinSize.Width; X++)
                for (int Y = 0; Y < WinSize.Height; Y++)
                    if(map[X][Y].Id > 0)
                        map[X][Y].Reset();
            structures.Clear();
        }

        public void _Main_KeyUp(object sender, KeyEventArgs e)
        {
            CtrlHolding = false;
        }

        private void TimerHold_Tick(object sender, EventArgs e)
        {
            Point Start = MousePositionPreviousTick == Point.Empty ? ClientMouse : MousePositionPreviousTick;
            Point End = ClientMouse;
            Point LerpPosition = Point.Empty;
            float d = Maths.Distance(Start, End);
            float tIncr = Maths.FloatsComparison(d, 0F, 0.0001F) ? 1F : (1F / d);
            for (float t = 0F; t <= 1F; t += tIncr)
            {
                LerpPosition = Maths.PositionTile(Maths.Lerp2(Start, End, t));
                if(!GetCellUnderPosition(LerpPosition.X, LerpPosition.Y).HasContainer)
                    SetMapId(MapIdToSet, LerpPosition.X, LerpPosition.Y);
            }
            MousePositionPreviousTick = ClientMouse;
        }

        private void TimerDraw_Tick(object sender, EventArgs e)
        {
            Image img = (Image)BackgroundAndGridImage.Clone();
            Graphics g = Graphics.FromImage(img);
            int RenderX, RenderY;

            for (int X = CameraPosition.X; X < CameraPosition.X + Camera.Width; X++)
            {
                for (int Y = CameraPosition.Y; Y < CameraPosition.Y + Camera.Height; Y++)
                {
                    RenderX = (X - CameraPosition.X) * TileSize;
                    RenderY = (Y - CameraPosition.Y) * TileSize;
                    if (map[X][Y].Id == 1)
                    {
                        if (Maths.FloatsComparison(map[X][Y].Tension, 0F, 0.001F))
                            g.FillRectangle(new SolidBrush(ColorWire), RenderX, RenderY, TileSize, TileSize);
                        else
                        {
                            byte tensionR = (byte)(ColorWire.R + map[X][Y].Tension * (ColorOn.R - ColorWire.R));
                            byte tensionG = (byte)(ColorWire.G + map[X][Y].Tension * (ColorOn.G - ColorWire.G));
                            byte tensionB = (byte)(ColorWire.B + map[X][Y].Tension * (ColorOn.B - ColorWire.B));
                            g.FillRectangle(new SolidBrush(Color.FromArgb(tensionR, tensionG, tensionB)), RenderX, RenderY, TileSize, TileSize);
                        }
                    }
                    else if (map[X][Y].Id > 1)
                    {
                        if (map[X][Y].HasContainer)
                        {
                            if (map[X][Y].Container.AlreadyDrawn)
                                continue;
                            else
                                map[X][Y].Container.AlreadyDrawn = true;

                            map[X][Y].Container.AdditionnalDraw_PreDraw(g);

                            g.DrawImage(RotateImage(ImagesManager.GetImageFromStructureType(map[X][Y].Container), map[X][Y].Container.Rotation), (map[X][Y].Container.Bounds.X - CameraPosition.X) * TileSize, (map[X][Y].Container.Bounds.Y - CameraPosition.Y) * TileSize);

                            map[X][Y].Container.AdditionnalDraw_ApDraw(g);
                        }
                    }
                }
            }

            structures.ForEach(s => s.AlreadyDrawn = false);

            g.DrawImage(RotateImage(Properties.Resources.ArrowOrientation), 0, (Camera.Height - 1) * TileSize);

            if (tool != Tools.Wire)
                g.DrawImage(RotateImage(ImagesManager.GetImageFromTool(tool)), MouseSnap);
            DrawDebugInfos(g);


            Render.Image = img;
            g.Dispose();
        }
        private void DrawDebugInfos(Graphics g)
        {
            if (!DebugInfos.MapIds)
                return;

            for (int X = CameraPosition.X; X < CameraPosition.X + Camera.Width; X++)
            {
                for (int Y = CameraPosition.Y; Y < CameraPosition.Y + Camera.Height; Y++)
                {
                    int x = (int)(X * TileSize + TileSize / 2 - g.MeasureString(map[X][Y].Id.ToString(), DebugInfos.DebugFont).Width / 2);
                    int y = (int)(Y * TileSize + TileSize / 2 - g.MeasureString(map[X][Y].Id.ToString(), DebugInfos.DebugFont).Height / 2);
                    g.DrawString(map[X][Y].Id.ToString(), DebugInfos.DebugFont, Brushes.White, x, y);
                }
            }
        }

        private void TimerUpdate_Tick(object sender, EventArgs e)
        {
            structures.ForEach(
                s =>
                {
                    if (s is StructureUserControl)
                        (s as StructureUserControl).Update();

                    s.DeployTensionToCells();
                });

            for (int X = CameraPosition.X; X < CameraPosition.X + Camera.Width; X++)
            {
                for (int Y = CameraPosition.Y; Y < CameraPosition.Y + Camera.Height; Y++)
                {
                    if (!MapIdUnderPositionIs0(X, Y))
                        map[X][Y].DiffuseTension();
                    map[X][Y].LooseTension();
                }
            }

            UpdateDebugInfos();

            PreviousMouse = ClientMouse;
        }
        private void UpdateDebugInfos()
        {
            if (DebugInfos.InfosUnderMouse)
            {
                if (!Render.Bounds.Contains(ClientMouse))
                    return;

                Cell cell = GetCellUnderMouse();
                if (PreviousMouse != ClientMouse || (cell.HasContainer ? cell.Container.Tension : cell.Tension) != DebugToolTipTension)
                {
                    string infos = "";
                    if (cell.HasContainer)
                        infos += $"Type : {cell.Container.StructureName}" + Environment.NewLine;
                    else
                    {
                        if (cell.Id == 0)
                            infos += "Type : Empty Cell" + Environment.NewLine;
                        else
                            infos += "Type : Wire" + Environment.NewLine;
                    }
                    infos += $"X : {MouseTile.X}, Y : {MouseTile.Y}" + Environment.NewLine;
                    infos += $"ID : {cell.Id}" + Environment.NewLine;
                    infos += $"Tension : {DebugToolTipTension = (cell.HasContainer ? cell.Container.Tension : cell.Tension)}" + Environment.NewLine;
                    if (cell.Container != null)
                        if (cell.Container is ISupportValue)
                            infos += (cell.Container as ISupportValue).Value + Environment.NewLine;

                    toolTip.Show(infos, Render);
                }
            }
            else
            {
                toolTip.Hide(Render);
            }
        }

        public void SaveMap(string filename)
        {
            using (Stream stream = File.OpenWrite(filename))
            {
                for (int x = 0; x < MapBounds.Width; x++)
                    for (int y = 0; y < MapBounds.Height; y++)
                        map[x][y].Tension = 0F;

                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, map);
            }
        }
        public void LoadMap(string filename)
        {
            try
            {
                using (Stream stream = File.OpenRead(filename))
                {
                    ClearMap();
                    IFormatter formatter = new BinaryFormatter();
                    map = (Cell[][])formatter.Deserialize(stream);
                }

                for (int x = 0; x < MapBounds.Width; x++)
                    for (int y = 0; y < MapBounds.Height; y++)
                        if (map[x][y].HasContainer)
                            if (!structures.Contains(map[x][y].Container))
                                structures.Add(map[x][y].Container);
            }
            catch(FileNotFoundException)
            {}
        }

        public void ForEach(Action<Cell> foreachInstructions)
        {
            map.ToList().ForEach(row => row.ToList().ForEach(cell => foreachInstructions(cell)));
        }
    }
}
