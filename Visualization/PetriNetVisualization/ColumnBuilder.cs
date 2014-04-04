using System.Collections.Generic;
using System.Linq;
using pgmpm.Model.PetriNet;

namespace pgmpm.Visualization.PetriNetVisualization
{
    /// <summary>
    ///     This class provides the methods to turn a PetriNet into a List of Columns that can be visualized.
    /// </summary>
    /// <author>Jannik Arndt</author>
    public static class ColumnBuilder
    {
        private static readonly List<Column> GlobalColumnList = new List<Column>();

        /// <summary>
        ///     Build a List of Columns from a PetriNet
        /// </summary>
        /// <param name="petriNet"></param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public static List<Column> Build(PetriNet petriNet)
        {
            // Initialize
            GlobalColumnList.Clear();

            Column firstColumn = new Column(0);
            Place startPlace = (Place) petriNet.GetSources()[0];

            firstColumn.HashSetOfNodes.Add(startPlace);
            GlobalColumnList.Add(firstColumn);

            // Recursively add all columns
            AddPlace(startPlace, 1);

            // Correct the end-place
            Place endPlace = (Place) petriNet.GetSinks()[0];
            Column temp = FindNode(endPlace);
            if (temp != null && temp.ColumnNumber < GlobalColumnList.Count)
            {
                temp.HashSetOfNodes.Remove(endPlace);
                TrimGlobalColumnList();
                GetOrCreateColumn(GlobalColumnList.Count).HashSetOfNodes.Add(endPlace);
            }

            return GlobalColumnList;
        }

        /// <summary>
        ///     Adds the transitions of a place (separated by AND-Splits/-Joins and others) to the correct Column of the
        ///     GlobalColumnList
        /// </summary>
        /// <param name="place">A Place that is already in the GlobalColumnList</param>
        /// <param name="columnNumber">The number of the column where the Transitions are supposed to be added</param>
        /// <author>Jannik Arndt</author>
        private static void AddPlace(Place place, int columnNumber)
        {
            Column column = GetOrCreateColumn(columnNumber);

            // 1. Handle AND-Transitions
            foreach (Transition transition in place.OutgoingANDTransitions)
            {
                column.ContainsAndTransitions = true;
                column.HashSetOfNodes.Add(transition);
                if (!transition.IsLoop)
                    AddTransition(transition, columnNumber + 1);
            }
            // if the next column contains an AND-transition, skip it (ColumnNumber + 2)
            if (column.ContainsAndTransitions)
            {
                columnNumber = columnNumber + 2;
                column = GetOrCreateColumn(columnNumber);
            }

            // 2. Handle other Transitions
            foreach (Transition transition in place.OutgoingTransitionsWithoutANDs)
            {
                if (!transition.IsLoop)
                {
                    if (!NodeAlreadyIsInColumns(transition))
                    {
                        column.HashSetOfNodes.Add(transition);
                        AddTransition(transition, columnNumber + 1);
                    }
                }
            }
            // If this place happens to be before a looping event, it will have a loop as an incoming places. Since the loop transition will be ignored it has to be added here
            if (place.IsBeforeLoopingEvent)
                foreach (Transition transition in place.IncomingTransitions)
                    if (transition.IsLoop)
                        column.HashSetOfNodes.Add(transition);
        }

        /// <summary>
        ///     Adds the places of a transition to the correct column of the GlobalColumnList
        /// </summary>
        /// <param name="transition">A Transition that is already in the GlobalColumnList</param>
        /// <param name="columnNumber">The number of the column where the Places are supposed to be added</param>
        /// <author>Jannik Arndt</author>
        private static void AddTransition(Transition transition, int columnNumber)
        {
            Column column = GetOrCreateColumn(columnNumber);
            foreach (Place place in transition.OutgoingPlaces)
            {
                if (!NodeAlreadyIsInColumns(place))
                {
                    column.HashSetOfNodes.Add(place);
                    AddPlace(place, columnNumber + 1);
                }
            }
        }

        #region Helpers

        /// <summary>
        ///     This returns the requested column from the GlobalColumnList. If a column with the requested number does not exists
        ///     it creates as many columns as needed to return the right one.
        /// </summary>
        /// <param name="columnNumber">The number of the column, starting at 0</param>
        /// <returns>A column-object</returns>
        /// <author>Jannik Arndt</author>
        private static Column GetOrCreateColumn(int columnNumber)
        {
            while (GlobalColumnList.Count < columnNumber + 1)
                GlobalColumnList.Add(new Column(GlobalColumnList.Count));
            return GlobalColumnList[columnNumber];
        }

        /// <summary>
        ///     Searches the GlobalColumnList if a Node is contained in any column
        /// </summary>
        /// <param name="node">The node you are looking for</param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        private static bool NodeAlreadyIsInColumns(Node node)
        {
            return GlobalColumnList.Any(column => column.HashSetOfNodes.Contains(node));
        }

        /// <summary>
        ///     Searches the GlobalColumnList for the specified node and returns the first column the node is in
        /// </summary>
        /// <param name="node">The node you are looking for</param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        private static Column FindNode(Node node)
        {
            return GlobalColumnList.FirstOrDefault(column => column.HashSetOfNodes.Contains(node));
        }

        /// <summary>
        ///     Iterates the list two times: The first time all empty columns are removed, the second time the .Row-Property is
        ///     updated.
        /// </summary>
        /// <author>Jannik Arndt</author>
        private static void TrimGlobalColumnList()
        {
            for (int column = GlobalColumnList.Count - 1; column >= 0; column--)
                if (GlobalColumnList[column].HashSetOfNodes.Count == 0)
                    GlobalColumnList.RemoveAt(column);

            for (int column = 0; column < GlobalColumnList.Count; column++)
                GlobalColumnList[column].ColumnNumber = column;
        }

        #endregion
    }
}