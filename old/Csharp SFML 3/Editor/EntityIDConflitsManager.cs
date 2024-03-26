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
using static Editor.ComboBoxIconed;
using static Editor.LevelEditor;
using static Framework.GlobalVariables;

namespace Editor
{
    public partial class EntityIDConflitsManager : Form
    {
        public class ListBoxSortedItem
        {
            public TreeNode node;
            public EntityProperties entity;
            public ListBoxSortedItem(TreeNode _node, EntityProperties _entity) { node = _node; entity = _entity; }
            public override string ToString() { return node.Text; }
        }

        List<TreeNode> conflicts = new List<TreeNode>();
        bool lockEvent_numID = false;


        public EntityIDConflitsManager()
        {
            InitializeComponent();

            InitializeTree();

            FormClosed += delegate { Dispose(); };
        }

        private void InitializeTree()
        {
            tree.Nodes.Clear();
            listSorted.Items.Clear();
            List<EntityProperties> entities = Tools.GetAllEntitiesAnUninstantiables();

            foreach(var entity in entities)
            {
                string[] paths = entity.EntityPath.Split('/').Skip(1).ToArray();
                paths = paths.Take(paths.Length - 1).ToArray();
                TreeNode node, current = null;
                foreach(var path in paths)
                {
                    node = new TreeNode(path);
                    node.Name = node.Text;

                    if (paths[0] == path)
                    {
                        if (tree.Nodes.Find(path, false).Count() == 0)
                        {
                            tree.Nodes.Add(node);
                        }
                        else
                        {
                            current = tree.Nodes.Find(path, false).First();
                            continue;
                        }
                    }
                    else
                    {
                        if (current.Nodes.Find(path, false).Count() == 0)
                        {
                            current.Nodes.Add(node);
                        }
                        else
                        {
                            current = current.Nodes.Find(path, false).First();
                            continue;
                        }
                    }

                    current = node;
                }

                node = new TreeNode(entity.Name);
                node.Name = node.Text;
                node.Tag = (entity.layer, entity.ID);
                current.Nodes.Add(node);

                listSorted.Items.Add(new ListBoxSortedItem(node, entity));
            }

            tree.ExpandAll();
            AnalyseConflicts();
        }

        private void btReloadEntities_Click(object sender, EventArgs e)
        {
            InitializeTree();
        }
        private void btUnselect_Click(object sender, EventArgs e)
        {
            tree.SelectedNode = null;
            listSorted.ClearSelected();
        }
        private void tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tree.SelectedNode.Nodes.Count > 0)
                tree.SelectedNode = null;

