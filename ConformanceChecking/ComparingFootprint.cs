using System;
using System.Collections.Generic;

namespace pgmpm.ConformanceChecking
{
    /// <summary>
    /// A representation for a Comparing-Footprint-Matrix
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public class ComparingFootprint
    {
        /// <summary>
        /// A two dimensional array of ComparingFootprintMatrixCell
        /// </summary>
        public CellType[,] ResultMatrix { get; set; }

        public List<String> HeaderWithEventNames { get; set; }

        public ComparingFootprint(CellType[,] resultMatrix = null)
        {
            ResultMatrix = resultMatrix;
            HeaderWithEventNames = new List<String>();
        }


        /// <summary>
        /// Add a string to the eventHeader
        /// </summary>
        /// <param name="text">The name of an event</param>
        /// <autor>Andrej Albrecht</autor>
        public void AddEventHeader(String text)
        {
            if (text == null) return;

            if (!HeaderWithEventNames.Contains(text))
                HeaderWithEventNames.Add(text);
        }

        /// <summary>
        /// Add the events from a ComparingFootprint list to the eventHeader.
        /// Finding common set of footprint header, so the names of all Events
        /// </summary>
        /// <param name="footprintList">A list of ComparingFootprint's</param>
        /// <autor>Andrej Albrecht</autor>
        internal void AddEventHeader(List<ComparingFootprint> footprintList)
        {
            foreach (ComparingFootprint footprint in footprintList)
                foreach (String header in footprint.HeaderWithEventNames)
                    AddEventHeader(header);
        }

#pragma warning disable 1570
        /// <summary>
        /// Method to merge a list of footprints to one result footprint.
        /// This method finds the CellTypes for the resultFootprint.
        /// 
        /// Merge each cell individually: 
        /// The cells of all other footprints are checked. 
        /// If in a cell of all footprints with a pound sign occurs, 
        /// then a hash mark character is also entered in the cell of the result Footprint.
        /// {CellState of a list of Footprints} -> Result footprint
        /// {#,#,#}       ->  #
        /// {->,#,#}      ->  ->
        /// {#,<-,#}      ->  <-
        /// {#,->,<-,#}   ->  ||
        /// </summary>
#pragma warning restore 1570
        /// <param name="footprintList">A list of ComparingFootprint's</param>
        /// <autor>Andrej Albrecht</autor>
        internal void MergeFootprints(List<ComparingFootprint> footprintList)
        {
            AddEventHeader(footprintList);
            ResultMatrix = new CellType[HeaderWithEventNames.Count, HeaderWithEventNames.Count];

            //Two loops, to go through each cell from a virtual table.
            for (int row = 0; row < HeaderWithEventNames.Count; row++)
                for (int column = 0; column < HeaderWithEventNames.Count; column++)
                {
                    int countSharp = 0;
                    int countLeft = 0;
                    int countRight = 0;
                    int countParallel = 0;
                    int countLoop = 0;

                    //go over all footprints and find the best CellTypes
                    foreach (ComparingFootprint footprint in footprintList)
                    {
                        switch (footprint.GetFootprintCellState(HeaderWithEventNames[row], HeaderWithEventNames[column]))
                        {
                            case CellType.Nothing:
                                countSharp++;
                                break;
                            case CellType.Left:
                                countLeft++;
                                break;
                            case CellType.Right:
                                countRight++;
                                break;
                            case CellType.Parallel:
                                countParallel++;
                                break;
                            case CellType.Loop:
                                countLoop++;
                                break;
                        }
                    }

                    //#
                    if (footprintList.Count == countSharp)
                        ResultMatrix[row, column] = CellType.Nothing; // #
                    // ||
                    else if (footprintList.Count == countParallel || countParallel > 0 && countRight == 0 && countLeft == 0 || countRight > 0 && countLeft > 0)
                        ResultMatrix[row, column] = CellType.Parallel; // ||
                    // ->
                    else if (footprintList.Count == countRight || countRight > 0 && countLeft == 0)
                        ResultMatrix[row, column] = CellType.Right; // ->
                    // <-
                    else if (footprintList.Count == countLeft || countLeft > 0 && countRight == 0)
                        ResultMatrix[row, column] = CellType.Left; // <-
                    // @
                    else if (footprintList.Count == countLoop || countLoop > 0)
                        ResultMatrix[row, column] = CellType.Loop; // @
                    else
                        throw new NotImplementedException("countFootprint:" + footprintList.Count + " countSharp:" + countSharp + " countLeft:" + countLeft + " countRight:" + countRight + " countParallel:" + countParallel);
                }
        }

        /// <summary>
        /// Get the CellState at a specific cell from a footprint
        /// </summary>
        /// <param name="headerWithEventNameY">The cellname from the y direction</param>
        /// <param name="headerWithEventNameX">The cellname from the x direction</param>
        /// <returns>The cellstate</returns>
        /// <autor>Andrej Albrecht</autor>
        internal CellType GetFootprintCellState(string headerWithEventNameY, string headerWithEventNameX)
        {
            for (int row = 0; row < HeaderWithEventNames.Count; row++)
                for (int column = 0; column < HeaderWithEventNames.Count; column++)
                    if (HeaderWithEventNames[row].Equals(headerWithEventNameY) && HeaderWithEventNames[column].Equals(headerWithEventNameX))
                        return ResultMatrix[row, column];

            //If no cellstate was found, then return the nothing cellstate
            return CellType.Nothing;
        }
    }
}