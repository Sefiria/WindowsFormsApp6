using _Console.Core.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _Console.Core.Skills.Skills;

namespace _Console.Core.Monsters
{
    public class Attacker : IBattler
    {
        public string Name { get; set; }
        public string Paint { get; set; }

        #region private fields
        private bool m_AnyAttrChanged = false;
        private int m_MaxHP;
        private int m_MaxMP;
        private int m_MaxSP;
        private int m_HP;
        private int m_MP;
        private int m_SP;
        private int m_DEF;
        private int m_STR;
        private int m_INT;
        #endregion

        public bool AnyAttrChanged => m_AnyAttrChanged;
        public int MaxHP { get => m_MaxHP; set { m_MaxHP = value; m_AnyAttrChanged = true; } }
        public int MaxMP { get => m_MaxMP; set { m_MaxMP = value; m_AnyAttrChanged = true; } }
        public int MaxSP { get => m_MaxSP; set { m_MaxSP = value; m_AnyAttrChanged = true; } }
        public int HP { get => m_HP; set { m_HP = value; m_AnyAttrChanged = true; } } // Health Points
        public int MP { get => m_MP; set { m_MP = value; m_AnyAttrChanged = true; } } // Magical Points
        public int SP { get => m_SP; set { m_SP = value; m_AnyAttrChanged = true; } } // Special Points (auto charge)
        public int DEF { get => m_DEF; set { m_DEF = value; m_AnyAttrChanged = true; } } // Defense
        public int STR { get => m_STR; set { m_STR = value; m_AnyAttrChanged = true; } } // Strong (physical)
        public int INT { get => m_INT; set { m_INT = value; m_AnyAttrChanged = true; } } // Intelligence : Strong (magic)
        public bool HasSpecial { get; set; }
        public bool HasRecover { get; set; }

        public int DEF_ModNextTurn = 0;
        public PhysicalSkills physicalSkill = PhysicalSkills.SimpleAttack;
        public List<MagicalSkills> magicalSkills = new List<MagicalSkills>();
        public List<SpecialSkills> specialSkills = new List<SpecialSkills>();
        public List<PermanentSkills> permanentSkills = new List<PermanentSkills>();

        public Attacker(string Name, int HP, int MP, int SP, int DEF, int STR, int INT, bool HasSpecial = false, bool HasRecover = false)
        {
            this.Name = Name;
            this.HP = this.MaxHP = HP;
            this.MP = this.MaxMP = MP;
            this.SP = this.MaxSP = SP;
            this.DEF = DEF;
            this.STR = STR;
            this.INT = INT;
            this.HasSpecial = HasSpecial;
            this.HasRecover = HasRecover;
        }

        public bool Attack(Attacker target)
        {
            switch(physicalSkill)
            {
                default:
                    return false;

                case PhysicalSkills.SimpleAttack:
                    return target.Hit(STR);

                case PhysicalSkills.DoubleAttack:
                    bool kill = target.Hit(STR);
                    if(!kill)
                        target.Hit(STR);
                    return kill;
            }
        }
        public bool Magic(Attacker target, MagicalSkills skill)
        {
            return MagicEffects.Magic(this, target, skill);
        }
        public bool Special(Attacker target, SpecialSkills skill)
        {
            return SpecialEffects.Special(this, target, skill);
        }
        public bool UseItem(Attacker target, NNItem item)
        {
            return ItemEffects.UseItem(this, target, item.item);
        }

        /// <summary>
        /// Get damages, return if die follows
        /// </summary>
        /// <param name="DMG"></param>
        /// <returns></returns>
        public bool Hit(int DMG)
        {
            DMG -= (DEF + DEF_ModNextTurn);
            if (DMG > 0)
            {
                HP -= DMG;
                if (HP <= 0)
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
