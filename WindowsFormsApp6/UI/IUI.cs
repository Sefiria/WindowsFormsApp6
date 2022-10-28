using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6.UI
{
    public interface IUI
    {
        string ID { get; set; }
        int X { get; set; }
        int Y { get; set; }
        int W { get; set; }
        int H { get; set; }
        [JsonIgnore] Bitmap Image { get; }
        event EventHandler OnClick;


        void Update();
        void Draw(Graphics g = null, Color? bounds_color = null);
        void Clicked();
    }
}
