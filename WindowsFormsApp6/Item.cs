using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp6
{
    public class Item<T> where T : class
    {
        public T ItemRef { get; set; }
        public int Count { get; set; }

        public Item(T itemRef, int count = 1)
        {
            ItemRef = itemRef;
            Count = count;
        }
    }
}
