using console_v3.res.entities;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;

namespace console_v3
{
    public class Entity
    {
        public static Font MidFont => GraphicsManager.MidFont;

        public Guid ID = Guid.NewGuid();
        public bool Exists = true;
        public string Name = null;
        public int DBRef;
        public vecf Position = vecf.Zero;
        public float X { get => Position.x; set => Position.x = value; }
        public float Y { get => Position.y; set => Position.y = value; }
        public float TileX => (int)(X / GraphicsManager.TileSize);
        public float TileY => (int)(Y / GraphicsManager.TileSize);
        public vec Tile => (TileX, TileY).V();

        public Statistics Stats = null;
        public Inventory Inventory = null;

        public Entity()
        {
        }
        public Entity(vecf position, bool addToCurrentChunkEntities = true)
        {
            Position = position;
            Initialize(addToCurrentChunkEntities);
        }
        private void Initialize(bool addToCurrentChunkEntities = true)
        {
            if(addToCurrentChunkEntities)
                Core.CurrentEntities.Add(this);
        }

        public virtual void Update()
        {
        }

        public virtual void Draw(Graphics g)
        {
            GraphicsManager.Draw(g, GetTexLum(DB.GetTexture(DBRef)), Position);
        }

        public Bitmap GetTexLum(Bitmap src)
        {
            const float LUM_DIST = 6F;
            const float LUM_BRIGHT = 0.35F;
            float time = Core.Instance.SceneAdventure.Time;
            float timelum = LUM_BRIGHT * (((int)(time / 12) == 0 ? time : 12F - (time - 11F)) % 12F) / 12F;
            var tex = src;
            var torchs_coords = Core.Instance.SceneAdventure.World.CurrentDimension.CurrentChunk.Entities.Where(e => (e as EntityStructure)?.DBRef == (int)DB.Tex.Torch).Select(t => t.Tile);
            var torchs_near = torchs_coords.Select(c => c.Distance(Tile)).Where(d => d <= LUM_DIST);
            float lum = torchs_near.Count() == 0 ? 0F : (LUM_DIST - torchs_near.Min()) * (LUM_BRIGHT / LUM_DIST);
            if (lum > 0f)
                tex = tex.GetAdjusted(1.001f - LUM_BRIGHT + timelum + lum);
            else
                tex = tex.GetAdjusted(1f - LUM_BRIGHT + timelum);
            return tex;
        }

        public virtual void DrawHint(Graphics gui)
        {
            var text = Name ?? (this == Core.Instance.TheGuy ? "You" : DB.DefineName(DBRef));
            if (string.IsNullOrWhiteSpace(text))
                return;
            var font = MidFont;
            var sz = TextRenderer.MeasureText(text, font);
            var position = MouseStates.Position.ToPoint().MinusF(sz.Width * 0.5f, sz.Height * 1.5f);
            var brush = Brushes.Black;
            gui.DrawString(text, font, brush, position);
        }

        public virtual void TickSecond() {}

        public virtual void Action(Entity triggerer) { }
    }
}
