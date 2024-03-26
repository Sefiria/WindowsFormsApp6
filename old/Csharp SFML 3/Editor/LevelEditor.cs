using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SFML.System;
using SFML.Window;
using SFML.Graphics;
using System.Windows.Forms.DataVisualization.Charting;
using System.Linq;

using sfColor = SFML.Graphics.Color;
using sfImage = SFML.Graphics.Image;
using Color = System.Drawing.Color;
using static Editor.ComboBoxIconed;
using Framework;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using zelecx;

namespace Editor
{
    public partial class LevelEditor : Form
    {
        public class ComboBoxItem
        {
            public string Name;
            public object Tag;
            public ComboBoxItem() { Name = ""; Tag = null; }
            public ComboBoxItem(string name, object tag = null) { Name = name; Tag = tag; }
            public override string ToString() { return Name; }
        }
        public class Selection
        {
            public bool ToBeDrawn = false;
            public Size size = Size.Empty;
            public byte[,,] selection = null;
            public Point position;
            public Point positionBeforeHoldingToMove, MousePointBeforeHoldingToMove;

            public Selection(){}
            public void Select(Level level, bool Edit_TrueBackground_FalseEntities)
            {
                size = new Size(level.selection.Width, level.selection.Height);
                selection = level.Copy(level.selection, level.SelectAllLayers, Edit_TrueBackground_FalseEntities);
                position = level.selection.Location;
            }
            public bool IsEmpty()
            {
                bool empty = true;
                for (byte l = 0; l < 3 && empty; l++)
                    for (int x = 0; x < size.Width && empty; x++)
                        for (int y = 0; y < size.Height && empty; y++)
                            if (selection[l, x, y] != 0)
                                empty = false;
                return empty;
            }
        }
        public static class TileInfo
        {
            public static (int X, int Y) TileUnderMouse = (0, 0);
        }
        public enum ToolMode
        {
            Pen = 0,
            Fill,
            Eraser,
            Selection,
            Edit,
            WarpEnter,
            WarpExit,
            RecalculateAutotile
        }

        private RenderWindow renderwindow;
        private Sprite grid, SpCamera;
        private (int X, int Y) Camera, InGameCamera = (0, 0);
        private Level level;
        private int CurrentLayer = 0;
        private TreeView EntityTree = new TreeView();
        private ToolMode toolMode = ToolMode.Pen;
        private Point mousePosition;// used for lerp
        private Point selectionStart, selectionEnd;
        private Selection VolatileSelection = new Selection();
        private bool X_r, C_r, V_r, Delete_r, Return_r, Left_r;
        private bool FillSelectionReplace = false;
        private bool Edit_TrueBackground_FalseEntities = false;
        private _Main zelecxModule = null;
        private object DrawLockObject = false;

