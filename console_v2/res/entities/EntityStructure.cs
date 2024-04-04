using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2.res.entities
{
    internal class EntityStructure : Entity
    {
        private int m_DBRef;
        public int DBRef
        {
            get => m_DBRef;
            set
            {
                m_DBRef = value;
                (CharToDisplay, DBResSpe) = DB.RetrieveDBResOrSpe(m_DBRef);
                (CharToDisplayON, DBResSpeON) = DB.RetrieveDBResOrSpe(m_DBRef + 1);
            }
        }
        private int OnParticlesTIcks;
        public bool IsOn = false;
        public int CharToDisplayON;
        public Bitmap DBResSpeON;

        public EntityStructure() : base() { }
        public EntityStructure(vec tile, Structures dbref, bool addToCurrentChunkEntities = true) : base(tile.ToWorld(), addToCurrentChunkEntities)
        {
            DBRef = (int)dbref;
            Stats = new Statistics(new Dictionary<Statistics.Stat, int> { [Statistics.Stat.HPMax] = 20, [Statistics.Stat.HP] = 20 });
        }

        public override void Update()
        {
        }
        public override void TickSecond()
        {
        }
        public override void Draw(Graphics g)
        {
            int charToDisplay = IsOn ? CharToDisplayON : CharToDisplay;
            Bitmap dbResSpe = IsOn ? DBResSpeON : DBResSpe;
            if (CharToDisplay != -1)
                GraphicsManager.DrawString(g, string.Concat((char)charToDisplay), CharBrush, Position);
            else if (DBResSpe != null)
                GraphicsManager.DrawImage(g, dbResSpe, Position);

            if (IsOn)
            {
                if (OnParticlesTIcks <= 0)
                {
                    OnParticlesTIcks = 50;
                    ParticlesManager.Generate(Position + GraphicsManager.CharSize.Vf() / 2f, 2f, 4f, Color.White, 5, 20, applyGravity:false);
                }
                else OnParticlesTIcks--;
            }
        }
        public override void Action(Entity triggerer)
        {
            if (!Exists) return;

            Core.Instance.SwitchScene(Core.Scenes.Craft, 2);
        }
    }
}
