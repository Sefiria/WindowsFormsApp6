using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace console_v2
{
    public class Statistics
    {
        public enum Stat
        {
            HP=0, HPMax, MP, MPMax, STR, INT, DEF, MPDEF, MOVSPD
        }
        private Dictionary<Stat, int> m_Stats = 0.ToDict<Stat, int>();
        public List<KeyValuePair<Stat, int>> List => m_Stats.Cast<KeyValuePair<Stat, int>>().ToList();
        public int _Get(Stat stat) => m_Stats[stat] + Effects.Select(e => e.ModTemp._Get(stat)).Sum();
        public int _Reset(Stat stat, int value) => m_Stats[stat] = value;
        public int _Add(Stat stat, int value) => m_Stats[stat] += value;
        public int _Substract(Stat stat, int value) => m_Stats[stat] -= value;
        public int _Multiply(Stat stat, int value) => m_Stats[stat] *= value;
        public int _Divide(Stat stat, int value) => m_Stats[stat] /= value == 0 ? 1 : value;
        public List<HealthEffect> Effects = new List<HealthEffect>();
        public Statistics(){}
        public Statistics(Dictionary<Stat, int> stats)
        {
            var keys = m_Stats.Keys.ToArray();
            foreach (var stat in keys)
                if(stats.ContainsKey(stat))
                    _Reset(stat, stats[stat]);
        }
        public void Apply(Statistics mod)
        {
            var keys = m_Stats.Keys.ToArray();
            foreach (var stat in keys)
                _Add(stat, mod._Get(stat));
        }
        public void Apply(HealthEffect effect)
        {
            if (effect.Active)
            {
                Apply(effect.ModPerm);
                effect.Tick();
            }
        }
        public void TickSecond()
        {
            Effects.ForEach(Apply);
            Effects.RemoveAll(effect => !effect.Active);
        }
    }
}
