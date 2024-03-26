using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;
using WindowsFormsApp26.Entities.Structures;

namespace WindowsFormsApp26
{
    public class Entity
    {
        public Guid ID = Guid.NewGuid();
        public bool Exists = true;
        public PointF Position = PointF.Empty;
        public float X { get => Position.X; set => Position.X = value; }
        public float Y { get => Position.Y; set => Position.Y = value; }
        public float Left => X;
        public float Right => X + W;
        public float Top => Y;
        public float Bottom => Y + H;
        public Size Size = Size.Empty;
        public int W { get => Size.Width; set => Size.Width = value; }
        public int H { get => Size.Height; set => Size.Height = value; }
        private Bitmap m_Image = null;
        public Bitmap Image { get => m_Image; set { m_Image = value; Size = Image.Size; } }
        public bool IsGravityApplied = true, IsKinetic = false, IsCollisionable = true;
        public PointF Forces = PointF.Empty;
        public float ForceThreshold = 10f;
        public float ForceThresholdForDrawMovement = 5f;
        public RectangleF ForceThresholdForDrawMovementRect => new RectangleF(-ForceThresholdForDrawMovement, -ForceThresholdForDrawMovement, ForceThresholdForDrawMovement*2f, ForceThresholdForDrawMovement*2f);
        public RectangleF Bounds => new RectangleF(Position, Size);
        public Collider Collider = null;
        public List<Rectangle> ForegroundAreas = new List<Rectangle>();

        public Entity()
        {
            Initialize();
        }
        public Entity(PointF position)
        {
            Position = position;
            Initialize();
        }
        private void Initialize()
        {
            Core.Instance.CurrentScene.Entities.Add(this);
        }

        public virtual void Update()
        {
            if (IsGravityApplied && !(Collider?.Collides() ?? true))
            {
                Forces.Y += Core.Instance.Gravity;
            }

            if (Forces.X < -ForceThreshold) Forces.X = -ForceThreshold;
            if (Forces.X > ForceThreshold) Forces.X = ForceThreshold;
            if (Forces.Y < -ForceThreshold) Forces.Y = -ForceThreshold;
            if (Forces.Y > ForceThreshold) Forces.Y = ForceThreshold;

            if (!IsKinetic)
            {
                var prev = Position;
                Position = Position.PlusF(Maths.Abs(Forces.X) < 1f ? 0f : Forces.X, Maths.Abs(Forces.Y) < 1f ? 0f : Forces.Y);
                if (Collider?.Collides() ?? false)
                {
                    Position = prev;
                    Forces = PointF.Empty;
                }
            }
        }
        public virtual void Draw(Graphics g)
        {
            var u = Core.CurrentEntities.FirstOrDefault(e => e.ID == ID);
            g.DrawImage(Image, Position);

            if(Core.DEBUG) Collider?.Boxes.ForEach(b => g.DrawRectangle(Pens.Red, b.ToIntRect()));

            Pen CreatePen(float force, float thr)
            {
                return new Pen(Color.FromArgb((int)(255f * Math.Min(1f, Math.Max(0f, (force - thr) / (thr * 2f)))), Color.White), 2f);
            }

            if (!IsKinetic)
            {
                if (Forces.X <= ForceThresholdForDrawMovementRect.Left)
                {
                    var pen = CreatePen(Forces.X, ForceThresholdForDrawMovementRect.Left);
                    g.DrawLine(pen, X + W * 1.3f, Y + H * 0.25f, X + W * 1.8f, Y + H * 0.25f);
                    g.DrawLine(pen, X + W * 1.5f, Y + H * 0.50f, X + W * 2.0f, Y + H * 0.50f);
                    g.DrawLine(pen, X + W * 1.3f, Y + H * 0.75f, X + W * 1.8f, Y + H * 0.75f);
                }
                else if (Forces.X >= ForceThresholdForDrawMovementRect.Right)
                {
                    var pen = CreatePen(Forces.X, ForceThresholdForDrawMovementRect.Right);
                    g.DrawLine(pen, X - W * 0.3f, Y + H * 0.25f, X - W * 0.8f, Y + H * 0.25f);
                    g.DrawLine(pen, X - W * 0.5f, Y + H * 0.50f, X - W * 1.0f, Y + H * 0.50f);
                    g.DrawLine(pen, X - W * 0.3f, Y + H * 0.75f, X - W * 0.8f, Y + H * 0.75f);
                }

                if (Forces.Y <= ForceThresholdForDrawMovementRect.Top)
                {
                    var pen = CreatePen(Forces.Y, ForceThresholdForDrawMovementRect.Top);
                    g.DrawLine(pen, X + W * 0.25f, Y + H * 1.3f, X + W * 0.25f, Y + H * 1.8f);
                    g.DrawLine(pen, X + W * 0.50f, Y + H * 1.5f, X + W * 0.50f, Y + H * 2.0f);
                    g.DrawLine(pen, X + W * 0.75f, Y + H * 1.3f, X + W * 0.75f, Y + H * 1.8f);
                }
                else if (Forces.Y >= ForceThresholdForDrawMovementRect.Bottom)
                {
                    var pen = CreatePen(Forces.Y, ForceThresholdForDrawMovementRect.Bottom);
                    g.DrawLine(pen, X + W * 0.25f, Y - H * 0.3f, X + W * 0.25f, Y - H * 0.8f);
                    g.DrawLine(pen, X + W * 0.50f, Y - H * 0.5f, X + W * 0.50f, Y - H * 1.0f);
                    g.DrawLine(pen, X + W * 0.75f, Y - H * 0.3f, X + W * 0.75f, Y - H * 0.8f);
                }
            }
        }
        public virtual void DrawForeground(Graphics g)
        {
            ForegroundAreas.ForEach(area => g.DrawImage(Image, new Rectangle((int)X + area.X, (int)Y + area.Y, area.Width, area.Height), area, GraphicsUnit.Pixel));
        }
    }
}
