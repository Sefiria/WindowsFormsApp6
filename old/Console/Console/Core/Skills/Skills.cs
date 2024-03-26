using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Core.Skills
{
    public class Skills
    {
        public enum PhysicalSkills
        {
            // One Hit
            SimpleAttack,
            // Two Hits
            DoubleAttack
        }
        public enum SpecialSkills
        {
            // Recover 10 HP to the team
            RecoverTeam,
            // Multiple Hits (random between 1 and 10)
            MultipleAttack,
            // Multiply STR by 2
            Berserk,
            // Multiply INT by 2
            DarkPower
        }
        public enum MagicalSkills
        {
            Thunder,
            ThunderPlus,
            ThunderX,
            Fire,
            FirePlus,
            FireX,
            Water,
            WaterPlus,
            WaterX,
            Recover,
            RecoverTeam
        }
        public enum PermanentSkills
        {
            // Show monsters HP
            Scan = 1,
            // Show monsters HP, DEF, STR
            ScanPlus,
            // Show monsters HP, DEF, STR, MP, INT, and ElementWeakness
            ScanX
        }
    }
}
