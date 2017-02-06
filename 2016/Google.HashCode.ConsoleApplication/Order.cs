using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace Google.HashCode.ConsoleApplication
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Order
    {
        public Order(int nbItemsTotal)
        {
            NbItemsOfType = Enumerable.Repeat(0, nbItemsTotal).ToList();
        }

        public bool Completed { get; set; }

        public Point Destination { get; set; }

        public int Id { get; set; }

        public IList<int> NbItemsOfType { get; set; }

        private string DebuggerDisplay
        {
            get
            {
                return string.Join("",
                                   NbItemsOfType.Select(
                                       (nbOfItems, i) => nbOfItems > 0 ? string.Format("{0}: {1}, ", i, nbOfItems) : ""));
            }
        }
    }
}