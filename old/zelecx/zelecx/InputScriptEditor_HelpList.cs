using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Script.InputScript;

namespace Script
{
    public partial class InputScriptEditor_HelpList : Form
    {
        private ListBox ListFonctions, ListConditions, ListSubjects, ListOperators, ListAssignators, ListComparators, ListModifiers;
        Action<string, KeywordType> Callback;
        Func<KeywordType, Color> CallbackGetColorFromKWType;
        Form PublicVariablesForm = null, PropertiesForm = null;

        public InputScriptEditor_HelpList(Func<KeywordType, Color> _CallbackGetColorFromKWType, Action<string, KeywordType> _Callback)
        {
            InitializeComponent();

            CallbackGetColorFromKWType = _CallbackGetColorFromKWType;
            Callback = _Callback;

            var PageFonctions = new TabPage("Fonctions    ");
            var PageConditions = new TabPage("Conditions    ");
            var PageSubjects = new TabPage("Subjects    ");
            var PageOperators = new TabPage("Operators    ");
            var PageAssignators = new TabPage("Assignators    ");
            var PageComparators = new TabPage("Comparators    ");
            var PageModifiers = new TabPage("Modifier    ");
            
            ListFonctions = new ListBox();
            ListConditions = new ListBox();
            ListSubjects = new ListBox();
            ListOperators = new ListBox();
            ListAssignators = new ListBox();
            ListComparators = new ListBox();
            ListModifiers = new ListBox();

            foreach (var fonction in InputScript.Functions)
                ListFonctions.Items.Add(fonction);
            foreach (var condition in InputScript.Conditions)
                ListConditions.Items.Add(condition);
            foreach (var subject in InputScript.Subjects)
                ListSubjects.Items.Add(subject);
            foreach (var Operator in InputScript.Operators)
                ListOperators.Items.Add(Operator);
            foreach (var Assignator in InputScript.Assignators)
                ListAssignators.Items.Add(Assignator);
            foreach (var comparator in InputScript.Comparators)
                ListComparators.Items.Add(comparator);
            foreach (var modifier in InputScript.Modifiers)
                ListModifiers.Items.Add(modifier);

            ListFonctions.Dock = DockStyle.Fill;
            ListConditions.Dock = DockStyle.Fill;
            ListSubjects.Dock = DockStyle.Fill;
            ListOperators.Dock = DockStyle.Fill;
            ListAssignators.Dock = DockStyle.Fill;
            ListComparators.Dock = DockStyle.Fill;
            ListModifiers.Dock = DockStyle.Fill;

            ListFonctions.SelectedIndexChanged += ListFonctions_SelectedIndexChanged;
            ListConditions.SelectedIndexChanged += ListConditions_SelectedIndexChanged;
            ListSubjects.SelectedIndexChanged += ListSubjects_SelectedIndexChanged;
            ListOperators.SelectedIndexChanged += ListOperators_SelectedIndexChanged;
            ListAssignators.SelectedIndexChanged += ListAssignators_SelectedIndexChanged;
            ListComparators.SelectedIndexChanged += ListComparators_SelectedIndexChanged;
            ListModifiers.SelectedIndexChanged += ListModifiers_SelectedIndexChanged;

            ListSubjects.SelectedIndexChanged += ListSubjects_SelectedIndexChanged;
            ListFonctions.MouseDoubleClick += ListFonctions_MouseDoubleClick;
            ListConditions.MouseDoubleClick += ListConditions_MouseDoubleClick;
            ListSubjects.MouseDoubleClick += ListSubjects_MouseDoubleClick;
            ListOperators.MouseDoubleClick += ListOperators_MouseDoubleClick;
            ListAssignators.MouseDoubleClick += ListAssignators_MouseDoubleClick;
            ListComparators.MouseDoubleClick += ListComparators_MouseDoubleClick;
            ListModifiers.MouseDoubleClick += ListModifiers_MouseDoubleClick;

            PageFonctions.Controls.Add(ListFonctions);
            PageConditions.Controls.Add(ListConditions);
            PageSubjects.Controls.Add(ListSubjects);
            PageOperators.Controls.Add(ListOperators);
            PageAssignators.Controls.Add(ListAssignators);
            PageComparators.Controls.Add(ListComparators);
            PageModifiers.Controls.Add(ListModifiers);
            
            PageFonctions.Controls[0].BackColor = CallbackGetColorFromKWType(KeywordType.Function);
            PageConditions.Controls[0].BackColor = CallbackGetColorFromKWType(KeywordType.Condition);
            PageSubjects.Controls[0].BackColor = CallbackGetColorFromKWType(KeywordType.Subject);
            PageOperators.Controls[0].BackColor = CallbackGetColorFromKWType(KeywordType.Operator);
            PageAssignators.Controls[0].BackColor = CallbackGetColorFromKWType(KeywordType.Assignator);
            PageComparators.Controls[0].BackColor = CallbackGetColorFromKWType(KeywordType.Comparator);
            PageModifiers.Controls[0].BackColor = CallbackGetColorFromKWType(KeywordType.Modifier);

            tabControl.TabPages.Add(PageFonctions);
            tabControl.TabPages.Add(PageConditions);
            tabControl.TabPages.Add(PageSubjects);
            tabControl.TabPages.Add(PageOperators);
            tabControl.TabPages.Add(PageAssignators);
            tabControl.TabPages.Add(PageComparators);
            tabControl.TabPages.Add(PageModifiers);

            tabControl.DrawItem += delegate (object sender, DrawItemEventArgs e) {
                e.Graphics.FillRectangle(new SolidBrush(tabControl.TabPages[e.Index].Controls[0].BackColor), e.Bounds);
                Rectangle paddedBounds = e.Bounds;
                paddedBounds.Inflate(-2, -2);
                e.Graphics.DrawString(tabControl.TabPages[e.Index].Text, new Font(Font, FontStyle.Bold), Brushes.Black, paddedBounds);
            };
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
        }

