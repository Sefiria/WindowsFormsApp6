using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;

namespace console_v3
{
    internal class NotificationsManager
    {
        public enum NotificationTypes
        {
            Header, SideLeft, Footer, SideRight,
        }
        public class Notification { public NotificationTypes Type; public string Text; public Color Color; public int Duration = 200, Ticks = 0; }

        public static List<Notification> Notifications = new List<Notification>();
        public static Font Font = new Font("Segoe UI", 12f);

        internal static void AddNotification(NotificationTypes type, string text, Color color)
        {
            Notifications.Add(new Notification { Type = type, Text = text, Color = color });
        }

        internal static void Update()
        {
            for(int i=0;i< Notifications.Count;i++)
            {
                Notifications[i].Ticks++;
                if(Notifications[i].Ticks > Notifications[i].Duration)
                    Notifications.RemoveAt(i--);
            }
        }

        internal static void Draw(Graphics gui)
        {
            int h = TextRenderer.MeasureText("A", Font).Height;

            for(int i=0;i< Notifications.Count;i++)
            {
                var notif = Notifications[i];
                switch(notif.Type)
                {
                    // TODO
                    case NotificationTypes.Header:
                    case NotificationTypes.SideRight:
                    case NotificationTypes.Footer:
                    case NotificationTypes.SideLeft: gui.DrawString(notif.Text, Font, new SolidBrush(Color.FromArgb((int)((1f - notif.Ticks / (float)notif.Duration) * byte.MaxValue), notif.Color)), 20, 20 + i * (h + 10)); break;
                }
            }
        }
    }
}
