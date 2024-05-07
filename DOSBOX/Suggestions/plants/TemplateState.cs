using Tooling;

namespace DOSBOX.Suggestions
{
    public class TemplateState : IState
    {
        public void Init()
        {
            Core.Layers.Clear();
            Core.Layers.Add(new byte[64, 64]); // BG
            Core.Layers.Add(new byte[64, 64]); // Sprites
            Core.Layers.Add(new byte[64, 64]); // UI
            
        }

        public void Update()
        {
            if (KB.IsKeyPressed(KB.Key.Escape))
            {
                Plants.Instance.CurrentState = null;
                return;
            }

            DisplayUI();
        }

        private void DisplayUI()
        {
        }
    }
}
