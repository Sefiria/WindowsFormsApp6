using Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using sfColor = SFML.Graphics.Color;
using sfImage = SFML.Graphics.Image;

namespace Editor
{
    public partial class EntityLoader : Form
    {
        public EntityLoader(List<EntityProperties> entities, Action<EntityProperties> callback)
        {
            InitializeComponent();
            ToolTip toolTip = new ToolTip();
            toolTip.AutomaticDelay = 0;
            toolTip.AutoPopDelay = 99999999;
            toolTip.ReshowDelay = 0;
            foreach (var entity in entities)
            {
                var button = new Button();
                button.Text = "";
                button.BackgroundImage = Tools.SfImageToBitmap(new sfImage(entity.image));
                button.BackgroundImageLayout = ImageLayout.Stretch;
                button.Size = new Size(35, 35);
                toolTip.SetToolTip(button, entity.Name);
                button.Click += delegate { callback(entity); Close(); };
                flowLayoutPanel1.Controls.Add(button);
            }
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
