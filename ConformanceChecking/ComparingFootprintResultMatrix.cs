using System;
using System.Collections.Generic;

namespace pgmpm.ConformanceChecking
{
    /// <summary>
    /// A representation for a Comparing-Footprint-Result-Matrix
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public class ComparingFootprintResultMatrix : ComparingFootprint
    {
        //ResultMatrix with ResultMatrix-cellTypes

        /// <summary>
        /// Compares two footprints and saves the result in ResultMatrix
        /// </summary>
        /// <param name="footprint1">The first footprint</param>
        /// <param name="footprint2">The second footprint</param>
        /// <autor>Andrej Albrecht</autor>
        public ComparingFootprintResultMatrix(ComparingFootprint footprint1, ComparingFootprint footprint2)
        {
            AddEventHeader(footprint1);
            AddEventHeader(footprint2);

            ResultMatrix = new ResultCellType[HeaderWithEventNames.Count, HeaderWithEventNames.Count];

            for (int row = 0; row < HeaderWithEventNames.Count; row++)
                for (int column = 0; column < HeaderWithEventNames.Count; column++)
                {
                    CellType matrix1Cell = footprint1.GetFootprintCellState(HeaderWithEventNames[row], HeaderWithEventNames[column]);
                    CellType matrix2Cell = footprint2.GetFootprintCellState(HeaderWithEventNames[row], HeaderWithEventNames[column]);

                    if (matrix1Cell.Equals(matrix2Cell))
                    {
                        //If the cellstates of the both footprints are equal, then they have no differences
                        ResultMatrix[row, column] = ResultCellType.NoDifferences;
                    }
                    else
                    {
                        // find the right CellState for the differences of both footprints
                        ResultMatrix[row, column] = ComparingFootprintResultMatrixCell.GetResultCellType(matrix1Cell, matrix2Cell);
                    }
                }
        }

        public ComparingFootprintResultMatrix()
        {

        }

        public new ResultCellType[,] ResultMatrix { get; set; }

        /// <summary>
        /// Adds the event header from a given footprint
        /// </summary>
        /// <param name="footprint">A Footprint</param>
        /// <autor>Andrej Albrecht</autor>
        private void AddEventHeader(ComparingFootprint footprint)
        {
            foreach (String name in footprint.HeaderWithEventNames)
                AddEventHeader(name);
        }

        /// <summary>
        /// Adds the event header from a list of Strings
        /// </summary>
        /// <param name="headerWithEventNames">A list of Strings with headernames</param>
        public void AddEventHeader(List<String> headerWithEventNames)
        {
            foreach (String name in headerWithEventNames)
                AddEventHeader(name);
        }

        /// <summary>
        /// Calculates the number of differences of the Comparing-Footprint-Result-Matrix
        /// </summary>
        /// <returns>Returns the number of differences of the Comparing-Footprint-Result-Matrix</returns>
        /// <autor>Andrej Albrecht</autor>
        public int GetNumberOfDifferences()
        {
            int differences = 0;
            for (int row = 0; row < HeaderWithEventNames.Count; row++)
                for (int column = 0; column < HeaderWithEventNames.Count; column++)
                    if (!ResultMatrix[row, column].Equals(ResultCellType.NoDifferences))
                        differences++;
            return differences;
        }

        /// <summary>
        /// Calculates the number of opportunities of the Comparing-Footprint-Result-Matrix
        /// </summary>
        /// <returns>Returns the number of opportunities of the Comparing-Footprint-Result-Matrix</returns>
        /// <autor>Andrej Albrecht</autor>
        public int GetNumberOfOpportunities()
        {
            return HeaderWithEventNames.Count * HeaderWithEventNames.Count;
        }
    }
}
