using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;
using pgmpm.ConformanceChecking;
using pgmpm.MainV2.Utilities;
using pgmpm.Model.PetriNet;

namespace pgmpm.MainV2.Viewer
{
    /// <summary>
    /// Logic for FootprintViewer.xaml
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public partial class FootprintViewer
    {
        /// <summary>
        /// Constructor for the output of a normal footprint
        /// </summary>
        public FootprintViewer()
        {
            InitializeComponent();

            ComparingFootprint footPrintEventlog = ComparingFootprintAlgorithm.CreateFootprint(Viewer.CurrentField.EventLog);
            ComparingFootprint footPrintPetrinet = ComparingFootprintAlgorithm.CreateFootprint((PetriNet)Viewer.CurrentField.ProcessModel);
            ComparingFootprintResultMatrix resultFootprint = new ComparingFootprintResultMatrix(footPrintEventlog, footPrintPetrinet);

            ShowFootprint(footPrintEventlog, EventLogGrid);
            ShowFootprint(footPrintPetrinet, PetriNetGrid);
            ShowFootprint(resultFootprint, ComparisonGrid);
        }

        /// <summary>
        /// Constructor the show a footprint of just one transition
        /// </summary>
        /// <param name="transitionName"></param>
        public FootprintViewer(String transitionName)
        {
            InitializeComponent();
            ComparingFootprint footPrintEventlog = ComparingFootprintAlgorithm.CreateFootprint(Viewer.CurrentField.EventLog);
            ComparingFootprint footPrintPetrinet = ComparingFootprintAlgorithm.CreateFootprint((PetriNet)Viewer.CurrentField.ProcessModel);
            ComparingFootprintResultMatrix resultFootprint = new ComparingFootprintResultMatrix(footPrintEventlog, footPrintPetrinet);
            ComparingFootprintResultMatrix result = StripFootprintForTransition(transitionName, resultFootprint);
            ShowFootprint(result, EventLogGrid);
        }

        /// <summary>
        /// Shows only the cells in a footprint in which the given transitionname appears
        /// </summary>
        /// <param name="transitionName">Transitionname</param>
        /// <param name="footPrint">Footprint</param>
        /// <returns>Returns the stripped footprint</returns>
        /// <autor>Andrej Albrecht</autor>
        private static ComparingFootprintResultMatrix StripFootprintForTransition(String transitionName, ComparingFootprintResultMatrix footPrint)
        {
            List<String> headerWithEventNames = new List<String>();

            //save the needed header names
            for (int row = 0; row < footPrint.HeaderWithEventNames.Count; row++)
                for (int column = 0; column < footPrint.HeaderWithEventNames.Count; column++)
                    if (!footPrint.ResultMatrix[row, column].Equals(ResultCellType.NoDifferences) &&
                        (footPrint.HeaderWithEventNames[row].Equals(transitionName) || footPrint.HeaderWithEventNames[column].Equals(transitionName)))
                    {
                        headerWithEventNames.Add(footPrint.HeaderWithEventNames[row]);
                        headerWithEventNames.Add(footPrint.HeaderWithEventNames[column]);
                    }

            //create new Footprint
            ComparingFootprintResultMatrix newFootPrint = new ComparingFootprintResultMatrix();
            newFootPrint.AddEventHeader(headerWithEventNames);

            ResultCellType[,] resultMatrix = new ResultCellType[newFootPrint.HeaderWithEventNames.Count, newFootPrint.HeaderWithEventNames.Count];

            var header1 = newFootPrint.HeaderWithEventNames;
            var header2 = footPrint.HeaderWithEventNames;

            //fill the new footprint matrix with the required values
            for (int leftRow = 0; leftRow < header1.Count; leftRow++)
                for (int leftColumn = 0; leftColumn < header1.Count; leftColumn++)
                    for (int rightRow = 0; rightRow < header2.Count; rightRow++)
                        for (int rightColumn = 0; rightColumn < header2.Count; rightColumn++)
                            if (header1[leftRow].Equals(header2[rightRow]) && header1[leftColumn].Equals(header2[rightColumn]))
                                resultMatrix[leftRow, leftColumn] = footPrint.ResultMatrix[rightRow, rightColumn];

            newFootPrint.ResultMatrix = resultMatrix;

            return newFootPrint;
        }

        #region Functionality

        /// <summary>
        /// Draws the footprint to the grid
        /// </summary>
        /// <param name="footprint"></param>
        /// <param name="dataGrid"></param>
        /// <autor>Andrej Albrecht</autor>
        private void ShowFootprint(ComparingFootprint footprint, DataGrid dataGrid)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add(""); //empty c on the upper left corner

                foreach (String headerNameY in footprint.HeaderWithEventNames)
                    dataTable.Columns.Add(headerNameY);

                for (int rowIndex = 0; rowIndex < footprint.HeaderWithEventNames.Count; rowIndex++)
                {
                    object[] row = new object[1 + footprint.HeaderWithEventNames.Count];

                    row[0] = footprint.HeaderWithEventNames[rowIndex]; //row header description

                    for (int columnIndex = 0; columnIndex < footprint.HeaderWithEventNames.Count; columnIndex++)
                    {
                        try
                        {
                            if (footprint is ComparingFootprintResultMatrix)
                                row[columnIndex + 1] = GetResultCellTypeName(((ComparingFootprintResultMatrix)footprint).ResultMatrix[rowIndex, columnIndex]);
                            else
                                row[columnIndex + 1] = GetCellTypeName(footprint.ResultMatrix[rowIndex, columnIndex]);
                        }
                        catch (Exception)
                        {
                            row[columnIndex + 1] = "-?-";
                        }
                    }
                    dataTable.Rows.Add(row);
                }
                dataGrid.DataContext = dataTable;
            }
            catch (Exception Ex)
            {
                ErrorHandling.ReportErrorToUser("Error: " + Ex.Message + Ex.StackTrace);
            }
        }

        /// <summary>
        /// Returns the name of the enum of CellTypes
        /// </summary>
        /// <param name="cellType">A CellType</param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        private string GetCellTypeName(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.NotExist:
                    return "X";
                case CellType.Nothing:
                    return "#";
                case CellType.Left:
                    return "<-";
                case CellType.Right:
                    return "->";
                case CellType.Parallel:
                    return "||";
                case CellType.Loop:
                    return "@";
            }
            return "X";
        }

        /// <summary>
        /// Returns the name of the enum of ResultCellTypes
        /// </summary>
        /// <param name="resultCellType">A ResultCellType</param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        private string GetResultCellTypeName(ResultCellType resultCellType)
        {
            switch (resultCellType)
            {
                case ResultCellType.NoDifferences: return "";
                case ResultCellType.LeftAndNothing: return "<-:#";
                case ResultCellType.LeftAndParallel: return "<-:||";
                case ResultCellType.LeftAndRight: return "<-:->";
                case ResultCellType.ParallelAndLeft: return "||:<-";
                case ResultCellType.ParallelAndNothing: return "||:#";
                case ResultCellType.ParallelAndRight: return "||:->";
                case ResultCellType.RightAndLeft: return "->:<-";
                case ResultCellType.RightAndNothing: return "->:#";
                case ResultCellType.RightAndParallel: return "->:||";
                case ResultCellType.NotExistAndLeft: return "X:<-";
                case ResultCellType.NotExistAndRight: return "X:->";
                case ResultCellType.NotExistAndParallel: return "X:||";
                case ResultCellType.LeftAndNotExist: return "<-:X";
                case ResultCellType.RightAndNotExist: return "->:X";
                case ResultCellType.ParallelAndNotExist: return "||:X";
                case ResultCellType.NotExistAndNothing: return "X:#";
                case ResultCellType.NothingAndNotExist: return "#:X";
                case ResultCellType.NothingAndRight: return "#:->";
                case ResultCellType.NothingAndLeft: return "#:<-";
                case ResultCellType.LoopAndNotExist: return "@:X";
                case ResultCellType.LoopAndNothing: return "@:#";
                case ResultCellType.LoopAndParallel: return "@:||";
                case ResultCellType.LoopAndLeft: return "@:<-";
                case ResultCellType.LoopAndRight: return "@:->";
                case ResultCellType.NotExistAndLoop: return "X:@";
                case ResultCellType.NothingAndLoop: return "#:@";
                case ResultCellType.NothingAndParallel: return "#:||";
                case ResultCellType.LeftAndLoop: return "<-:@";
                case ResultCellType.RightAndLoop: return "->:@";
                case ResultCellType.ParallelAndLoop: return "||:@";
            }
            return "?";
        }

        #endregion
    }
}