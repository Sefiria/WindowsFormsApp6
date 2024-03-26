using _Console.Core.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _Console.Core.Skills.Skills;

namespace _Console.Core.Skills
{
    public class MagicEffects
    {
        public static bool Magic(Attacker source, Attacker target, MagicalSkills skill)
        {
            return (bool) typeof(MagicEffects).GetMethod(skill.ToString()).Invoke(null, new[] { source, target });
        }
        
        public static bool Thunder(Attacker source, Attacker target) { return false; }
        public static bool ThunderPlus(Attacker source, Attacker target) { return false; }
        public static bool ThunderX(Attacker source, Attacker target) { return false; }
        public static bool Fire(Attacker source, Attacker target) { return false; }
        public static bool FirePlus(Attacker source, Attacker target) { return false; }
        public static bool FireX(Attacker source, Attacker target) { return false; }
        public static bool Water(Attacker source, Attacker target) { return false; }
        public static bool WaterPlus(Attacker source, Attacker target) { return false; }
        public static bool WaterX(Attacker source, Attacker target) { return false; }
        public static bool Recover(Attacker source, Attacker target) { return false; }
        public static bool RecoverTeam(Attacker source, Attacker target) { return false; }
    }
}
