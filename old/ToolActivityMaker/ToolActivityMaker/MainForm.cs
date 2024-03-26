using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolActivityMaker
{
    public partial class MainForm : Form
    {
        public enum Tool { Activity, Text, Draw, Rectangle, Circle, Sticker }

        public static MainForm Instance = null;



        #region Variables
        public enum TemplateType { Day, TwoDays, Week, Custom, Undefined }
        public TemplateType TType = TemplateType.Undefined;
        bool MouseHolding = false, CreatingSegment = false;
        Point MouseDownPosition = Point.Empty;
        Timer TimerDraw = new Timer() { Interval = 10 };
        List<Segment> Segments = new List<Segment>();
        Segment SelectedSegment = null;
        Segment ClickedSegment = null;
        string TimeFormat = @"hh\:mm";
        string CurrentFilename = "";
        PictureBox DrawLayer;
        Tool tool = Tool.Activity;
        List<EditableText> Texts = new List<EditableText>();
        Bitmap DrawLayer_Content;
        int CustomTemplate_DaysCount = 1;
        ButtonWithCallback CustomTempalte_AddButton = null;
        ButtonWithCallback CustomTempalte_RemoveButton = null;
        Bitmap ActivityPB_Image = null;
        Bitmap TemporaryObjectToDraw = null;
        bool AltKeyHolding = false;
        (int Index, Segment Segment) AwaitingSegmentToBeCuttedByNewSegment = (-1, null);
        #endregion


        #region Initialize
        public MainForm()
        {
            InitializeComponent();

            Icon = Icon.FromHandle(new Bitmap(Properties.Resources.ts).GetHicon());

            DrawLayer = new PictureBox();
            DrawLayer.Parent = ActivityPB;
            DrawLayer.Dock = DockStyle.Fill;
            DrawLayer.BackColor = Color.Transparent;
            DrawLayer.MouseDown += DrawLayer_MouseDown;
            DrawLayer.MouseUp += DrawLayer_MouseUp;
            DrawLayer.MouseLeave += DrawLayer_MouseLeave;
            DrawLayer.MouseMove += DrawLayer_MouseMove;
            DrawLayer.MouseWheel += DrawLayer_MouseWheel;

            Instance = this;

            dayToolStripMenuItem_Click(1, null);

            TimerDraw.Tick += Draw;
            TimerDraw.Start();
        }
        #endregion




        #region Menu

        #region Template
        private void dayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TType == TemplateType.Day)
                return;

            if(Segments.Count == 0 || (sender is int && (int)sender == 1) || MessageBox.Show("Activity will be lost, are you sure to proceed ?", "Change Template Type", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                PanelActivity.Size = new Size(PanelActivity.Width, 148);
                ActivityPB.Size = new Size(1251, 126);
                TType = TemplateType.Day;
                SetImageFromTType();
                ResetControlsRelatedToTType();
            }
        }
        private void twoDaysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TType == TemplateType.TwoDays)
                return;

            if (Segments.Count == 0 || (sender is int && (int)sender == 1) || MessageBox.Show("Activity will be lost, are you sure to proceed ?", "Change Template Type", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                PanelActivity.Size = new Size(PanelActivity.Width, 273);
                ActivityPB.Size = new Size(1251, 251);
                TType = TemplateType.TwoDays;
                SetImageFromTType();
                ResetControlsRelatedToTType();
            }
        }
        private void weekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TType == TemplateType.Week)
                return;

            if (Segments.Count == 0 || (sender is int && (int)sender == 1) || MessageBox.Show("Activity will be lost, are you sure to proceed ?", "Change Template Type", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                PanelActivity.Size = new Size(PanelActivity.Width, 500);
                ActivityPB.Size = new Size(1251, 870);
                TType = TemplateType.Week;
                SetImageFromTType();
                ResetControlsRelatedToTType();
            }
        }
        private void customToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Segments.Count == 0 || (sender is int && (int)sender == 1) || MessageBox.Show("Activity will be lost, are you sure to proceed ?", "Change Template Type", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                PanelActivity.Size = new Size(PanelActivity.Width, 500);
                ActivityPB.Size = new Size(1251, 124);
                TType = TemplateType.Custom;
                SetImageFromTType();

                ActivityPB_Image = new Bitmap(ActivityPB.Width, ActivityPB.Height);
                using (Graphics g = Graphics.FromImage(ActivityPB_Image))
                {
                    g.DrawImage(ActivityPB.Image, 0, 0);
                    g.DrawString("1", DefaultFont, Brushes.Black, 10, 10);
                    g.DrawString("1", DefaultFont, Brushes.Black, ActivityPB.Image.Width - 20, 10);
                }
                ActivityPB.Image = ActivityPB_Image;

                ResetControlsRelatedToTType();

                Bitmap Icon = new Bitmap(Properties.Resources.add, new Size(32, 32));
                int padding = 5;
                CustomTempalte_AddButton = new ButtonWithCallback(
                    new Point(ActivityPB.Location.X + ActivityPB.Width / 2 - Icon.Size.Width / 2, ActivityPB.Height - Icon.Height - padding),
                    Icon,
                    delegate { CustomTemplate_AddDay(); });
            }
        }

        private void ResetControlsRelatedToTType()
        {
            CustomTempalte_AddButton = null;
            CustomTempalte_RemoveButton = null;

            int padding = 5;
            PanelEdit.Location = new Point(PanelActivity.Location.X, PanelActivity.Location.Y + PanelActivity.Height + padding);
            Rectangle screenRectangle = RectangleToScreen(this.ClientRectangle);
            int titleHeight = screenRectangle.Top - this.Top;
            this.Size = new Size(this.Width, titleHeight + PanelEdit.Location.Y + PanelEdit.Height + padding);
            Segments.Clear();
            Texts.Clear();
            DrawLayer_Content = new Bitmap(ActivityPB.Width, ActivityPB.Height);
        }

        private void CustomTemplate_AddDay(bool update_DrawLayer_Content = true)
        {
            CustomTemplate_DaysCount++;

            Bitmap day = new Bitmap(Properties.Resources.TemplateCustom);
            ActivityPB.Size = new Size(ActivityPB.Width, ActivityPB.Height + day.Size.Height);
            Bitmap img = new Bitmap(ActivityPB_Image);
            ActivityPB_Image = new Bitmap(ActivityPB.Width, ActivityPB.Height);
            using (Graphics g = Graphics.FromImage(day))
            {
                g.DrawString(CustomTemplate_DaysCount.ToString(), DefaultFont, Brushes.Black, 10, 10);
                g.DrawString(CustomTemplate_DaysCount.ToString(), DefaultFont, Brushes.Black, day.Width - 20, 10);
            }
            using (Graphics g = Graphics.FromImage(ActivityPB_Image))
            {
                g.DrawImage(img, 0, 0);
                g.DrawImage(day, 0, ActivityPB.Height - day.Height);
            }
            ActivityPB.Image = ActivityPB_Image;

            int padding = 5;
            CustomTempalte_AddButton.Location = new Point(ActivityPB.Location.X + ActivityPB.Width / 2 - Icon.Size.Width / 2, ActivityPB.Height - Icon.Height - padding);
            if (CustomTempalte_RemoveButton == null)
            {
                var Icon = new Bitmap(Properties.Resources.remove, new Size(32, 32));
                CustomTempalte_RemoveButton = new ButtonWithCallback(
                    new Point(ActivityPB.Location.X + ActivityPB.Width - Icon.Size.Width - padding, ActivityPB.Location.Y + day.Height * (CustomTemplate_DaysCount - 1) + padding),
                    Icon,
                    CustomTemplate_RemoveDay);
            }
            else
            {
                CustomTempalte_RemoveButton.Location = new Point(ActivityPB.Location.X + ActivityPB.Width - Icon.Size.Width - padding, ActivityPB.Location.Y + day.Height * (CustomTemplate_DaysCount - 1) + padding);
            }


            if (update_DrawLayer_Content)
                Update_DrawLayer_Content();
        }
        private void Update_DrawLayer_Content()
        {
            Bitmap img = new Bitmap(DrawLayer_Content);
            DrawLayer_Content = new Bitmap(ActivityPB_Image.Width, ActivityPB_Image.Height);
            using (Graphics g = Graphics.FromImage(DrawLayer_Content))
                g.DrawImage(img, 0, 0);
        }
        private void CustomTemplate_RemoveDay()
        {
            RemoveSegmentsOnDay(CustomTemplate_DaysCount--);

            Image day = Properties.Resources.TemplateCustom;
            ActivityPB.Size = new Size(ActivityPB.Width, ActivityPB.Height - day.Size.Height);
            Bitmap img = new Bitmap(ActivityPB_Image);
            ActivityPB_Image = new Bitmap(ActivityPB.Width, ActivityPB.Height);
            using (Graphics g = Graphics.FromImage(ActivityPB_Image))
                g.DrawImage(img, 0, 0);

            int padding = 5;
            CustomTempalte_AddButton.Location = new Point(ActivityPB.Location.X + ActivityPB.Width / 2 - Icon.Size.Width / 2, ActivityPB.Height - Icon.Height - padding);
            if (CustomTemplate_DaysCount == 1)
                CustomTempalte_RemoveButton = null;
            else
                CustomTempalte_RemoveButton.Location = new Point(ActivityPB.Location.X + ActivityPB.Width - Icon.Size.Width - padding, ActivityPB.Location.Y - PanelActivity.AutoScrollPosition.Y + day.Height * (CustomTemplate_DaysCount - 1) + padding);


            img = new Bitmap(DrawLayer_Content);
            DrawLayer_Content = new Bitmap(ActivityPB_Image.Width, ActivityPB_Image.Height);
            using (Graphics g = Graphics.FromImage(DrawLayer_Content))
                g.DrawImage(img, 0, 0);
        }
        #endregion

        #region File
        bool NewCancel = false;
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to proceed ? You will lost all the activities.", "Clear", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Segments.Clear();
                Texts.Clear();
                CurrentFilename = "";
                dayToolStripMenuItem_Click(sender, e);
                DrawLayer_Content = new Bitmap(DrawLayer_Content.Width, DrawLayer_Content.Height);
                TemporaryObjectToDraw = new Bitmap(DrawLayer_Content.Width, DrawLayer_Content.Height);
            }
            else
            {
                NewCancel = true;
            }
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to proceed ? Unsaved data will be lost.", "Close", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Close();
            }
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.Filter = "ACT Files (.act)|*.act";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CurrentFilename = dialog.FileName;
                Save();
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.Filter = "Activity Files (*.act;*.txt)|*.act;*.txt|ACT Files (*.act)|*.act|STTRANS Files (*.txt)|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                NewCancel = false;
                newToolStripMenuItem_Click(null, null);
                if (NewCancel)
                    return;

                CurrentFilename = dialog.FileName;

                string ext = Path.GetExtension(CurrentFilename).ToLower();
                if (ext == ".act")
                    LoadACT();
                else if (ext == ".txt")
                    LoadSTTRANS();
            }
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentFilename))
                saveAsToolStripMenuItem_Click(null, null);
            else
                Save();
        }
        private void LoadACT()
        {
            using (FileStream file = File.OpenRead(CurrentFilename))
            {
                BinaryFormatter bf = new BinaryFormatter();

                TType = (TemplateType)bf.Deserialize(file);
                switch (TType)
                {
                    case TemplateType.Day: dayToolStripMenuItem_Click(1, null); break;
                    case TemplateType.TwoDays: twoDaysToolStripMenuItem_Click(1, null); break;
                    case TemplateType.Week: weekToolStripMenuItem_Click(1, null); break;
                    case TemplateType.Custom: customToolStripMenuItem_Click(1, null); break;
                }

                Segments = bf.Deserialize(file) as List<Segment>;
                Texts = bf.Deserialize(file) as List<EditableText>;
                CustomTemplate_DaysCount = (int)bf.Deserialize(file);
                DrawLayer_Content = (Bitmap)bf.Deserialize(file);
            }

            if (TType == TemplateType.Custom && CustomTemplate_DaysCount > 1)
                AddDaysCustom();
        }
        private void LoadSTTRANS()
        {
            string file = File.ReadAllText(CurrentFilename);
            if (string.IsNullOrWhiteSpace(file))
                return;

            string[] lines = file.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string header = lines[0];

            if (!header.Contains("TX-VISIO") && !header.Contains("TX-SOCIAL"))
                return;
            
            List<Segment> ReadSegments = new List<Segment>();
            string line = "";
            DateTime? FirstDay = null;
            DateTime? LastDay = null;
            for (int i = 1; i < lines.Length; i++)
            {
                line = lines[i];

                if (line[0] != '1')
                    continue;

                string[] infos = line.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                string str_date = infos[3];
                string str_start = infos[4];
                string str_end = infos[5];
                string str_type = infos[8];

                if (str_start == str_end)
                    continue;// start & end = same time -> segment of 0 minute

                int int_start_H = int.Parse("" + str_start[0] + str_start[1]);
                int int_start_M = int.Parse("" + str_start[2] + str_start[3]);
                int int_end_H = int.Parse("" + str_end[0] + str_end[1]);
                int int_end_M = int.Parse("" + str_end[2] + str_end[3]);

                if (int_start_H * 60 + int_start_M > int_end_H * 60 + int_end_M)
                    continue;// start > end -> wrong duration

                if (FirstDay == null)
                    FirstDay = DateTime.ParseExact(str_date, "yyyyMMdd", CultureInfo.CurrentCulture);
                LastDay = DateTime.ParseExact(str_date, "yyyyMMdd", CultureInfo.CurrentCulture);
                TimeSpan date = (LastDay.Value - FirstDay.Value).Add(new TimeSpan(1,0,0,0));

                Segment.ActivityType Type = Segment.ActivityType.Undefined;
                switch (str_type)
                {
                    default: throw new Exception("Parsed Type not expected");
                    case "00C": Type = Segment.ActivityType.Drive; break;
                    case "00T": Type = Segment.ActivityType.Work; break;
                    case "004": Type = Segment.ActivityType.Rest; break;
                    case "00R": Type = Segment.ActivityType.Rest; break;
                    case "006": Type = Segment.ActivityType.Available; break;
                    case "00D": Type = Segment.ActivityType.Available; break;
                }

                TimeSpan Start = new TimeSpan(date.Days, int_start_H, int_start_M, 0);
                TimeSpan End = new TimeSpan(date.Days, int_end_H, int_end_M, 0);
                ReadSegments.Add(new Segment(Start, End, Type));
            }

            CustomTemplate_DaysCount = (LastDay.Value - FirstDay.Value).Days + 1;
            switch(CustomTemplate_DaysCount)
            {
                case 0: throw new Exception("DaysCount not expected (0)");
                case 1: dayToolStripMenuItem_Click(null, null); break;
                case 2: twoDaysToolStripMenuItem_Click(null, null); break;
                case 7: weekToolStripMenuItem_Click(null, null); break;
                default: customToolStripMenuItem_Click(null, null); AddDaysCustom(); break;
            }

            ReadSegments.ForEach(x => AddSegment(x, true));
        }
        private void AddDaysCustom()
        {
            int count = CustomTemplate_DaysCount;// CustomTemplate_DaysCount will changes, so use 'constant' value of dayscount
            CustomTemplate_DaysCount = 1;
            for (int i = 1; i < count; i++)
                CustomTemplate_AddDay(false);
            Update_DrawLayer_Content();
        }
        private void Save()
        {
            Texts.ForEach(x => x.Editing = x.Hover = false);

            using (FileStream file = File.OpenWrite(CurrentFilename))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(file, TType);
                bf.Serialize(file, Segments);
                bf.Serialize(file, Texts);
                bf.Serialize(file, CustomTemplate_DaysCount);
                bf.Serialize(file, DrawLayer_Content);
            }
        }
        #endregion

        #region Actions
        private void captureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(ActivityPB.Image);
            using (Graphics g = Graphics.FromImage(img))
                g.DrawImage(DrawLayer.Image, 0, 0);
            img.Save(Directory.GetCurrentDirectory() + @"\Capture.png", System.Drawing.Imaging.ImageFormat.Png);
        }
        private void capturePressePapiersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(ActivityPB.Image);
            using (Graphics g = Graphics.FromImage(img))
                g.DrawImage(DrawLayer.Image, 0, 0);
            Clipboard.SetImage(img);
        }
        private void makeSTTRANSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.InitialDirectory = Directory.GetCurrentDirectory();
            dialog.Filter = "TXT Files (.txt)|*.txt";
            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            var DriverId = TextboxDialog.ShowDialog("DriverId", "123456", 6, true);
            if (DriverId.Result != DialogResult.OK)
                return;
            int ID = int.Parse(DriverId.Instance.tbValue.Text);

            var DateFirstDay = DateTimeDialog.ShowDialog("Date of the first day");
            if (DriverId.Result != DialogResult.OK)
                return;
            DateTime Date = DateFirstDay.Instance.dtValue.Value;

            string header = "0; 21;            TX-VISIO;  1.26.1.2;";
            string body = "";

            foreach(Segment seg in Segments)
            {
                if (seg.Type == Segment.ActivityType.Rest && (Segments.IndexOf(seg) == 0 || Segments.IndexOf(seg) == Segments.Count - 1))
                    continue;

                body += Environment.NewLine;

                string type = "";
                switch(seg.Type)
                {
                    case Segment.ActivityType.Drive: type = "C"; break;
                    case Segment.ActivityType.Work: type = "T"; break;
                    case Segment.ActivityType.Available: type = "D"; break;
                    case Segment.ActivityType.Rest: type = "R"; break;
                }

                body += $"1;{ID};      ;{new DateTime(Date.Year, Date.Month, Date.Day + (seg.Start.Days - 1)).ToString("yyyyMMdd")};{seg.Start.ToString("hhmm")};{seg.End.ToString("hhmm")};      ;      ;00{type};      ;00;      ;86;";
            }

            File.WriteAllText(dialog.FileName, header + body);

            System.Diagnostics.Process.Start(Path.GetDirectoryName(dialog.FileName));
        }
        #endregion

        #endregion


        #region Tools

        private void SetTool(object ButtonWithFormattedName, EventArgs e)
        {
            PanelActivity.Cursor = Cursors.Default;

            ClickedSegment = null;
            SelectedSegment = null;
            text_clicked = null;
            Segments.ForEach(x => x.OnEdition = false);
            Texts.ForEach(x => x.Editing = x.Hover = false);

            string ButtonName = (ButtonWithFormattedName as ToolStripButton).Name;
            tool = (Tool)Enum.Parse(typeof(Tool), ButtonName.Replace("Tool", ""));
            ToolCurrent.Image = (Image) Properties.Resources.ResourceManager.GetObject(ButtonName);

            if(tool == Tool.Sticker)
                PanelActivity.Cursor = new Cursor(new Bitmap(StickerImages[StickerIdSelected]).GetHicon());
        }

        #endregion


        #region Mouse
        private void DrawLayer_MouseDown(object sender, MouseEventArgs e)
        {
            if (CustomTempalte_AddButton != null && CustomTempalte_AddButton.Bounds.Contains(e.Location))
            {
                CustomTempalte_AddButton.OnAnyClick(e);
                return;
            }
            if (CustomTempalte_RemoveButton != null && CustomTempalte_RemoveButton.Bounds.Contains(e.Location))
            {
                CustomTempalte_RemoveButton.OnAnyClick(e);
                return;
            }

            switch (tool)
            {
                case Tool.Activity: ToolActivity_MouseDown(sender, e); break;
                case Tool.Text: ToolText_MouseDown(sender, e); break;
                case Tool.Draw: ToolDraw_MouseDown(sender, e); break;
                case Tool.Rectangle: ToolRectangle_MouseDown(sender, e); break;
                case Tool.Circle: ToolCircle_MouseDown(sender, e); break;
                case Tool.Sticker: ToolSticker_MouseDown(sender, e); break;
            }

            /*foreach(Segment seg in Segments)
                Console.WriteLine($"{seg.Start}  -  {seg.End}" + "\n");
            Console.WriteLine("\n");*/
        }
        private void DrawLayer_MouseUp(object sender, MouseEventArgs e)
        {
            switch (tool)
            {
                case Tool.Activity: ToolActivity_MouseUp(sender, e); break;
                case Tool.Text: ToolText_MouseUp(sender, e); break;
                case Tool.Draw: ToolDraw_MouseUp(sender, e); break;
                case Tool.Rectangle: ToolRectangle_MouseUp(sender, e); break;
                case Tool.Circle: ToolCircle_MouseUp(sender, e); break;
            }
        }
        private void DrawLayer_MouseLeave(object sender, EventArgs e)
        {
            switch (tool)
            {
                case Tool.Activity: ToolActivity_MouseLeave(); break;
                case Tool.Text: ToolText_MouseLeave(); break;
                case Tool.Draw: ToolDraw_MouseLeave(); break;
                case Tool.Rectangle: ToolRectangle_MouseLeave(); break;
                case Tool.Circle: ToolCircle_MouseLeave(); break;
            }
        }
        private void DrawLayer_MouseMove(object sender, MouseEventArgs e)
        {
            switch (tool)
            {
                case Tool.Activity: ToolActivity_MouseMove(sender, e); break;
                case Tool.Text: ToolText_MouseMove(sender, e); break;
                case Tool.Draw: ToolDraw_MouseMove(sender, e); break;
                case Tool.Rectangle: ToolRectangle_MouseMove(sender, e); break;
                case Tool.Circle: ToolCircle_MouseMove(sender, e); break;
            }
        }
        private void DrawLayer_MouseWheel(object sender, MouseEventArgs e)
        {
            switch (tool)
            {
                case Tool.Draw: ToolDraw_MouseWheel(sender, e); break;
                case Tool.Rectangle: ToolRectangle_MouseWheel(sender, e); break;
                case Tool.Circle: ToolCircle_MouseWheel(sender, e); break;
                case Tool.Sticker: ToolSticker_MouseWheel(sender, e); break;
            }
        }
        #endregion




        #region App
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string data_text = Clipboard.GetText();
                if (!string.IsNullOrWhiteSpace(data_text))
                {
                    tool = Tool.Text;
                    text_clicked = new EditableText();
                    text_clicked.Text = data_text;
                    Texts.Add(text_clicked);
                    text_clicked.Editing = true;
                }

                Image data_image = Clipboard.GetImage();
                if (data_image != null)
                {
                    text_clicked = null;
                    tool = Tool.Sticker;
                    StickerIdSelected = -1;
                    StickerPastedImage = (Image)data_image.Clone();
                    Size size = new Size((int)(StickerPastedImage.Width * StickerSize), (int)(StickerPastedImage.Height * StickerSize));
                    PanelActivity.Cursor = new Cursor((new Bitmap(StickerPastedImage, size)).GetHicon());
                }
            }


            if (Segments.Count > 0 && tool == Tool.Activity)
            {
                if (ClickedSegment != null)
                    ClickedSegment.OnEdition = false;
                if (e.KeyCode == Keys.Left)
                {
                    if (ClickedSegment == null)
                        ClickedSegment = Segments[0];
                    else
                        if (Segments.IndexOf(ClickedSegment) > 0)
                        ClickedSegment = Segments[Segments.IndexOf(ClickedSegment) - 1];
                }
                if (e.KeyCode == Keys.Right)
                {
                    if (ClickedSegment == null)
                        ClickedSegment = Segments[Segments.Count - 1];
                    else
                        if (Segments.IndexOf(ClickedSegment) < Segments.Count - 1)
                        ClickedSegment = Segments[Segments.IndexOf(ClickedSegment) + 1];
                }
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                {
                    ClickedSegment.OnEdition = true;
                    SelectedSegment = ClickedSegment;
                }
            }


            AltKeyHolding = e.Alt;
            PanelActivity.AutoScroll = !AltKeyHolding;

            List<EditableText> texts = new List<EditableText>(Texts);
            foreach (EditableText text in texts)
                text.Edition(e);
        }
        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            AltKeyHolding = false;
            PanelActivity.AutoScroll = true;
        }
        internal void DeleteText(EditableText Text)
        {
            Texts.Remove(Text);
        }

        private void SetImageFromTType()
        {
            ActivityPB_Image = GetImageFromTType();
            ActivityPB.Image = new Bitmap(ActivityPB_Image);
        }
        private Bitmap GetImageFromTType()
        {
            switch (TType)
            {
                default:
                case TemplateType.Undefined:
                    throw new Exception("Template Type not defined");
                case TemplateType.Day:
                    return new Bitmap(Properties.Resources.TemplateDay);
                case TemplateType.TwoDays:
                    return new Bitmap(Properties.Resources.Template2Days);
                case TemplateType.Week:
                    return new Bitmap(Properties.Resources.TemplateWeek);
                case TemplateType.Custom:
                    return new Bitmap(Properties.Resources.TemplateCustom);
            }
        }

        public static Point GetOffsetFromTType(int day)
        {
            int offsetX = 0, offsetY = Hotspots.GetYFromDay(day);
            switch (Instance.TType)
            {
                default:
                case TemplateType.Undefined:
                    throw new Exception("Template Type not defined");
                case TemplateType.Day:
                    offsetX = Hotspots.HSDay.OffsetX;
                    break;
                case TemplateType.TwoDays:
                    offsetX = Hotspots.HSTwoDays.OffsetX;
                    break;
                case TemplateType.Week:
                    offsetX = Hotspots.HSWeek.OffsetX;
                    break;
                case TemplateType.Custom:
                    offsetX = Hotspots.HSCustom.OffsetX;
                    break;
            }
            return new Point(offsetX, offsetY);
        }
        private bool DayBoundsContainsMouse()
        {
            return GetBoundsFromTType(Hotspots.GetDayFromPixels(ActivityPB.PointToClient(MousePosition).Y)).Contains(ActivityPB.PointToClient(MousePosition));
        }

        private void Draw(object sender, EventArgs e)
        {
            Bitmap img = new Bitmap(ActivityPB_Image);
            using (Graphics g = Graphics.FromImage(img))
            {
                Segment SegmentOnEdition = null;// the segment on edition should be drawn in last (for edition cubes not overloaded graphically)

                foreach (Segment segment in Segments)
                {
                    if (segment.OnEdition)
                        SegmentOnEdition = segment;
                    else
                        segment.Draw(g);
                }

                if (CreatingSegment)
                {
                    GetActivityBlocFromMouse().Draw(g);
                }

                SelectedSegment?.Draw(g, true);

                SegmentOnEdition?.Draw(g);// should be the last segment drawn

                CustomTempalte_AddButton?.Draw(g);
                CustomTempalte_RemoveButton?.Draw(g);
            }

            ActivityPB.Image = img;

            DrawLayer_Draw();
        }
        private void DrawLayer_Draw()
        {
            Bitmap img = new Bitmap(DrawLayer.Size.Width, DrawLayer.Size.Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                g.DrawImage(DrawLayer_Content, 0, 0);

                foreach (EditableText text in Texts)
                    text.Draw(g);

                if (tool == Tool.Draw)
                {
                    float radius = draw_size / 2F;
                    Point Mouse = ActivityPB.PointToClient(MousePosition);
                    g.DrawEllipse(Pens.LightGray, Mouse.X - radius, Mouse.Y - radius, draw_size, draw_size);
                }

                if (TemporaryObjectToDraw != null)
                    g.DrawImage(TemporaryObjectToDraw, 0, 0);
            }
            DrawLayer.Image = img;
        }

        private Segment GetActivityBlocFromMouse()
        {
            int day = Hotspots.GetDayFromPixels(ActivityPB.PointToClient(MousePosition).Y);
            TimeSpan Start = Segment.PixelsToTime(MouseDownPosition.X, day);
            TimeSpan End = Segment.PixelsToTime(ActivityPB.PointToClient(MousePosition).X, day);

            SetStartIfEndBefore(ref Start, ref End);
            ManageOverlap(ref Start, ref End);

            return new Segment(Start, End);
        }
        private void AddSegment(Segment segment, bool MakeACopy = false)
        {
            if (segment.Duration == TimeSpan.Zero)
                return;

            var seg = MakeACopy ? new Segment(segment.Start, segment.End, segment.Type) : segment;
            Segments.Add(seg);

            SortSegmentsIndexes();

            if(!MakeACopy)
                ManageRestCreationBetweenSegments(seg);
        }
        private void RemoveSegment(Segment segment)
        {
            if (!Segments.Contains(segment))
                return;

            int SegIndex = Segments.IndexOf(segment);
            Segments.Remove(segment);

            if (SegIndex == 0 || SegIndex - 1 /*because removed, index shifted << */ == Segments.Count - 1)// destroying first / last segment : now can have rest at first/last, so remove them
            {
                if (Segments.Count > 0)
                {
                    if (Segments[0].Type == Segment.ActivityType.Rest)
                        Segments.RemoveAt(0);
                    if (Segments[Segments.Count - 1].Type == Segment.ActivityType.Rest)
                        Segments.RemoveAt(Segments.Count - 1);
                }
            }
            else // destroying any middle segment, now can have prev/next segs as rest, so concat them
            {
                int prev = SegIndex - 1;
                int next = SegIndex; // because removed in Segments, so shifted to <<

                List<Segment> RestsToConcat = new List<Segment>();

                // prev
                if (prev >= 0 && Segments[prev].Type == Segment.ActivityType.Rest)
                    RestsToConcat.Add(Segments[prev]);
                // curr
                RestsToConcat.Add(new Segment(segment.Start, segment.End, Segment.ActivityType.Rest));
                // next
                if (next < Segments.Count && Segments[next].Type == Segment.ActivityType.Rest)
                    RestsToConcat.Add(Segments[next]);

                if(RestsToConcat.Count > 1) // this is the case, so work on
                {
                    foreach (Segment seg in RestsToConcat)
                        if(Segments.Contains(seg)) // the curr seg isn't on Segments
                            Segments.Remove(seg);
                    TimeSpan start = RestsToConcat[0].Start;
                    TimeSpan end = RestsToConcat[RestsToConcat.Count - 1].End;
                    Segments.Insert(prev, new Segment(start, end, Segment.ActivityType.Rest));
                }
            }

            SortSegmentsIndexes();
        }
        private void RemoveSegmentsOnDay(int day)
        {
            List<Segment> SegmentsToRemove = Segments.Where(x => x.Start.Days == day).ToList();
            foreach (Segment segment in SegmentsToRemove)
                Segments.Remove(segment);
        }
        private void SortSegmentsIndexes()
        {
            Segments.Sort((A, B) => A.Start > B.Start ? 1 : -1);
        }
        private void ManageRestCreationBetweenSegments(Segment CurrentSegment)
        {
            // Goal : we'll create rest segments between this segment and : (A) the previous, (B) the next


            // get prev. and next segments

            int CurSegIndex = Segments.IndexOf(CurrentSegment);
            int PrevSegIndex = CurSegIndex - 1;
            int NextSegIndex = CurSegIndex + 1;


            // create rest segments

            if (PrevSegIndex >= 0)// (A)
            {
                Segment RestSegment = GetSegmentRest(Segments[PrevSegIndex].End.Add(new TimeSpan(0, 1, 0)));

                if (RestSegment.Duration > TimeSpan.Zero)
                {
                    Segments.Insert(CurSegIndex, RestSegment);
                    NextSegIndex++;
                }
            }

            if (NextSegIndex < Segments.Count)// (B)
            {
                Segment RestSegment = GetSegmentRest(Segments[NextSegIndex].Start.Subtract(new TimeSpan(0, 1, 0)));

                if (RestSegment.Duration > TimeSpan.Zero)
                    Segments.Insert(NextSegIndex, RestSegment);
            }
        }
        private void ManageRestsAroundSegment(Segment segment)
        {
            // Goal : merge rests before and after segment


            // get prev and next segments not rest

            int CurSegIndex = Segments.IndexOf(segment);
            if (CurSegIndex == -1)
                return;
            int PrevSegIndex = -1;
            int NextSegIndex = -1;
            for (int i = CurSegIndex - 1; i >= 0 && PrevSegIndex == -1; --i)
                if (Segments[i].Type != Segment.ActivityType.Rest)
                    PrevSegIndex = i;
            for (int i = CurSegIndex + 1; i < Segments.Count && NextSegIndex == -1; ++i)
                if (Segments[i].Type != Segment.ActivityType.Rest)
                    NextSegIndex = i;


            // working on prev

            if (PrevSegIndex != -1)
            {
                if (PrevSegIndex < CurSegIndex - 1)
                {
                    int count = CurSegIndex - PrevSegIndex - 1;
                    for (int i = 0; i < count; i++)
                        Segments.RemoveAt(PrevSegIndex + 1);

                    CurSegIndex -= count;
                    NextSegIndex -= count;
                    if (Segments[PrevSegIndex].End < Segments[CurSegIndex].Start)
                    {
                        Segments.Insert(PrevSegIndex, new Segment(Segments[PrevSegIndex].End, Segments[CurSegIndex].Start, Segment.ActivityType.Rest));
                        CurSegIndex += count;
                        NextSegIndex += count;
                    }
                }
                else if (CurSegIndex > 0 && Segments[PrevSegIndex].End < Segments[CurSegIndex].Start)
                    Segments.Insert(PrevSegIndex, new Segment(Segments[PrevSegIndex].End, Segments[CurSegIndex].Start, Segment.ActivityType.Rest));
            }

            // working on next

            if (NextSegIndex != -1)
            {
                if (NextSegIndex > CurSegIndex + 1)
                {
                    int count = NextSegIndex - CurSegIndex - 1;
                    for (int i = 0; i < count; i++)
                        Segments.RemoveAt(NextSegIndex - 1);

                    NextSegIndex -= count;

                    if (CurSegIndex - 1 >= 0 && Segments[CurSegIndex].End < Segments[NextSegIndex].Start)
                        Segments.Insert(CurSegIndex - 1, new Segment(Segments[CurSegIndex].End, Segments[NextSegIndex].Start, Segment.ActivityType.Rest));
                }
                else if (CurSegIndex < Segments.Count - 1 && Segments[CurSegIndex].End < Segments[NextSegIndex].Start)
                    Segments.Insert(CurSegIndex + 1, new Segment(Segments[CurSegIndex].End, Segments[NextSegIndex].Start, Segment.ActivityType.Rest));
            }

            SortSegmentsIndexes();
        }
        private void SetStartIfEndBefore(ref TimeSpan Start, ref TimeSpan End)
        {
            int W = Segment.TimeToPixels(End - Start);

            if (W < 0)
            {
                TimeSpan Temp = Start;
                Start = End;
                End = Temp;
            }


            int StartDay = Hotspots.GetDayFromPixels(MouseDownPosition.Y);

            if (Start.Hours < 0) Start = new TimeSpan(StartDay, 0, 0, 0);
            if (Start.Days < 1) Start = new TimeSpan(1, 0, 0, 0);
            if (Start.Days > 7) Start = new TimeSpan(7, 0, 0, 0);

            if (End.Days < StartDay) End = new TimeSpan(StartDay, Start.Hours, Start.Minutes, 0);
            if (End.Days > StartDay) End = new TimeSpan(StartDay, 23, 59, 0);
        }
        private void ManageOverlap(ref TimeSpan Start, ref TimeSpan End, Segment SegmentToExclude = null)
        {
            // get start pixel and end pixel

            int day = Start.Days;
            int StartPixel = Segment.TimeToPixels(Start);
            int EndPixel = Segment.TimeToPixels(End);

            // get overlaped segments

            List<Segment> AllSegmentsOnOverlap = new List<Segment>();
            for (int pixel = StartPixel; pixel < EndPixel; pixel++)
            {
                Segment SegmentOnOverlap = GetSegmentAtPixel(pixel, day, SegmentToExclude);
                if (SegmentOnOverlap == SegmentToExclude)
                    continue;
                if (SegmentOnOverlap != null && SegmentOnOverlap.Type != Segment.ActivityType.Rest)
                    if(!AllSegmentsOnOverlap.Contains(SegmentOnOverlap))
                        AllSegmentsOnOverlap.Add(SegmentOnOverlap);
            }

            // no overlap -> return

            if (AllSegmentsOnOverlap.Count == 0)
                    goto FinestDetail;

            // -- if only one overlaped segment

            if (AllSegmentsOnOverlap.Count == 1)
            {
                if (ClickedSegment != null && ClickedSegment.OnEdition)
                {
                    if (ClickedSegment.LeftSideEdition)
                        Start = AllSegmentsOnOverlap[0].End.Add(new TimeSpan(0, 1, 0));
                    else
                        End = AllSegmentsOnOverlap[0].Start.Subtract(new TimeSpan(0, 1, 0));
                }
                else
                {
                    if (!Segment.PixelsCompare(StartPixel, MouseDownPosition.X))
                        Start = AllSegmentsOnOverlap[0].End.Add(new TimeSpan(0, 1, 0));
                    else
                        End = AllSegmentsOnOverlap[0].Start.Subtract(new TimeSpan(0, 1, 0)); ;
                }
            }

            else

            // -- else (multiple overlaped segments)

            {

                // we'll treat only the moving side of the segment, not the fixed one

                if (!Segment.PixelsCompare(StartPixel, MouseDownPosition.X))
                {
                    // -- The moving one is the Start --

                    // all ends of overlaped segs

                    List<TimeSpan> AllOSEnds = new List<TimeSpan>();

                    // loop onto all Overlaped Segemnts (OS) in order to fill the both lists instantiated above

                    foreach (Segment os in AllSegmentsOnOverlap)
                    {
                        if (Start < os.End)
                        {
                            AllOSEnds.Add(os.End);
                        }
                    }

                    TimeSpan ClosestEnd = TimeSpan.Zero;// relevant to the current Start

                    foreach (TimeSpan OSEnd in AllOSEnds)// Go for the Ends
                    {
                        if (OSEnd > ClosestEnd)
                            ClosestEnd = OSEnd;
                    }

                    if (AllOSEnds.Count == 0)// no End side of OS seg where to limit the Start -> return
                        goto FinestDetail;

                    Start = ClosestEnd.Subtract(new TimeSpan(0, 1, 0)); ;// we limit the Sart to the closest OS End side
                }
                else
                {
                    // -- The moving one is the End --

                    // all starts of overlaped segs

                    List<TimeSpan> AllOSStarts = new List<TimeSpan>();

                    // loop onto all Overlaped Segemnts (OS) in order to fill the both lists instantiated above

                    foreach (Segment os in AllSegmentsOnOverlap)
                    {
                        if (os.Start < End)
                        {
                            AllOSStarts.Add(os.Start);
                        }
                    }

                    TimeSpan ClosestStart = new TimeSpan(7, 23, 59, 0);// relevant to the current End

                    foreach (TimeSpan OSStart in AllOSStarts)// Go for the Starts
                    {
                        if (OSStart < ClosestStart)
                            ClosestStart = OSStart;
                    }

                    if (AllOSStarts.Count == 0)// no End side of OS seg where to limit the Start -> return
                        goto FinestDetail;

                    End = ClosestStart.Add(new TimeSpan(0, 1, 0)); ;// we limit the End to the closest OS Start side
                }
            }



            FinestDetail:
            for(int i=0; i< Segments.Count; i++)
            {
                /*if (i > 0 && Segments[i].Start < Segments[i - 1].End)
                    Segments[i].Start = Segments[i - 1].End;

                if (i + 1 < Segments.Count && Segments[i].End > Segments[i + 1].Start)
                    Segments[i].End = Segments[i + 1].Start;*/

                
                if (i > 0 && Segments[i - 1].Type != Segment.ActivityType.Rest)
                    if (Segments[i].Start < Segments[i - 1].End)
                        Segments[i].Start = Segments[i - 1].End;

                if (i + 1 < Segments.Count && Segments[i + 1].Type != Segment.ActivityType.Rest)
                    if (Segments[i].End > Segments[i + 1].Start)
                        Segments[i].End = Segments[i + 1].Start;
                
            }
        }
        private Rectangle GetBoundsFromTType(int day)
        {
            Console.WriteLine(day);
            switch (TType)
            {
                default:
                case TemplateType.Undefined:
                    return Rectangle.Empty;
                case TemplateType.Day:
                    return Hotspots.HSDay.Bounds;
                case TemplateType.TwoDays:
                    return Hotspots.HSTwoDays.GetBounds(day);
                case TemplateType.Week:
                    return Hotspots.HSWeek.GetBounds(day);
                case TemplateType.Custom:
                    return Hotspots.HSCustom.GetBounds(day);
            }
        }
        private Segment GetSegmentAtPixel(int pixel, int day, Segment SegmentToExclude = null)
        {
            foreach (Segment segment in Segments)
                if(segment != SegmentToExclude)
                    if (segment.Contains(pixel, day))
                        return segment;

            return null;
        }
        private Segment GetSegmentAtTime(TimeSpan time)
        {
            foreach (Segment segment in Segments)
                if (segment.Contains(time))
                    return segment;
            return null;
        }
        private Segment GetSegmentRest(int pixel, int day)
        {
            TimeSpan Start = TimeSpan.Zero;
            TimeSpan End = new TimeSpan(day, 23, 59, 0);
            foreach (Segment seg in Segments)
            {
                if (seg.Start.Days != day)
                    continue;

                // left side

                if (seg.EndPixel < pixel)
                    if (Start < seg.End)
                        Start = seg.End;

                // right side

                if (seg.StartPixel >= pixel)
                    if (seg.Start <= End)
                        End = seg.Start;
            }

            Start = new TimeSpan(day, Start.Hours, Start.Minutes, 0);
            End = new TimeSpan(day, End.Hours, End.Minutes, 0);
            return new Segment(Start, End, Segment.ActivityType.Rest);
        }
        private Segment GetSegmentRest(TimeSpan time)
        {
            return GetSegmentRest(Segment.TimeToPixels(time), time.Days);
        }
        
        private void CompleteInfos_TimesActivities()
        {
            List<Segment> SegmentsOfTheDay = GetSegmentsOfDay(Hotspots.GetDayFromPixels(ActivityPB.PointToClient(MousePosition).Y));
            var result = GetTotalActivitiesTimesFromSegments(SegmentsOfTheDay);

            tsTimes.Items["tslbDriving"].Text = result.Driving.ToString(TimeFormat);
            tsTimes.Items["tslbWork"].Text = result.Work.ToString(TimeFormat);
            tsTimes.Items["tslbAvailability"].Text = result.Availability.ToString(TimeFormat);
            tsTimes.Items["tslbServiceTimes"].Text = result.ServiceTimes.ToString(TimeFormat);
            tsTimes.Items["tslbRest"].Text = result.Resting.ToString(TimeFormat);

            lbDriving.Text = tsTimes.Items["tslbDriving"].Text;
            lbWork.Text = tsTimes.Items["tslbWork"].Text;
            lbAvailability.Text = tsTimes.Items["tslbAvailability"].Text;
            lbServiceTimes.Text = tsTimes.Items["tslbServiceTimes"].Text;
            lbRest.Text = tsTimes.Items["tslbRest"].Text;
        }
        private void CompleteInfos_Times(Segment segment)
        {
            lbStart.Text = "Start : " + (segment == null ? "-" : segment.Start.ToString(TimeFormat));
            lbEnd.Text = "End : " + (segment == null ? "-" : segment.End.ToString(TimeFormat));
            lbDuration.Text = "Duration : " + (segment == null ? "-" : segment.Duration.ToString(TimeFormat));
            lbDay.Text = "Day : " + (segment == null ? "-" : segment.Start.Days.ToString());

            lbSelectedDriving.Text = segment == null ? "00:00" : (segment.Type == Segment.ActivityType.Drive ? lbDuration.Text.Replace("Duration : ", "") : "00:00");
            lbSelectedWork.Text = segment == null ? "00:00" : (segment.Type == Segment.ActivityType.Work ? lbDuration.Text.Replace("Duration : ", "") : "00:00");
            lbSelectedAvailability.Text = segment == null ? "00:00" : (segment.Type == Segment.ActivityType.Available ? lbDuration.Text.Replace("Duration : ", "") : "00:00");
            lbSelectedServiceTimes.Text = segment == null ? "00:00" : (segment.Type != Segment.ActivityType.Rest && segment.Type != Segment.ActivityType.Undefined ? lbDuration.Text.Replace("Duration : ", "") : "00:00");
            lbSelectedRest.Text = segment == null ? "00:00" : (segment.Type == Segment.ActivityType.Rest ? lbDuration.Text.Replace("Duration : ", "") : "00:00");
        }
        private List<Segment> GetSegmentsOfDay(int day)
        {
            List<Segment> result = new List<Segment>();
            foreach(Segment seg in Segments)
            {
                if (seg.Start.Days == day)
                    result.Add(seg);
            }
            return result;
        }
        
        private (TimeSpan Driving, TimeSpan Work, TimeSpan Availability, TimeSpan Resting, TimeSpan ServiceTimes) GetTotalActivitiesTimesFromSegments(List<Segment> segments)
        {
            TimeSpan Driving, Work, Availability, Rest, ServiceTimes;
            Driving = Work = Availability = ServiceTimes = TimeSpan.Zero;
            Rest = new TimeSpan(23, 59, 0);

            foreach (Segment seg in segments)
            {
                switch(seg.Type)
                {
                    case Segment.ActivityType.Drive: Driving = Driving.Add(seg.Duration); break;
                    case Segment.ActivityType.Work: Work = Work.Add(seg.Duration); break;
                    case Segment.ActivityType.Available: Availability = Availability.Add(seg.Duration); break;
                }
                ServiceTimes = ServiceTimes.Add(seg.Duration);
                Rest = Rest.Subtract(ServiceTimes);
            }

            return (Driving, Work, Availability, Rest, ServiceTimes);
        }

        #endregion



    }
}
