using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApp21.Map;

namespace WindowsFormsApp21
{
    internal class Map
    {
        internal class Field
        {
            public string Name = "Unnamed";
            public List<RawField> Raws;
            public Field()
            {
                Raws = new List<RawField>();
            }
            public void Add(string name, List<Point> pts)
            {
                Add(pts);
                Raws[Raws.Count-1].Name = name;
            }
            public void Add(List<Point> pts)
            {
                Raws.Add(new RawField(pts));
            }
            public RawField GetRawByName(string find)
            {
                return Raws.FirstOrDefault(x => x.Name == find);
            }
            public Behavior AddRawBehavior(string rawName, string behaviorName, Action<Behavior, RawField> behaviorAction)
            {
                var behavior = new Behavior(behaviorName, behaviorAction);
                var raw = GetRawByName(rawName);
                raw?.Behaviors.Add(behavior);
                behavior.LinkedRaw = raw;
                return behavior;
            }
        }
        internal class RawField
        {
            public string Name = "Unnamed";
            public List<Point> Raw;
            public PointF Offset = PointF.Empty;
            public Point iOffset => new Point((int)Offset.X, (int)Offset.Y);
            public List<Behavior> Behaviors;
            public RawField()
            {
                Raw = new List<Point>();
                Behaviors = new List<Behavior>();
            }
            public RawField(List<Point> pts)
            {
                Raw = new List<Point>(pts);
                Behaviors = new List<Behavior>();
            }
            public Behavior GetBehaviorByName(string find)
            {
                return Behaviors.FirstOrDefault(x => x.Name == find);
            }
            public void Update()
            {
                Behaviors.ForEach(behavior => behavior.Call());
            }

            public PointF DrawPoint(int i) => Core.CenterPoint.Plus(Offset.PlusF(Raw[i].x(Core.Cube)));
        }
        internal class Behavior
        {
            public string Name;
            public Action<Behavior, RawField> Action;
            public int iA, iB, iC;
            public float fA, fB, fC;
            public string sA, sB, sC;
            public bool bA, bB, bC;
            public RawField LinkedRaw;
            public Behavior()
            {
                Name = "Unnamed";
                Action = null;
                LinkedRaw = null;
            }
            public Behavior(string name, Action<Behavior, RawField> action)
            {
                Name = name;
                Action = action;
                LinkedRaw = null;
            }
            public void Call()
            {
                Action?.Invoke(this, LinkedRaw);
            }


            public static Action<Behavior, RawField> CreateTemplate_Move(bool horizontal, float amount, float step)
            {
                return (b, r) =>
                {
                    if (b.iA >= (amount + 1F) / step * Core.Cube - 1)
                    {
                        b.iA = 0;
                        b.bA = !b.bA;
                    }
                    else
                        b.iA++;
                    if(horizontal)
                        r.Offset.Add((b.bA ? -1F : 1F) * step, 0F);
                    else
                        r.Offset.Add(0F, (b.bA ? -1F : 1F) * step);
                };
            }
        }

        private Field m_Field;

        public Map()
        {
            m_Field = new Field();
            Generate();
        }

        public void Generate()
        {
            m_Field.Add("center", new List<Point>()
            {
                new Point(-10, 4),
                new Point( 10, 4),
            });
            m_Field.Add("left", new List<Point>()
            {
                new Point(-20, -5),
                new Point(-10, -5),
            });
            m_Field.Add("right", new List<Point>()
            {
                new Point(10, -5),
                new Point(20, -5),
            });
            m_Field.Add("center_cube", new List<Point>()
            {
                new Point(-6, -10),
                new Point(-2, -10),
                new Point(-2, -14),
                new Point(-6, -14),
            });

            m_Field.AddRawBehavior("center_cube", "move_h", Behavior.CreateTemplate_Move(true, 8F, 0.5F));
        }

        public void Draw()
        {
            Pen pen = new Pen(Color.White, Core.Cube);

            foreach (var rawfield in m_Field.Raws)
                for (int i = 0; i < rawfield.Raw.Count - (rawfield.Raw.Count == 2 ? 1 : 0); i++)
                    Core.g.DrawLine(pen, rawfield.DrawPoint(i), rawfield.DrawPoint(i == rawfield.Raw.Count - 1 ? 0 : i + 1));
        }

        public void Update()
        {
            m_Field.Raws.ForEach(raw => raw.Update());
        }
    }
}
