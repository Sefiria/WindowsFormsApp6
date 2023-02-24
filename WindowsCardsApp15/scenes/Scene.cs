using System.Collections.Generic;
using WindowsCardsApp15.UI;

namespace WindowsCardsApp15.scenes
{
    internal abstract class Scene
    {
        public List<IUI> UI = new List<IUI>();

        public Scene()
        {
            Initialize();
        }

        public abstract void Initialize();
        public abstract void Update();
        public abstract void Draw();
        protected abstract void DrawUI();
    }
}
