using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.HashCode.ConsoleApplication.Practice
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
        /// at [R , C] . In particular, the command “PAINT_SQUARE R C 0” paints a single cell [R , C ].
        /// For the command to be valid, the entire square has to fit within the dimensions of the painting.
        /// </summary>
        /// <param name="columnIndex">Abscissa (x).</param>
        /// <param name="rowIndex">Ordinate (y).</param>
        /// <param name="radius">Radius (excluded center).</param>
        public void PaintSquare(int rowIndex, int columnIndex, int radius)
        {
            // Validate command :
            var upperLeftCorner = new Point(columnIndex - radius, rowIndex + radius);
            var upperRightCorner = new Point(columnIndex + radius, rowIndex + radius);
            var lowerLeftCorner = new Point(columnIndex - radius, rowIndex - radius);
            var lowerRightCorner = new Point(columnIndex + radius, rowIndex - radius);
            if (!this.Painting.Contains(upperLeftCorner))
                throw new ArgumentOutOfRangeException("upperLeftCorner");
            if (!this.Painting.Contains(upperRightCorner))
                throw new ArgumentOutOfRangeException("upperRightCorner");
            if (!this.Painting.Contains(lowerLeftCorner))
                throw new ArgumentOutOfRangeException("lowerLeftCorner");
            if (!this.Painting.Contains(lowerRightCorner))
                throw new ArgumentOutOfRangeException("lowerRightCorner");
            // Execute command :
            for (int i = upperLeftCorner.X; i <= lowerRightCorner.X; i++)
                for (int j = upperLeftCorner.Y; j <= lowerRightCorner.Y; j++)
                    this.Painting.Paint(i, j);
        }

        /// <summary>
        /// PAINT_LINE R 1 C 1 R 2 C 2
        /// Paints all cells in a horizontal or vertical run between [R1, C1] and [R2, C2],
        /// including both ends, as long as both cells are in the same row or column or both.
        /// That is, at least one of the two has to be true: R1 = R2 or/and C1 = C2.
        /// </summary>
        public void PaintLine(int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex)
        {
            // Validate command :
            if (!this.Painting.Contains(new Point(startColumnIndex, startRowIndex)))
                throw new ArgumentOutOfRangeException("start");
            if (!this.Painting.Contains(new Point(endColumnIndex, endRowIndex)))
                throw new ArgumentOutOfRangeException("end");
            if (startRowIndex > endRowIndex || startColumnIndex > endColumnIndex)
                throw new ArgumentException("Command’s start coordinate must be lesses than end coordinate.");
            if (startRowIndex != endRowIndex && startColumnIndex != endColumnIndex)
                throw new ArgumentException("Command’s coordinates must be a line or a row.");
            // Execute command :
            // Draw a line :
            if (startRowIndex == endRowIndex)
                for (int columnIndex = startColumnIndex; columnIndex <= endColumnIndex; columnIndex++)
                    this.Painting.Paint(columnIndex, startRowIndex);
            // Draw a column :
            else if (startColumnIndex == endColumnIndex)
                for (int rowIndex = startRowIndex; rowIndex <= endRowIndex; rowIndex++)
                    this.Painting.Paint(startColumnIndex, rowIndex);
        }

        /// <summary>
        /// ERASE_CELL R C
        /// Clears the cell [R, C].
        /// </summary>
        public void EraseCell(int rowIndex, int columnIndex)
        {
            // Validate command :
            if (!this.Painting.Contains(new Point(columnIndex, rowIndex)))
                throw new ArgumentOutOfRangeException("cell");
            // Execute command :
            this.Painting.Erase(columnIndex, rowIndex);
        }
    }
}