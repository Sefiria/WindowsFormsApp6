using DOSBOX.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOSBOX.Suggestions
{
    public class User
    {
        vecf cursor = new vecf(32, 32);
        List<(Plant plant, int count)> seeds = new List<(Plant plant, int count)>();
        int selected_seed = 0;

        public User()
        {
            seeds.Clear();
            seeds.Add((new Plant(vecf.Zero), 1));
        }

        public void Update()
        {
            float speed = 1.5F;

            if (KB.IsKeyDown(KB.Key.Left))
                cursor.x -= speed;
            if (KB.IsKeyDown(KB.Key.Right))
                cursor.x += speed;
            if (KB.IsKeyDown(KB.Key.Up))
                cursor.y -= speed;
            if (KB.IsKeyDown(KB.Key.Down))
                cursor.y += speed;

            if(cursor.x < 0) cursor.x = 0;
            if(cursor.y < 0) cursor.y = 0;
            if(cursor.x > 63) cursor.x = 63;
            if(cursor.y > 63) cursor.y = 63;


            if (KB.IsKeyDown(KB.Key.Space))
            {
                if (cursor.y < Garden.FloorLevel)
                    WaterSprayShot();
                else
                    DropSeed();
            }
        }

        private void WaterSprayShot()
        {
            int count = 4;
            float factor = 3F;
            for (int i = 0; i < count; i++)
            {
                Garden.Instance.WaterDrops.Add(new WaterDrop()
                {
                    vec = cursor,
                    look = new vecf(((Core.RND.Next(21) - 10F) / 10F) * factor, (Core.RND.Next(5, 10) / 10F) * factor)
                });
            }
        }

        private void DropSeed()
        {
            again:
            if(seeds.Count == 0) return;
            if (selected_seed >= seeds.Count) selected_seed = seeds.Count - 1;
            if (seeds[selected_seed].count == 0)
            {
                seeds.RemoveAt(selected_seed);
                goto again;
            }
            Garden.Instance.ScenePlants.Add((Plant)Activator.CreateInstance(seeds[selected_seed].plant.GetType(), new[] { cursor }));
            seeds[selected_seed] = (seeds[selected_seed].plant, seeds[selected_seed].count - 1);
            if (seeds[selected_seed].count == 0)
                seeds.RemoveAt(selected_seed);
        }

        public void Display(int layer)
        {
            for (int i = -2; i < 3; i++)
            {
                if(cursor.x + i >= 0 && cursor.x + i < 64)
                    Core.Layers[layer][cursor.i.x + i, cursor.i.y] = Graphic.InvertOf(Core.Layers[0][cursor.i.x + i, cursor.i.y], layer);
                if(cursor.y + i >= 0 && cursor.y + i < 64)
                    Core.Layers[layer][cursor.i.x, cursor.i.y + i] = Graphic.InvertOf(Core.Layers[0][cursor.i.x, cursor.i.y + i], layer);
            }
        }
    }
}
