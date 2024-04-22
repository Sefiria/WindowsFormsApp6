using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp25
{
    internal class EntityFactory
    {
        public static Entity CreateTable(int tile_left, int tile_top)
        {
            var A = new Entity("Table", DB.Tex.TableA, tile_left, tile_top);
            var B = new Entity("Table", DB.Tex.TableB, tile_left, tile_top + 1);
            var C = new Entity("Table", DB.Tex.TableC, tile_left, tile_top + 2);
            A.Linked.AddRange(new[] { B, C });
            B.Linked.AddRange(new[] { A, C });
            C.Linked.AddRange(new[] { A, B });
            return A;
        }
    }
}
