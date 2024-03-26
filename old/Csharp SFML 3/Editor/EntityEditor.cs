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
using Framework;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Editor
{
    public partial class EntityEditor : Form
    {
        public RenderWindow renderwindow, renderwindowAutoTile;
        Sprite pen = new Sprite(), eraser = new Sprite();
        enum PenType { Pen, Eraser, Fill }
        PenType penType = PenType.Pen;
        List<Button> ColorSaves = new List<Button>();
        List<Button> AutoTileButtons = new List<Button>();
        List<Bitmap> AutoTileTemplateRaws = new List<Bitmap>();
        List<Bitmap> AutoTileImagesRaws = new List<Bitmap>();
        int AutoTileSelectedID = 0;
        bool AutoTileTemplateDisplaying = false;
        BehaviorScript behaviorScript = new BehaviorScript();
        public byte behaviorIDToLoad = 0;


        public EntityEditor()
        {
            Tools.Initialize(16);

            InitializeComponent();
            renderwindow = new RenderWindow(RenderPanel.Handle);
            renderwindow.Clear(sfColor.White);
            renderwindow.Display();
            renderwindow.Clear(sfColor.White);
            renderwindow.Display();
            renderwindowAutoTile = new RenderWindow(RenderPanelAutoTile.Handle);
            renderwindowAutoTile.Clear(sfColor.White);
            renderwindowAutoTile.Display();
            renderwindowAutoTile.Clear(sfColor.White);
            renderwindowAutoTile.Display();

            numLayer.Maximum = Tools.MapLayers - 1;

            cbbEntityLevel1.Items.AddRange(Enum.GetValues(typeof(GlobalVariables.Entity)).Cast<object>().ToArray());
            SetPen();

            ColorSaves.AddRange(new Button[]{ btColorSave1, btColorSave2, btColorSave3, btColorSave4, btColorSave5, btColorSave6, btColorSave7, btColorSave8, btColorSave9, btColorSave10});
            foreach(var bt in ColorSaves)
            {
                bt.MouseDown += delegate (object sender, MouseEventArgs e)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        bt.BackColor = btColor.BackColor;
                        return;
                    }

                    if (e.Button == MouseButtons.Right)
                    {
                        btColor.BackColor = bt.BackColor;
                        SetPen();
                        return;
                    }

                    if (e.Button == MouseButtons.Middle)
                    {
                        bt.BackColor = btColorTransparent.BackColor;
                        return;
                    }
                };
            }

            FormClosed += delegate { Dispose(); };
        }
        private void AutoTilesSetTemplateButtonsImages(Bitmap template)
        {
            AutoTileTemplateRaws.Clear();
            Bitmap target;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    target = new Bitmap(Tools.TileSize, Tools.TileSize);
                    using (Graphics graphics = Graphics.FromImage(target))
                    {
                        graphics.DrawImage(
                            template,
                            new Rectangle(0, 0, Tools.TileSize, Tools.TileSize),
                            new Rectangle(x * Tools.TileSize, y * Tools.TileSize, Tools.TileSize, Tools.TileSize),
                            GraphicsUnit.Pixel);

                        AutoTileTemplateRaws.Add(target);
                    }
                }
            }

            AutoTileButtons.AddRange(new Button[] { btAutoTile1, btAutoTile2, btAutoTile3, btAutoTile4,  btAutoTile5,  btAutoTile6,
                                            btAutoTile7, btAutoTile8, btAutoTile9, btAutoTile10, btAutoTile11, btAutoTile12 });
            for (int i = 0; i < 12; i++)
                AutoTileImagesRaws.Add(new Bitmap(Tools.TileSize, Tools.TileSize));
            foreach (var bt in AutoTileButtons)
            {
                bt.BackgroundImageLayout = ImageLayout.Stretch;
                bt.Click += delegate (object button, EventArgs e) { AutoTileSelectedID = AutoTileButtons.IndexOf(bt); SelectAutoTileRawInRender(); };
            }
            LoadAutoTileButtonsImages();
        }
        private void timerDraw_Tick(object sender, EventArgs e)
        {
            Application.DoEvents();

            renderwindow.DispatchEvents();
            renderwindow.Display();
            renderwindow.Display();

            renderwindowAutoTile.DispatchEvents();
            renderwindowAutoTile.Display();
            renderwindowAutoTile.Display();
        }
        private void cbbEntityLevel1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btCreate.Enabled = false;
            cbbEntityLevel2.Enabled = false;
            cbbEntityLevel3.Enabled = false;
            cbbEntityLevel4.Enabled = false;
            cbbEntityLevel2.SelectedItem = null;
            cbbEntityLevel3.SelectedItem = null;
            cbbEntityLevel4.SelectedItem = null;
            cbbEntityLevel2.Items.Clear();
            cbbEntityLevel3.Items.Clear();
            cbbEntityLevel4.Items.Clear();

            var assembly = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "Framework");
            var type = assembly.GetType("Framework.GlobalVariables+"+cbbEntityLevel1.SelectedItem.ToString());
            if (type == null)
                btCreate.Enabled = true;
            else
            {
                cbbEntityLevel2.Items.AddRange(Enum.GetValues(type).Cast<object>().ToArray());
                cbbEntityLevel2.Enabled = true;
            }
        }
        private void cbbEntityLevel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbbEntityLevel2.SelectedItem == null)
                return;

            btCreate.Enabled = false;
            cbbEntityLevel3.Enabled = false;
            cbbEntityLevel4.Enabled = false;
            cbbEntityLevel3.SelectedItem = null;
            cbbEntityLevel4.SelectedItem = null;
            cbbEntityLevel3.Items.Clear();
            cbbEntityLevel4.Items.Clear();

            var assembly = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "Framework");
            var type = assembly.GetType("Framework.GlobalVariables+" + cbbEntityLevel2.SelectedItem.ToString());
            if (type == null)
                btCreate.Enabled = true;
            else
            {
                cbbEntityLevel3.Items.AddRange(Enum.GetValues(type).Cast<object>().ToArray());
                cbbEntityLevel3.Enabled = true;
            }
        }
        private void cbbEntityLevel3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbEntityLevel3.SelectedItem == null)
                return;

            btCreate.Enabled = false;
            cbbEntityLevel4.Enabled = false;
            cbbEntityLevel4.SelectedItem = null;
            cbbEntityLevel4.Items.Clear();

            var assembly = AppDomain.CurrentDomain.GetAssemblies().Single(x => x.GetName().Name == "Framework");
            var type = assembly.GetType("Framework.GlobalVariables+" + cbbEntityLevel3.SelectedItem.ToString());
            if (type == null)
                btCreate.Enabled = true;
            else
            {
                cbbEntityLevel4.Items.AddRange(Enum.GetValues(type).Cast<object>().ToArray());
                cbbEntityLevel4.Enabled = true;
            }
        }
        private void cbbEntityLevel4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbEntityLevel4.SelectedItem == null)
                return;

            btCreate.Enabled = true;
        }
        private void btCreate_Click(object sender, EventArgs e)
        {
            btCreate.Enabled = false;
            cbbEntityLevel1.Enabled = false;
            cbbEntityLevel2.Enabled = false;
            cbbEntityLevel3.Enabled = false;
            cbbEntityLevel4.Enabled = false;

            ManagePropertiesControls();

            if ((GlobalVariables.Entity)cbbEntityLevel1?.SelectedItem == GlobalVariables.Entity.Organic)
                AutoTilesSetTemplateButtonsImages(Properties.Resources.templateOrganic);
            else
            {
                if ((GlobalVariables.Material)cbbEntityLevel2?.SelectedItem == GlobalVariables.Material.Door)
                    AutoTilesSetTemplateButtonsImages(Properties.Resources.templateDoor);
                else
                    AutoTilesSetTemplateButtonsImages(Properties.Resources.templateautotile16);
            }

            panel1.Visible = true;
            btChange.Enabled = true;
        }
        private void RenderPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None)
            {
                RenderPanel_MouseDown(sender, e);
            }
        }
        private void SetPen()
        {
            pen    =    new Sprite(new Texture(new sfImage((uint)Tools.TileSize, (uint)Tools.TileSize, Tools.ColorToSfColor(btColor.BackColor))));
            eraser =    new Sprite(new Texture(new sfImage((uint)Tools.TileSize, (uint)Tools.TileSize, Tools.ColorToSfColor(btColorTransparent.BackColor))));
        }
        private void Draw(Panel panel, Point position, Sprite sp = null)
        {
            RenderWindow rw = panel == RenderPanel ? renderwindow : renderwindowAutoTile;

            if (position.X < 0 || position.Y < 0 || position.X >= rw.Size.X || position.Y >= rw.Size.Y)
                return;

            if (sp == null)
                sp = pen;

            var pos = Tools.Snap(position);
            sp.Position = new Vector2f(pos.X, pos.Y);

            rw.Draw(sp);
            rw.Display();
            rw.Draw(sp);
            rw.Display();
        }
        private void Fill(Panel panel, Point position, Sprite sp = null)
        {
            RenderWindow rw = panel == RenderPanel ? renderwindow : renderwindowAutoTile;

            void RecursiveDraw(uint X, uint Y, sfColor color)
            {
                if (X < 0 || Y < 0 || X >= rw.Size.X || Y >= rw.Size.Y)
                    return;

                var c = rw.Capture().GetPixel(X, Y);
                if (Tools.CompareSfColors(c, color))
                {
                    Draw(panel, new Point((int)X, (int)Y));
                    RecursiveDraw(X - (uint)Tools.TileSize, Y, color);
                    RecursiveDraw(X + (uint)Tools.TileSize, Y, color);
                    RecursiveDraw(X, Y - (uint)Tools.TileSize, color);
                    RecursiveDraw(X, Y + (uint)Tools.TileSize, color);
                }
            }

            var col = rw.Capture().GetPixel((uint)position.X, (uint)position.Y);
            if(!Tools.CompareSfColorToColor(col, btColor.BackColor))
                RecursiveDraw((uint)position.X, (uint)position.Y, col);
        }
        private void RenderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            Panel panel = (Panel)sender;
            RenderWindow rw = panel == RenderPanel ? renderwindow : renderwindowAutoTile;

            if (e.Button == MouseButtons.Right)
            {
                btColor.BackColor = Tools.SfColorToColor(rw.Capture().GetPixel((uint)e.X, (uint)e.Y));
                SetPen();
                return;
            }

            if (e.Button == MouseButtons.Left)
            {
                switch(penType)
                {
                    default:
                    case PenType.Pen:
                        if(Keyboard.IsKeyPressed(Keyboard.Key.LAlt))
                            Draw(panel, e.Location, eraser);
                        else
                            Draw(panel, e.Location);
                        break;
                    case PenType.Eraser:
                        if(Keyboard.IsKeyPressed(Keyboard.Key.LAlt))
                            Draw(panel, e.Location);
                        else
                            Draw(panel, e.Location, eraser);
                        break;
                    case PenType.Fill:
                        if (Keyboard.IsKeyPressed(Keyboard.Key.LAlt))
                            Fill(panel, e.Location, eraser);
                        else
                            Fill(panel, e.Location);
                        break;
                }
                return;
            }
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
        private void btColor_Click(object sender, EventArgs e)
        {
            colorDialog.Color = btColor.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                btColor.BackColor = colorDialog.Color;
                SetPen();
            }
        }
        private void btColorTransparent_Click(object sender, EventArgs e)
        {
            colorDialog.Color = btColorTransparent.BackColor;
            if (colorDialog.ShowDialog() == DialogResult.OK)
                btColorTransparent.BackColor = colorDialog.Color;
        }
        private void btFill_Click(object sender, EventArgs e)
        {
            penType = PenType.Fill;

            btPen.FlatStyle = FlatStyle.Popup;
            btEraser.FlatStyle = FlatStyle.Popup;
            btFill.FlatStyle = FlatStyle.Standard;
        }
        private string GetCurrentEntityPath()
        {
            string result = "Entities/Entity/";
            if (cbbEntityLevel1.SelectedItem?.ToString() != null)
                result += cbbEntityLevel1.SelectedItem.ToString() + "/";
            if (cbbEntityLevel2.SelectedItem?.ToString() != null)
                result += cbbEntityLevel2.SelectedItem.ToString() + "/";
            if (cbbEntityLevel3.SelectedItem?.ToString() != null)
                result += cbbEntityLevel3.SelectedItem.ToString() + "/";
            if (cbbEntityLevel4.SelectedItem?.ToString() != null)
                result += cbbEntityLevel4.SelectedItem.ToString() + "/";
            return result.Remove(result.Length - 1);
        }
        private void btNew_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Confirm clear the current modification ?", "New", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                LoadEntity(new EntityProperties());
                ManagePropertiesControls();
            }
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Please fill the 'Name' field.");
                return;
            }

            var entity = CreateEntity();
            entity.Save();
                       
            MessageBox.Show("Save complete !");
        }
        private void btLoad_Click(object sender, EventArgs e)
        {
            var imglist = new ImageList();
            var filesNames = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "\\" + GetCurrentEntityPath()).ToList();
            var entities = new List<EntityProperties>();
            foreach (var entityPath in filesNames)
                entities.Add(EntityProperties.Load(entityPath));

            void callback(EntityProperties entity)
            {
                LoadEntity(entity);
            }

            var entityLoader = new EntityLoader(entities, callback);
            entityLoader.ShowDialog();
        }
        private void btChange_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Confirm clear the current modification ?", "Change", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            LoadEntity(new EntityProperties());

            panel1.Visible = false;
            btCreate.Enabled = false;
            cbbEntityLevel1.Enabled = true;
            cbbEntityLevel2.Enabled = false;
            cbbEntityLevel3.Enabled = false;
            cbbEntityLevel4.Enabled = false;
            cbbEntityLevel1.SelectedIndexChanged -= cbbEntityLevel1_SelectedIndexChanged;
            cbbEntityLevel1.SelectedItem = null;
            cbbEntityLevel1.SelectedIndexChanged += cbbEntityLevel1_SelectedIndexChanged;
            cbbEntityLevel2.SelectedItem = null;
            cbbEntityLevel3.SelectedItem = null;
            cbbEntityLevel4.SelectedItem = null;
            cbbEntityLevel2.Items.Clear();
            cbbEntityLevel3.Items.Clear();
            cbbEntityLevel4.Items.Clear();
        }

        private EntityProperties CreateEntity()
        {
            var entity = new EntityProperties();
            entity.Name = tbName.Text;
            entity.behaviorIDToLoad = behaviorIDToLoad;
            entity.behaviorScript = behaviorScript;
            entity.EntityPath = GetCurrentEntityPath();
            entity.image = Tools.SfImageNormalize(renderwindow.Capture());
            entity.TransparentColor = (btColorTransparent.BackColor.R, btColorTransparent.BackColor.G, btColorTransparent.BackColor.B);
            entity.autoTiled = cbAutoTile.Checked;
            entity.AutoTilesRaws = Tools.ListBitmapToListSfImage(AutoTileImagesRaws);
            entity.indestructible = cbIndestructible.Checked;
            entity.trigger = cbTriggerCollision.Checked; 
            entity.ID = (byte)numID.Value;
            entity.layer = (byte)numLayer.Value;
            entity.HP = (int)numHP.Value;
            entity.gravityEffect = cbGravityEffect.Checked;
            return entity;
        }
        private void LoadEntity(EntityProperties entity)
        {
            tbName.Text = entity.Name;

            var sp = new Sprite(new Texture(Tools.SfImageScale(entity.image)));
            Draw(RenderPanel, new Point(0, 0), sp);

            AutoTileImagesRaws = Tools.ListSfImageToListBitmap(entity.AutoTilesRaws);
            if(!AutoTileTemplateDisplaying)
                LoadAutoTileButtonsImages();

            behaviorIDToLoad = entity.behaviorIDToLoad;
            lbSelectedBehavior.Text = "Selected Behavior ID : " + (behaviorIDToLoad > 0 ? behaviorIDToLoad.ToString() : "");
            cbIndestructible.Checked = entity.indestructible;
            numID.Value = (entity.ID < numID.Minimum ? numID.Minimum : (entity.ID > numID.Maximum ? numID.Maximum : entity.ID));
            numLayer.Value = (entity.layer < numLayer.Minimum ? numLayer.Minimum : (entity.layer > numLayer.Maximum ? numLayer.Maximum : entity.layer));
            numHP.Value = (entity.HP < numHP.Minimum ? numHP.Minimum : (entity.HP > numHP.Maximum ? numHP.Maximum : entity.HP));
            btColorTransparent.BackColor = System.Drawing.Color.FromArgb(255, entity.TransparentColor.R, entity.TransparentColor.G, entity.TransparentColor.B);
            cbAutoTile.Checked = entity.autoTiled;
            cbGravityEffect.Checked = entity.gravityEffect;
            cbTriggerCollision.Checked = entity.trigger;

            behaviorScript = entity.behaviorScript == null ? new BehaviorScript() : entity.behaviorScript;
        }

        private void ManagePropertiesControls()
        {
            cbAutoTile.Checked = true;
            cbAutoTile.Enabled = true;
            numID.Enabled = true;
            numLayer.Enabled = true;
            cbIndestructible.Enabled = true;
            numHP.Enabled = true;
            btEditBehavior.Enabled = false;
            btSetBehavior.Enabled = false;
            btClearBehavior.Enabled = false;
            lbSelectedBehavior.Enabled = false;
            lbSelectedBehavior.Text = "Selected Behavior ID : ";

            if ((GlobalVariables.Entity)cbbEntityLevel1?.SelectedItem == GlobalVariables.Entity.Organic)
            {
                cbAutoTile.Checked = false;
                cbAutoTile.Enabled = false;
                groupAutoTile.Visible = true;

                if(cbbEntityLevel4?.SelectedItem != null)
                if ((GlobalVariables.Buildable)cbbEntityLevel4?.SelectedItem == GlobalVariables.Buildable.Behavior)
                {
                    groupAutoTile.Visible = false;
                    numLayer.Value = 1;
                    numLayer.Enabled = false;
                    numHP.Value = 1;
                    numHP.Enabled = false;
                    cbIndestructible.Enabled = false;
                    cbTriggerCollision.Enabled = false;
                    btEditBehavior.Enabled = true;
                    return;
                }

                return;
            }

            if ((GlobalVariables.Material)cbbEntityLevel2?.SelectedItem == GlobalVariables.Material.Block)
            {
                cbAutoTile.Checked = false;
                cbAutoTile.Enabled = true;
                btSetBehavior.Enabled = true;
                btClearBehavior.Enabled = true;
                lbSelectedBehavior.Enabled = true;
                return;
            }

            if ((GlobalVariables.Material)cbbEntityLevel2?.SelectedItem == GlobalVariables.Material.Door)
            {
                cbAutoTile.Checked = true;
                cbAutoTile.Enabled = false;
                numLayer.Value = 1;
                numLayer.Enabled = false;
                cbIndestructible.Enabled = false;
                numHP.Enabled = false;
                return;
            }

            cbAutoTile.Checked = false;
            cbAutoTile.Enabled = false;
            groupAutoTile.Visible = false;
            return;
        }
        private void btAutotileShowTemplate_Click(object sender, EventArgs e)
        {
            AutoTileTemplateDisplaying = true;
            for (int i = 0; i < 12; i++)
                AutoTileButtons[i].BackgroundImage = AutoTileTemplateRaws[i];
        }
        private void btAutotileHideTemplate_Click(object sender, EventArgs e)
        {
            AutoTileTemplateDisplaying = false;
            AutoTileImagesRaws[AutoTileSelectedID] = Tools.SfImageToBitmap_Normalized(renderwindowAutoTile.Capture());
            LoadAutoTileButtonsImages();
        }
        private void LoadAutoTileButtonsImages()
        {
            for (int i = 0; i < 12; i++)
                AutoTileButtons[i].BackgroundImage = AutoTileImagesRaws[i];
        }
        private void cbAutoTile_CheckedChanged(object sender, EventArgs e)
        {
            groupAutoTile.Visible = (sender as CheckBox).Checked;
        }
        private void SelectAutoTileRawInRender()
        {
            renderwindowAutoTile.Clear(Tools.ColorToSfColor(btColorTransparent.BackColor));
            renderwindowAutoTile.Display();
            renderwindowAutoTile.Clear(Tools.ColorToSfColor(btColorTransparent.BackColor));
            renderwindowAutoTile.Display();

            var sp = new Sprite(new Texture(Tools.BitmapToSfImage_Scaled(new Bitmap(AutoTileButtons[AutoTileSelectedID].BackgroundImage))));
            Draw(RenderPanelAutoTile, new Point(0, 0), sp);
        }

        private void cbIndestructible_CheckedChanged(object sender, EventArgs e)
        {
            numHP.Enabled = !cbIndestructible.Checked;
        }
        
        private void btEditBehavior_Click(object sender, EventArgs e)
        {
            void Callback(BehaviorScript script)
            {
                behaviorScript = script;
            }
            
            var form = new BehaviorEdit.BehaviorEditor(behaviorScript, Callback);
            form.ShowDialog();
        }
        private void btClearBehavior_Click(object sender, EventArgs e)
        {
            behaviorIDToLoad = 0;
            lbSelectedBehavior.Text = "Selected Behavior ID : " + behaviorIDToLoad;
        }
        private void btSetBehavior_Click(object sender, EventArgs e)
        {
            var imglist = new ImageList();
            var filesNames = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "\\" + "Entities/Entity/Organic/UnPlayable/Buildable/Behavior").ToList();
            var entities = new List<EntityProperties>();
            foreach (var entityPath in filesNames)
                entities.Add(EntityProperties.Load(entityPath));

            void callback(EntityProperties entity)
            {
                behaviorIDToLoad = entity.ID;
                lbSelectedBehavior.Text = "Selected Behavior ID : " + behaviorIDToLoad;
            }

            var entityLoader = new EntityLoader(entities, callback);
            entityLoader.ShowDialog();
        }
    }
}
