﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;
using WindowsFormsApp6.Particules;
using WindowsFormsApp6.UI;
using WindowsFormsApp6.World.Blocs;
using WindowsFormsApp6.World.Blocs.Tool;
using WindowsFormsApp6.World.Ores;
using WindowsFormsApp6.World.Structures;
using WindowsFormsApp6.Properties;
using System.Numerics;

namespace WindowsFormsApp6.World
{
    [Serializable]
    public class StateShop : IState
    {
        public int TileSz => 0;
        [JsonIgnore] public Bitmap StaticImage { get; set; }
        public List<Particule> Particules { get; set; } = new List<Particule>();
        public IState PreviousState { get; set; } = null;
        public List<IUI> UI { get; set; } = new List<IUI>();
        public Dictionary<string, BigInteger> ItemPrice { get; set; } = new Dictionary<string, BigInteger>()
        {
            [Enum.GetName(typeof(OreType), OreType.Bronze)] = 1,
            [Enum.GetName(typeof(OreType), OreType.Silver)] = 2,
            [Enum.GetName(typeof(OreType), OreType.Gold)] = 4,
            [Enum.GetName(typeof(OreType), OreType.Titanium)] = 8,
            [Enum.GetName(typeof(OreType), OreType.Diamond)] = 20,
            ["Bronze Pickaxe"] = 50,
            ["Silver Pickaxe"] = 200,
            ["Gold Pickaxe"] = 600,
            ["Titanium Pickaxe"] = 1000,
            ["Diamond Pickaxe"] = 5000,
            ["Bronze Large Pickaxe"] = 10000,
            ["Silver Large Pickaxe"] = 20000,
            ["Gold Large Pickaxe"] = 50000,
            ["Titanium Large Pickaxe"] = 100000,
            ["Diamond Large Pickaxe"] = 500000,
        };

        public StateShop()
        {
            UIButton b;
            int x = 20, y = 50;

            b = new UIButton("Acheter", x, y);
            b.ID = "Bouton Acheter";
            UI.Add(b);
            x = b.X + b.W + 20;
            b.OnClick += (s, a) => RefreshAcheter();

            b = new UIButton("Vendre", x, y);
            b.ID = "Bouton Vendre";
            UI.Add(b);
            x = b.X + b.W + 20;
            b.OnClick += (s, a) => RefreshVendre();
        }
        private void RefreshAcheter() { RemoveAcheterAndVendreUI(); AddAcheterUI(); }
        private void RefreshVendre() { RemoveAcheterAndVendreUI(); AddVendreUI(); }
        public void RemoveAcheterAndVendreUI()
        {
            var list = UI.Where(x => x.ID == "Acheter" || x.ID == "Vendre").ToList();
            foreach (var ui in list)
                UI.Remove(ui);
        }
        private void AddAcheterUI()
        {
            UIButton b;
            UILabel l;
            int x = 20, y = 100;
            var ToolsToBuy = new List<(string Name, BigInteger Price, ITool Tool)>()
            {
                ("Bronze Pickaxe", GetPrice("Bronze Pickaxe"), new ToolPickaxe(OreType.Bronze)),
                ("Silver Pickaxe", GetPrice("Silver Pickaxe"), new ToolPickaxe(OreType.Silver)),
                ("Gold Pickaxe", GetPrice("Gold Pickaxe"), new ToolPickaxe(OreType.Gold)),
                ("Titanium Pickaxe", GetPrice("Titanium Pickaxe"), new ToolPickaxe(OreType.Titanium)),
                ("Diamond Pickaxe", GetPrice("Diamond Pickaxe"), new ToolPickaxe(OreType.Diamond)),
                ("Bronze Large Pickaxe", GetPrice("Bronze Large Pickaxe"), new ToolPickaxe(OreType.Bronze, 2)),
                ("Silver Large Pickaxe", GetPrice("Silver Large Pickaxe"), new ToolPickaxe(OreType.Silver, 3)),
                ("Gold Large Pickaxe", GetPrice("Gold Large Pickaxe"), new ToolPickaxe(OreType.Gold, 4)),
                ("Titanium Large Pickaxe", GetPrice("Titanium Large Pickaxe"), new ToolPickaxe(OreType.Titanium, 5)),
                ("Diamond Large Pickaxe", GetPrice("Diamond Large Pickaxe"), new ToolPickaxe(OreType.Diamond, 6)),
            };

            foreach (var item in ToolsToBuy)
            {
                b = new UIButton(item.Tool.Image, x, y);
                b.ID = "Acheter";
                UI.Add(b);
                x = b.X + b.W + 10;
                l = new UILabel(item.Price.ToString(), x, y);
                l.ID = "Acheter";
                x = l.X + l.W + 25;
                UI.Add(l);
                if (x > Core.H * 0.8F)
                {
                    x = 20;
                    y += b.H + 20;
                }
                b.OnClick += (s, a) => { if (Data.Instance.StatInfo.Money >= item.Price) { Data.Instance.StatInfo.Money -= item.Price; Data.Instance.StatInfo.Inventory.AddItem(item.Tool); }};
            }
        }
        private void AddVendreUI()
        {
            UIButton b;
            UILabel l;
            int x = 20, y = 100;

            foreach (var ore in Data.Instance.StatInfo.OresCount)
            {
                b = new UIButton(Ore.GetOreRes(ore.Key).Resized(16), x, y);
                b.ID = "Vendre";
                b.OnClick += (s, a) => { if (Data.Instance.StatInfo.OresCount[ore.Key] > 0) { Data.Instance.StatInfo.OresCount[ore.Key]--; Data.Instance.StatInfo.Money += GetPrice(Enum.GetName(typeof(OreType), ore.Key)); RefreshVendre(); } };
                UI.Add(b);

                b = new UIButton(Ore.GetOreRes(ore.Key).Resized(16), x, y + b.H + 20);
                b.ID = "Vendre";
                b.OnClick += (s, a) => { Data.Instance.StatInfo.Money += Data.Instance.StatInfo.OresCount[ore.Key] * GetPrice(Enum.GetName(typeof(OreType), ore.Key)); Data.Instance.StatInfo.OresCount[ore.Key] = 0; RefreshVendre(); };
                UI.Add(b);

                x = b.X + b.W + 10;

                l = new UILabel(GetPrice(Enum.GetName(typeof(OreType), ore.Key)).ToString(), x, y);
                l.ID = "Vendre";
                UI.Add(l);

                l = new UILabel((Data.Instance.StatInfo.OresCount[ore.Key] * GetPrice(Enum.GetName(typeof(OreType), ore.Key))).ToString(), x, y + b.H + 20);
                l.ID = "Vendre";
                UI.Add(l);

                x = l.X + l.W + 25;

                if (x > Core.H * 0.8F)
                {
                    x = 20;
                    y += (b.H + 20) * 2;
                }
            }
        }

        private BigInteger GetPrice(string item)
        {
            return ItemPrice.ContainsKey(item) ? ItemPrice[item] : 0;
        }

        public void Update()
        {
        }

        public void DrawStatic()
        {
        }

        public void Draw()
        {
            foreach (var ui in UI)
                ui.Draw();
        }

        public void MouseDown(MouseEventArgs e)
        {
            UI.Where(x => e.X >= x.X && e.X < x.X + x.W && e.Y >= x.Y && e.Y < x.Y + x.H).ToList().ForEach(x => x.Clicked());
        }

        public void ReturnToPreviousState()
        {
            Data.Instance.State = PreviousState;
            PreviousState = null;
        }
    }
}
