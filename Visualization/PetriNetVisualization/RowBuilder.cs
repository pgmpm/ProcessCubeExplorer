using System;
using System.Collections.Generic;
using System.Linq;
using pgmpm.Model.PetriNet;

namespace pgmpm.Visualization.PetriNetVisualization
{
    /// <summary>
    ///     This class can set the row-properties of petrinet-nodes to something beautiful.
    /// </summary>
    /// <author>Jannik Arndt</author>
    public static class RowBuilder
    {
        /// <summary>
        /// This calculates the height for each node by aligning it to the side with the most neighbors
        /// </summary>
        /// <param name="listOfColumns">The result of ColumnBuilder.Build(petriNet)</param>
        /// <author>Jannik Arndt</author>
        public static List<Column> CalculateRows(List<Column> listOfColumns)
        {
            // initialize: simply stack nodes onto each other
            foreach (Column column in listOfColumns)
            {
                int rowNumber = 1;
                foreach (Node node in column.HashSetOfNodes)
                    node.Row = rowNumber++;
            }

            // align nodes with multiple neighbors on either side
            foreach (Column column in listOfColumns)
                foreach (Node node in column.HashSetOfNodes)
                {
                    // align to either left or right side
                    if (node.OutgoingNodes.Count > 1 && node.OutgoingNodes.Count > node.IncomingNodes.Count)
                        node.Row = GetMeanRowOf(node.OutgoingNodes);
                    if (node.IncomingNodes.Count > 1 && node.OutgoingNodes.Count <= node.IncomingNodes.Count)
                        node.Row = GetMeanRowOf(node.IncomingNodes);

                    // snap to 0.5-Grid
                    node.Row = Math.Round((2 * node.Row), MidpointRounding.AwayFromZero) / 2;
                }

            // left to right: align single nodes
            for (int column = 0; column < listOfColumns.Count; column++)
                if (listOfColumns[column].HashSetOfNodes.Count == 1)
                    foreach (Node node in listOfColumns[column].HashSetOfNodes)
                    {
                        if (node.IncomingNodes.Count == 1 && node.OutgoingNodes.Count == 1)
                            node.Row = Math.Max(node.IncomingNodes.First().Row, node.OutgoingNodes.First().Row);
                        // end
                        if (node.IncomingNodes.Count == 1 && node.OutgoingNodes.Count == 0)
                            node.Row = node.IncomingNodes.First().Row;
                    }

            // right to left: align single nodes
            for (int column = listOfColumns.Count - 1; column >= 0; column--)
                if (listOfColumns[column].HashSetOfNodes.Count == 1)
                    foreach (Node node in listOfColumns[column].HashSetOfNodes)
                    {
                        if (node.IncomingNodes.Count == 1 && node.OutgoingNodes.Count == 1)
                            node.Row = Math.Max(node.IncomingNodes.First().Row, node.OutgoingNodes.First().Row);
                        // start
                        if (node.IncomingNodes.Count == 0 && node.OutgoingNodes.Count == 1)
                            node.Row = node.OutgoingNodes.First().Row;
                    }

            return listOfColumns;
        }

        /// <summary>
        /// Goes through the nodes in the given list and returns the mean value of their current row-attribute
        /// </summary>
        /// <param name="nodes">A list of nodes</param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        private static double GetMeanRowOf(List<Node> nodes)
        {
            if (nodes.Count == 0)
                return 1;
            double result = nodes.Sum(node => node.Row);
            return result / nodes.Count;
        }
    }
}