using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp21.Animations
{
    public class Animation
    {
        public static List<Animation> Templates = null;

        public string Name;

        static Animation()
        {
            Templates = new List<Animation>()
            {
                Idle,
            };
        }

        public Animation()
        {
            Name = "";
        }
        public static Animation Load(string animationName)
        {
            return Templates.FirstOrDefault(x => x.Name == animationName);
        }

        public static Animation Idle = new Animation() { Name = "Idle" };
    }
}
