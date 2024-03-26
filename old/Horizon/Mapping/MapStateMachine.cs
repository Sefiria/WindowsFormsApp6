using Core.Interfaces;
using Core.Utils;
using Mapping.Maps;
using System.Drawing;

namespace Mapping
{
    public class MapStateMachine : IDrawable, IUpdatable
    {
        static private MapStateMachine m_Instance = null;
        static public MapStateMachine Instance
        {
            get
            {
                if (m_Instance == null)
                    m_Instance = new MapStateMachine();
                return m_Instance;
            }
        }
        static public void KillInstance()
        {
            Instance?.Dispose();
            m_Instance = null;
        }


        enum States
        {
            Initialize = 0,
            Play,
            Pause,
            ResetLevel,
            NextLevel,
            Menu,
            GameOver
        }
        States State;
        Map map = null;
        public Bitmap Texture => null;
        Size size => new Size(800, 450);


        private MapStateMachine()
        {
            State = States.Initialize;
            Inputs.Initialize();
        }
        private void Dispose()
        {
            if(map != null)
            {
                map.Dispose();
                map = null;
            }
        }

        public void Render(Graphics g)
        {
            switch(State)
            {
                case States.Initialize: RenderInitialize(g); break;
                case States.Play:       RenderPlay(g);       break;
                case States.Pause:      RenderPause(g);      break;
                case States.ResetLevel: RenderResetLevel(g); break;
                case States.NextLevel:  RenderNextLevel(g);  break;
                case States.Menu:       RenderMenu(g);       break;
                case States.GameOver:   RenderGameOver(g);   break;
            }
        }
        public void Update()
        {
            switch (State)
            {
                case States.Initialize: UpdateInitialize(); break;
                case States.Play:       UpdatePlay();       break;
                case States.Pause:      UpdatePause();      break;
                case States.ResetLevel: UpdateResetLevel(); break;
                case States.NextLevel:  UpdateNextLevel();  break;
                case States.Menu:       UpdateMenu();       break;
                case States.GameOver:   UpdateGameOver();   break;
            }
        }

        #region Render
        private void RenderInitialize(Graphics g)
        {
        }
        private void RenderPlay(Graphics g)
        {
            Inputs.Update();
            map.Render(g);
        }
        private void RenderPause(Graphics g)
        {
        }
        private void RenderResetLevel(Graphics g)
        {
        }
        private void RenderNextLevel(Graphics g)
        {
        }
        private void RenderMenu(Graphics g)
        {
        }
        private void RenderGameOver(Graphics g)
        {
        }
        #endregion

        #region Update
        private void UpdateInitialize()
        {
            map = new Map1(size);
            State++;
        }
        private void UpdatePlay()
        {
            map.Update();
        }
        private void UpdatePause()
        {
        }
        private void UpdateResetLevel()
        {
        }
        private void UpdateNextLevel()
        {
        }
        private void UpdateMenu()
        {
        }
        private void UpdateGameOver()
        {
        }
        #endregion
    }
}
