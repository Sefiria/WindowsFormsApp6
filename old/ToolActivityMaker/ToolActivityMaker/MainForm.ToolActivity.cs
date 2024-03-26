using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolActivityMaker
{
    partial class MainForm
    {
        private void ToolActivity_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {

                case MouseButtons.Left:

                    if (ClickedSegment != null)
                    {
                        if (ClickedSegment.EditCubeLeft.Contains(e.Location))
                        {
                            ClickedSegment.LeftSideEdition = true;
                            return;
                        }
                        else if (ClickedSegment.EditCubeRight.Contains(e.Location))
                        {
                            ClickedSegment.RightSideEdition = true;
                            return;
                        }
                    }

                    // Unclick the ClickedSegment

                    if (ClickedSegment != null)
                    {
                        ClickedSegment.OnEdition = false;
                        ClickedSegment.LeftSideEdition = false;
                        ClickedSegment.RightSideEdition = false;
                    }
                    ClickedSegment = null;

                    // if no segment under mouse, we create it

                    Segment SegmentUnderMouse = GetSegmentAtPixel(e.X, Hotspots.GetDayFromPixels(e.Y));
                    if (SegmentUnderMouse == null)
                    {
                        MouseHolding = CreatingSegment = true;
                        MouseDownPosition = e.Location;
                    }

                    else

                    // else we click on it (ClickedSegment)

                    {

                        // but only after cheking the Y coord (contained in the segment bounds or not, so)

                        if (SegmentUnderMouse.Contains(e.Location))
                        {
                            ClickedSegment = SegmentUnderMouse;
                            ClickedSegment.OnEdition = true;
                        }
                        else
                        {
                            AwaitingSegmentToBeCuttedByNewSegment = (Segments.IndexOf(SegmentUnderMouse), SegmentUnderMouse);
                            RemoveSegment(SegmentUnderMouse);
                            MouseHolding = CreatingSegment = true;
                            MouseDownPosition = e.Location;
                        }
                    }

                    break;


                case MouseButtons.Middle:

                    if (SelectedSegment != null)
                    {
                        // if Selected is a Rest Segment retrieved which aren't in the Segment List (not created, only selectable), we add it

                        if (!Segments.Contains(SelectedSegment))
                            AddSegment(SelectedSegment);

                        // we change the activity type of the segment

                        SelectedSegment.Type = (Segment.ActivityType)((int)SelectedSegment.Type + 1);
                        if ((int)SelectedSegment.Type >= Enum.GetValues(typeof(Segment.ActivityType)).Length)
                            SelectedSegment.Type = (Segment.ActivityType)1;

                        // in the case where the new type is Rest, we simply remove the segment

                        if (SelectedSegment.Type == Segment.ActivityType.Rest)
                        {
                            RemoveSegment(SelectedSegment);
                        }
                    }

                    break;


                case MouseButtons.Right:

                    Segment segment = SelectedSegment;
                    if (segment == null)
                    {
                        if(ClickedSegment == null)
                            break;

                        segment = ClickedSegment;
                        if (!ClickedSegment.EditCubeLeft.Contains(e.Location) && !ClickedSegment.EditCubeRight.Contains(e.Location))
                            break;
                    }

                    ToolStripMenuItem Item_Type_Driving = new ToolStripMenuItem("Driving", Properties.Resources.driving, delegate { segment.Type = Segment.ActivityType.Drive; });
                    ToolStripMenuItem Item_Type_Work = new ToolStripMenuItem("Work", Properties.Resources.work, delegate { segment.Type = Segment.ActivityType.Work; });
                    ToolStripMenuItem Item_Type_Availability = new ToolStripMenuItem("Availability", Properties.Resources.availability, delegate { segment.Type = Segment.ActivityType.Available; });
                    ToolStripMenuItem Item_Type_Rest = new ToolStripMenuItem("Rest", Properties.Resources.rest, delegate { RemoveSegment(segment); segment = null; });
                    ToolStripMenuItem Item_Type_Undefined = new ToolStripMenuItem("Undefined", Properties.Resources.undefinedactivity, delegate { segment.Type = Segment.ActivityType.Undefined; });
                    ToolStripMenuItem Item_Type = new ToolStripMenuItem("Set Type →", Properties.Resources.ts, new[] { Item_Type_Driving, Item_Type_Work, Item_Type_Availability, Item_Type_Rest, Item_Type_Undefined });

                    DateTimePicker Item_Start_Edit = new DateTimePicker();
                    Item_Start_Edit.Format = DateTimePickerFormat.Custom;
                    Item_Start_Edit.CustomFormat = "HH:mm";
                    Item_Start_Edit.Value = new DateTime(2000, 1, segment.Start.Days, segment.Start.Hours, segment.Start.Minutes, 0);
                    Item_Start_Edit.ShowUpDown = true;
                    Item_Start_Edit.Size = new Size(50, Item_Start_Edit.Height);
                    Item_Start_Edit.ValueChanged += delegate {
                        segment.Start = new TimeSpan(segment.Start.Days, Item_Start_Edit.Value.Hour, Item_Start_Edit.Value.Minute, 0);
                        SetStartIfEndBefore(ref segment.Start, ref segment.End);
                        ManageOverlap(ref segment.Start, ref segment.End, segment);
                        SortSegmentsIndexes();
                        CompleteInfos_Times(segment);
                    };

                    DateTimePicker Item_End_Edit = new DateTimePicker();
                    Item_End_Edit.Format = DateTimePickerFormat.Custom;
                    Item_End_Edit.CustomFormat = "HH:mm";
                    Item_End_Edit.Value = new DateTime(2000, 1, segment.End.Days, segment.End.Hours, segment.End.Minutes, 0);
                    Item_End_Edit.ShowUpDown = true;
                    Item_End_Edit.Size = new Size(50, Item_End_Edit.Height);
                    Item_End_Edit.ValueChanged += delegate {
                        segment.End = new TimeSpan(segment.End.Days, Item_End_Edit.Value.Hour, Item_End_Edit.Value.Minute, 0);
                        SetStartIfEndBefore(ref segment.Start, ref segment.End);
                        ManageOverlap(ref segment.Start, ref segment.End, segment);
                        SortSegmentsIndexes();
                        CompleteInfos_Times(segment);
                    };

                    ToolStripMenuItem Item_Start = new ToolStripMenuItem("Start →", Properties.Resources.Start, new ToolStripControlHost(Item_Start_Edit));
                    ToolStripMenuItem Item_End = new ToolStripMenuItem("End →", Properties.Resources.End, new ToolStripControlHost(Item_End_Edit));
                    ToolStripMenuItem Item_Remove = new ToolStripMenuItem("Remove", Properties.Resources.remove);
                    Item_Remove.Click += delegate { RemoveSegment(segment); segment = null; };

                    ToolStripDropDown dropdown = new ToolStripDropDown();
                    dropdown.LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow;
                    dropdown.Items.Add(Item_Type);
                    dropdown.Items.Add(new ToolStripSeparator());
                    dropdown.Items.Add(Item_Start);
                    dropdown.Items.Add(Item_End);
                    dropdown.Items.Add(new ToolStripSeparator());
                    dropdown.Items.Add(Item_Remove);

                    dropdown.Show(MousePosition);

                    break;
            }
        }
        private void ToolActivity_MouseUp(object sender, MouseEventArgs e)
        {
            if (MouseHolding && CreatingSegment)
            {
                Segment segment = GetActivityBlocFromMouse();
                if (segment.Duration == TimeSpan.Zero && AwaitingSegmentToBeCuttedByNewSegment.Index != -1 && AwaitingSegmentToBeCuttedByNewSegment.Segment != null)
                {
                    int id = AwaitingSegmentToBeCuttedByNewSegment.Index >= Segments.Count ? Segments.Count - 1 : AwaitingSegmentToBeCuttedByNewSegment.Index;
                    if(Segments.Count == 0)
                        Segments.Add(AwaitingSegmentToBeCuttedByNewSegment.Segment);
                    else
                        Segments.Insert(id, AwaitingSegmentToBeCuttedByNewSegment.Segment);
                    SortSegmentsIndexes();
                }
                else
                    AddSegment(segment);
                AwaitingSegmentToBeCuttedByNewSegment = (-1, null);
            }
            MouseHolding = CreatingSegment = false;
            if (ClickedSegment != null)
            {
                ClickedSegment.LeftSideEdition = false;
                ClickedSegment.RightSideEdition = false;
            }
        }
        private void ToolActivity_MouseLeave()
        {
            MouseHolding = false;
            if (AwaitingSegmentToBeCuttedByNewSegment.Index != -1 && AwaitingSegmentToBeCuttedByNewSegment.Segment != null)
            {
                Segments.Insert(AwaitingSegmentToBeCuttedByNewSegment.Index, AwaitingSegmentToBeCuttedByNewSegment.Segment);
                AwaitingSegmentToBeCuttedByNewSegment = (-1, null);
            }
        }
        private void ToolActivity_MouseMove(object sender, MouseEventArgs e)
        {
            CompleteInfos_TimesActivities();

            if (MouseHolding)
            {
                if (MouseHolding)
                {
                    if (!new Rectangle(new Point(3, 3), ActivityPB_Image.Size).Contains(e.Location))
                        ToolActivity_MouseLeave();
                    return;
                }
            }
            else
            {
                if (ClickedSegment != null)
                {
                    bool EditSegmentLeftSide_NoModifDone = false;
                    bool EditSegmentRightSide_NoModifDone = false;
                    if (ClickedSegment.LeftSideEdition)
                    {
                        TimeSpan NewStart = Segment.PixelsToTime(e.X, Hotspots.GetDayFromPixels(e.Y));
                        if (NewStart < ClickedSegment.End.Subtract(new TimeSpan(0,1,0)))
                            ClickedSegment.Start = NewStart;
                        else
                            EditSegmentLeftSide_NoModifDone = true;
                    }
                    if (ClickedSegment.RightSideEdition)
                    {
                        TimeSpan NewEnd = Segment.PixelsToTime(e.X, Hotspots.GetDayFromPixels(e.Y));
                        if (NewEnd > ClickedSegment.Start.Add(new TimeSpan(0, 1, 0)))
                            ClickedSegment.End = NewEnd;
                        else
                            EditSegmentRightSide_NoModifDone = true;
                    }
                    if ((ClickedSegment.LeftSideEdition && !EditSegmentLeftSide_NoModifDone) || (ClickedSegment.RightSideEdition && !EditSegmentRightSide_NoModifDone))
                    {
                        SetStartIfEndBefore(ref ClickedSegment.Start, ref ClickedSegment.End);
                        ManageOverlap(ref ClickedSegment.Start, ref ClickedSegment.End, ClickedSegment);
                        SortSegmentsIndexes();
                        ManageRestsAroundSegment(ClickedSegment);
                        CompleteInfos_Times(ClickedSegment);
                        return;
                    }
                }


                SelectedSegment = GetSegmentAtPixel(e.X, Hotspots.GetDayFromPixels(e.Y));
                if (SelectedSegment == null)// if there's no segment, we get a created rest on the void place
                    SelectedSegment = GetSegmentRest(e.X, Hotspots.GetDayFromPixels(e.Y));
                if (SelectedSegment != null && !SelectedSegment.Contains(e.Location))// if SelectedSegment doesn't contains mouse, it becomes null
                    SelectedSegment = null;
                CompleteInfos_Times(SelectedSegment);
                return;
            }
        }
    }
}
