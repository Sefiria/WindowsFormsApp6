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
        List<Image> StickerImages = new List<Image>()
                {
                    Properties.Resources.driving,
                    Properties.Resources.work,
                    Properties.Resources.availability,
                    Properties.Resources.rest,
                    Properties.Resources.ts,
                    Properties.Resources.undefinedactivity,
                    Properties.Resources.RestSymbol,
                    Properties.Resources.RestHebdoSymbol,
                    Properties.Resources.Start,
                    Properties.Resources.End,
                    Properties.Resources.Cost,
                    Properties.Resources.DC,
                    Properties.Resources.Configs,
                    Properties.Resources.Calendar,
                    Properties.Resources.Clock,
                    Properties.Resources.Len,
                    Properties.Resources.Infringement,
                    Properties.Resources.Export,
                    Properties.Resources.Import,
                    Properties.Resources.Refresh,
                    Properties.Resources.Alert
                };
        float StickerSize = 1F;
        int StickerIdSelected = 0;
        Image StickerPastedImage = null;

        private void ToolSticker_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                using (Graphics g = Graphics.FromImage(DrawLayer_Content))
                {
                    Image img = StickerIdSelected == -1 ? StickerPastedImage : (Image)StickerImages[StickerIdSelected].Clone();
                    img = new Bitmap(img, (int)(StickerSize * img.Width), (int)(StickerSize * img.Height));
                    g.DrawImage(img, e.X - img.Width / 2, e.Y - img.Height / 2);
                }
            }

            else

            if (e.Button == MouseButtons.Right)
            {
                ListView tata = new ListView();
                tata.View = View.Tile;
                tata.Size = new Size(152, 80);
                var imageList = new ImageList();
                imageList.ImageSize = new Size(16, 16);
                imageList.Images.AddRange(StickerImages.ToArray());
                tata.LargeImageList = imageList;
                tata.MultiSelect = false;
                tata.TileSize = new Size(32, 32);

                var listImages = new List<ListViewItem>();
                for (int i = 0; i < StickerImages.Count; i++)
                    listImages.Add(new ListViewItem("", i));
                tata.Items.AddRange(listImages.ToArray());

                ToolStripControlHost toto = new ToolStripControlHost(tata);
                toto.Click += delegate
                {
                    if (tata.SelectedItems.Count == 0)
                        return;
                    StickerIdSelected = tata.SelectedItems[0].ImageIndex;
                    Image img = (Image)StickerImages[StickerIdSelected].Clone();
                    Size size = new Size((int)(img.Width * StickerSize), (int)(img.Height * StickerSize));
                    PanelActivity.Cursor = new Cursor((new Bitmap(img, size)).GetHicon());
                };

                ToolStripDropDown dropdown = new ToolStripDropDown();
                dropdown.Items.Add(toto);

                dropdown.Show(MousePosition);
            }
        }
        private void ToolSticker_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0F)
            {
                StickerSize *= 1.5F;
                if (StickerSize > 5F)
                    StickerSize = 5F;
            }
            else
            {
                StickerSize /= 1.5F;
                if (StickerSize < 0.5F)
                    StickerSize = 0.5F;
            }

            Image img = StickerIdSelected == -1 ? StickerPastedImage : (Image)StickerImages[StickerIdSelected].Clone();
            Size size = new Size((int)(img.Width * StickerSize), (int)(img.Height * StickerSize));
            PanelActivity.Cursor = new Cursor((new Bitmap(img, size)).GetHicon());
        }
    }
}
