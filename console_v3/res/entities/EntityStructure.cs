using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v3.res.entities
{
    internal class EntityStructure : Entity
    {
        public bool IsOn = false;
        public int CharToDisplayON = -1;
        public Bitmap DBResSpeON = null;

        public EntityStructure() : base() { }
        public EntityStructure(vec tile, int dbref, bool addToCurrentChunkEntities = true) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            DBRef = dbref;
            Stats = new Statistics(new Dictionary<Statistics.Stat, int> { [Statistics.Stat.HPMax] = 20, [Statistics.Stat.HP] = 20 });
        }

        public override void Update()
        {
            switch (DBRef)
            {
                case (int)DB.Tex.Workbench: break;
                case (int)DB.Tex.Furnace: case (int)DB.Tex.FurnaceOn: break;
                case (int)DB.Tex.Torch: case (int)DB.Tex.TorchOn: break;
            }
        }
        public override void TickSecond()
        {
            //if (IsOn)
            //{
            //    vecf ofst = DBRef == (int)DB.Tex.Torch ? (0, -12).Vf() : vecf.Zero;
            //    ParticlesManager.Generate(Position + GraphicsManager.TileSize / 2f + ofst, 2f, 4f, Color.White, 5, 20, applyGravity: false);
            //}
        }
        public override void Draw(Graphics g)
        {
            GraphicsManager.Draw(g, GetTexLum(DB.GetTexture(DBRef + (IsOn ? 1 : 0))), Position);
        }
        public override void Action(Entity triggerer)
        {
            if (!Exists) return;

            switch(DBRef)
            {
                case (int)DB.Tex.Workbench: Core.Instance.SwitchScene(Core.Scenes.Craft, 3); break;
                case (int)DB.Tex.Furnace: case (int)DB.Tex.FurnaceOn:  break;
                case (int)DB.Tex.Torch: case (int)DB.Tex.TorchOn: break;
            }
        }
    }
}
