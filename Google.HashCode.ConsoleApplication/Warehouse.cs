using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Google.HashCode.ConsoleApplication
{
    public class Warehouse
    {
        public Point Position { get; set; }

        public IList<int> NbItemsOfType { get; set; }

        public Warehouse(int nbProductTotal)
        {
            NbItemsOfType = Enumerable.Repeat(0, nbProductTotal).ToList();
        }
    }
}
