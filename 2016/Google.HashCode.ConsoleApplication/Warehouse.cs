using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Google.HashCode.ConsoleApplication
{
    public class Warehouse
    {
        public Warehouse(int nbProductTotal)
        {
            NbItemsOfType = Enumerable.Repeat(0, nbProductTotal).ToList();
        }

        public int Id { get; set; }
        public IList<int> NbItemsOfType { get; set; }
        public Point Position { get; set; }

        public void RemoveProducts(IEnumerable<int> groupOfProduct)
        {
            foreach (var product in groupOfProduct)
            {
                NbItemsOfType[product] -= 1;
            }
        }
    }
}