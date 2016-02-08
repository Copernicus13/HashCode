using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Google.HashCode.ConsoleApplication
{
    /// <summary>
    /// Représente la zone de dessin.
    /// </summary>
    public class Painting
    {
        /// <summary>
        /// Zone de dessin sous forme de tableau à 2 dimensions.
        /// </summary>
        public char[,] DrawingZone { get; set; }

        /// <summary>
        /// Nombre de colonnes (abscisses, x).
        /// </summary>
        private int ColumnsCount { get; set; }

        /// <summary>
        /// Nombre de lignes (ordonnées, y).
        /// </summary>
        private int RowsCount { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la zone de dessin.
        /// </summary>
        /// <param name="columnsCount">Nombre de colonnes (abscisses, x).</param>
        /// <param name="rowsCount">Nombre de lignes (ordonnées, y).</param>
        public Painting(int columnsCount, int rowsCount)
        {
            this.RowsCount = rowsCount;
            this.ColumnsCount = columnsCount;
            this.DrawingZone = new char[columnsCount, rowsCount];

            // Fill drawing zone with blanks :
            for (int rowIndex = 0; rowIndex < this.RowsCount; rowIndex++)
                for (int columnIndex = 0; columnIndex < this.ColumnsCount; columnIndex++)
                    this.DrawingZone[rowIndex, columnIndex] = '.';
        }

        /// <summary>
        /// Initialise une nouvelle instance de la zone de dessin
        /// avec un dessin spécifié.
        /// </summary>
        public Painting(string drawing)
        {
            var rows = drawing.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            this.RowsCount = rows.Length;
            this.ColumnsCount = rows[0].Length;
            this.DrawingZone = new char[this.RowsCount, this.ColumnsCount];

            // Fill drawing zone :
            for (int rowIndex = 0; rowIndex < this.RowsCount; rowIndex++)
                for (int columnIndex = 0; columnIndex < this.ColumnsCount; columnIndex++)
                    this.DrawingZone[rowIndex, columnIndex] = rows[rowIndex][columnIndex];
        }

        /// <summary>
        /// Indique si le <see cref="Point"/> spécifié est inclus dans la zone de dessin.
        /// </summary>
        /// <param name="point">Vrai, si le <see cref="Point"/> spécifié est inclus dans la zone de dessin.
        /// Faux, sinon.</param>
        public bool Contains(Point point)
        {
            return this.Contains(point.X, point.Y);
        }

        /// <summary>
        /// Indique si le point formé par les coordonnées spécifiées est inclus dans la zone de dessin.
        /// </summary>
        /// <param name="point">Vrai, si le point formé par les coordonnées spécifiées est inclus dans la zone de dessin.
        /// Faux, sinon.</param>
        public bool Contains(int columnIndex, int rowIndex)
        {
            return this.ContainsAbscissa(columnIndex) && this.ContainsOrdinate(rowIndex);
        }

        private bool ContainsAbscissa(int columnIndex)
        {
            return columnIndex >= 0 && columnIndex < ColumnsCount;
        }

        private bool ContainsOrdinate(int rowIndex)
        {
            return rowIndex >= 0 && rowIndex < RowsCount;
        }

        /// <summary>
        /// Paint the point given by abscissa and ordinate.
        /// </summary>
        /// <param name="columnIndex">Abscissa (x).</param>
        /// <param name="rowIndex">Ordinate (y).</param>
        public void Paint(int columnIndex, int rowIndex)
        {
            if (!this.ContainsAbscissa(columnIndex))
                throw new ArgumentOutOfRangeException("columnIndex");
            if (!this.ContainsOrdinate(rowIndex))
                throw new ArgumentOutOfRangeException("rowIndex");

            this.DrawingZone[rowIndex, columnIndex] = '#';
        }

        public void Display()
        {
            for (int rowIndex = 0; rowIndex < this.RowsCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.ColumnsCount; columnIndex++)
                    Console.Write(this.DrawingZone[rowIndex, columnIndex]);
                Console.WriteLine();
            }
        }

        public override string ToString()
        {
            var capacity = (this.RowsCount * this.ColumnsCount) + this.RowsCount;
            var draw = new StringBuilder(capacity);
            for (int rowIndex = 0; rowIndex < this.RowsCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < this.ColumnsCount; columnIndex++)
                    draw.Append(this.DrawingZone[rowIndex, columnIndex]);
                draw.AppendLine();
            }
            return draw.ToString();
        }
    }
}
