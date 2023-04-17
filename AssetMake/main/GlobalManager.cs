using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetMake.main
{
    internal class GlobalManager
    {
        public TextureMgr TextureMgr { get; set; }
        public AnimationMgr AnimationMgr { get; set; }

        public GlobalManager()
        {
            TextureMgr = new TextureMgr();
            AnimationMgr = new AnimationMgr();
        }
    }
}
