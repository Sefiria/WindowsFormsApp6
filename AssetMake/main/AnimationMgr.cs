using AssetMake.Properties;
using AssetMake.utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetMake.main
{
    internal class AnimationMgr
    {
        public Dictionary<string, Animation> Animations;
        public AnimationMgr()
        {
            Animations = new Dictionary<string, Animation>()
            {
                ["walk"] = new Animation(Resources.anim_walking.SplitHorizontally(26)),
                ["run"] = new Animation(Resources.anim_running.SplitHorizontally(26)),
            };
        }
    }
}
