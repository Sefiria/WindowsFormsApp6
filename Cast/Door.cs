using Cast.Utilities;
using System.Drawing;
using static Cast.Tools.KB;

namespace Cast
{
    public class Door : Structural
    {
        public enum DoorState
        {
            Close,
            Opening,
            Open,
            Closing,
        }
        public DoorState State = DoorState.Close;

        float sliding_t = 0F, speed_sliding = 0.05F;
        float TriggerLength = 70F;

        public Door(PointF A, PointF B) { _A = A; _B = B; C = Color.Cyan; }
        public override PointF A => Maths.Lerp(_A, _A.Minus(_B.Minus(_A)), sliding_t);
        public override PointF B => Maths.Lerp(_B, _B.Minus(_B.Minus(_A)), sliding_t);
        public void Update()
        {
            if (IsKeyPressed(Key.Space) && (Maths.Distance(_A, Core.Cam.Position) < TriggerLength || Maths.Distance(_B, Core.Cam.Position) < TriggerLength))
            {
                switch (State)
                {
                    case DoorState.Closing:
                    case DoorState.Close: State = DoorState.Opening; break;
                    case DoorState.Opening:
                    case DoorState.Open: State = DoorState.Closing; break;
                }
            }

            if (State == DoorState.Opening)
            {
                if (sliding_t < 1F)
                    sliding_t += speed_sliding;
                if (sliding_t > 1F)
                    sliding_t = 1F;
                if (sliding_t == 1F)
                    State = DoorState.Open;
            }

            if (State == DoorState.Closing)
            {
                if (sliding_t > 0F)
                    sliding_t -= speed_sliding;
                if (sliding_t < 0F)
                    sliding_t = 0F;
                if (sliding_t == 0F)
                    State = DoorState.Close;
            }
        }
    }
}
