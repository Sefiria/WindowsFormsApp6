using _Console.Core.Monsters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Console.Battle
{
    public class BattleInfo
    {
        public List<Attacker> team = null;
        public List<Attacker> monsters = null;
        private int m_Turn = 0;
        public bool EndBattle = false;
        public bool TeamGameOver = false;
        public int Turn { get => m_Turn; set { m_Turn = (value >= team.Count + monsters.Count) ? 0 : value; } }

        public BattleInfo(ref List<Attacker> team, Attacker[] monsters)
        {
            this.team = team;
            this.monsters = monsters.ToList();


            int x = 2;
            foreach (Monster monster in monsters)
            {
                string[] monsterPaintLines = monster.Paint.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                monster.RenderPosX = x;
                x += monsterPaintLines[0].Length + 2;
            }
        }

        public bool IsTeamTurn => Turn < team.Count;
        public bool IsMonstersTurn => (Turn - team.Count) >= 0;
        public Attacker GetCurrentTeammate() => IsTeamTurn ? team[Turn] : null;
        public Attacker GetCurrentMonster() => IsMonstersTurn ? monsters[Turn - team.Count] : null;

        public void ResetAttrMods()
        {
            foreach (Attacker teammate in team)
            {
                teammate.DEF_ModNextTurn = 0;
            }

            foreach (Attacker monster in monsters)
            {
                monster.DEF_ModNextTurn = 0;
            }
        }

        public void DestroyTarget(Attacker target, Action BattleRenderMonsters)
        {
            if (monsters.Contains(target))
            {
                monsters.Remove(target);
                BattleRenderMonsters();

                if (monsters.Count == 0)
                {
                    EndBattle = true;
                    return;
                }
            }

            if (team.Contains(target))
            {
                team.Remove(target);

                if (team.Count == 0)
                {
                    EndBattle = true;
                    TeamGameOver = true;
                    return;
                }
            }
        }
    }
}
