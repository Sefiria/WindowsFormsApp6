using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace WindowsFormsApp22.Entities
{
    public class Behaviored : Entity
    {
        public Action<List<object>> Behavior;
        public List<object> Data = new List<object>();
        public float move_speed = 1F;
        public bool IsLegacyBehavior = true;

        public Behaviored(string name) : base(name)
        {
            IsDrawable = false;
        }
        public Behaviored(string tex, Color color, float x, float y, int w, int h, Action<List<object>> behavior, List<object> data = null) : base(tex, color, w, h)
        {
            X = x; Y = y;
            Behavior = behavior;
            Data = data ?? new List<object>();
        }

        public override void Update()
        {
            base.Update();
            if (Behavior != null)
            {
                if(IsLegacyBehavior)
                    Cascade(Behavior, Data.Prepend(this).ToList());
                else
                    Behavior.Invoke(Data.Prepend(this).ToList());
            }
            if (IsDrawable && !Core.VisibleBoundsExt.Contains((int)X, (int)Y))
                Exist = false;
        }

        public static Action<List<object>> Default_AddLook(PointF look, float mv_speed)
        => data =>
        {
            //(data[0] as Behaviored).Angle.Value = look.ToAngle();
            (data[0] as Behaviored).Pos = (data[0] as Behaviored).Pos.PlusF(look.x(mv_speed));
        };
    }
}
