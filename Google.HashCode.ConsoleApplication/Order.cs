using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Google.HashCode.ConsoleApplication
{
    public class Order
    {
        public Point Destination { get; set; }

        public IList<int> NbItemsOfType { get; set; }

        public Order(int nbItemsTotal)
        {
            NbItemsOfType = Enumerable.Repeat(0, nbItemsTotal).ToList();
        }
    }
}
