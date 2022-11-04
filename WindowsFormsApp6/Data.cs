using System;
using System.Collections.Generic;
using System.Numerics;
using WindowsFormsApp6.World;
using WindowsFormsApp6.World.Ores;

namespace WindowsFormsApp6
{
    [Serializable]
    public class StatInfo
    {
        public Dictionary<OreType, int> OresCount = new Dictionary<OreType, int>()
        {
            [OreType.Bronze] = 0,
            [OreType.Silver] = 0,
            [OreType.Gold] = 0,
            [OreType.Titanium] = 0,
            [OreType.Diamond] = 0,
        };
        public BigInteger Money = 1000000000000;
        public Inventory Inventory = new Inventory();
        public StatInfo(){}
    }

    [Serializable]
    public class Data
    {
        private static Data m_Instance { get; set; } = null;
        public static Data Instance
        {
            get => m_Instance == null ? new Data() : m_Instance;
            set => m_Instance = value;
        }
        public IState State { get; set; }
        public StateWorld World { get; set; }
        public StateMine Mine { get; set; }
        public StateShop Shop { get; set; }
        public StateFactory Factory { get; set; }
        public StateInventory Inventory { get; set; }
        public StateTower Tower { get; set; }
        public StatInfo StatInfo { get; set; } = new StatInfo();

        public Data()
        {
            m_Instance = this;

            World = new StateWorld();
            State = World;
            World.DrawStatic();
            World.InitializeWorld();

            Shop = new StateShop();

            Factory = new StateFactory();

            Inventory = new StateInventory();

            Mine = new StateMine();
            Mine.DrawStatic();

            Tower = new StateTower();
            Tower.DrawStatic();
        }
    }
}