        public LevelEditor()
        {
            InitializeComponent();
            renderwindow = new RenderWindow(RenderPanel.Handle);

            SpriteManager.Initialize();
            IDManager.Initialize();

            var array = Enum.GetValues(typeof(GlobalVariables.Instantiable));
            var files = new List<(Bitmap image, string filename, EntityProperties entity)>();
            foreach (var enumValue in array)
            {
                files = Tools.GetListBitmapAndNameInEntityPropertiesInEntitySubFolder(enumValue.ToString());

                var list = new List<DropDownItem>();
                foreach (var file in files)
                    list.Add(new DropDownItem(file.image, file.filename, file.entity));
                comboBox1.Items.Add(new ComboBoxItem(enumValue.ToString(), list));
            }
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;

            InitializeEntityTree();

            hScrollBarX.Maximum = Tools.MapWidth - Tools.EditorRenderWidth;
            vScrollBarY.Maximum = Tools.MapHeight - Tools.EditorRenderHeight;
            selectionStart = selectionEnd = new Point(0, 0);
            btDisplayLayerAll_Click(null, null);
            vScrollBarY.Value = vScrollBarY.Maximum / 2 - Tools.ChunkSize;
            Camera = (0, vScrollBarY.Value);
            InGameCamera = Camera;
            nupCameraX.Value = InGameCamera.X;
            nupCameraY.Value = InGameCamera.Y;
            nupCameraX.Maximum = Tools.MapWidth - Tools.PlayRenderWidth;
            nupCameraY.Maximum = Tools.MapHeight - Tools.PlayRenderHeight;

            SetGridColor(Color.Gray);

            level = new Level();

            saveFileDialog1.InitialDirectory = (Environment.CurrentDirectory + @"\\Levels").Replace(@"\\", @"\");
            openFileDialog1.InitialDirectory = (Environment.CurrentDirectory + @"\\Levels").Replace(@"\\", @"\");

            ToolMode_Click(btPen, null);

            FormClosed += delegate { Dispose(); };

            timerDraw.Start();
            timerCommandKey.Start();

            InitializeSpCamera(Color.Red);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    level?.AbortThreads();
                    components.Dispose();
                }

                SpriteManager.Dispose();
                IDManager.Dispose();

                if (File.Exists("ElectricPanel.MAP.temp"))
                    File.Delete("ElectricPanel.MAP.temp");
            }

            base.Dispose(disposing);
        }
        private void InitializeEntityTree()
        {
            EntityTree.Nodes.Clear();
            foreach (ComboBoxItem item in comboBox1.Items)
            {
                var node = new TreeNode(item.Name);
                node.Tag = item.Tag;
                EntityTree.Nodes.Add(node);
                comboBox1.SelectedItem = item;
                comboBoxIconed1.Items.Clear();
                comboBoxIconed1.Items.AddRange(((comboBox1.SelectedItem as ComboBoxItem).Tag as List<DropDownItem>).ToArray());

                foreach (DropDownItem ddi in comboBoxIconed1.Items)
                {
                    var child = new TreeNode(ddi.Text);
                    child.Tag = ddi.entity;
                    node.Nodes.Add(child);
                }
            }
        }
        private void SetGridColor(Color color)
        {
            var gridtile = new Bitmap(Tools.TileSize, Tools.TileSize);
            using (var g = Graphics.FromImage(gridtile))
                g.DrawRectangle(new Pen(color), 0, 0, Tools.TileSize, Tools.TileSize);
            grid = new Sprite(new Texture(Tools.BitmapToSfImage(gridtile)));
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxIconed1.Items.Clear();
            comboBoxIconed1.Items.AddRange(((comboBox1.SelectedItem as ComboBoxItem).Tag as List<DropDownItem>).ToArray());
            if(((ComboBoxItem)comboBox1.SelectedItem).Name != "Background")
                for(int i = 0; i < comboBoxIconed1.Items.Count; i++)
                    if (((DropDownItem)comboBoxIconed1.Items[i]).entity.layer != CurrentLayer)
                        comboBoxIconed1.Items.RemoveAt(i--);
            btSetBackground.Visible = false;
            if (comboBoxIconed1.Items.Count > 0)
            {
                comboBoxIconed1.SelectedIndex = 0;
                btSetBackground.Visible = comboBox1.SelectedItem.ToString() == "Background";
            }
        }
        private void btSetBackground_Click(object sender, EventArgs e)
        {
            if (comboBoxIconed1.SelectedItem == null)
                return;

            if (MessageBox.Show("Are you sure to proceed ?\nIt will completely replace all the background with this only block.", "Replace complete Background", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            for (int x = 0; x < Tools.MapWidth; x++)
                for (int y = 0; y < Tools.MapHeight; y++)
                    level.SetTile(0, x, y, (comboBoxIconed1.SelectedItem as DropDownItem).entity.ID, true);
        }

        private void timerDraw_Tick(object sender, EventArgs e)
        {
            Draw();
        }
        private void Draw()
        {
            lock (DrawLockObject)
            {
                Application.DoEvents();
                renderwindow.DispatchEvents();


                if (level.Cam_Request != (-1, -1))
                {
                    Camera = level.Cam_Request;
                    level.Cam_Request = (-1, -1);

                    hScrollBarX.Value = Camera.X;
                    vScrollBarY.Value = Camera.Y;
                }

                for (int i = 0; i < 2; i++)
                {
                    renderwindow.Clear();

                    level.DrawBackground(renderwindow, Camera, Edit_TrueBackground_FalseEntities);

                    for (int x = 0; x < renderwindow.Size.X / Tools.TileSize; x++)
                    {
                        for (int y = 0; y < renderwindow.Size.Y / Tools.TileSize; y++)
                        {
                            grid.Position = new Vector2f(x * Tools.TileSize, y * Tools.TileSize);
                            renderwindow.Draw(grid);
                        }
                    }

                    level.Draw(renderwindow, Camera, new bool[] { cbDisplayLayer0.Checked, cbDisplayLayer1.Checked, cbDisplayLayer2.Checked }, Edit_TrueBackground_FalseEntities);

                    DrawPossibleSelection();

                    DrawCamera();

                    renderwindow.Display();
                }

                Tools.Increment_t_lerpColor();


                rtbTileInfo.Text = "" + TileInfo.TileUnderMouse;
            }
        }

        private void btGridWhite_Click(object sender, EventArgs e)
        {
            btGridColorClicked(sender);
            RenderPanel.Focus();
        }
        private void btGridColorClicked(object sender)
        {
            SetGridColor((sender as Button).BackColor);
        }
        private void hScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            Camera.X = hScrollBarX.Value;
        }
        private void vScrollBarY_ValueChanged(object sender, EventArgs e)
        {
            Camera.Y = vScrollBarY.Value;
        }

        private void RenderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (toolMode == ToolMode.Edit && Mouse.IsButtonPressed(Mouse.Button.Left) && Left_r)
            {
                var info = level.GetTileInfosFromPoint(RenderPanel.PointToClient(MousePosition));
                var door = level.GetDoor(info.x, info.y);
                var warp = level.GetWarp(info.x, info.y);

                if(door != null && warp != null)
                {
                    ToolStripDropDown DropDown = new ToolStripDropDown();
                    DropDown.Items.Add("Door").Click += delegate {
                        var menu = new DoorsPropertiesMenu(level, door);
                        menu.ShowDialog();
                    };
                    DropDown.Items.Add("Warp").Click += delegate {
                        var menu = new WarpsPropertiesMenu(level, warp);
                        menu.ShowDialog();
                    };
                    DropDown.Show(MousePosition);
                }
                else if (door != null)
                {
                    var menu = new DoorsPropertiesMenu(level, door);
                    menu.ShowDialog();
                }
                else if (warp != null)
                {
                    var menu = new WarpsPropertiesMenu(level, warp);
                    menu.ShowDialog();
                }

                return;
            }

            if ((toolMode == ToolMode.WarpEnter || toolMode == ToolMode.WarpExit) && Mouse.IsButtonPressed(Mouse.Button.Left) && Left_r)
            {
                var info = level.GetTileInfosFromPoint(RenderPanel.PointToClient(MousePosition));
                level.ToggleWarp((toolMode == ToolMode.WarpEnter ? Warp.WarpType.Enter : Warp.WarpType.Exit), info.x, info.y);
                return;
            }

            if (toolMode == ToolMode.Selection)
            {
                if (!VolatileSelection.ToBeDrawn)
                {
                    if (Mouse.IsButtonPressed(Mouse.Button.Left))
                    {
                        selectionStart = Tools.ClientToWorld(RenderPanel.PointToClient(MousePosition));
                        selectionStart.X += Camera.X;
                        selectionStart.Y += Camera.Y;
                        if (!timerMouseHold.Enabled)
                            selectionEnd = selectionStart;
                    }
                    else if (Mouse.IsButtonPressed(Mouse.Button.Right))
                    {
                        VolatileSelection.positionBeforeHoldingToMove = selectionStart;
                        VolatileSelection.MousePointBeforeHoldingToMove = Tools.ClientToWorld(RenderPanel.PointToClient(MousePosition));
                    }
                }
                else if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    VolatileSelection.positionBeforeHoldingToMove = VolatileSelection.position;
                    VolatileSelection.MousePointBeforeHoldingToMove = Tools.ClientToWorld(RenderPanel.PointToClient(MousePosition));
                }
            }
            else
            {
                mousePosition = MousePosition;
            }

