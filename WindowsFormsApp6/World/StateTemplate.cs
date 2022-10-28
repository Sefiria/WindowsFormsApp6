using System.Collections.Generic;
using System.Drawing;
using Newtonsoft.Json;
using System.Windows.Forms;
using WindowsFormsApp6.Particules;
using WindowsFormsApp6.World.Blocs;
using WindowsFormsApp6.World.Structures;

namespace WindowsFormsApp6.World
{
    public class StateTemplate : IState
    {
        public int TileSz => 0;
        [JsonIgnore] public Bitmap StaticImage { get; set; }
        public List<Particule> Particules { get; set; } = new List<Particule>();
        public IState PreviousState { get; set; } = null;

        public StateTemplate()
        {
            StaticImage = new Bitmap(Core.W, Core.H);
        }

        public void Update()
        {
        }

        public void DrawStatic()
        {
        }

        public void Draw()
        {
            Core.g.DrawImage(StaticImage, 0, 0);
        }

        public void MouseDown(MouseEventArgs e)
        {
        }

        public void ReturnToPreviousState()
        {
            Data.Instance.State = PreviousState;
            PreviousState = null;
        }
    }
}
