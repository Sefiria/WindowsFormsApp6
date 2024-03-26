using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ObjectHierarchyBuilder
{
    public partial class Form1 : Form
    {
        public class ListTypeItem
        {
            public string text, description;
            public Color color, backColor;
            public FontStyle fontStyle;

            public ListTypeItem()
            {
                text = "";
                description = "";
                color = Color.Black;
                backColor = Color.White;
                fontStyle = FontStyle.Regular;
            }
            public ListTypeItem(BinaryReader br)
            {
                Read(br);
            }
            public ListTypeItem(string _text, string _description, Color _color, Color _backColor, FontStyle _fontStyle)
            {
                text = _text;
                description = _description;
                color = _color;
                backColor = _backColor;
                fontStyle = _fontStyle;
            }
            public void Write(BinaryWriter bw)
            {
                bw.Write(text);
                bw.Write(description);
                bw.Write(color.R);
                bw.Write(color.G);
                bw.Write(color.B);
                bw.Write(backColor.R);
                bw.Write(backColor.G);
                bw.Write(backColor.B);
                bw.Write(Enum.GetName(typeof(FontStyle), fontStyle));
            }
            public void Read(BinaryReader br)
            {
                text = br.ReadString();
                description = br.ReadString();
                color = Color.FromArgb(br.ReadByte(), br.ReadByte(), br.ReadByte());
                backColor = Color.FromArgb(br.ReadByte(), br.ReadByte(), br.ReadByte());
                fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), br.ReadString());
            }
            public override string ToString()
            {
                return text;
            }
        }

        private Button lastAddButtonClicked = null;


        public Form1()
        {
            InitializeComponent();

            lastAddButtonClicked = btAddRoot;
            cbbFontStyle.Items.AddRange(Enum.GetValues(typeof(FontStyle)).Cast<object>().ToArray());
            cbbFontStyle.SelectedItem = FontStyle.Regular;
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
        }

        private void btAddRoot_Click(object sender, EventArgs e)
        {
            lastAddButtonClicked = (Button)sender;

            if (string.IsNullOrWhiteSpace(tbName.Text))
                return;
            if (cbbType.SelectedIndex == -1)
                return;

            var node = treeView.Nodes.Add(tbName.Text);
            node.Tag = cbbType.SelectedItem;
            node.EnsureVisible();
            treeView.SelectedNode = node;

            tbName.Text = "";
            tbName.Focus();
        }
        private void btAddChild_Click(object sender, EventArgs e)
        {
            lastAddButtonClicked = (Button)sender;

            if(treeView.SelectedNode == null)
                return;
            if (string.IsNullOrWhiteSpace(tbName.Text))
                return;
            if (cbbType.SelectedIndex == -1)
                return;

            var node = treeView.SelectedNode.Nodes.Add(tbName.Text);
            node.Tag = cbbType.SelectedItem;
            node.EnsureVisible();

            tbName.Text = "";
            tbName.Focus();
        }
        private void btRemoveSelection_Click(object sender, EventArgs e)
        {
            var node = treeView.SelectedNode;

            if (node == null)
                return;

            MoveChildrenToParent(node);

            node.Remove();
        }
        private void btRemoveSubTree_Click(object sender, EventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;

            treeView.SelectedNode.Remove();
        }

        private void UpdateComboBoxType()
        {
            int indexSelected = listType.Items.Count == 0 ? -1 : 0;
            if (listType.Items.Count > 1)
                indexSelected = cbbType.SelectedIndex == -1 ? 0 : cbbType.SelectedIndex;

            cbbType.Items.Clear();
            foreach (ListTypeItem item in listType.Items)
                cbbType.Items.Add(item);
            
            cbbType.SelectedIndex = indexSelected;
        }
        private void btAddType_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbType.Text) || cbbFontStyle.SelectedIndex == -1)
                return;

            listType.Items.Add(new ListTypeItem(tbType.Text, tbDescription.Text, picColor.BackColor, picBackColor.BackColor, (FontStyle)cbbFontStyle.SelectedItem));
            UpdateComboBoxType();
        }
        private void btApply_Click(object sender, EventArgs e)
        {
            if (listType.SelectedIndex == -1)
                return;

            ListTypeItem item = (ListTypeItem)listType.SelectedItem;
            var index = listType.Items.IndexOf(item);
            if (!string.IsNullOrWhiteSpace(tbType.Text))
                item.text = tbType.Text;
            if (!string.IsNullOrWhiteSpace(tbDescription.Text))
                item.description = tbDescription.Text;
            item.color = picColor.BackColor;
            item.backColor = picBackColor.BackColor;
            if(cbbFontStyle.SelectedIndex != -1)
                item.fontStyle = (FontStyle)cbbFontStyle.SelectedItem;
            listType.Items.RemoveAt(index);
            listType.Items.Insert(index, item);

            treeView.Refresh();
        }
        private void btRemoveType_Click(object sender, EventArgs e)
        {
            if (listType.SelectedIndex == -1)
                return;

            listType.Items.Remove(listType.SelectedItem);
            UpdateComboBoxType();
        }
        private void listType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listType.SelectedIndex == -1)
                return;

            tbType.Text = (listType.SelectedItem as ListTypeItem).text;
            tbDescription.Text = (listType.SelectedItem as ListTypeItem).description;
            picColor.BackColor = (listType.SelectedItem as ListTypeItem).color;
            picBackColor.BackColor = (listType.SelectedItem as ListTypeItem).backColor;
            cbbFontStyle.SelectedItem = (listType.SelectedItem as ListTypeItem).fontStyle;
        }
        private void tbName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                lastAddButtonClicked.PerformClick();
        }
        private void picColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
                picColor.BackColor = colorDialog.Color;
        }
        private void picBackColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
                picBackColor.BackColor = colorDialog.Color;
        }
        private void treeView_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (treeView.SelectedNode == null)
                return;

            var node = treeView.SelectedNode;
            var index = node.Index;
            var parent = node.Parent;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    btRemoveSubTree.PerformClick();
                    break;

                case Keys.Left:
                    MoveNodeToParent(node);
                    break;

                case Keys.Up:
                    if (index == 0)
                        break;
                    node.Remove();
                    if (parent == null)
                        treeView.Nodes.Insert(index - 1, node);
                    else
                        parent.Nodes.Insert(index - 1, node);
                    treeView.SelectedNode = node;
                    break;

                case Keys.Down:
                    if (index == (parent == null ? treeView.GetNodeCount(false) : parent.GetNodeCount(false)) - 1)
                        break;
                    node.Remove();
                    if (parent == null)
                        treeView.Nodes.Insert(index + 1, node);
                    else
                        parent.Nodes.Insert(index + 1, node);
                    treeView.SelectedNode = node;
                    break;
            }

        }
        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void treeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            float indent = e.Node.Level * treeView.Indent;
            var item = (ListTypeItem)e.Node.Tag;

            Font font = new Font(treeView.Font.Name, treeView.Font.SizeInPoints, item.fontStyle);
            float width1 = e.Graphics.MeasureString(item.text + " ", font).Width;
            float width2 = e.Graphics.MeasureString(e.Node.Text, font).Width;
            var brushHighlight = new SolidBrush(Color.FromArgb(210, 230, 255));
            var brushBack = new SolidBrush(item.backColor);
            var brushSymbol = new SolidBrush(Color.Black);
            var brush = new SolidBrush(item.color);
            var pen = new Pen(item.backColor);

            e.Graphics.FillRectangle(brushBack, indent + width1 + e.Bounds.X, e.Bounds.Y, width2, e.Bounds.Height);
            if(treeView.SelectedNode == e.Node)
                e.Graphics.DrawRectangle(new Pen(Color.Black, 1F), indent + width1 + e.Bounds.X, e.Bounds.Y, width2 - 1, e.Bounds.Height - 1);

            e.Graphics.DrawString(item.text + " ", font, brushSymbol, indent + e.Bounds.X, e.Bounds.Y);
            e.Graphics.DrawString(e.Node.Text, font, brush, indent + e.Bounds.X + width1, e.Bounds.Y);
        }
        private void btClearTree_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to clear the tree?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                treeView.SelectedNode = null;
                treeView.Nodes.Clear();
                listType.Items.Clear();
                UpdateComboBoxType();

                numIndent.Value = 15;
                tbType.Text = "";
                picColor.BackColor = Color.Black;
                picBackColor.BackColor = Color.White;
                cbbFontStyle.SelectedIndex = 0;
                tbDescription.Text = "";
            }
        }
        private void btSaveTree_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (Stream file = File.Open(saveFileDialog.FileName, FileMode.Create))
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    using (var bw = new BinaryWriter(file))
                    {
                        // Misc

                        bw.Write((int)numIndent.Value);

                        // Types Count

                        bw.Write(listType.Items.Count);

                        // Types

                        foreach (ListTypeItem item in listType.Items)
                            item.Write(bw);

                        // Tree

                        bf.Serialize(file, treeView.Nodes.Cast<TreeNode>().ToList());

                        void RecursiveWriteTag(TreeNode node)
                        {
                            ((ListTypeItem)node.Tag).Write(bw);

                            foreach (TreeNode child in node.Nodes)
                                RecursiveWriteTag(child);
                        }
                        foreach (TreeNode node in treeView.Nodes)
                            RecursiveWriteTag(node);
                    }
                }
            }
        }
        private void btLoadTree_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure to clear the tree?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    treeView.SelectedNode = null;
                    treeView.Nodes.Clear();
                    listType.Items.Clear();

                    numIndent.Value = 15;
                    tbType.Text = "";
                    picColor.BackColor = Color.Black;
                    picBackColor.BackColor = Color.White;
                    cbbFontStyle.SelectedIndex = 0;
                    tbDescription.Text = "";

                    using (Stream file = File.Open(openFileDialog.FileName, FileMode.Open))
                    {
                        BinaryFormatter bf = new BinaryFormatter();

                        using (var br = new BinaryReader(file))
                        {
                            // Misc

                            numIndent.Value = br.ReadInt32();

                            // Types Count

                            int ItemsCount = br.ReadInt32();

                            // Types
                            
                            for (int i = 0; i < ItemsCount; i++)
                                listType.Items.Add(new ListTypeItem(br));

                            // Tree

                            object obj = bf.Deserialize(file);
                            TreeNode[] nodeList = (obj as IEnumerable<TreeNode>).ToArray();
                            treeView.Nodes.AddRange(nodeList);

                            void RecursiveReadTag(TreeNode node)
                            {
                                node.Tag = new ListTypeItem(br);

                                foreach (TreeNode child in node.Nodes)
                                    RecursiveReadTag(child);
                            }
                            foreach (TreeNode node in treeView.Nodes)
                                RecursiveReadTag(node);
                        }
                    }

                    UpdateComboBoxType();
                    treeView.ExpandAll();
                }
            }
        }
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if(e.Node == null)
            {
                tbName.Text = "";
            }
            else
            {
                tbName.Text = e.Node.Text;
                cbbType.SelectedItem = e.Node.Tag;
            }
        }
        private void btApplyNodeModifications_Click(object sender, EventArgs e)
        {
            var node = treeView.SelectedNode;

            if(node != null)
            {
                node.Text = tbName.Text;
                node.Tag = cbbType.SelectedItem;
            }
        }

        private void MoveChildrenToParent(TreeNode node)
        {
            while(node.GetNodeCount(false) > 0)
            {
                if (node.Level == 0)
                    treeView.Nodes.Insert(node.Index, (TreeNode)node.FirstNode.Clone());
                else
                    node.Parent.Nodes.Insert(node.Index, (TreeNode)node.FirstNode.Clone());

                node.FirstNode.Remove();
            }

            treeView.ExpandAll();
        }
        private void MoveNodeToParent(TreeNode node)
        {
            if (node.Level == 0)
                return;

            if (node.Parent.Level == 0)
                treeView.Nodes.Insert(node.Parent.Index, (TreeNode)node.Clone());
            else
                node.Parent.Parent.Nodes.Insert(node.Parent.Index, (TreeNode)node.Clone());

            node.Parent.Nodes.Remove(node);
        }

        private void numIndent_ValueChanged(object sender, EventArgs e)
        {
            treeView.Indent = (int)numIndent.Value;
        }
        private void treeView_MouseDown(object sender, MouseEventArgs e)
        { 
            if (treeView.HitTest(e.X, e.Y).Node == null)
                treeView.SelectedNode = null;
        }
        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var item = (ListTypeItem)e.Node.Tag;
            var g = treeView.CreateGraphics();
            float width = g.MeasureString(item.text + " ", new Font("Arial", (int)numSize.Value, item.fontStyle)).Width;
            var rect = new RectangleF(treeView.Indent * e.Node.Level, e.Node.Bounds.Y, width, e.Node.Bounds.Height);

            if(rect.Contains(e.Location))
            {
                if (e.Node.IsExpanded)
                    e.Node.Collapse();
                else
                    e.Node.Expand();
            }
            else
                treeView.SelectedNode = e.Node;
        }

        private void numSize_ValueChanged(object sender, EventArgs e)
        {
            treeView.Font = new Font("Arial", (int)numSize.Value, FontStyle.Regular);
            treeView.Refresh();
        }

        private void btCreateFolders_Click(object sender, EventArgs e)
        {
            void Recursive(TreeNode parent)
            {
                if(parent.Nodes.Count == 0)
                {
                    Directory.CreateDirectory("Entities\\"+parent.FullPath);
                }
                else
                {
                    foreach (TreeNode node in parent.Nodes)
                        Recursive(node);
                }
            }

            foreach (TreeNode node in treeView.Nodes)
                Recursive(node);

            MessageBox.Show("Done");
        }
    }
}
