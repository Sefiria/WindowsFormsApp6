using Framework;
using Play;
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

namespace Editor
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void btEntityEditor_Click(object sender, EventArgs e)
        {
            new EntityEditor().ShowDialog();
        }

        private void btLevelEditor_Click(object sender, EventArgs e)
        {
            new LevelEditor().ShowDialog();
        }

        private void btEntityIDConflictsManager_Click(object sender, EventArgs e)
        {
            new EntityIDConflitsManager().ShowDialog();
        }

        private void btBehaviorEditor_Click(object sender, EventArgs e)
        {
            var imglist = new ImageList();
            var filesNames = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "\\" + "Entities/Entity/Organic/UnPlayable/Buildable/Behavior").ToList();
            var entities = new List<EntityProperties>();
            foreach (var entityPath in filesNames)
                entities.Add(EntityProperties.Load(entityPath));

            void callback(EntityProperties entity)
            {
                var form = new BehaviorEdit.BehaviorEditor(ref entity);
                form.ShowDialog();
            }

            var entityLoader = new EntityLoader(entities, callback);
            entityLoader.ShowDialog();
        }

        private void btPlayTest_Click(object sender, EventArgs e)
        {
            MainForm.GetPlayMainForm().ShowDialog();
        }
    }
}
