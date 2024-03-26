using _Console.Core.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _Console.Core.Skills.Skills;

namespace _Console.Core.Skills
{
    public class SpecialEffects
    {
        public static bool Special(Attacker source, Attacker target, SpecialSkills skill)
        {
            return (bool) typeof(SpecialEffects).GetMethod(skill.ToString()).Invoke(null, new[] { source, target });
        }
        
        public static bool RecoverTeam(Attacker source, Attacker target) { return false; }
        public static bool MultipleAttack(Attacker source, Attacker target) { return false; }
        public static bool Berserk(Attacker source, Attacker target) { return false; }
        public static bool DarkPower(Attacker source, Attacker target) { return false; }
    }
}
