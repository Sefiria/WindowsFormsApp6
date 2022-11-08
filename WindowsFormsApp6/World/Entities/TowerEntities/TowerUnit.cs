using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;
using WindowsFormsApp6.World.Blocs.Consommables;

namespace WindowsFormsApp6.World.Entities
{
    public class TowerUnit : Entity
    {
        public bool Destroy = false;
        public float t;
        public int CheckpointID;
        public float Speed;
        public int HP, DMG;
        public long Gain;
        public TowerUnit(float x, float y, long gain, int hp, int dmg, float speed, int checkpointID = 0) : base(x, y, "unit")
        {
            Gain = gain;
            HP = hp;
            DMG = dmg;
            Speed = speed;
            CheckpointID = checkpointID;
        }

        public int Update(List<Point> checkpoints)
        {
            if (CheckpointID < checkpoints.Count - 1)
            {
                int tsz = Core.TileSz;
                int w = ImageRight.Width / 2;
                int h = ImageRight.Height / 2;
                X = Maths.Lerp(checkpoints[CheckpointID].X * tsz + tsz / 2 - w / 2, checkpoints[CheckpointID + 1].X * tsz + tsz / 2 - w / 2, t);
                Y = Maths.Lerp(checkpoints[CheckpointID].Y * tsz + tsz / 2 - h / 2, checkpoints[CheckpointID + 1].Y * tsz + tsz / 2 - h / 2, t);
                t += Speed / Math.Max(Math.Abs(checkpoints[CheckpointID + 1].X - checkpoints[CheckpointID].X), Math.Abs(checkpoints[CheckpointID + 1].Y - checkpoints[CheckpointID].Y));
                if(t >= 1F)
                {
                    t = 0F;
                    CheckpointID++;
                }
            }
            else
            {
                if(t >= 100)
                {
                    t = 0F;
                    return DMG;
                }
                else t++;
            }
            return 0;
        }
    }
}