        private void ListFonctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ListFonctions.SelectedItem == null)
            {
                rtbDescription.Text = "";
                return;
            }

            rtbDescription.Text = InputScript.GetFunctions().First(x => x.Key == ListFonctions.SelectedItem.ToString()).Value.Description;
        }
        private void ListConditions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
            {
                rtbDescription.Text = "";
                return;
            }

            rtbDescription.Text = InputScript.GetConditions().First(x => x.Key == ListConditions.SelectedItem.ToString()).Value;
        }
        private void ListSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
            {
                rtbDescription.Text = "";
                return;
            }

            rtbDescription.Text = InputScript.GetSubjects().First(x => x.Key == ListSubjects.SelectedItem.ToString()).Value;
        }
        private void ListOperators_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
            {
                rtbDescription.Text = "";
                return;
            }

            rtbDescription.Text = InputScript.GetOperators().First(x => x.Key == ListOperators.SelectedItem.ToString()).Value;
        }
        private void ListAssignators_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
            {
                rtbDescription.Text = "";
                return;
            }

            rtbDescription.Text = InputScript.GetAssignators().First(x => x.Key == ListAssignators.SelectedItem.ToString()).Value;
        }
        private void ListComparators_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
            {
                rtbDescription.Text = "";
                return;
            }

            rtbDescription.Text = InputScript.GetComparators().First(x => x.Key == ListComparators.SelectedItem.ToString()).Value;
        }
        private void ListModifiers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == -1)
            {
                rtbDescription.Text = "";
                return;
            }

            rtbDescription.Text = InputScript.GetModifiers().First(x => x.Key == ListModifiers.SelectedItem.ToString()).Value;
        }

        private void ListFonctions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ListFonctions.IndexFromPoint(e.X, e.Y) == -1)
                return;

            Callback(ListFonctions.SelectedItem.ToString(), KeywordType.Function);
        }
        private void ListConditions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ListConditions.IndexFromPoint(e.X, e.Y) == -1)
                return;

            Callback(ListConditions.SelectedItem.ToString(), KeywordType.Condition);
        }
        private void ListSubjects_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ListSubjects.IndexFromPoint(e.X, e.Y) == -1)
                return;

            Callback(ListSubjects.SelectedItem.ToString(), KeywordType.Subject);
        }
        private void ListOperators_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ListOperators.IndexFromPoint(e.X, e.Y) == -1)
                return;

            Callback(ListOperators.SelectedItem.ToString(), KeywordType.Operator);
        }
        private void ListAssignators_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ListAssignators.IndexFromPoint(e.X, e.Y) == -1)
                return;

            Callback(ListAssignators.SelectedItem.ToString(), KeywordType.Assignator);
        }
        private void ListComparators_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ListComparators.IndexFromPoint(e.X, e.Y) == -1)
                return;

            Callback(ListComparators.SelectedItem.ToString(), KeywordType.Comparator);
        }
        private void ListModifiers_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ListModifiers.IndexFromPoint(e.X, e.Y) == -1)
                return;

            Callback(ListModifiers.SelectedItem.ToString(), KeywordType.Modifier);
        }

        private void btPublicVariables_Click(object sender, EventArgs e)
        {
            if (PublicVariablesForm != null)
            {
                PublicVariablesForm.Close();
                PublicVariablesForm = null;
                return;
            }

            PublicVariablesForm = new Form();
            PublicVariablesForm.Text = "Public Variables";
            PublicVariablesForm.MinimizeBox = PublicVariablesForm.MaximizeBox = false;
            PublicVariablesForm.ClientSize = new Size(100, 200);
            var list = new ListBox();
            list.Location = new Point(0, 0);
            list.Dock = DockStyle.Fill;
            PublicVariablesForm.Controls.Add(list);

            var PublicVariables = GetPublicVariables();
            foreach (var publicVariable in PublicVariables)
                list.Items.Add(publicVariable);

            PublicVariablesForm.Show();
        }
        private void btProperties_Click(object sender, EventArgs e)
        {
            if (PropertiesForm != null)
            {
                PropertiesForm.Close();
                PropertiesForm = null;
                return;
            }

            PropertiesForm = new Form();
            PropertiesForm.Text = "Properties";
            PropertiesForm.MinimizeBox = PropertiesForm.MaximizeBox = false;
            PropertiesForm.ClientSize = new Size(100, 200);
            var list = new ListBox();
            list.Location = new Point(0, 0);
            list.Dock = DockStyle.Fill;
            PropertiesForm.Controls.Add(list);

            var Properties = GetProperties();
            foreach (var Property in Properties)
                list.Items.Add(Property);

            PropertiesForm.Show();
        }
    }
}