            timerMouseHold.Start();
        }
        private void timerMouseHold_Tick(object sender, EventArgs e)
        {
            if (!RenderPanel.ClientRectangle.Contains(RenderPanel.PointToClient(MousePosition)))
                RenderPanel_MouseLeave(null, null);

            var tileInfosStart = level.GetTileInfosFromPoint(RenderPanel.PointToClient(MousePosition));
            var tileInfosEnd = level.GetTileInfosFromPoint(RenderPanel.PointToClient(mousePosition));

            if (toolMode != ToolMode.Selection)
            for (double t = 0.0; t < 1.0; t += 1.0 / Framework.Maths.DistanceNonZero(mousePosition, MousePosition, 0.0001))
            {
                var lerpMousePosition = Framework.Maths.Lerp2(tileInfosStart.x, tileInfosStart.y, tileInfosEnd.x, tileInfosEnd.y, t);

                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
                    {
                        if (toolMode == ToolMode.Pen)
                            level.DeleteTile(CurrentLayer, lerpMousePosition.X, lerpMousePosition.Y, Edit_TrueBackground_FalseEntities);
                        if (toolMode == ToolMode.Fill)
                            FillEmpty(CurrentLayer, lerpMousePosition.X, lerpMousePosition.Y);
                    }
                    else
                    {
                        if (toolMode == ToolMode.Pen || toolMode == ToolMode.Fill)
                        {
                            if (comboBox1.SelectedItem?.ToString() != "Background")
                            {
                                if (comboBoxIconed1.SelectedItem != null)
                                {
                                    if (toolMode == ToolMode.Pen)
                                        level.SetTile(CurrentLayer, lerpMousePosition, (comboBoxIconed1.SelectedItem as DropDownItem).entity.ID, Edit_TrueBackground_FalseEntities);
                                    if (toolMode == ToolMode.Fill)
                                        Fill(CurrentLayer, lerpMousePosition, (comboBoxIconed1.SelectedItem as DropDownItem).entity.ID);
                                }
                            }
                        }
                        else if (toolMode == ToolMode.Eraser)
                        {
                            level.DeleteTile(CurrentLayer, lerpMousePosition, Edit_TrueBackground_FalseEntities);
                        }
                    }
                }

                if (Mouse.IsButtonPressed(Mouse.Button.Right))
                {
                    (int type, int id) = GetTileTypeAndIDFromPoint(lerpMousePosition, true, true);
                    if (type == 0 && id == 0)
                        return;

                    comboBox1.SelectedIndex = type;
                    comboBoxIconed1.SelectedIndex = id - 1;
                }
            }

