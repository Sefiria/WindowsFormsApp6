using Core.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Definitions;

namespace Core
{
    public class TrafficMgr
    {
        public TileMap tileMap;
        public List<Car> Cars;

        public TrafficMgr(TileMap tileMap)
        {
            this.tileMap = tileMap;
            Cars = new List<Car>();
        }

        public void Update(int TrafficFlow)
        {
            List<Car> NextCars = new List<Car>(Cars);
            foreach (Car car in NextCars)
            {
                car.Update();

                if (!car.Exists)
                {
                    Cars.Remove(car);
                }
            }
            Cars = NextCars;

            if (Cars.Count < TrafficFlow && Tools.rnd.Next(20) == 10)
            {
                Point? tilePos = tileMap.GetRandomRouteTilePosition();
                PathDots? dot = null;
                if (tilePos != null)
                    dot = tileMap.GetRandomDotIn(tilePos.Value);
                if (dot != null)
                {
                    CarPath path = Tools.FindAnyPath(tilePos.Value.X, tilePos.Value.Y, tileMap, dot.Value);
                    if (path != null)
                    {
                        Cars.Add(new Car(tileMap, tilePos.Value, path));
                    }
                }
            }
        }

        public void Draw(Graphics g, float zoom = 1F)
        {
            foreach(Car car in Cars)
            {
                car.Draw(g, zoom);
            }
        }
    }
}
