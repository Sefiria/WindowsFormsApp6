using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Script.StructuresAndCells;

namespace Script
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
        public Size TileCount = new Size(8, 8);
        #endregion
        #region Common Members
        private bool UpdateDirectionSwitch = false;
        public Cell[][] map;
        public Tools tool = Tools.Wire;
        public List<Structure> structures = new List<Structure>();
        public int Rotation = 0;
        public int WireType = 1;
        public List<int> WireIds = new List<int>() { 1, 100, 101, 102 };
        public Stopwatch Watch = new Stopwatch();
        public bool IsNull(Cell cell) => IsNull(cell.Id);
        public bool IsWire(Cell cell) => IsWire(cell.Id);
        public bool IsStructure(Cell cell) => IsStructure(cell.Id);
        public bool IsNull(int id) => id == 0;
        public bool IsWire(int id) => WireIds.Contains(id);
        public bool IsStructure(int id) => !IsNull(id) && !WireIds.Contains(id);
        #endregion
        #region Palette Colors
        public Color ColorBackground, ColorGrid, ColorOn;
        public Color[] ColorWire = new Color[4];
        public Color GetColorWire(int WireType)
        {
            switch(WireType)
            {
                default: throw new Exception("'WireType' isn't a wire type");
                case 1: return ColorWire[0];
                case 100: return ColorWire[1];
                case 101: return ColorWire[2];
                case 102: return ColorWire[3];
            }
        }
        public void LoadPalette(string PaletteName)
        {
            string PAL = Properties.Resources.ResourceManager.GetString(PaletteName);
            var PALColorStrings = PAL.Split(new string[] { "#" }, StringSplitOptions.RemoveEmptyEntries);

            ColorBackground = (Color)new ColorConverter().ConvertFromString(    "#"+PALColorStrings[0]);
            ColorGrid = (Color)new ColorConverter().ConvertFromString(          "#"+PALColorStrings[1]);
            ColorWire[0] = (Color)new ColorConverter().ConvertFromString(       "#"+PALColorStrings[2]);
            ColorWire[1] = (Color)new ColorConverter().ConvertFromString(       "#"+PALColorStrings[3]);
            ColorWire[2] = (Color)new ColorConverter().ConvertFromString(       "#"+PALColorStrings[4]);
            ColorWire[3] = (Color)new ColorConverter().ConvertFromString(       "#"+PALColorStrings[5]);
            ColorOn = (Color)new ColorConverter().ConvertFromString(            "#"+PALColorStrings[6]);
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
        public Point ClientMouse => Render.PointToClient(MousePosition);
        public Point MouseTile => Maths.PositionTile(ClientMouse);
        public Point MouseSnap => Maths.PositionSnap(ClientMouse);
        #endregion
        #region Map
        public Rectangle MapBounds => new Rectangle(0, 0, TileCount.Width, TileCount.Height);
        public bool CellUnderMouseHasStructure => GetCellUnderMouse().HasContainer;
        public Structure GetStructureUnderMouse => GetCellUnderMouse().Container;
        public Cell GetCellUnderMouse() { var MouseTile = this.MouseTile; return MapBounds.Contains(MouseTile) ? map[MouseTile.X][MouseTile.Y] : new Cell(MouseTile.X, MouseTile.Y); }
        public Cell GetCellUnderPosition(int x, int y) => MapBounds.Contains(x, y) ? map[x][y] : new Cell(x, y);
        public int GetMapIdUnderMouse => GetCellUnderMouse().Id;
        public int GetMapIdUnderPosition(int x, int y) => GetCellUnderPosition(x, y).Id;
        public bool MapIdUnderMouseIs0 => IsNull(GetCellUnderMouse());
        public bool MapIdUnderPositionIs0(int x, int y) =>IsNull( GetCellUnderPosition(x, y));
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

            if (!IsStructure(id))
            {
                if (id == 0)
                    map[x][y].Reset();
                else
                {
                    map[x][y].Id = id;
                    map[x][y].WireType = WireType;
                }
                return;
            }

            map[x][y].Container = Structure.CreateStructure(tool, x, y);
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

        public _Main(bool CallFromMainRun, Size _WinSize, int _TileSize = 8)
        {
            InitializeComponent();

            if (CallFromMainRun)
                m_instance = this;

            WinSize = _WinSize;
            TileSize = _TileSize;
            Size = new Size(WinSize.Width + 16, WinSize.Height + 39);
            Render.Size = WinSize;
            TileCount = new Size(WinSize.Width / TileSize, WinSize.Height / TileSize);

            ImagesManager.Initialize(TileSize);
            LoadPalette("PAL_DEFAULT");

            Watch.Start();

            TimerHold.Tick += TimerHold_Tick;
            TimerDraw.Tick += TimerDraw_Tick; ;
            TimerUpdate.Tick += TimerUpdate_Tick;

            map = new Cell[TileCount.Width][];
            for(int x=0; x<TileCount.Width; x++)
            {
                map[x] = new Cell[TileCount.Height];
                for (int y = 0; y < TileCount.Height; y++)
                    map[x][y] = new Cell(x, y);
            }

            TimerDraw.Start();
            TimerUpdate.Start();
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)// Add
            {
                var cell = GetCellUnderMouse();
                if (tool == Tools.Wire && !CellUnderMouseHasStructure)
                {
                    MousePositionAtDown = ClientMouse;
                    MapIdToSet = IsWire(GetMapIdUnderMouse) ? 0 : WireType;
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
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            TimerHold.Stop();
            MousePositionAtDown = Point.Empty;
            MousePositionPreviousTick = Point.Empty;
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Tools prevTool = tool; 
            if (e.KeyCode == Keys.D1)
            {
                tool = Tools.Wire;
                switch(WireType)
                {
                    case 1: WireType = 100; break;
                    case 100: WireType = 101; break;
                    case 101: WireType = 102; break;
                    case 102: WireType = 1; break;
                }
            }
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

            if(prevTool != tool)
                WireType = 1;

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
            for (int X = 0; X < TileCount.Width; X++)
                for (int Y = 0; Y < TileCount.Height; Y++)
                    map[X][Y].Reset();
            structures.Clear();
        }

        private void _Main_KeyUp(object sender, KeyEventArgs e)
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
            var img = new Bitmap(WinSize.Width, WinSize.Height);
            Graphics g = Graphics.FromImage(img);

            g.FillRectangle(new SolidBrush(ColorBackground), 0, 0, WinSize.Width, WinSize.Height);

            for (int X = 0; X < TileCount.Width; X++)
                for (int Y = 0; Y < TileCount.Height; Y++)
                    g.DrawRectangle(new Pen(ColorGrid, 1), X * TileSize, Y * TileSize, TileSize, TileSize);

            for (int X = 0; X < TileCount.Width; X++)
            {
                for (int Y = 0; Y < TileCount.Height; Y++)
                {
                    Cell cell = map[X][Y];
                    if (IsWire(cell))
                    {
                        if (Maths.FloatsComparison(cell.Tension, 0F, 0.001F))
                            g.FillRectangle(new SolidBrush(GetColorWire(cell.WireType)), X * TileSize, Y * TileSize, TileSize, TileSize);
                        else
                        {
                            byte tensionR = (byte)(GetColorWire(cell.WireType).R + cell.Tension * (ColorOn.R - GetColorWire(cell.WireType).R));
                            byte tensionG = (byte)(GetColorWire(cell.WireType).G + cell.Tension * (ColorOn.G - GetColorWire(cell.WireType).G));
                            byte tensionB = (byte)(GetColorWire(cell.WireType).B + cell.Tension * (ColorOn.B - GetColorWire(cell.WireType).B));
                            g.FillRectangle(new SolidBrush(Color.FromArgb(tensionR, tensionG, tensionB)), X * TileSize, Y * TileSize, TileSize, TileSize);
                        }
                    }
                    else if (IsStructure(cell))
                    {
                        if (cell.HasContainer)
                        {
                            if (cell.Container.AlreadyDrawn)
                                continue;
                            else
                                cell.Container.AlreadyDrawn = true;

                            cell.Container.AdditionnalDraw_PreDraw(g);

                            g.DrawImage(RotateImage(ImagesManager.GetImageFromStructureType(cell.Container), cell.Container.Rotation), cell.Container.Bounds.X * TileSize, cell.Container.Bounds.Y * TileSize);

                            cell.Container.AdditionnalDraw_ApDraw(g);
                        }
                    }
                }
            }

            structures.ForEach(s => s.AlreadyDrawn = false);

            g.DrawImage(RotateImage(Properties.Resources.ArrowOrientation), 0, WinSize.Height - TileSize * 2);

            g.DrawString(tool.ToString(), DefaultFont, Brushes.White, 3 * TileSize, WinSize.Height - TileSize * 2);
            if (tool == Tools.Wire)
            {
                string szWireType = "";
                switch(WireType)
                {
                    default:
                    case 1: szWireType = "(Gray)"; break;
                    case 100: szWireType = "(Red)"; break;
                    case 101: szWireType = "(Green)"; break;
                    case 102: szWireType = "(Blue)"; break;
                }
                g.DrawString(szWireType, DefaultFont, Brushes.White, 7 * TileSize, WinSize.Height - TileSize * 2);
            }

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

            for (int X = 0; X < TileCount.Width; X++)
            {
                for (int Y = 0; Y < TileCount.Height; Y++)
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
                    {
                        (s as StructureUserControl).Update();
                    }

                    s.DeployTensionToCells();
                });


            void DiffuseLooseTensionAction(int X, int Y)
            {
                if (!MapIdUnderPositionIs0(X, Y))
                {
                    map[X][Y].DiffuseTension();
                }
                map[X][Y].LooseTension();
            }
            if (UpdateDirectionSwitch)
            {
                for (int X = TileCount.Width - 1; X >= 0 ; X--)
                {
                    for (int Y = TileCount.Height - 1; Y >= 0; Y--)
                    {
                        DiffuseLooseTensionAction(X, Y);
                    }
                }
            }
            else
            {
                for (int X = 0; X < TileCount.Width; X++)
                {
                    for (int Y = 0; Y < TileCount.Height; Y++)
                    {
                        DiffuseLooseTensionAction(X, Y);
                    }
                }
            }
            UpdateDirectionSwitch = !UpdateDirectionSwitch;

            UpdateDebugInfos();

            PreviousMouse = ClientMouse;
        }
        private void UpdateDebugInfos()
        {
            if (DebugInfos.InfosUnderMouse)
            {
                Cell cell = GetCellUnderMouse();
                if (PreviousMouse != ClientMouse || (cell.HasContainer ? cell.Container.Tension : cell.Tension) != DebugToolTipTension)
                {
                    string infos = "";
                    if (cell.HasContainer)
                        infos += $"Type : {cell.Container.StructureName}" + Environment.NewLine;
                    else
                    {
                        if (IsNull(cell))
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
    }
}
