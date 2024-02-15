using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using Tooling.UI;
using static WindowsFormsApp24.Enumerations;

namespace WindowsFormsApp24.Events
{
    internal class WateringCan : Event
    {
        internal WateringCan(float maxVolume, int x, int y, int z) : base(Core.NamedTextures[NamedObjects.WateringCan], true, x, y, z)
        {
            Stats[WatercanStats.MaxVolume] = maxVolume;
        }
        internal WateringCan(float maxVolume, float x, float y, float z) : base(Core.NamedTextures[NamedObjects.WateringCan], true, x, y, z)
        {
            Stats[WatercanStats.MaxVolume] = maxVolume;
        }

        private float m_Volume = 0F;
        internal float Volume
        {
            get => m_Volume;
            set
            {
                m_Volume = value;
                var max = Stats[WatercanStats.MaxVolume];
                if (m_Volume > max)
                    m_Volume = max;
                if (m_Volume < 0F)
                    m_Volume = 0F;
            }
        }
        internal Dictionary<WatercanStats, float> Stats = new Dictionary<WatercanStats, float>()
        {
            [WatercanStats.MaxVolume] = 5F,
            [WatercanStats.FlowRate] = 0.01F,
            [WatercanStats.FlowLoss] = 0.0005F,
            [WatercanStats.EvaporationRate] = 0.00001F,
            [WatercanStats.Spread] = 2F,
        };

        internal override void Update()
        {
            Volume -= Stats[WatercanStats.EvaporationRate];
            base.Update();
        }

        internal override void SecondaryAction()
        {
            PredefinedAction_TakeDrop(this);
        }
        internal override void PrimaryAction()
        {
            PrimaryActionDown();
        }
        internal override void PrimaryActionDown()
        {
            if (Volume == 0F)
                return;
            if (Core.CurrentMainScene.MainCharacter.HandObject != Guid)
                return;
            int x = (int)((float)Data["X"]) / Core.TileSize;
            int y = (int)((float)Data["Y"]) / Core.TileSize;
            var tiledata = Map.Current.Tiles.PointedData(x, y);
            var index = tiledata.tile?.TilesetIndex;
            var layer = tiledata.layer;
            //if (/*(index == null || index == 0 || index == 1) &&*/ Map.Current.Events.FirstOrDefault(ev => ev.TileX != x && ev.TileY != y) != null && Map.Current.Tiles[layer, x, y] != null)
            {
                float spread = Stats[WatercanStats.Spread];
                float fs = spread;
                for (int i = -(int)fs; i < (int)fs; i++)
                    for (int j = -(int)fs; j < (int)fs; j++)
                    {
                        var len = (float)Math.Round((i, j).P().Length(), 5);
                        for (int l = 0; l < Map.LAYERS && Map.Current.Tiles.check_pass(x + i, y + j) && Map.Current.Tiles[l, x + i, y + j] != null; l++)
                            Map.Current.Tiles[l, x + i, y + j].wet += Math.Max(0F, Stats[WatercanStats.FlowRate] * (len == 0 ? 1 : 1F / len) * fs - Stats[WatercanStats.FlowLoss]);
                    }
                Volume -= Stats[WatercanStats.FlowRate];
            }
        }
        internal override void Draw()
        {
            base.Draw();
            if (Core.MainCharacter.HandObject == Guid || MouseHover)
                DrawExtraInfos();
        }
        internal override void DrawExtraInfos()
        {
            var cam = Core.Cam;

            var txt = $"{Math.Round(Volume, 2)}L / {Stats[WatercanStats.MaxVolume]}L";
            float x, y;
            if (Core.MainCharacter.HandObject == Guid)
            {
                var p = Core.MainCharacter.Position;
                x = p.X - cam.X;
                y = p.Y - cam.Y - Core.TileSize * 2F;
            }
            else
            {
                x = X - cam.X + Core.TileSize / 2;
                y = Y - Core.TileSize - cam.Y;
            }
            UIDisplay.Display(Core.Instance.gUI, txt, x, y, Core.MainCharacter.HandObject == Guid ? 0.25F : 1F);

            if (Core.MainCharacter.HandObject == Guid)
            {
                new UIBar()
                {
                    EmptyColor = Color.Black,
                    OutlineColor = Color.Black,
                    FillColor = Color.DeepSkyBlue,
                    RangeValue = new RangeValueF(0, Volume / Stats[WatercanStats.MaxVolume], 1F),
                    Position = new vecf(X + W - cam.X, Y - cam.Y - Core.TileSize / 2),
                    Size = new vecf(8, 24),
                    Style = Tooling.Enumerations.UIBarStyle.Vertical,
                }
                .Draw(Core.Instance.gUI);
            }
        }
    }
}
