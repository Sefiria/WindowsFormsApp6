using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    public class Inventory
    {
        public static IEnumerable<int> Collectibles = DB.GetValues<Objets>().Concat(DB.GetValues<Consommables>()).Concat(DB.GetValues<Structures>());

        public Entity Owner;
        public long Gils;
        public List<Item> Items;
        public List<Tool> Tools;
        public Inventory() { }
        public Inventory(Entity owner)
        {
            Owner = owner;
            Gils = 0;
            Items = new List<Item>();
            Tools = new List<Tool>();
        }

        public bool Contains(Objets item) => Items?.ContainsItem((int)item) ?? false;
        public bool Contains(Outils tool) => Tools?.ContainsItem((int)tool) ?? false;
        public bool Contains(int dbref)
        {
            if (Collectibles.Contains(dbref)) return Items?.ContainsItem(dbref) ?? false;
            if (dbref.Is<Outils>()) return Contains((Outils)dbref);
            return false;
        }

        //public void Add(params Item[] items) => _addItem(items.ToList().Select(x => (x.DBItem, x.Count)).ToArray());
        //public void Add(params Tool[] tools) => _addTool(tools.ToList().Select(x => (x.DBItem, x.Count)).ToArray());
        public void Add(params Item[] items)
        {
            foreach (var item in items)
            {
                var id = Items.FindIndex(i => i.Name == item.Name);
                if (id > -1)
                    Items[id].Count += item.Count;
                else
                    Items.Add(item);
                NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"+ {item.Name} x {item.Count}", DB.Colors[typeof(Objets)]);
            }
        }
        public void Add(params Tool[] tools)
        {
            foreach (var tool in tools)
            {
                var id = Tools.FindIndex(i => i.Name == tool.Name);
                if (id > -1)
                    Tools[id].Count += tool.Count;
                else
                    Tools.Add(tool);
                NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $" +{tool.Name} x {tool.Count}", DB.Colors[typeof(Outils)]);
            }
        }
        public void Add(params (int id, int count)[] dbrefs)
        {
            foreach (var dbref in dbrefs)
            {
                if (Collectibles.Contains(dbref.id))
                {
                    _addItem((dbref.id, dbref.count));
                    _addItemNotif((dbref.id, dbref.count));
                }
                else if (dbref.id.Is<Outils>())
                {
                    _addTool((dbref.id, dbref.count));
                    _addToolNotif((dbref.id, dbref.count));
                }
            }
        }
        public void Add(params int[] dbrefs) => Add(dbrefs.Select(i => (i, 1)).ToArray());
        private void _addItem(params (int dbref, int count)[] refs)
        {
            refs.ToList().ForEach(item => _addItem(item));
            _addItemNotif(refs);
        }
        private void _addItem((int dbref, int count) itemref)
        {
            if (Items?.ContainsItem(itemref.dbref) ?? false)
                Items[Items.IndexOf(Items.First(i => i.DBRef == itemref.dbref))].Count++;
            else
                Items?.AddRange(new[] { new Item(DB.DefineName(itemref.dbref), itemref.dbref, itemref.count) });
        }
        private void _addTool(params (int dbref, int count)[] refs)
        {
            refs.ToList().ForEach(tool => _addTool(tool));
            _addToolNotif(refs);
        }
        private void _addTool((int dbref, int count) toolref)
        {
            if (Contains((Outils)toolref.dbref))
                Tools[Tools.IndexOf(Tools.First(i => i.DBRef == toolref.dbref))].Count += toolref.count;
            else
                Tools?.AddRange(new[] { new Tool(Enum.GetName(typeof(Outils), toolref.dbref), (Outils)toolref.dbref) { Count = toolref.count } });
        }

        public void Remove(params Item[] items) => Items?.AddRange(items);
        public void Remove(params Tool[] tools) => Tools?.AddRange(tools);
        public void RemoveAll(params int[] dbrefs)
        {
            foreach (int dbref in dbrefs)
            {
                if (Items?.ContainsItem(dbref) ?? false)
                {
                    var item = Items.FirstOrDefault(it => (int)it.DBRef == dbref);
                    if(item != null)
                        Items.Remove(item);
                }
                else if (dbref.Is<Outils>())
                {
                    var tool = Tools.FirstOrDefault(t => (int)t.DBRef == dbref);
                    if (tool != null)
                        Tools.Remove(tool);
                }
            }
        }
        public void RemoveOne(params int[] dbrefs)
        {
            foreach (int dbref in dbrefs)
            {
                if (Items?.ContainsItem(dbref) ?? false)
                {
                    var item = Items.FirstOrDefault(it => (int)it.DBRef == dbref);
                    if (item != null)
                    {
                        Items[Items.IndexOf(item)].Count--;
                        if(Items[Items.IndexOf(item)].Count == 0)
                            Items.Remove(item);
                    }
                }
                else if (dbref.Is<Outils>())
                {
                    var tool = Tools.FirstOrDefault(t => (int)t.DBRef == dbref);
                    if (tool != null)
                    {
                        Tools[Tools.IndexOf(tool)].Count--;
                        if (Tools[Tools.IndexOf(tool)].Count == 0)
                            Tools.Remove(tool);
                    }
                }
            }
        }

        private void _addItemNotif(params (int dbref, int count)[] refs)
        {
            if (Owner == Core.Instance.TheGuy)
            {
                foreach (var item in refs)
                {
                    string item_name = Items[Items.IndexOf(Items.First(i => i.DBRef == item.dbref))].Name;
                    NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"+ {item_name} x {item.count}", DB.Colors[typeof(Objets)]);
                }
            }
        }
        private void _addToolNotif(params (int dbref, int count)[] refs)
        {
            if (Owner == Core.Instance.TheGuy)
            {
                foreach (var tool in refs)
                {
                    string tool_name = Tools[Tools.IndexOf(Tools.First(i => i.DBRef == tool.dbref))].Name;
                    NotificationsManager.AddNotification(NotificationsManager.NotificationTypes.SideLeft, $"+ {tool_name} x {tool.count}", DB.Colors[typeof(Outils)]);
                }
            }
        }

        public (string name, int dbref) GetObjectInfosByUniqueId(Guid id)
        {
            int dbref = -1;
            string name = "";
            Item item = null;
            Tool tool = Tools.FirstOrDefault(t => t.UniqueId == id);
            if (tool != null)
            {
                dbref = (int)tool.DBRef;
                name = tool.Name;
            }
            else
            {
                item = Items.FirstOrDefault(it => it.UniqueId == id);
                if (item != null)
                {
                    dbref = (int)item.DBRef;
                    name = item.Name;
                }
            }
            return (name, dbref);
        }
        public int GetDBRefByUniqueId(Guid id)
        {
            int dbref = -1;
            Item item = null;
            Tool tool = Tools.FirstOrDefault(t => t.UniqueId == id);
            if (tool != null)
                dbref = (int)tool.DBRef;
            else
            {
                item = Items.FirstOrDefault(it => it.UniqueId == id);
                if (item != null)
                    dbref = (int)item.DBRef;
            }
            return dbref;
        }
        public string GetNameByUniqueId(Guid id)
        {
            string name = null;
            Item item = null;
            Tool tool = Tools.FirstOrDefault(t => t.UniqueId == id);
            if (tool != null)
                name = tool.Name;
            else
            {
                item = Items.FirstOrDefault(it => it.UniqueId == id);
                if (item != null)
                    name = item.Name;
            }
            return name;
        }
        public int GetCountByUniqueId(Guid id)
        {
            int count = 0;
            Item item = null;
            Tool tool = Tools.FirstOrDefault(t => t.UniqueId == id);
            if (tool != null)
                count = tool.Count;
            else
            {
                item = Items.FirstOrDefault(it => it.UniqueId == id);
                if (item != null)
                    count = item.Count;
            }
            return count;
        }

        internal (string name, int dbref, int count, Guid content) GetFullInfosByUniqueId(Guid id)
        {
            int dbref = -1;
            string name = "";
            int count = 0;
            Guid content = Guid.Empty;
            Item item = null;
            Tool tool = Tools.FirstOrDefault(t => t.UniqueId == id);
            if (tool != null)
            {
                dbref = (int)tool.DBRef;
                name = tool.Name;
                count = tool.Count;
                content = tool.UniqueId;
            }
            else
            {
                item = Items.FirstOrDefault(it => it.UniqueId == id);
                if (item != null)
                {
                    dbref = (int)item.DBRef;
                    name = item.Name;
                    count = item.Count;
                    content = item.UniqueId;
                }
            }
            return (name, dbref, count, content);
        }
    }
}
