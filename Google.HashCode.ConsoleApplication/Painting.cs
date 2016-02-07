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
        /// Zone de dessin.
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
            ColumnsCount = columnsCount;
            RowsCount = rowsCount;
            DrawingZone = new char[columnsCount, rowsCount];

            // Fill drawing zone with blanks :
            for (int columnIndex = 0; columnIndex < columnsCount; columnIndex++)
                for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                    this.DrawingZone[columnIndex, rowIndex] = '.';
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
        /// Peint le point représenté par l'abscisse et l'ordonnée spécifiées.
        /// </summary>
        /// <param name="columnIndex">Abscisse (x).</param>
        /// <param name="rowIndex">Ordonnée (y).</param>
        public void Paint(int columnIndex, int rowIndex)
        {
            if (!this.ContainsAbscissa(columnIndex))
                throw new ArgumentOutOfRangeException("columnIndex");
            if (!this.ContainsOrdinate(rowIndex))
                throw new ArgumentOutOfRangeException("rowIndex");

            this.DrawingZone[columnIndex, rowIndex] = '#';
        }
    }
}
