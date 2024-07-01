using DOSBOX2.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX2.Anims
{
    internal class Anim_Player_Move : Anim
    {
        public Anim_Player_Move() : base()
        {
            Frames.Add(GraphicExt.CreateBatch(@"
33033
33033
30033
03003
03003
33033
30303
03330
"));
            Frames.Add(GraphicExt.CreateBatch(@"
33033
33033
30003
03030
03030
33033
30303
33033
"));
            Frames.Add(GraphicExt.CreateBatch(@"
33033
33033
33003
30030
33030
33003
33030
30330
"));
        }
    }
}
