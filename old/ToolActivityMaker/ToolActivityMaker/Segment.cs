using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolActivityMaker
{
    [Serializable]
    public class Segment
    {
        static public float PixelsByHour = 12.92F * 4;
        static public int SegmentHeightInPixels = 75;

        public enum ActivityType { Drive = 1, Work, Available, Rest, Undefined }
        public ActivityType Type = ActivityType.Undefined;

        public TimeSpan Start = TimeSpan.Zero, End = TimeSpan.Zero;
        public TimeSpan Duration => new TimeSpan(End.Ticks - Start.Ticks);
        public bool Contains(int pixel, int day)
        {
            TimeSpan time = PixelsToTime(pixel, day);
            return time >= Start && time < End;
        }
        public bool Contains(Point P)
        {
            return Bounds.Contains(P);
        }
        public bool Contains(TimeSpan time)
        {
            return time > Start && time < End;
        }
        public int StartPixel => TimeToPixels(Start);
        public int EndPixel => TimeToPixels(End);
        public Rectangle Bounds
        {
            get
            {
                Point offset = MainForm.GetOffsetFromTType(Start.Days);
                int X = TimeToPixels(Start);
                int Y = offset.Y + 1;
                int W = TimeToPixels(Duration) - offset.X;
                int H = Type == ActivityType.Undefined ? SegmentHeightInPixels : (Type == ActivityType.Rest ? 12 : (int)(SegmentHeightInPixels * ((5F - (int)Type)) * 0.25F));
                return new Rectangle(X, Y - H, W, H);
            }
        }
        public bool OnEdition = false;
        public bool LeftSideEdition = false, RightSideEdition = false;
        public Rectangle EditCubeLeft
        {
            get
            {
                Rectangle bounds = Bounds;
                int BtEditionCubeSizeHalf = 4;
                int BtEditionCubeSize = BtEditionCubeSizeHalf * 2;
                return new Rectangle(bounds.X - BtEditionCubeSizeHalf, bounds.Y + bounds.Height / 2 - BtEditionCubeSizeHalf, BtEditionCubeSize, BtEditionCubeSize);
            }
        }
        public Rectangle EditCubeRight
        {
            get
            {
                Rectangle bounds = Bounds;
                int BtEditionCubeSizeHalf = 4;
                int BtEditionCubeSize = BtEditionCubeSizeHalf * 2;
                return new Rectangle(bounds.X + bounds.Width - BtEditionCubeSizeHalf, bounds.Y + bounds.Height / 2 - BtEditionCubeSizeHalf, BtEditionCubeSize, BtEditionCubeSize);
            }
        }
        
        public Segment(TimeSpan Start, TimeSpan End, ActivityType Type = ActivityType.Undefined)
        {
            this.Start = Start;
            this.End = End;
            this.Type = Type;
        }

        public void Draw(Graphics g, bool IsSelection = false)
        {
            Rectangle bounds = Bounds;
            g.FillRectangle(new SolidBrush(IsSelection || OnEdition ? SelectionColor : Color), bounds);

            if (Type == ActivityType.Undefined)
                g.DrawRectangle(new Pen(Color.Black, 1F), bounds);

            if (OnEdition)
            {
                g.DrawRectangle(new Pen(Color.Black, 2.5F), bounds);
                Pen BlackPen = new Pen(Color.Black, 2F);

                // left
                g.FillRectangle(Brushes.White, EditCubeLeft);
                g.DrawRectangle(BlackPen, EditCubeLeft);
                // Right
                g.FillRectangle(Brushes.White, EditCubeRight);
                g.DrawRectangle(BlackPen, EditCubeRight);
            }
        }

        public Color Color => Type == ActivityType.Drive ? Color.FromArgb(200, 0, 0)
                            : Type == ActivityType.Work ? Color.FromArgb(0, 0, 200)
                            : Type == ActivityType.Available ? Color.FromArgb(200, 200, 0)
                            : Type == ActivityType.Rest ? Color.FromArgb(0, 128, 0)
                            : Color.LightGray;
        public Color SelectionColor
        {
            get
            {
                if (Type == ActivityType.Undefined)
                    return Color.White;

                Color color = Color;
                byte R, G, B;
                byte Enlightenment = 50;

                R = color.R;
                G = color.G;
                B = color.B;

                if (R + Enlightenment <= byte.MaxValue)
                    R += Enlightenment;
                if (G + Enlightenment <= byte.MaxValue)
                    G += Enlightenment;
                if (B + Enlightenment <= byte.MaxValue)
                    B += Enlightenment;

                return Color.FromArgb(R, G, B);
            }
        }


        static public int TimeToPixels(TimeSpan time)
        {
            Point offset = MainForm.GetOffsetFromTType(time.Days);
            return (int)(offset.X + (time.Hours + time.Minutes / 60F) * PixelsByHour);
        }
        static public TimeSpan PixelsToTime(int pixels, int day)
        {
            Point Offset = MainForm.GetOffsetFromTType(day);
            int OffsetPixels = pixels - Offset.X;
            int hours = (int)(OffsetPixels / PixelsByHour);
            int minutes = (int)(OffsetPixels / (float)PixelsByHour * 60) - hours * 60;
            int seconds = 0;
            return new TimeSpan(day, hours, minutes, seconds);
        }
        static public bool PixelsCompare(int P1, int P2)
        {
            return P1 - 1 == P2 || P1 == P2 || P1 + 1 == P2;
        }
    }
}
