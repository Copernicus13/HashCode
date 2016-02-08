using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.HashCode.ConsoleApplication
{
    public class PaintingMachine
    {
        private Painting Painting { get; set; }

        public PaintingMachine(Painting painting)
        {
            this.Painting = painting;
        }

        /// <summary>
        /// PAINT_SQUARE R C S
        /// Paints all cells within the square of (2S + 1) × (2S + 1) dimensions centered
        /// at [R , C ] . In particular, the command “PAINT_SQUARE R C 0” paints a single cell [R , C ].
        /// For the command to be valid, the entire square has to fit within the dimensions of the painting.
        /// </summary>
        /// <param name="columnIndex">Abscissa (x).</param>
        /// <param name="rowIndex">Ordinate (y).</param>
        /// <param name="radius">Radius (excluded center).</param>
        public void PaintSquare(int rowIndex, int columnIndex, int radius)
        {
            // Validate command :
            var lowerLeftCorner = new Point(columnIndex - radius, rowIndex - radius);
            var lowerRightCorner = new Point(columnIndex + radius, rowIndex - radius);
            var upperLeftCorner = new Point(columnIndex - radius, rowIndex + radius);
            var upperRightCorner = new Point(columnIndex + radius, rowIndex + radius);
            if (!this.Painting.Contains(lowerLeftCorner))
                throw new ArgumentOutOfRangeException("lowerLeftCorner");
            if (!this.Painting.Contains(lowerRightCorner))
                throw new ArgumentOutOfRangeException("lowerRightCorner");
            if (!this.Painting.Contains(upperLeftCorner))
                throw new ArgumentOutOfRangeException("upperLeftCorner");
            if (!this.Painting.Contains(upperRightCorner))
                throw new ArgumentOutOfRangeException("upperRightCorner");

            // Execute command :
            for (int i = lowerLeftCorner.X; i <= upperRightCorner.X; i++)
                for (int j = lowerLeftCorner.Y; j <= upperRightCorner.Y; j++)
                    this.Painting.Paint(i, j);
        }

        //PAINT_LINE R 1 C 1 R 2 C 2 - paints all cells in a horizontal or vertical run between [R 1, C 1] and [R 2,
        //C2] , including both ends, a s long as both cells are in the same row or column or both. That is, at
        //least one of the two has to be true: R1 = R2 or/and C1 = C2 .
        //ERASE_CELL R C - clears the cell [ R , C ].
    }
}