            if (tree.SelectedNode == null)
            {
                tbName.Text = "";
                numLayer.Value = 0;
                lockEvent_numID = true; numID.Value = 0; lockEvent_numID = false;
                numLayer.Enabled = false;
                numID.Enabled = false;
            }
            else
            {
                tbName.Text = tree.SelectedNode.Text;
                (byte layer, byte id) value = ((byte, byte)) tree.SelectedNode.Tag;
                numLayer.Value = value.layer;
                numID.Value = value.id;
                numLayer.Enabled = true;
                numID.Enabled = true;

                if (tree.SelectedNode.Nodes.Count == 0)
                    if(listSorted.SelectedIndex != listSorted.FindStringExact(tbName.Text));
                        listSorted.SelectedIndex = listSorted.FindStringExact(tbName.Text);
            }
        }
        private void rbtTree_CheckedChanged(object sender, EventArgs e)
        {
            tree.Visible = true;
            listSorted.Visible = false;
        }
        private void rbtSortedListLayer_CheckedChanged(object sender, EventArgs e)
        {
            tree.Visible = false;

            var selectedNode = listSorted.SelectedItem;
            var list = listSorted.Items.Cast<ListBoxSortedItem>().OrderBy(item => (((byte layer, byte ID))item.node.Tag).layer).ToList();
            listSorted.Items.Clear();
            foreach (var item in list)
                listSorted.Items.Add(item);
            listSorted.SelectedItem = selectedNode;

            listSorted.Visible = true;
        }
        private void rbtSortedListID_CheckedChanged(object sender, EventArgs e)
        {
            tree.Visible = false;

            var selectedNode = listSorted.SelectedItem;
            var list = listSorted.Items.Cast<ListBoxSortedItem>().OrderBy(item => (((byte layer, byte ID))item.node.Tag).ID).ToList();
            listSorted.Items.Clear();
            foreach (var item in list)
                listSorted.Items.Add(item);
            listSorted.SelectedItem = selectedNode;

            listSorted.Visible = true;
        }
        private void listSorted_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listSorted.SelectedItem == null)
            {
                tbName.Text = "";
                numLayer.Value = 0;
                lockEvent_numID = true; numID.Value = 0; lockEvent_numID = false;
                numLayer.Enabled = false;
                numID.Enabled = false;
            }
            else
            {
                tbName.Text = (listSorted.SelectedItem as ListBoxSortedItem).node.Text;
                (byte layer, byte id) = ((byte, byte))(listSorted.SelectedItem as ListBoxSortedItem).node.Tag;
                numLayer.Value = layer;
                numID.Value = id;
                numLayer.Enabled = true;
                numID.Enabled = true;

                var list = new List<TreeNode>();
                void FillList(TreeNode parent)
                {
                    if (parent.Nodes.Count == 0)
                        list.Add(parent);
                    else
                        foreach (TreeNode node in parent.Nodes)
                            FillList(node);
                }
                foreach (TreeNode node in tree.Nodes)
                    FillList(node);
                var nodeToSelect = list.First(x => x.Text == tbName.Text && (((byte layer, byte id))x.Tag).layer == layer && (((byte layer, byte id))x.Tag).id == id);
                if(tree.SelectedNode != nodeToSelect)
                    tree.SelectedNode = nodeToSelect;
            }

            listSorted.Refresh();
        }
        private void AnalyseConflicts()
        {
            conflicts.Clear();
            foreach(ListBoxSortedItem first in listSorted.Items)
            {
                foreach (ListBoxSortedItem second in listSorted.Items)
                {
                    if (first.node == second.node)
                        continue;

                    if ((((byte layer, byte ID))first.node.Tag).ID == (((byte layer, byte ID))second.node.Tag).ID)
                    {
                        if (!conflicts.Contains(first.node))
                            conflicts.Add(first.node);
                        if (!conflicts.Contains(second.node))
                            conflicts.Add(second.node);
                    }
                }

                if (!conflicts.Contains(first.node))
                    if((((byte layer, byte ID))first.node.Tag).ID == 0)
                        conflicts.Add(first.node);
            }

            tree.Refresh();
            listSorted.Refresh();
        }
        private void tree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            if (conflicts.Contains(e.Node))
            {
                e.Graphics.FillRectangle(new SolidBrush(tree.SelectedNode == null ? Color.Yellow : (tree.SelectedNode == e.Node ? Color.Gold : Color.Yellow)), e.Bounds);
                e.Graphics.DrawString(e.Node.Text, new Font("Arial", 8.75F, FontStyle.Bold), new SolidBrush(Color.Red), e.Bounds.Location);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(tree.SelectedNode == null ? Color.White : (tree.SelectedNode == e.Node ? Color.LightSkyBlue : Color.White)), e.Bounds);
                e.Graphics.DrawString(e.Node.Text, new Font("Arial", 8.75F, FontStyle.Regular), new SolidBrush(Color.Black), e.Bounds.Location);
            }
        }
        private void listSorted_DrawItem(object sender, DrawItemEventArgs e)
        {
            var selectedNode = listSorted.SelectedIndex == -1 ? null : (listSorted.Items[listSorted.SelectedIndex] as ListBoxSortedItem).node;
            var node = (listSorted.Items[e.Index] as ListBoxSortedItem).node;
            if (conflicts.Contains(node))
            {
                e.Graphics.FillRectangle(new SolidBrush(selectedNode == null ? Color.Yellow : (selectedNode == node ? Color.Gold : Color.Yellow)), e.Bounds);
                e.Graphics.DrawString(node.Text, new Font("Arial", 8.75F, FontStyle.Bold), new SolidBrush(Color.Red), e.Bounds.Location);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(selectedNode == null ? Color.White : (selectedNode == node ? Color.LightSkyBlue : Color.White)), e.Bounds);
                e.Graphics.DrawString(node.Text, new Font("Arial", 8.75F, FontStyle.Regular), new SolidBrush(Color.Black), e.Bounds.Location);
            }
        }
        private void numID_ValueChanged(object sender, EventArgs e)
        {
            if (lockEvent_numID)
                return;

            if(rbtTree.Checked && tree.SelectedNode != null)
            {
                (byte layer, byte ID) = ((byte, byte)) tree.SelectedNode.Tag;
                tree.SelectedNode.Tag = (layer, (byte)numID.Value);
            }
            else
            {
                if (listSorted.SelectedItem == null)
                    return;

                (byte layer, byte ID) = ((byte, byte))(listSorted.SelectedItem as ListBoxSortedItem).node.Tag;
                (listSorted.SelectedItem as ListBoxSortedItem).node.Tag = (layer, (byte)numID.Value);
            }

            AnalyseConflicts();
        }

        private void btSaveIDs_Click(object sender, EventArgs e)
        {
            foreach (ListBoxSortedItem item in listSorted.Items)
            {
                (byte layer, byte ID) = ((byte, byte))item.node.Tag;
                item.entity.layer = layer;
                item.entity.ID = ID;
                item.entity.Save();
            }
            MessageBox.Show("Save complete !", "Save");
        }

        private void btFind_Click(object sender, EventArgs e)
        {
            foreach (ListBoxSortedItem item in listSorted.Items)
            {
                (byte layer, byte ID) = ((byte, byte))item.node.Tag;
                if(layer == (byte)numSearchLayer.Value && ID == (byte)numSearchID.Value)
                {
                    lockEvent_numID = true; listSorted.SelectedItem = item; lockEvent_numID = false;
                    return;
                }
            }

            MessageBox.Show("Entity [" + numSearchLayer.Value + "," + numSearchID.Value + "] not found.", "Search");
        }
    }
}
