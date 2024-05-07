using Tooling;

namespace DOSBOX.Suggestions.plants.Fruits
{
    public class Tomate : Fruit
    {
        public Tomate(vecf vec) : base(vec)
        {
            CreateGraphics();
        }

        private void CreateGraphics()
        {
            g = new byte[5, 5]
            {
                { 2, 3, 3, 3, 2 },
                { 3, 1, 2, 3, 3 },
                { 3, 2, 3, 3, 3 },
                { 3, 3, 3, 3, 3 },
                { 2, 3, 3, 3, 2 },
            };
        }
    }
}
