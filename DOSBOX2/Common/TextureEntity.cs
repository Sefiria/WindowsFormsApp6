using System;
using Tooling;

namespace DOSBOX2.Common
{
    internal class TextureEntity : Entity
    {
        public byte[,] Texture = null;

        public override float x { get; set; }
        public override float y { get; set; }
        public override float w => Texture?.GetLength(0) ?? 0;
        public override float h => Texture?.GetLength(1) ?? 0;

        public TextureEntity(
            byte faction = 0,
            CharacterStatus.Init status_init = CharacterStatus.Init.Mid,
            byte[,] texture = null,
            Inventory inventory = null)
            : base(faction, status_init, inventory)
        {
            Texture = texture;
        }
        public override void Draw()
        {
            if (!IsKinetic && !IsTrigger && (PrevPosition?.i == (x, y).V() || !first_before_update_done))
                return;
            if (Texture != null)
            {
                if (PrevPosition != null)
                    Graphic.SetBatch(Texture, (int)PrevPosition.x, (int)PrevPosition.y, Graphic.BatchMode.Reset, Facing == Faces.Left);
                Graphic.SetBatch(Texture, (int)x, (int)y, Graphic.BatchMode.Raw, Facing == Faces.Left);
            }
        }
        public override void ClearDraw()
        {
            if (Texture != null)
                Graphic.SetBatch(Texture, (int)x, (int)y, Graphic.BatchMode.Reset, Facing == Faces.Left);
        }
    }
}
