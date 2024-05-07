using DOSBOX.Suggestions.plants.Fruits;
using DOSBOX.Utilities;
using System.Linq;
using Tooling;
using static DOSBOX.Utilities.effects.effect;

namespace DOSBOX.Suggestions.plants
{
    public class User
    {
        vecf cursor;
        waterdrop waterdrop = new waterdrop();
        public bool HasMovedCamX = false;

        public User()
        {
            cursor = new vecf(32, 32);
        }

        bool fruittaken = false;
        public void Update()
        {
            HasMovedCamX = false;
            float speed = 1.5F;

            if (KB.IsKeyDown(KB.Key.Left))
                cursor.x -= speed;
            if (KB.IsKeyDown(KB.Key.Right))
                cursor.x += speed;
            if (KB.IsKeyDown(KB.Key.Up))
                cursor.y -= speed;
            if (KB.IsKeyDown(KB.Key.Down))
                cursor.y += speed;

            if(cursor.y < 0) cursor.y = 0;
            if (cursor.y > 63) cursor.y = 63;

            var camx = Core.Cam.i.x;
            if (cursor.x < 0)
            {
                cursor.x = 0;
                if (camx > 0)
                {
                    Core.Cam.x--;
                    HasMovedCamX = true;
                }
            }
            if (cursor.x > 63)
            {
                cursor.x = 63;
                if (camx + 1 + cursor.x < Core.Layers[0].GetLength(0))
                {
                    Core.Cam.x++;
                    HasMovedCamX = true;
                }
            }


            if (KB.IsKeyPressed(KB.Key.D))
            {
                var plant = Data.Garden.ScenePlants.FirstOrDefault(p => p.px_seed.Any(px => px == (cursor + Core.Cam - p.vec).i));
                if (plant != null)
                {
                    string dna = (plant as Plant<OGM>)?.DNA;
                    int fruitsPrice = 0;
                    if (dna == null)
                        fruitsPrice  = SellCatalogue.FruitsPrice[plant.CreateFruit(vecf.Zero).GetType().Name];
                    else
                        fruitsPrice = SellCatalogue.PriceOf($"OGM_{dna}");
                        Data.Instance.Coins += plant.GetPotential() + fruitsPrice;
                    Data.Garden.ScenePlants.Remove(plant);
                }
            }

            if (KB.IsKeyUp(KB.Key.Space))
                fruittaken = false;
            if (KB.IsKeyPressed(KB.Key.Space))
            {
                var dic = Garden.Instance.GetPlantsFruits();
                foreach (var kv in dic)
                {
                    var fruit = kv.Value.FirstOrDefault(f => f.IsHover(cursor + Core.Cam));
                    if (fruit != null)
                    {
                        kv.Key.fruits.Remove(fruit);
                        string name = fruit.GetType().Name;
                        if (name == "OGM")
                            name += $"_{(fruit as OGM).dna}";
                        if (Data.Instance.Fruits.ContainsKey(name))
                            Data.Instance.Fruits[name]++;
                        else
                            Data.Instance.Fruits[name] = 1;
                        fruittaken = true;
                        break;
                    }
                }
            }
            if (!fruittaken && KB.IsKeyDown(KB.Key.Space))
            {
                if (cursor.y < Data.Garden.FloorLevel)
                    WaterSprayShot();
                else if (KB.IsKeyPressed(KB.Key.Space))
                    DropSeed();
            }
        }

        private void WaterSprayShot()
        {
            if (Data.Instance.WaterBucket == 0)
                return;
            Data.Instance.WaterBucket--;
            if (Data.Instance.WaterBucket < 0)
                Data.Instance.WaterBucket = 0;

            int count = 4;
            float spread = 3F;
            for (int i = 0; i < count; i++)
            {
                Data.Garden.WaterDrops.Add(new WaterDrop()
                {
                    vec = cursor + Core.Cam,
                    look = new vecf(((Core.RND.Next(21) - 10F) / 10F) * spread, (Core.RND.Next(5, 10) / 10F) * spread)
                });
            }
        }

        private void DropSeed()
        {
            if (string.IsNullOrWhiteSpace(Data.Instance.SelectedSeed) && Data.Instance.Seeds.Count > 0)
                Data.Instance.SelectedSeed = Data.Instance.Seeds.ElementAt(0).Key;

            if (!string.IsNullOrWhiteSpace(Data.Instance.SelectedSeed))
            {
                var plant = Data.Instance.DropSeed(cursor + Core.Cam);
                if(plant != null)
                    Data.Garden.ScenePlants.Add(plant);
            }
        }

        public void Display(int layer)
        {
            waterdrop.Display(layer, v: new vec(1, 1));
            int y = 1 + waterdrop.h(0) + 1;
            Graphic.DisplayRectAndBounds(1, y, waterdrop.w(0), Data.Garden.FloorLevel - 2 - y, 2, 1, 1, layer);
            int v = (int) ((Data.Garden.FloorLevel - 4 - y) * (Data.Instance.WaterBucket / (float)Data.Instance.WaterBucketMax));
            Graphic.DisplayRect(2, y + 1, waterdrop.w(0) - 2, Data.Garden.FloorLevel - 4 - y - v, 4, layer);


            for (int i = -2; i < 3; i++)
            {
                if (cursor.x + i >= 0 && cursor.x + i < 64)
                    Core.Layers[layer][cursor.i.x + i, cursor.i.y] = Graphic.InvertOfForeground(Core.Cam.i.x + cursor.i.x + i, Core.Cam.i.y + cursor.i.y, layer);
                if (cursor.y + i >= 0 && cursor.y + i < 64)
                    Core.Layers[layer][cursor.i.x, cursor.i.y + i] = Graphic.InvertOfForeground(Core.Cam.i.x + cursor.i.x, Core.Cam.i.y + cursor.i.y + i, layer);
            }
        }
    }
}
