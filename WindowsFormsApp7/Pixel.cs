using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp7
{
    public class Pixel
    {
        [JsonIgnore]
        public List<Color> Gradient = new List<Color>() { Color.Black };
        public List<int> GradientArgb => Gradient.Select(x => x.ToArgb()).ToList();
        public bool IsLerp = true;
        
        public void Update(int x, int y)
        {
        }

        public Pixel()
        {
        }
        public Pixel(Pixel copy)
        {
            Gradient = copy.Gradient;
        }
    }
}
