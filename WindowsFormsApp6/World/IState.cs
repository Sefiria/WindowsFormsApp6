using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp6.Particules;

namespace WindowsFormsApp6.World
{
    public interface IState
    {
        int TileSz { get; }
        [JsonIgnore] Bitmap StaticImage { get; set; }
        List<Particule> Particules { get; set; }
        IState PreviousState { get; set; }
        void Update();
        void Draw();
        void DrawStatic();
        void MouseDown(MouseEventArgs e);
    }
}
