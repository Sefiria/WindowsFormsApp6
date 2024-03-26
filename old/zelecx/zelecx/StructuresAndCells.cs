using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Script
{
    public interface ISupportValue
    {
        object Value { get; set; }
    }
    public interface ISupportStructureName
    {
        string StructureName { get; }
    }
    public interface IExternalOutputInfos
    {
        float Tension { get; }
    }
    public interface IExternalInputInfos
    {
        bool Toggle { get; set; }
        bool Holding { get; set; }
        bool Pressure { get; set; }
        InputScript Script { get; set; }
        StructuresAndCells.Structure structure { get; }
    }

    [Serializable]
    public class StructuresAndCells
    {
        static _Main Global = _Main.Instance;

        [Serializable]
        public class Cell
        {
            public int Id = 0;
            public int WireType = 1;
            public int X = 0, Y = 0;
            public float Tension = 0F;
            public Structure Container { get; set; }
            public float Resistance = 0.99F;

            public bool HasContainer => Container != null;
            private bool m_canReceiveTension = true;
            public bool CanReceiveTension
            {
                get { return !(Container is StructureGenerator) && m_canReceiveTension; }
                set { m_canReceiveTension = value; }
            }
            public bool CanEmitTension = true;

            public Cell(int x, int y, int id = 0, Structure container = null) { X = x; Y = y; Id = id; Container = container; }
            public void Reset(bool KeepPosition = true)
            {
                Id = 0;
                if (!KeepPosition)
                {
                    X = 0;
                    Y = 0;
                }
                Tension = 0F;
                if (Container != null)
                    if (!Container.Disposing)
                        Container.Dispose();
                Container = null;
                CanReceiveTension = true;
                CanEmitTension = true;
            }

            public void DiffuseTension()
            {
                if (!CanEmitTension || Id == 0)
                    return;

                if (Maths.FloatsComparison(Tension, 0F, 0.001F))
                    return;
                int NumberOfDivisions = 0;
                void DiffuseTo(Point RelativeTile)
                {
                    if (Global.MapBounds.Contains(X + RelativeTile.X, Y + RelativeTile.Y))
                    {
                        Cell cell = Global.map[X + RelativeTile.X][Y + RelativeTile.Y];
                        if (!Global.IsNull(cell) && ((Global.IsWire(cell) && Global.IsWire(this)) ? cell.WireType == WireType : true) && cell.CanReceiveTension)
                        {
                            if (cell.Container != null && Container != null)
                                return;

                            if (cell.Container == null && Container != null)
                            {
                                cell.Tension = Tension;
                            }
                            else
                            {
                                float tAv = Tension * Resistance / ((NumberOfDivisions == 1 ? 2 : NumberOfDivisions) - 1);
                                if (cell.Tension < tAv)
                                    cell.Tension = tAv;
                            }
                        }
                    }
                }

                Point Z, Q, S, D;
                Z = new Point(0, -1);
                Q = new Point(-1, 0);
                S = new Point(0, 1);
                D = new Point(1, 0);
                Cell CZ = Global.GetCellUnderPosition(X + Z.X, Y + Z.Y);
                Cell CQ = Global.GetCellUnderPosition(X + Q.X, Y + Q.Y);
                Cell CS = Global.GetCellUnderPosition(X + S.X, Y + S.Y);
                Cell CD = Global.GetCellUnderPosition(X + D.X, Y + D.Y);
                if (CZ.Id != 0 && CZ.Id != 2 && CZ.WireType == WireType) NumberOfDivisions++;
                if (CQ.Id != 0 && CQ.Id != 2 && CQ.WireType == WireType) NumberOfDivisions++;
                if (CS.Id != 0 && CS.Id != 2 && CS.WireType == WireType) NumberOfDivisions++;
                if (CD.Id != 0 && CD.Id != 2 && CD.WireType == WireType) NumberOfDivisions++;
                DiffuseTo(Z);
                DiffuseTo(Q);
                DiffuseTo(S);
                DiffuseTo(D);
            }
            public void LooseTension()
            {
                if (Tension <= 0.01F)
                    Tension = 0F;
                else
                    Tension -= 1F - Resistance;
            }
        }
        [Serializable]
        public class Structure : IDisposable, ISupportStructureName
        {
            public List<(int x, int y)> cells = new List<(int x, int y)>();
            public Rectangle Bounds = Rectangle.Empty;
            public float Tension = 0F;
            public bool Disposing = false;
            public float MinimumTensionDetected => 0.01F;
            public bool AlreadyDrawn = false;
            public virtual string StructureName => "Structure";
            public int Rotation = 0;

            public bool HasCells => cells != null ? (cells.Count > 0 ? true : false) : false;

            public Structure() { }
            ~Structure() => Dispose();
            public void Dispose()
            {
                Disposing = true;
                cells.ForEach(cell => Global.map[cell.x][cell.y].Reset());
                cells.Clear();
                Global.structures.Remove(this);
            }


            static public bool HasSufficentPlaceToBeCreated(int x, int y, int w, int h)
            {
                bool SufficentPlaceInMap = Global.MapBounds.Contains(x, y) && Global.MapBounds.Contains(x + w, y + h);
                if (!SufficentPlaceInMap)
                    return false;
                bool CellsAreEmpty = true;
                for (int X = 0; X < w && CellsAreEmpty; X++)
                    for (int Y = 0; Y < h && CellsAreEmpty; Y++)
                        if (!Global.MapIdUnderPositionIs0(x + X, y + Y))
                            CellsAreEmpty = false;
                return SufficentPlaceInMap && CellsAreEmpty;
            }
            static public Structure CreateStructure(_Main.Tools tool, int x, int y)
            {
                Type StructureType = Assembly.GetExecutingAssembly().GetTypes().First(t => t.FullName.EndsWith("Structure" + tool));

                int Width = 1, Height = 1;
                Width = (int)StructureType.GetField("Width").GetValue(StructureType);
                Height = (int)StructureType.GetField("Height").GetValue(StructureType);
                if ((Global.Rotation % 180) != 0)
                {
                    var _Width = Width;
                    Width = Height;
                    Height = _Width;
                }

                if (HasSufficentPlaceToBeCreated(x, y, Width, Height))
                {
                    var s = (Structure)Activator.CreateInstance(StructureType, new object[] { x, y });
                    Global.structures.Add(s);
                    return s;
                }

                return null;
            }
            public void InitializeCellsOfStructure(int id)
            {
                if (Bounds == Rectangle.Empty)
                    throw new Exception("InitializeCellsOfStructure method called before setting Bounds");

                int x = Bounds.X, y = Bounds.Y;
                for (int w = 0; w < Bounds.Width; w++)
                {
                    for (int h = 0; h < Bounds.Height; h++)
                    {
                        cells.Add((x + w, y + h));
                        Global.map[x + w][y + h].Id = id;
                        Global.map[x + w][y + h].Container = this;
                    }
                }
            }

            public virtual void DoAction()
            {
            }
            public virtual void AdditionnalDraw_PreDraw(Graphics g)
            {

            }
            public virtual void AdditionnalDraw_ApDraw(Graphics g)
            {

            }
            public virtual void DeployTensionToCells() { }
        }

        [Serializable]
        public class StructureMergeTool : Structure
        {
            static public int Width = 1, Height = 1, Id = (int)_Main.Tools.MergeTool;
            public override string StructureName => "StructureMergeTool";

            public StructureMergeTool(int x, int y)
            {
                Bounds = new Rectangle(x, y, Width, Height);
                InitializeCellsOfStructure(Id);
            }
            public override void DeployTensionToCells()
            {
                List<Cell> cellsAround = new List<Cell>();
                if(!Global.IsNull(Global.map[Bounds.X][Bounds.Y - 1]))
                    cellsAround.Add(Global.map[Bounds.X][Bounds.Y - 1]);
                if (!Global.IsNull(Global.map[Bounds.X - 1][Bounds.Y]))
                    cellsAround.Add(Global.map[Bounds.X - 1][Bounds.Y]);
                if (!Global.IsNull(Global.map[Bounds.X][Bounds.Y + 1]))
                    cellsAround.Add(Global.map[Bounds.X][Bounds.Y + 1]);
                if (!Global.IsNull(Global.map[Bounds.X + 1][Bounds.Y]))
                    cellsAround.Add(Global.map[Bounds.X + 1][Bounds.Y]);

                List<Cell> WiresAround = new List<Cell>();
                cellsAround.ForEach(cell => { if (Global.IsWire(cell)) WiresAround.Add(cell); });

                if (WiresAround.Count < 2)
                    return;

                Cell CellWithLessTension = null;
                Tension = 1F;
                WiresAround.ForEach(cell => { if (Tension > cell.Tension) { Tension = cell.Tension; CellWithLessTension = cell; } });
                if (CellWithLessTension == null)
                    return;
                Tension = 0F;
                cellsAround.ForEach(cell => { if (Tension < cell.Tension) Tension = cell.Tension; });
                CellWithLessTension.Tension = Tension;
            }
        }

        [Serializable]
        public class StructureGate : Structure
        {
            public float Resistance = 0.9F;
            public virtual bool IsOpen => false;
            public override string StructureName => "StructureMergeTool";
        }
        [Serializable]
        public class StructureAndGate : StructureGate
        {
            public static int Width = 3, Height = 2, Id = (int)_Main.Tools.AndGate;
            public Cell CellInA, CellInB, CellOut;
            public override bool IsOpen => CellInA.Tension >= MinimumTensionDetected && CellInB.Tension >= MinimumTensionDetected;
            public override string StructureName => "StructureAndGate";

            public StructureAndGate(int x, int y)
            {
                Rotation = Global.Rotation;
                if ((Rotation % 180) != 0)
                {
                    var _Width = Width;
                    Width = Height;
                    Height = _Width;
                }
                Bounds = new Rectangle(x, y, Width, Height);

                if (!HasSufficentPlaceToBeCreated(x, y, Width, Height))
                    throw new Exception("Not enough space to be created, HasSufficentPlaceToBeCreated call missing from caller method.");

                InitializeCellsOfStructure(Id);

                if (Rotation == 0)
                {
                    CellInA = Global.map[x][y];
                    CellInB = Global.map[x + 2][y];
                    CellOut = Global.map[x + 1][y + 1];
                }
                else if (Rotation == 90)
                {
                    CellInA = Global.map[x+1][y];
                    CellInB = Global.map[x+1][y+2];
                    CellOut = Global.map[x][y+1];
                }
                else if (Rotation == 180)
                {
                    CellInA = Global.map[x+2][y+1];
                    CellInB = Global.map[x][y+1];
                    CellOut = Global.map[x+1][y];
                }
                else if (Rotation == 270)
                {
                    CellInA = Global.map[x][y + 2];
                    CellInB = Global.map[x][y];
                    CellOut = Global.map[x + 1][y + 1];
                }
                CellInA.CanReceiveTension = true;
                CellInB.CanReceiveTension = true;
                CellOut.CanReceiveTension = false;
                CellInA.CanEmitTension = false;
                CellInB.CanEmitTension = false;
                CellOut.CanEmitTension = true;

                for (int w = 0; w < Bounds.Width; w++)
                {
                    for (int h = 0; h < Bounds.Height; h++)
                    {
                        cells.Add((x + w, y + h));
                        Global.map[x + w][y + h].Id = Id;
                        Global.map[x + w][y + h].Container = this;
                    }
                }
            }
            public override void AdditionnalDraw_PreDraw(Graphics g)
            {
                if (IsOpen)
                    g.FillRectangle(new SolidBrush(Global.ColorOn), Bounds.X * _Main.TileSize + 8, Bounds.Y * _Main.TileSize + 4, _Main.TileSize, _Main.TileSize);
            }
            public override void DeployTensionToCells()
            {
                if (IsOpen)
                {
                    Tension = Math.Max(CellInA.Tension, CellInB.Tension);
                    if (CellOut.Tension < Tension)
                        CellOut.Tension = Tension;
                }
            }
            public override void AdditionnalDraw_ApDraw(Graphics g)
            {
                /*g.FillEllipse(Brushes.Red, CellInA.X * 8, CellInA.Y * 8, 8, 8);
                g.FillEllipse(Brushes.Green, CellInB.X * 8, CellInB.Y * 8, 8, 8);
                g.FillEllipse(Brushes.Blue, CellOut.X * 8, CellOut.Y * 8, 8, 8);*/
            }
        }
        [Serializable]
        public class StructureXorGate : StructureGate
        {
            static public int Width = 3, Height = 2, Id = (int)_Main.Tools.XorGate;
            public Cell CellInA, CellInB, CellOut;
            public override bool IsOpen => CellInA.Tension >= MinimumTensionDetected ^ CellInB.Tension >= MinimumTensionDetected;
            public override string StructureName => "StructureXorGate";

            public StructureXorGate(int x, int y)
            {
                Rotation = Global.Rotation;
                if ((Rotation % 180) != 0)
                {
                    var _Width = Width;
                    Width = Height;
                    Height = _Width;
                }
                Bounds = new Rectangle(x, y, Width, Height);

                if (!HasSufficentPlaceToBeCreated(x, y, Width, Height))
                    throw new Exception("Not enough space to be created, HasSufficentPlaceToBeCreated call missing from caller method.");

                InitializeCellsOfStructure(Id);

                if (Rotation == 0)
                {
                    CellInA = Global.map[x][y];
                    CellInB = Global.map[x + 2][y];
                    CellOut = Global.map[x + 1][y + 1];
                }
                else if (Rotation == 90)
                {
                    CellInA = Global.map[x + 1][y];
                    CellInB = Global.map[x + 1][y + 2];
                    CellOut = Global.map[x][y + 1];
                }
                else if (Rotation == 180)
                {
                    CellInA = Global.map[x + 2][y + 1];
                    CellInB = Global.map[x][y + 1];
                    CellOut = Global.map[x + 1][y];
                }
                else if (Rotation == 270)
                {
                    CellInA = Global.map[x][y + 2];
                    CellInB = Global.map[x][y];
                    CellOut = Global.map[x + 1][y + 1];
                }
                CellInA.CanReceiveTension = true;
                CellInB.CanReceiveTension = true;
                CellOut.CanReceiveTension = false;
                CellInA.CanEmitTension = false;
                CellInB.CanEmitTension = false;
                CellOut.CanEmitTension = true;

                for (int w = 0; w < Bounds.Width; w++)
                {
                    for (int h = 0; h < Bounds.Height; h++)
                    {
                        cells.Add((x + w, y + h));
                        Global.map[x + w][y + h].Id = Id;
                        Global.map[x + w][y + h].Container = this;
                    }
                }
            }
            public override void AdditionnalDraw_PreDraw(Graphics g)
            {
                if (IsOpen)
                    g.FillRectangle(new SolidBrush(Global.ColorOn), Bounds.X * _Main.TileSize + 8, Bounds.Y * _Main.TileSize + 4, _Main.TileSize, _Main.TileSize);
            }
            public override void DeployTensionToCells()
            {
                if (IsOpen)
                {
                    Tension = Math.Max(CellInA.Tension, CellInB.Tension);
                    if (CellOut.Tension < Tension)
                        CellOut.Tension = Tension;
                }
            }
        }

        [Serializable]
        public class StructureUserControl : Structure, ISupportValue
        {
            public virtual object Value { get; set; } = "";
            public override string StructureName => "StructureUserControl";

            public virtual void Update()
            {
            }
            public virtual void UpdateOnClick(MouseEventArgs e)
            {
            }
        }
        [Serializable]
        public class StructureGenerator : StructureUserControl
        {
            static public int Width = 3, Height = 3, Id = (int)_Main.Tools.Generator;
            public List<(int x, int y)> GeneratedCells = new List<(int x, int y)>();
            public override string StructureName => "StructureGenerator";
            public bool Enabled = true;

            public StructureGenerator(int x, int y)
            {
                if (!HasSufficentPlaceToBeCreated(x, y, Width, Height))
                    throw new Exception("Not enough space to be created, HasSufficentPlaceToBeCreated call missing from caller method.");

                Bounds = new Rectangle(x, y, Width, Height);
                InitializeCellsOfStructure(Id);

                GeneratedCells.Add((x + 1, y + 0));
                GeneratedCells.Add((x + 0, y + 1));
                GeneratedCells.Add((x + 2, y + 1));
                GeneratedCells.Add((x + 1, y + 2));

                Tension = 1F;
            }
            public override void DeployTensionToCells()
            {
                if (Enabled)
                    GeneratedCells.ForEach(cell => { Global.map[cell.x][cell.y].Tension = Tension; });
            }
            public override void DoAction()
            {
                Enabled = !Enabled;
            }
            public override void UpdateOnClick(MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    if (Bounds.Location == Global.MouseTile)
                    {
                        DoAction();
                    }
                }
            }
            public override void AdditionnalDraw_ApDraw(Graphics g)
            {
                if (!Enabled)
                    g.DrawImage(ImagesManager.GetResizedImageIfNeeded(Properties.Resources.GeneratorDisabled), Bounds.X * _Main.TileSize, Bounds.Y * _Main.TileSize);
            }
        }
        [Serializable]
        public class StructureSwitch : StructureUserControl
        {
            static public int Width = 1, Height = 1, Id = (int)_Main.Tools.Switch;
            public bool IsOpen = false;
            public override object Value => $"IsOpen : {IsOpen}";
            public override string StructureName => "StructureSwitch";

            public StructureSwitch(int x, int y)
            {
                Bounds = new Rectangle(x, y, Width, Height);
                InitializeCellsOfStructure(Id);
                Global.map[x][y].CanReceiveTension = Global.map[x][y].CanEmitTension = false;
            }
            public override void DeployTensionToCells()
            {
                if (IsOpen)
                {
                    List<Cell> cellsAround = new List<Cell>();
                    cellsAround.Add(Global.map[Bounds.X][Bounds.Y - 1]);
                    cellsAround.Add(Global.map[Bounds.X-1][Bounds.Y]);
                    cellsAround.Add(Global.map[Bounds.X][Bounds.Y+1]);
                    cellsAround.Add(Global.map[Bounds.X+1][Bounds.Y]);

                    cellsAround.ForEach(cell => { if (Tension < cell.Tension && !Global.IsNull(cell)) Tension = cell.Tension; });
                    cellsAround.ForEach(cell => { if (cell.Tension < Tension && cell.CanReceiveTension) cell.Tension = Tension; });
                }
            }
            public override void DoAction()
            {
                IsOpen = !IsOpen;
            }
            public override void AdditionnalDraw_PreDraw(Graphics g)
            {
                if (IsOpen)
                    g.FillRectangle(new SolidBrush(Global.ColorOn), Bounds.X * _Main.TileSize, Bounds.Y * _Main.TileSize, _Main.TileSize, _Main.TileSize);

            }
            public override void AdditionnalDraw_ApDraw(Graphics g)
            {
                if (!IsOpen)
                    g.DrawImage(Properties.Resources.SwitchClosed, Bounds.X * _Main.TileSize, Bounds.Y * _Main.TileSize);
            }
            public override void UpdateOnClick(MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    if (Bounds.Location == Global.MouseTile)
                    {
                        base.DoAction();
                    }
                }
            }
        }
        [Serializable]
        public class StructurePotentiometer : StructureUserControl
        {
            static public int Width = 3, Height = 2, Id = (int)_Main.Tools.Potentiometer;
            public int Percent = 100;
            public override object Value => $"Percent : {Percent}%";
            public Cell CellIn, CellOut;
            public override string StructureName => "StructurePotentiometer";

            public StructurePotentiometer(int x, int y)
            {
                Rotation = Global.Rotation;
                if ((Rotation % 180) != 0)
                {
                    var _Width = Width;
                    Width = Height;
                    Height = _Width;
                }
                Bounds = new Rectangle(x, y, Width, Height);

                InitializeCellsOfStructure(Id);

                cells.ForEach(cell => Global.map[cell.x][cell.y].CanReceiveTension = Global.map[cell.x][cell.y].CanEmitTension = false);

                if (Rotation == 0)
                {
                    CellIn = Global.map[x + 1][y];
                    CellOut = Global.map[x + 1][y + 1];
                }
                else if (Rotation == 90)
                {
                    CellIn = Global.map[x+1][y+1];
                    CellOut = Global.map[x][y+1];
                }
                else if (Rotation == 180)
                {
                    CellIn = Global.map[x+1][y+1];
                    CellOut = Global.map[x+1][y];
                }
                else if (Rotation == 270)
                {
                    CellIn = Global.map[x][y+1];
                    CellOut = Global.map[x+1][y+1];
                }

                CellIn.CanReceiveTension = true;
                CellOut.CanEmitTension = true;
            }
            public override void DeployTensionToCells()
            {
                if (Percent > 0 && CellIn.Tension > 0F)
                    CellOut.Tension = Percent * CellIn.Tension / 100F;
            }
            public override void AdditionnalDraw_ApDraw(Graphics g)
            {
                if (Percent > 0 && CellIn.Tension > 0F)
                    g.FillRectangle(new SolidBrush(Global.ColorOn), Bounds.X * _Main.TileSize + 1, Bounds.Y * _Main.TileSize + 1, Percent / 100F * Width * _Main.TileSize - 2, Height * _Main.TileSize - 2);
            }
            public override void UpdateOnClick(MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    var toto = Global.ClientMouse.X;
                    if (Bounds.Contains(Global.MouseTile))
                        Percent = (int)((toto - Bounds.X * _Main.TileSize) / (float)(Width * _Main.TileSize) * 100);
                }
            }
        }

        [Serializable]
        public class StructureExternalOutput_1x1 : Structure, IExternalOutputInfos
        {
            static public int Width = 1, Height = 1, Id = (int)_Main.Tools.ExternalOutput_1x1;
            public override string StructureName => "StructureExternalOutput_1x1";
            float IExternalOutputInfos.Tension => Tension;
            const float TensionMinimumToActivate = 0.2F;
            public new float Tension => Global.map[Bounds.X][Bounds.Y].Tension;

            public StructureExternalOutput_1x1(int x, int y)
            {
                Bounds = new Rectangle(x, y, Width, Height);
                InitializeCellsOfStructure(Id);
                Global.map[x][y].CanEmitTension = false;
            }
            public override void AdditionnalDraw_ApDraw(Graphics g)
            {
                if(Tension >= TensionMinimumToActivate)
                    g.FillRectangle(new SolidBrush(Global.ColorOn), Bounds.X * _Main.TileSize + 2, Bounds.Y * _Main.TileSize + 2, Width * _Main.TileSize - 4, Height * _Main.TileSize - 4);
            }
            public override void DeployTensionToCells()
            {
                // Loose Tension
                float FallingValue = 0.1F;
                Cell cell = Global.map[Bounds.X][Bounds.Y];
                if (cell.Tension - FallingValue >= 0F)
                    cell.Tension -= FallingValue;
                else
                    cell.Tension = 0F;
            }
        }
        [Serializable]
        public class StructureExternalOutput_3x3 : Structure, IExternalOutputInfos
        {
            static public int Width = 3, Height = 3, Id = (int)_Main.Tools.ExternalOutput_3x3;
            public override string StructureName => "StructureExternalOutput_3x3";
            float IExternalOutputInfos.Tension => Tension;
            const float TensionMinimumToActivate = 0.5F;
            public new float Tension { get { float MaxTension = 0F; cells.ForEach(cell => { if (MaxTension < Global.map[cell.x][cell.y].Tension) MaxTension = Global.map[cell.x][cell.y].Tension; }); return MaxTension; } }

            public StructureExternalOutput_3x3(int x, int y)
            {
                Bounds = new Rectangle(x, y, Width, Height);
                InitializeCellsOfStructure(Id);
                cells.ForEach(cell => Global.map[cell.x][cell.y].CanEmitTension = false);
            }
            public override void AdditionnalDraw_ApDraw(Graphics g)
            {
                if (Tension >= TensionMinimumToActivate)
                {
                    Color color = Global.ColorOn;// Step for avoid runtime error
                    color = Color.FromArgb(color.R - 50, color.G - 50, color.B);
                    g.FillRectangle(new SolidBrush(color), Bounds.X * _Main.TileSize + 2, Bounds.Y * _Main.TileSize + 2, Width * _Main.TileSize - 4, Height * _Main.TileSize - 4);
                }
            }
            public override void DeployTensionToCells()
            {
                // Loose Tension for each cell
                float FallingValue = 0.1F;
                cells.ForEach(cell => { Cell _c = Global.map[cell.x][cell.y]; if (_c.Tension - FallingValue >= 0F) _c.Tension -= FallingValue; else _c.Tension = 0F; });
            }
        }

        [Serializable]
        public class StructureExternalInput : Structure, IExternalInputInfos
        {
            static public int Width = 1, Height = 1, Id = (int)_Main.Tools.ExternalInput;
            public override string StructureName => "StructureExternalInput";
            public bool Toggle { get; set; }
            public bool Holding { get; set; }
            public bool Pressure { get; set; }
            public InputScript Script { get; set; }
            Structure IExternalInputInfos.structure => this;
            public new float Tension => (Toggle || Holding || Pressure) ? 1F : 0F;


            List<(int x, int y)> GeneratedCells = new List<(int x, int y)>();
            [NonSerialized]
            public InputScriptEditor scriptManager;
            [NonSerialized]
            private bool init = true;
            [NonSerialized]
            private TimeSpan lastExeTime;
            private bool ScriptEdit = false;

            public StructureExternalInput(int x, int y)
            {
                Bounds = new Rectangle(x, y, Width, Height);
                InitializeCellsOfStructure(Id);
                Global.map[x][y].CanReceiveTension = false;

                GeneratedCells.Add((x, y - 1));
                GeneratedCells.Add((x - 1, y));
                GeneratedCells.Add((x, y + 1));
                GeneratedCells.Add((x + 1, y));

                lastExeTime = Global.Watch.Elapsed;
            }
            public override void AdditionnalDraw_ApDraw(Graphics g)
            {
                if (Tension >= 0.9F)
                    g.FillRectangle(new SolidBrush(Global.ColorOn), Bounds.X * _Main.TileSize + 2, Bounds.Y * _Main.TileSize + 2, Width * _Main.TileSize - 4, Height * _Main.TileSize - 4);
            }
            public override void DeployTensionToCells()
            {
                if (init|| scriptManager == null)
                {
                    init = false;
                    if(Script == null)
                        Script = new InputScript();
                    scriptManager = new InputScriptEditor(Global.map[Bounds.X][Bounds.Y]);
                }

                if (Global.Watch.Elapsed.Subtract(lastExeTime).TotalSeconds * 1000 < Script.ExecutionRate)
                    return;

                lastExeTime = Global.Watch.Elapsed;

                if(!ScriptEdit)
                    scriptManager.Execute();
            }

            public override void DoAction()
            {
                ScriptEdit = true;
                scriptManager.ShowDialog();
                ScriptEdit = false;
            }
        }
    }
}