            if (toolMode == ToolMode.Selection)
            {
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    if (!level.ValidatingPasteSelection)
                    {
                        selectionEnd = Tools.ClientToWorld(RenderPanel.PointToClient(MousePosition));
                        selectionEnd.X += Camera.X;
                        selectionEnd.Y += Camera.Y;
                    }
                    else
                    {
                        var P = VolatileSelection.positionBeforeHoldingToMove;
                        var A = VolatileSelection.MousePointBeforeHoldingToMove;
                        var B = Tools.ClientToWorld(RenderPanel.PointToClient(MousePosition));
                        VolatileSelection.position = new Point(P.X + B.X - A.X, P.Y + B.Y - A.Y);
                    }
                }
                else if(Mouse.IsButtonPressed(Mouse.Button.Right))
                {
                    var P = VolatileSelection.positionBeforeHoldingToMove;
                    var A = VolatileSelection.MousePointBeforeHoldingToMove;
                    var B = Tools.ClientToWorld(RenderPanel.PointToClient(MousePosition));
                    var size = new Point(selectionEnd.X - selectionStart.X, selectionEnd.Y - selectionStart.Y);
                    selectionStart = new Point(P.X + B.X - A.X, P.Y + B.Y - A.Y);
                    selectionEnd = new Point(selectionStart.X + size.X, selectionStart.Y + size.Y);
                }
            }


            if (toolMode == ToolMode.RecalculateAutotile)
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                    level.RecalculateAutoTilesIDsAround((byte)CurrentLayer, tileInfosStart.x, tileInfosStart.y, Edit_TrueBackground_FalseEntities);

            mousePosition = MousePosition;
        }
        private void RenderPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (toolMode == ToolMode.Selection && e.Button == MouseButtons.Left)
            {
                if (!VolatileSelection.ToBeDrawn)
                {
                    selectionEnd = Tools.ClientToWorld(RenderPanel.PointToClient(MousePosition));
                    selectionEnd.X += Camera.X;
                    selectionEnd.Y += Camera.Y;
                }
            }

            timerMouseHold.Stop();
        }
        private void RenderPanel_MouseLeave(object sender, EventArgs e)
        {
            timerMouseHold.Stop();
        }
        private void RenderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            var info = level.GetTileInfosFromMouse(renderwindow, Camera);
            TileInfo.TileUnderMouse = (info.x, info.y);
        }

        private void DrawPossibleSelection()
        {
            if (!VolatileSelection.ToBeDrawn)
            {
                var Ax = Math.Min(selectionStart.X, selectionEnd.X);
                var Ay = Math.Min(selectionStart.Y, selectionEnd.Y);
                var Bx = Math.Max(selectionStart.X, selectionEnd.X);
                var By = Math.Max(selectionStart.Y, selectionEnd.Y);
                if (Ax < 0) Ax = 0;
                if (Ay < 0) Ay = 0;
                var size = new Point(Bx - Ax, By - Ay);
                size.X++;
                size.Y++;


                if (Ax < 0) Ax = 0;
                if (Ay < 0) Ay = 0;
                if (Ax + size.X > Tools.EditorRenderWidth * Tools.TileSize) size.X = Tools.EditorRenderWidth * Tools.TileSize - Ax;
                if (Ay + size.Y > Tools.EditorRenderHeight * Tools.TileSize) size.Y = Tools.EditorRenderHeight * Tools.TileSize - Ay;
                if (size.X <= 0) size.X = 1;
                if (size.Y <= 0) size.Y = 1;
                if (size.X > Tools.EditorRenderWidth * Tools.TileSize) size.X = Tools.EditorRenderWidth * Tools.TileSize - 1;
                if (size.Y > Tools.EditorRenderHeight * Tools.TileSize) size.Y = Tools.EditorRenderHeight * Tools.TileSize - 1;
                if (Bx > Tools.EditorRenderWidth * Tools.TileSize) Bx = Tools.EditorRenderWidth * Tools.TileSize - 1;
                if (By > Tools.EditorRenderHeight * Tools.TileSize) By = Tools.EditorRenderHeight * Tools.TileSize - 1;


                var img = new Bitmap(size.X * Tools.TileSize, size.Y * Tools.TileSize);
                using (var g = Graphics.FromImage(img))
                    g.DrawRectangle(new Pen(Tools.GetColorRainbow(), 5), 0, 0, size.X * Tools.TileSize, size.Y * Tools.TileSize);
                var spSelection = new Sprite(new Texture(Tools.BitmapToSfImage(img)));
                spSelection.Position = new Vector2f((Ax - Camera.X) * Tools.TileSize, (Ay - Camera.Y) * Tools.TileSize);
                renderwindow.Draw(spSelection);

                level.selection = new Rectangle(Ax, Ay, size.X, size.Y);
                level.selectionLayer = CurrentLayer;
            }
            else
            {
                level.selection = new Rectangle(0,0,0,0);
                Sprite sp;
                byte layer;
                byte value;
                for (byte l = 0; l < (level.SelectAllLayers ? 3 : 1); l++)
                {
                    layer = level.SelectAllLayers ? l : (byte)level.selectionLayer;
                    for (int x = 0; x < VolatileSelection.size.Width; x++)
                        for (int y = 0; y < VolatileSelection.size.Height; y++)
                        {
                            value = VolatileSelection.selection[layer, x, y];
                            if (value == 0)
                                continue;
                            sp = SpriteManager.GetSprite(layer, value);
                            sp.Position = new Vector2f((VolatileSelection.position.X + x - Camera.X) * Tools.TileSize, (VolatileSelection.position.Y + y - Camera.Y) * Tools.TileSize);
                            renderwindow.Draw(sp);
                        }
                }
            }
        }
        private void DrawCamera()
        {
            SpCamera.Position = new Vector2f((InGameCamera.X - Camera.X) * Tools.TileSize, (InGameCamera.Y - Camera.Y) * Tools.TileSize);
            renderwindow.Draw(SpCamera);
        }

        private (int type, int ID) GetTileTypeAndIDFromMouse()
        {
            return GetTileTypeAndIDFromPoint(RenderPanel.PointToClient(MousePosition));
        }
        private (int type, int ID) GetTileTypeAndIDFromPoint(int x, int y, bool alreadyTiled = false, bool alreadyCamera = false)
        {
            return GetTileTypeAndIDFromPoint(new Point(x, y), alreadyTiled, alreadyCamera);
        }
        private (int type, int ID) GetTileTypeAndIDFromPoint(Point point, bool alreadyTiled = false, bool alreadyCamera = false)
        {
            var info = level.GetTileInfosFromPoint(point, alreadyTiled, alreadyCamera);
            if (info.value[CurrentLayer] != 0)
            {
                var (entityTypeID, entityID) = RetrieveCombinaisonEntityFromMapInfo(info);
                if (entityTypeID == -1 || entityID == -1)
                    return (0, 0);
                return (entityTypeID, entityID);
            }
            return (0, 0);
        }
        private (int entityTypeID, int entityID) RetrieveCombinaisonEntityFromMapInfo(Level.TileInfos info)
        {
            (int entityTypeID, int entityID) result = (-1, -1);
            for (int node = 0; node < EntityTree.GetNodeCount(false) && result.entityTypeID == -1; node++)
            {
                for (int child = 0; child < EntityTree.Nodes[node].Nodes.Count && result.entityID == -1; child++)
                {
                    if ((EntityTree.Nodes[node].Nodes[child].Tag as EntityProperties).layer != CurrentLayer)
                        continue;
                    if ((EntityTree.Nodes[node].Nodes[child].Tag as EntityProperties).ID == info.value[CurrentLayer])
                        result = (node, child+1);
                }
            }
            return result;
        }
        private void btLayer0_Click(object sender, EventArgs e)
        {
            btLayer0.BackColor = Color.LightGray;
            btLayer1.BackColor = Color.LightGray;
            btLayer2.BackColor = Color.LightGray;

            var bt = sender as Button;
            CurrentLayer = int.Parse(bt.Text);
            bt.BackColor = Color.White;

            comboBox1_SelectedIndexChanged(null, null);
            RenderPanel.Focus();
        }
        private void btNew_Click(object sender, EventArgs e)
        {
            bool showMessage = false;
            DialogResult result = DialogResult.None;
            if (sender is bool)
                showMessage = (bool)sender;
            if (showMessage)
                result = MessageBox.Show("Are you sure to clear the current project ?", "New", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes || !showMessage)
            {
                if (comboBox1.Items.Count > 0)
                    comboBox1.SelectedIndex = 0;
                btDisplayLayerAll_Click(true, null);
                Camera = (0, 0);
                CurrentLayer = 0;
                level?.AbortThreads();
                level = new Level();
            }

            RenderPanel.Focus();
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            level.lockDraw = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                level.InGameCamera = InGameCamera;

                BinaryFormatter bf = new BinaryFormatter();
                using (Stream stream = File.Open(saveFileDialog1.FileName, FileMode.Create))
                {
                    bf.Serialize(stream, level);
                    bf.Serialize(stream, Camera);
                    bf.Serialize(stream, CurrentLayer);
                }
                if (File.Exists("ElectricPanel.MAP.temp"))
                    File.Copy("ElectricPanel.MAP.temp", saveFileDialog1.FileName + ".MAP", true);
            }
            level.lockDraw = false;

            RenderPanel.Focus();
        }
        private void btLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (MessageBox.Show("Are you sure to clear the current project ?", "Load", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    using (Stream stream = File.Open(openFileDialog1.FileName, FileMode.Open))
                    {
                        level?.AbortThreads();
                        level = bf.Deserialize(stream) as Level;
                        InGameCamera = level.InGameCamera;
                        level.InitializeNonSerialized();

                        Camera = ((int, int))bf.Deserialize(stream);
                        CurrentLayer = (int)bf.Deserialize(stream);
                    }

                    // Camera ScrollBars set

                    hScrollBarX.Value = Camera.X;
                    vScrollBarY.Value = Camera.Y;

                    // Mics

                    btLayer0.BackColor = Color.LightGray;
                    btLayer1.BackColor = Color.LightGray;
                    btLayer2.BackColor = Color.LightGray;
                    if (CurrentLayer == 0) btLayer0.BackColor = Color.White;
                    else if (CurrentLayer == 1) btLayer1.BackColor = Color.White;
                    else if (CurrentLayer == 2) btLayer2.BackColor = Color.White;
                    comboBox1_SelectedIndexChanged(null, null);

                    if (File.Exists(openFileDialog1.FileName + ".MAP"))
                        File.Copy(openFileDialog1.FileName + ".MAP", "ElectricPanel.MAP.temp", true);
                }
            }

            RenderPanel.Focus();
        }
        private void btDisplayLayerAll_Click(object sender, EventArgs e)
        {
            bool value = (sender is bool ? (bool)sender : false) || !(cbDisplayLayer0.Checked && cbDisplayLayer1.Checked && cbDisplayLayer2.Checked);
            cbDisplayLayer0.Checked = value;
            cbDisplayLayer1.Checked = value;
            cbDisplayLayer2.Checked = value;
            RenderPanel.Focus();
        }
        private void ToolMode_Click(object sender, EventArgs e)
        {
            if (level.ValidatingPasteSelection)
                return;

            if (sender == btPen) toolMode = ToolMode.Pen;
            else if (sender == btFill) toolMode = ToolMode.Fill;
            else if (sender == btEraser) toolMode = ToolMode.Eraser;
            else if (sender == btSelect) toolMode = ToolMode.Selection;
            else if (sender == btEdit) toolMode = ToolMode.Edit;
            else if (sender == btWarpEnter) toolMode = ToolMode.WarpEnter;
            else if (sender == btWarpExit) toolMode = ToolMode.WarpExit;
            else if (sender == btRecalculateAutotile) toolMode = ToolMode.RecalculateAutotile;
            HighlightButton((sender as Button));
            RenderPanel.Focus();
        }
        private void HighlightButton(Button bt)
        {
            btPen.FlatAppearance.BorderSize = 1;
            btEraser.FlatAppearance.BorderSize = 1;
            btSelect.FlatAppearance.BorderSize = 1;
            btFill.FlatAppearance.BorderSize = 1;
            btEdit.FlatAppearance.BorderSize = 1;
            btWarpEnter.FlatAppearance.BorderSize = 1;
            btWarpExit.FlatAppearance.BorderSize = 1;
            btCamera.FlatAppearance.BorderSize = 1;
            btRecalculateAutotile.FlatAppearance.BorderSize = 1;

            bt.FlatAppearance.BorderSize = 2;

            SetVisibleCameraButtons(false);
        }

        private void InitializeSpCamera(Color color)
        {
            Bitmap img = new Bitmap(512, 256);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawRectangle(new Pen(color, (float)nupCameraW.Value), 1, 1, img.Size.Width - 2, img.Size.Height - 2);
            }
            SpCamera = new Sprite(new Texture(Tools.BitmapToSfImage(img)));
        }
        private void SetVisibleCameraButtons(bool visible)
        {
            btCameraGo.Visible = visible;
            btCameraColor.Visible = visible;
            nupCameraX.Visible = visible;
            nupCameraY.Visible = visible;
            nupCameraW.Visible = visible;
            lbCameraW.Visible = visible;
            lbCameraX.Visible = visible;
            lbCameraY.Visible = visible;
            nupCameraW.Visible = visible;
        }
        private void btCamera_Click(object sender, EventArgs e)
        {
            HighlightButton(btCamera);
            SetVisibleCameraButtons(true);
        }
        private void btCameraGo_Click(object sender, EventArgs e)
        {
            Camera = (  InGameCamera.X > hScrollBarX.Maximum ? hScrollBarX.Maximum : InGameCamera.X,
                        InGameCamera.Y > vScrollBarY.Maximum ? vScrollBarY.Maximum : InGameCamera.Y);
            hScrollBarX.Value = Camera.X;
            vScrollBarY.Value = Camera.Y;
        }
        private void btCameraColor_Click(object sender, EventArgs e)
        {
            var dial = new ColorDialog();
            dial.Color = btCameraColor.BackColor;
            if(dial.ShowDialog() == DialogResult.OK)
            {
                btCameraColor.BackColor = dial.Color;
                InitializeSpCamera(dial.Color);
            }
        }
        private void nupCameraX_ValueChanged(object sender, EventArgs e)
        {
            InGameCamera.X = (int)nupCameraX.Value;
        }
        private void nupCameraY_ValueChanged(object sender, EventArgs e)
        {
            InGameCamera.Y = (int)nupCameraY.Value;
        }
        private void nupCameraW_ValueChanged(object sender, EventArgs e)
        {
            InitializeSpCamera(btCameraColor.BackColor);
        }

        private void btFillSelection_Click(object sender, EventArgs e)
        {
            if (comboBoxIconed1.SelectedItem == null)
                return;

            int Layer, X, Y, Value = (comboBoxIconed1.SelectedItem as DropDownItem).entity.ID;
            for (int l = 0; l < (level.SelectAllLayers ? 3 : 1); l++)
            {
                Layer = level.SelectAllLayers ? l : level.selectionLayer;
                for (int x = 0; x < level.selection.Size.Width; x++)
                {
                    for (int y = 0; y < level.selection.Size.Height; y++)
                    {
                        X = level.selection.Location.X + x;
                        Y = level.selection.Location.Y + y;
                        if(FillSelectionReplace)
                            level.SetTile(Layer, X, Y, (byte)Value, Edit_TrueBackground_FalseEntities);
                        else
                            level.SetTileIfEmpty(Layer, X, Y, (byte)Value, Edit_TrueBackground_FalseEntities);
                    }
                }
            }
        }
        private void FillEmpty(int layer, Point point)
        {
            FillEmpty(layer, point.X, point.Y);
        }
        private void FillEmpty(int layer, int x, int y)
        {
            Fill(layer, x, y, 0);
        }
        private void Fill(int layer, Point point, byte ID)
        {
            Fill(layer, point.X, point.Y, ID);
        }
        private void Fill(int layer, int x, int y, byte ID)
        {
            int currentID = GetTileTypeAndIDFromPoint(x, y, true, true).ID;
            if (ID == currentID)
                return;

            var data = new bool[Tools.EditorRenderWidth, Tools.EditorRenderHeight];

            void RecursiveFillEmpty(int X, int Y, int fromX, int fromY)
            {
                if (X - Camera.X < 0 || Y - Camera.Y < 0 || X - Camera.X >= Tools.EditorRenderWidth || Y - Camera.Y >= Tools.EditorRenderHeight)
                    return;
                if (data[X - Camera.X, Y - Camera.Y])
                    return;

                if (currentID == GetTileTypeAndIDFromPoint(X, Y, true, true).ID)
                {
                    level.SetTile(CurrentLayer, X, Y, ID, Edit_TrueBackground_FalseEntities);
                    data[X - Camera.X, Y - Camera.Y] = true;
                    if (X - 1 != fromX) RecursiveFillEmpty(X - 1, Y, X, Y);
                    if (X + 1 != fromX) RecursiveFillEmpty(X + 1, Y, X, Y);
                    if (Y - 1 != fromY) RecursiveFillEmpty(X, Y - 1, X, Y);
                    if (Y + 1 != fromY) RecursiveFillEmpty(X, Y + 1, X, Y);
                }
            }

            RecursiveFillEmpty(x, y, 0, 0);
        }
        private void btSelectionAllLayers_Click(object sender, EventArgs e)
        {
            level.SelectAllLayers = !level.SelectAllLayers;
            btSelectionAllLayers.BackColor = level.SelectAllLayers ? Color.Gold : Color.White;
            RenderPanel.Focus();
        }

        private void hScrollBarX_Scroll(object sender, ScrollEventArgs e)
        {
            Camera.X = hScrollBarX.Value;
            //Draw();
            //zelecxModule?.UpdateCamera(Camera.X, Camera.Y, Tools.SfImageToBitmap(renderwindow.Capture()));
        }
        private void vScrollBarY_Scroll(object sender, ScrollEventArgs e)
        {
            Camera.Y = vScrollBarY.Value;
            //Draw();
            //zelecxModule?.UpdateCamera(Camera.X, Camera.Y, Tools.SfImageToBitmap(renderwindow.Capture()));
        }

        private void btFillSelectionReplace_Click(object sender, EventArgs e)
        {
            FillSelectionReplace = !FillSelectionReplace;
            btFillSelectionReplace.BackColor = FillSelectionReplace ? Color.Gold : Color.White;
            RenderPanel.Focus();
        }

        private void timerCommandKey_Tick(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.X) && X_r) // Cut
                {
                    VolatileSelection.Select(level, Edit_TrueBackground_FalseEntities);
                    level.RemoveSelection(Edit_TrueBackground_FalseEntities);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.C) && C_r) // Copy
                {
                    VolatileSelection.Select(level, Edit_TrueBackground_FalseEntities);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.V) && V_r) // Paste
                {
                    if (!VolatileSelection.IsEmpty())
                    {
                        VolatileSelection.ToBeDrawn = true;
                        level.ValidatingPasteSelection = true;
                    }
                }
            }
            else
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Delete) && Delete_r) // Delete
                {
                    level.RemoveSelection(Edit_TrueBackground_FalseEntities);
                }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Return) && Return_r) // Validate Paste
                {
                    if (level.ValidatingPasteSelection)
                    {
                        VolatileSelection.ToBeDrawn = false;
                        level.ValidatingPasteSelection = false;
                        level.PasteRaw(VolatileSelection.selection, VolatileSelection.size, VolatileSelection.position, Edit_TrueBackground_FalseEntities);
                        VolatileSelection.position = VolatileSelection.positionBeforeHoldingToMove;
                    }
                }
            }

            Left_r = !Mouse.IsButtonPressed(Mouse.Button.Left);
            X_r = !Keyboard.IsKeyPressed(Keyboard.Key.X);
            C_r = !Keyboard.IsKeyPressed(Keyboard.Key.C);
            V_r = !Keyboard.IsKeyPressed(Keyboard.Key.V);
            Delete_r = !Keyboard.IsKeyPressed(Keyboard.Key.Delete);
            Return_r = !Keyboard.IsKeyPressed(Keyboard.Key.Return);
        }

        private void btDoorProperties_Click(object sender, EventArgs e)
        {
            var menu = new DoorsPropertiesMenu(level);
            menu.ShowDialog();
        }
        private void btWarpsProperties_Click(object sender, EventArgs e)
        {
            var menu = new WarpsPropertiesMenu(level);
            menu.ShowDialog();
        }

        private void btEditBackground_Click(object sender, EventArgs e)
        {
            Edit_TrueBackground_FalseEntities = true;
            BackColor = Color.Azure;
            btEdit.Enabled = false;
            btWarpEnter.Enabled = false;
            btWarpExit.Enabled = false;
            btLayer0.PerformClick();
            btLayer1.Enabled = false;
            btLayer2.Enabled = false;
            btCamera.Enabled = false;
        }
        private void btEditEntities_Click(object sender, EventArgs e)
        {
            Edit_TrueBackground_FalseEntities = false;
            BackColor = SystemColors.Control;
            btEdit.Enabled = true;
            btWarpEnter.Enabled = true;
            btWarpExit.Enabled = true;
            btLayer1.Enabled = true;
            btLayer2.Enabled = true;
            btCamera.Enabled = true;
        }
        private void tkbBgContrast_ValueChanged(object sender, EventArgs e)
        {
            level.bgContrast = (byte)(tkbBgContrast.Value / 4.0 * 255);
        }

        private void btRefreshRender_Click(object sender, EventArgs e)
        {
            renderwindow.Dispose();
            renderwindow = new RenderWindow(RenderPanel.Handle);
        }

        private void btElectricPanel_Click(object sender, EventArgs e)
        {
            if(zelecxModule != null)
            {
                zelecxModule.SaveMap("ElectricPanel.MAP.temp");

                RenderPanel.MouseDown -= zelecxModule.Render_MouseDown;
                RenderPanel.MouseUp -= zelecxModule.Render_MouseUp;
                KeyDown -= zelecxModule._Main_KeyDown;
                KeyUp -= zelecxModule._Main_KeyUp;

                zelecxModule.EndModuleTask();
                zelecxModule = null;

                hScrollBarX.Enabled = vScrollBarY.Enabled = true;
            }
            else
            {
                zelecxModule = new _Main(true, new Size(Tools.MapWidth, Tools.MapHeight), Tools.TileSize, RenderPanel, Tools.SfImageToBitmap(renderwindow.Capture()), new Rectangle(Camera.X, Camera.Y, Tools.EditorRenderWidth, Tools.EditorRenderHeight));
                zelecxModule.LoadMap("ElectricPanel.MAP.temp");

                RenderPanel.MouseDown += zelecxModule.Render_MouseDown;
                RenderPanel.MouseUp += zelecxModule.Render_MouseUp;
                KeyDown += zelecxModule._Main_KeyDown;
                KeyUp += zelecxModule._Main_KeyUp;

                hScrollBarX.Enabled = vScrollBarY.Enabled = false;
            }
        }
    }
}