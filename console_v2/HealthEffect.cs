using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace console_v2
{
    public class HealthEffect
    {
        public bool Active = true;
        public int Time, Duration;
        /// <summary>
        /// Modification permanente (ex. HP - 1 / s)
        /// </summary>
        public Statistics ModPerm;
        /// <summary>
        /// Modification temporaire, ne dure que durant l'effet (ex. vitesse de déplacement ralentie)
        /// </summary>
        public Statistics ModTemp;
        public HealthEffect(){}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration">Temps d'effet en secondes</param>
        /// <param name="modPerm">Modification permanente (ex. HP - 1 / s)</param>
        /// <param name="modTemp">Modification temporaire, ne dure que durant l'effet (ex. vitesse de déplacement ralentie)</param>
        public HealthEffect(int duration, Statistics modPerm, Statistics modTemp)
        {
            Duration = duration;
            ModPerm = modPerm;
            ModTemp = modTemp;
        }
        public void Tick()
        {
            if (Active)
            {
                Time++;
                if (Time >= Duration)
                {
                    Time = 0;
                    Active = false;
                }
            }
        }
    }
}
