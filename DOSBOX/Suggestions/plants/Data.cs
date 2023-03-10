using DOSBOX.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace DOSBOX.Suggestions.plants
{
    public class Data
    {
        public class DataGarden
        {
            public int FloorLevel => 48;

            public User User { get; set; }
            public List<WaterDrop> WaterDrops { get; set; }
            public List<ClassIPlant> ScenePlants { get; set; }
            public int Ticks { get; set; }
            public int TicksMax { get; set; }
            public byte[,] ActiveBG;
            public int MapWidth { get; set; }
            public Meteo Meteo { get; set; }
            public int sunimpact_y { get; set; }
            public int Ticksimpact { get; set; }
            public int Ticksimpact_max { get; set; }

            public void Init()
            {
                if(WaterDrops == null) WaterDrops = new List<WaterDrop>();
                if(ScenePlants == null) ScenePlants = new List<ClassIPlant>();
                WaterDrops.Clear();
                ScenePlants.Clear();
                Meteo = new Meteo();
                Ticks = 0;
                TicksMax = 9;
                User = new User();
                MapWidth = 100;
                sunimpact_y = FloorLevel;
                Ticksimpact = 0;
                Ticksimpact_max = 3;
                ActiveBG = new byte[MapWidth, FloorLevel];
                int w = ActiveBG.GetLength(0);
                int h = ActiveBG.GetLength(1);
                for (int y = 0; y < h; y++)
                    for (int x = 0; x < w; x++)
                        ActiveBG[x, y] = 2;
            }
        }


        private static Data m_Instance = null;
        public static Data Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new Data();
                    m_Instance.Init();
                    m_Instance.m_Garden.Init();
                }
                return m_Instance;
            }
        }
        public static void KillInstance()
        {
            m_Instance = null;
        }
        public static void LoadInstance(Data _instance) => m_Instance = _instance;

        public DataGarden m_Garden { get; set; } = new DataGarden();
        public static DataGarden Garden => Instance.m_Garden;

        public Dictionary<string, int> Fruits { get; set; }
        public Dictionary<string, int> Seeds { get; set; }
        public Dictionary<string, int> Items { get; set; }
        public Dictionary<string, int> LabsItems { get; set; }
        public int Coins { get; set; }
        public int WaterBucket { get; set; }
        public int WaterBucketMax { get; set; }

        public string SelectedSeed { get; set; }

        public Dictionary<string, int> FruitsAndItems => new List<Dictionary<string, int>>() { Fruits, Items }.SelectMany(dict => dict).ToDictionary(pair => pair.Key, pair => pair.Value);

        public void Init()
        {
            Fruits = new Dictionary<string, int>();
            Seeds = new Dictionary<string, int>();
            Items = new Dictionary<string, int>();
            LabsItems = new Dictionary<string, int>();

            SelectedSeed = "";
            WaterBucketMax = 100;
            WaterBucket = WaterBucketMax / 2;
            Coins = 90;
        }

        public int SelectedSeedCount => Seeds.ContainsKey(SelectedSeed) ? Seeds[SelectedSeed] : 0;
        public ClassIPlant DropSeed(vecf v)
        {
            ClassIPlant result;
            if (SelectedSeed == "")
                return null;
            result = PlantFactory.Create(SelectedSeed, v);
            Seeds[SelectedSeed]--;
            if (Seeds[SelectedSeed] == 0)
            {
                Seeds.Remove(SelectedSeed);
                SelectedSeed = "";
            }
            return result;
        }
    }
}
