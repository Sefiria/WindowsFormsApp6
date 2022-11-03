using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp6.Properties;

namespace WindowsFormsApp6.World.Entities
{
    public class Entity
    {
        public float X, Y;
        public bool LookAtRight = false;
        [JsonIgnore] public Bitmap ImageLeft = null;
        [JsonIgnore] public Bitmap ImageRight = null;
        [JsonIgnore] public Bitmap ImageLeftSelected = null;
        [JsonIgnore] public Bitmap ImageRightSelected = null;

        public Entity(float x, float y, string resource)
        {
            X = x;
            Y = y;

            var image = (Resources.ResourceManager.GetObject(resource) as Bitmap).Transparent();
            ImageLeft = image;
            ImageRight = new Bitmap(image);
            ImageRight.RotateFlip(RotateFlipType.RotateNoneFlipX);

            var imageSelected = (Resources.ResourceManager.GetObject($"{resource}_selected") as Bitmap)?.Transparent();
            ImageLeftSelected = imageSelected != null ? imageSelected : image;
            ImageRightSelected = new Bitmap(imageSelected != null ? imageSelected : image);
            ImageRightSelected.RotateFlip(RotateFlipType.RotateNoneFlipX);
        }

        public virtual void Update() { }
        public virtual void Draw(bool selected = false)
        {
            var img = selected ? (LookAtRight ? ImageRightSelected : ImageLeftSelected) : (LookAtRight ? ImageRight : ImageLeft);
            Core.g.DrawImage(img, X, Y);
        }
    }
}
