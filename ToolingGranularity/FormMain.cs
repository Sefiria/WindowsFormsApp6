using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ToolingGranularity
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Reload();
        }

        private void Reload()
        {
            float md_margin = 8f;
            list.Clear();
            list.LargeImageList = new ImageList();
            list.LargeImageList.ImageSize = new Size(64, 64);
            var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory()).Where(file => Common.ImageExtensions.Contains(Path.GetExtension(file).ToUpper()));
            foreach(var file in files)
            {
                var name = Path.GetFileNameWithoutExtension(file);
                var img = Image.FromFile(file);
                var item = new ListViewItem();

                if (File.Exists(Path.Combine(Directory.GetParent(file).FullName, $"{name}.{Common.COLLIDER_FILE_EXTENSION}")))
                    using (var g = Graphics.FromImage(img))
                        g.DrawRectangle(new Pen(Color.IndianRed, md_margin), 0, 0, img.Width - md_margin, img.Height);
                list.LargeImageList.Images.Add(name, img);
                item.ImageKey = name;
                item.Text = name;
                item.Name = Path.GetFileName(file);
                list.Items.Add(item);
            }
        }

        private void list_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var menu = new ContextMenuStrip();
            menu.ShowImageMargin = false;
            menu.Items.Add("Refresh").Click += (_, _e) => Reload();
            menu.Show(MousePosition);
        }

        private void list_DoubleClick(object sender, EventArgs e)
        {
            if(list.SelectedItems.Count > 0)
            {
                new ToolRects(list.SelectedItems[0].Name).Show(this);
            }
        }
    }
}
