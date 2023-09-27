using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using static System.Environment;

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
            MessageBox.Show(this, "Please select the Factorio mod folder to open");
            var dialog = new FolderBrowserDialog();
            dialog.RootFolder = SpecialFolder.ApplicationData;
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                var path = dialog.SelectedPath;
                bool CheckFolder(string subdir) { if (!Directory.Exists(Path.Combine(path, subdir))) { MessageBox.Show(this, $"Missing root folder '{subdir}'{Environment.NewLine}Path: {path}"); return false; } return true; }
                bool CheckFile(string subdir, string filename) { var p = subdir != "" ? Path.Combine(path, subdir, filename) : Path.Combine(path, filename); if (!File.Exists(p)) { MessageBox.Show(this, $"Missing file '{filename}' in '{subdir}'{Environment.NewLine}Path: {path}"); return false; } return true; }
                if (!CheckFolder("graphics")) return;
                if (!CheckFolder("locale")) return;
                if (!CheckFolder("prototypes")) return;
                if (!CheckFile("", "info.json")) return;
                var errors = Data.Load(path);
                if (errors.Count == 0)
                    MessageBox.Show(this, "Successfully Loaded");
                else
                {
                    string err = "";
                    foreach (var kv in errors)
                        err += $"{kv.Key} : {kv.Value}" + Environment.NewLine;
                    MessageBox.Show(this, $"Loading failed due to one or more issues :{Environment.NewLine}{err}");
                }
            }
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Please select Factorio mods folder");
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
                MessageBox.Show(this, "Successfully Saved");
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
        private void btTechs_Click(object sender, EventArgs e)
        {
            GoPrototypes(Data.Technologies);
        }
        private void GoPrototypes<T>(List<T> data) where T : Prototype
        {
            using (var form = new FormPrototype<T>(data))
                form.ShowDialog(this);
        }
    }
}
