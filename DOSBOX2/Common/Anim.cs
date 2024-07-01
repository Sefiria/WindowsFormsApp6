using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DOSBOX2.Common.Entity;

namespace DOSBOX2.Common
{
    internal class Anim
    {
        public List<byte[,]> Frames = new List<byte[,]>();
        protected float m_CurrentFrame = 0F;
        public int CurrentFrame => (int)m_CurrentFrame;
        public int MaxFrame => Frames.Count - 1;
        public float FrameSpeed = 0.1F;

        public float w => Frames?[CurrentFrame].GetLength(0) ?? 0;
        public float h => Frames?[CurrentFrame].GetLength(1) ?? 0;

        public void Draw(Entity e)
        {
            var tex = Frames[CurrentFrame];
            if (e.PrevPosition != null)
                Graphic.SetBatch(tex, (int)e.PrevPosition.x, (int)e.PrevPosition.y, Graphic.BatchMode.Reset, e.Facing == Faces.Left);
            Graphic.SetBatch(tex, (int)e.x, (int)e.y, Graphic.BatchMode.Raw, e.Facing == Faces.Left);
            m_CurrentFrame += FrameSpeed;
            while (CurrentFrame > MaxFrame)
                m_CurrentFrame -= MaxFrame + 1;
            while (CurrentFrame < 0)
                m_CurrentFrame += MaxFrame + 1;
        }
        public void ClearDraw(Entity e)
        {
            if (e.PrevPosition == null)
                return;
            var tex = Frames[CurrentFrame];
            Graphic.SetBatch(tex, (int)e.PrevPosition.x, (int)e.PrevPosition.y, Graphic.BatchMode.Reset, e.Facing == Faces.Left);
        }

        public void Reset() => m_CurrentFrame = 0;
    }
}
