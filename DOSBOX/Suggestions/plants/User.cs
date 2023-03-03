using DOSBOX.Suggestions.plants.Fruits;
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
        List<(IPlant plant, int count)> seeds = new List<(IPlant plant, int count)>();
        int selected_seed = 0;

        public User()
        {
            seeds.Clear();
            seeds.Add((PlantFactory.CreateRandom(vecf.Zero), 1));
        }

        bool fruittaken = false;
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


            if (KB.IsKeyUp(KB.Key.Space))
                fruittaken = false;
            if (KB.IsKeyPressed(KB.Key.Space))
            {
                var dic = Garden.Instance.GetPlantsFruits();
                foreach (var kv in dic)
                {
                    var fruit = kv.Value.FirstOrDefault(f => f.IsHover(cursor));
                    if (fruit != null)
                    {
                        kv.Key.fruits.Remove(fruit);
                        string name = fruit.GetType().Name;
                        if (plants.Data.Fruits.ContainsKey(name))
                            plants.Data.Fruits[name]++;
                        else
                            plants.Data.Fruits[name] = 1;
                        fruittaken = true;
                        break;
                    }
                }
            }
            if (!fruittaken && KB.IsKeyDown(KB.Key.Space))
            {
                if (cursor.y < Garden.FloorLevel)
                    WaterSprayShot();
                else if(KB.IsKeyPressed(KB.Key.Space))
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
            if (!string.IsNullOrWhiteSpace(plants.Data.SelectedSeed))
            {
                var plant = plants.Data.DropSeed(cursor);
                if(plant != null)
                    Garden.Instance.ScenePlants.Add(plant);
            }
        }

        public void Display(int layer)
        {
            for (int i = -2; i < 3; i++)
            {
                if(cursor.x + i >= 0 && cursor.x + i < 64)
                    Core.Layers[layer][cursor.i.x + i, cursor.i.y] = Graphic.InvertOfForeground(cursor.i.x + i, cursor.i.y);
                if(cursor.y + i >= 0 && cursor.y + i < 64)
                    Core.Layers[layer][cursor.i.x, cursor.i.y + i] = Graphic.InvertOfForeground(cursor.i.x, cursor.i.y + i);
            }
        }
    }
}
