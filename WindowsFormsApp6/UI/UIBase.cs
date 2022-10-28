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
    public class UIBase : IUI
    {
        public string ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        [JsonIgnore] public Bitmap Image { get; set; }
        public static Font Font = new Font("Segoe UI", 12F);
        public virtual event EventHandler OnClick;

        [JsonConstructor]
        public UIBase()
        {
            X = 0;
            Y = 0;
            W = 0;
            H = 0;
            Image = null;
        }
        public UIBase(int x, int y, Bitmap img = null, int w = 0, int h = 0)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
            Image = img;
        }

        public virtual void Draw(Graphics g = null, Color? bounds_color = null) { }
        public virtual void Update() { }
        public virtual void Clicked() { }
    }
}
