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

namespace Defacto_rio
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btCreate_Click(object sender, EventArgs e)
        {
            var form = new FormCreate();
            if (form.ShowDialog(this) == DialogResult.OK)
            {
                lbProjectName.Text = form.tbName.Text;
            }
        }
        private void btOpen_Click(object sender, EventArgs e)
        {

        }
        private void btSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please select Factorio mods folder");
            var dialog = new FolderBrowserDialog();
            dialog.RootFolder = Environment.SpecialFolder.ApplicationData;
            if(dialog.ShowDialog(this) == DialogResult.OK)
            {
                var path = Path.Combine(dialog.SelectedPath, Data.Project.name);
                bool full_creation = false;
                if (!Directory.Exists(path))
                {
                    full_creation = true;
                    Directory.CreateDirectory(path);
                }

                void CreateFolderIfNotExist(string _path)
                {
                    if (full_creation || !Directory.Exists(_path)) Directory.CreateDirectory(_path);
                }
                void CreateFileIfNotExist(string _path, string content)
                {
                    if (full_creation || !File.Exists(_path)) File.WriteAllText(_path, content);
                }

                CreateFolderIfNotExist(Path.Combine(path, "graphics"));
                CreateFolderIfNotExist(Path.Combine(path, "locale"));
                CreateFolderIfNotExist(Path.Combine(path, "prototypes"));
                CreateFileIfNotExist(Path.Combine(path, "info.json"), Data.Project.ToJson());
                //CreateFileIfNotExist(Path.Combine(path, "data.lua"), Data.CreateDataLua());

                Data.CreatePrototypesFiles(Path.Combine(path, "prototypes"));
            }
        }

        private void btGroups_Click(object sender, EventArgs e)
        {
            GoPrototypes(Data.Groups);
        }
        private void btSubGroups_Click(object sender, EventArgs e)
        {
            GoPrototypes(Data.SubGroups);
        }
        private void btItems_Click(object sender, EventArgs e)
        {
            GoPrototypes(Data.Items);
        }
        private void btRecipes_Click(object sender, EventArgs e)
        {
            GoPrototypes(Data.Recipes);
        }
        private void GoPrototypes<T>(List<T> data) where T : Prototype
        {
            using (var form = new FormPrototype<T>(data))
                form.ShowDialog(this);
        }
    }
}
