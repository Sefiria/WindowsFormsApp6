using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Properties;

namespace WindowsFormsApp2.Entities
{
    public class Chest : DrawableEntity, IActionable
    {
        private static Bitmap ImageOpen = Resources.chest_open, ImageClosed = Resources.chest_closed;
        public bool IsOpen = false;
        public Dictionary<Item, int> Content;

        public Chest(Dictionary<Item, int> Content, int TX, int TY)
        {
            this.TX = TX;
            this.TY = TY;
            Image = ImageClosed;
            this.Content = Content;
        }

        public void Action()
        {
            Open();
        }

        public void Open()
        {
            if (IsOpen)
                return;

            IsOpen = true;
            Image = ImageOpen;
            SharedData.Player.AddItems(Content, Logger.LogInventoryAddFrom.Chest);
            Content.Clear();
        }
        public override void Update()
        {
        }
    }
}
