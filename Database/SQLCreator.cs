using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pgmpm.Database.AggregationConfiguration;
using pgmpm.Database.Properties;
using pgmpm.MatrixSelection.Dimensions;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.Database
{
    public class SQLCreator
    {
        public readonly string CaseColumnName = Settings.Default.CaseColumnName; // case_id
        public readonly string CaseTableName = Settings.Default.CaseTableName; // CASE
        public readonly string EventSortArgument = Settings.Default.EventSortArgument; // timestamp
        public readonly string EventTableName = Settings.Default.EventTableName; // EVENT
        public readonly string FactColumnName = Settings.Default.FactColumnName; // fact_id
        public readonly string FactTableName = Settings.Default.FactTableName; // FACT

        public string StringMarker = "'";

        /// <summary>
        /// Caches the Join- and WHERE-Statements for the SQL-Queries for each matrix-field, 
        /// since these are used a lot and only change if the selection is changed (at which point this method should be called)
        /// </summary>
        /// <param name="selectedFactDimensions">The selected dimensions of the matrix selection</param>
        /// <param name="selectedEventDimensions">The selected event-dimensions</param>
        /// <param name="field">The current field to extract the column-content</param>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public virtual string GetSQLCode(List<SelectedDimension> selectedFactDimensions, List<SelectedDimension> selectedEventDimensions, Field field)
        {
            StringBuilder result = new StringBuilder();

            result.Append(GetSelectString(selectedEventDimensions))
                .Append(" \n")
                .Append(GetJoinString(selectedFactDimensions, selectedEventDimensions, true))
                .Append(" \n")
                .Append(GetWhereString(selectedFactDimensions, selectedEventDimensions, field))
                .Append(" \n")
                .Append(GetGroupByString(selectedEventDimensions))
                .Append(" \n")
                .Append(GetOrderByString());

            return result.ToString().Replace("\n \n", "\n");
        }

        #region Count-Statements
        /// <summary>
        /// Creates a SQL-Statement which count the event per field
        /// </summary>
        /// <param name="selectedFactDimensions">The selected dimensions of the matrix selection</param>
        /// <param name="selectedEventDimensions">The selected event-dimensions</param>
        /// <param name="field">The current field to extract the column-content</param>
        /// <returns></returns>
        /// <author>Bernhard Bruns, Bernd Nottbeck, Jannik Arndt</author>
        public virtual string GetSQLCodeCountEvents(List<SelectedDimension> selectedFactDimensions, List<SelectedDimension> selectedEventDimensions, Field field)
        {
            StringBuilder result = new StringBuilder();

            result.Append("SELECT COUNT(*) FROM " + TableNameFormatter(FactTableName) + " ")
                .Append(" \n")
                .Append(GetJoinString(selectedFactDimensions, selectedEventDimensions, true))
                .Append(" \n")
                .Append(GetWhereString(selectedFactDimensions, selectedEventDimensions, field))
                .Append(" \n")
                .Append(GetGroupByString(selectedEventDimensions));

            return result.ToString().Replace("\n", " ");
        }


        /// <summary>
        /// Creates a SQL-Statement which count only unique events per field
        /// </summary>
        /// <param name="selectedFactDimensions">The selected dimensions of the matrix selection</param>
        /// <param name="selectedEventDimensions">The selected event-dimensions</param>
        /// <param name="field">The current field to extract the column-content</param>
        /// <returns></returns>
        /// <author>Bernhard Bruns, Bernd Nottbeck, Jannik Arndt</author>
        public virtual string GetSQLCodeCountUniqueEvents(List<SelectedDimension> selectedFactDimensions, List<SelectedDimension> selectedEventDimensions, Field field)
        {
            StringBuilder result = new StringBuilder();

            result.Append("SELECT COUNT(DISTINCT ACTIVITY) FROM " + TableNameFormatter(FactTableName) + " ")
                .Append(" \n")
                .Append(GetJoinString(selectedFactDimensions, selectedEventDimensions, true))
                .Append(" \n")
                .Append(GetWhereString(selectedFactDimensions, selectedEventDimensions, field))
                .Append(" \n")
                .Append(GetGroupByString(selectedEventDimensions));

            return result.ToString().Replace("\n", " ");
        }

        /// <summary>
        /// Creates a SQL-Statement which count the cases per field.
        /// </summary>
        /// <param name="selectedFactDimensions">The selected dimensions of the matrix selection</param>
        /// <param name="selectedEventDimensions">The selected event-dimensions</param>
        /// <param name="field">The current field to extract the column-content</param>
        /// <returns></returns>
        /// <author>Bernhard Bruns, Bernd Nottbeck, Jannik Arndt</author>
        public virtual string GetSQLCodeCountCases(List<SelectedDimension> selectedFactDimensions, List<SelectedDimension> selectedEventDimensions, Field field)
        {
            StringBuilder result = new StringBuilder();

            result.Append("SELECT COUNT(*) FROM " + TableNameFormatter(FactTableName) + " ")
                .Append(" \n")
                .Append(GetJoinString(selectedFactDimensions, selectedEventDimensions, false))
                .Append(" \n")
                .Append(GetWhereString(selectedFactDimensions, selectedEventDimensions, field))
                .Append(" \n")
                .Append(GetGroupByString(selectedEventDimensions));

            return result.ToString().Replace("\n", " ");
        }
        #endregion

        #region Select

        /// <summary>
        /// Assembles a Select-String
        /// </summary>
        /// <param name="selectedEventDimensions"></param>
        /// <returns>The complete string, e.g. "SELECT FACT.fact_id, EVENT.event_id, ... FROM FACT "</returns>
        /// <author>Jannik Arndt</author>
        private string GetSelectString(IEnumerable<SelectedDimension> selectedEventDimensions)
        {
            List<string> selectStatements = new List<string>();

            // 1) Add Fact-Table to select-statement ("FACT.fact_id")
            selectStatements.Add(CreateFactSelectStatement(containsAggregation(selectedEventDimensions)));

            // 2) Add Event-Table-Columns to select-statement ("EVENT.event_id", "Event.case_id", ...)
            selectStatements.AddRange(CreateEventSelectStatements(containsAggregation(selectedEventDimensions)));

            if (Settings.Default.JoinAllDimensions)
                selectStatements.AddRange(GetAllDimensionColumns(containsAggregation(selectedEventDimensions)));

            // 3) Build the Select-Query ("SELECT FACT.fact_id, EVENT.event_id, ... FROM `FACT` ")
            return "SELECT " + string.Join(", \n", selectStatements) + " \nFROM " + TableNameFormatter(FactTableName) + " ";
        }

        /// <summary>
        /// Creates a fact-table-column for select clause, i.e. "FACT.fact_id".
        /// </summary>
        /// <param name="withAggregation">Wether an aggregation on the event-table was selected</param>
        /// <returns>String that can be used for the select statement</returns>
        /// <author>Jannik Arndt, Roman Bauer</author>
        private string CreateFactSelectStatement(bool withAggregation)
        {
            if (withAggregation)
            {
                AbstractAggregationConfigurator aggConfigurator = new DefaultAggregationConfigurator();
                return embedAttributeInFunction(aggConfigurator.GetDefaultAggregationFunction(), FactTableName, FactColumnName);
            }
            else
            {
                return TableColumnFormatter(FactTableName, FactColumnName);
            }
        }

        /// <summary>
        /// Creates a list of event-table-columns, i.e. "EVENT.event_id", "Event.case_id", ...
        /// </summary>
        /// <param name="withAggregation">Whether an aggregation on the event-table was selected</param>
        /// <returns>A list of strings that can be used for the select statement</returns>
        /// <author>Jannik Arndt, Roman Bauer</author>
        private IEnumerable<string> CreateEventSelectStatements(bool withAggregation)
        {
            List<string> result = new List<string>();

            //aggregation selected --> build select clause with database functions because we will use group by clause
            if (withAggregation)
            {
                AbstractAggregationConfigurator aggConfigurator = new DefaultAggregationConfigurator();

                foreach (string columnName in DBWorker.MetaData.ListOfEventsTableColumnNames)
                {
                    string aggFunction = aggConfigurator.GetAggregationFunction(columnName);
                    string projectionOnAttribute = aggFunction + "(" + TableColumnFormatter(EventTableName, columnName) + ")" + columnName;
                    result.Add(projectionOnAttribute);
                }
            }
            //no aggregation selected --> build select clause in absence of group by clause
            else
                foreach (string columnName in DBWorker.MetaData.ListOfEventsTableColumnNames)
                    result.Add(TableColumnFormatter(EventTableName, columnName));

            return result;
        }

        /// <summary>
        /// Creates a list of dimension-table-columns
        /// </summary>
        /// <param name="withAggregation">Whether an aggregation on the event-table was selected</param>
        /// <returns>A list of strings that can be used for the select statement</returns>
        private IEnumerable<string> GetAllDimensionColumns(bool withAggregation)
        {
            if (withAggregation)
            {
                var result = new List<string>();
                AbstractAggregationConfigurator aggConfigurator = new DefaultAggregationConfigurator();

                foreach (var dimension in DBWorker.MetaData.ListOfFactDimensions)
                    if (!dimension.IsEmptyDimension)
                        result.Add(embedAttributeInFunction(aggConfigurator.GetDefaultAggregationFunction(), dimension.ToTable, dimension.DimensionColumnNames.Col_Content));
                return result;
            }
            else
            {
                var result = new List<string>();
                foreach (var dimension in DBWorker.MetaData.ListOfFactDimensions)
                    if (!dimension.IsEmptyDimension)
                        result.Add(TableColumnFormatter(dimension.ToTable, dimension.DimensionColumnNames.Col_Content) + " AS " + dimension.ToTable);
                return result;
            }
        }

        /// <summary>
        /// Embed the given attribute in the given function, for example "MIN(FACT.fact_id)fact_id" ...
        /// </summary>
        /// <param name="function">Function to use</param>
        /// <param name="tableName">Name of the table</param>
        /// <param name="columnName">Name of the column</param>
        /// <returns>String that can be used for select clause</returns>
        /// <author>Jannik Arndt, Roman Bauer</author>
        private string embedAttributeInFunction(string function, string tableName, string columnName)
        {
            AbstractAggregationConfigurator aggConfigurator = new DefaultAggregationConfigurator();
            if (aggConfigurator.GetDefaultAggregationFunction().Equals(function))
            {
                if(tableName.Equals(FactTableName)) 
                {
                    return function + "(" + TableColumnFormatter(tableName, columnName) + ")" + columnName;
                }
                else 
                {
                    return function + "(" + TableColumnFormatter(tableName, columnName) + ")" + tableName;
                }
            }
            else
            {
                //here you can implement another functions
                return null;
            }
        }

        #endregion

        #region Join

        /// <summary>
        /// Create join statements between dimensions and the table who reference them.
        /// </summary>
        /// <param name="selectedDimensions"></param>
        /// <author>Roman Bauer</author>
        private IEnumerable<string> CreateDimensionJoinStatements(IEnumerable<SelectedDimension> selectedDimensions)
        {
            List<string> listOfJoinStatements = new List<string>();

            if (selectedDimensions == null)
                return listOfJoinStatements;

            //joins between dimensions and table which reference them
            foreach (SelectedDimension selectedDimension in selectedDimensions)
            {
                if (!selectedDimension.Dimension.IsEmptyDimension && !(selectedDimension.IsAllLevelSelected && selectedDimension.AggregationDepth == 0))
                {
                    if ((selectedDimension.AggregationDepth >= selectedDimension.LevelDepth) || ((selectedDimension.AggregationDepth < selectedDimension.LevelDepth && selectedDimension.IsAllLevelSelected)))
                    {
                        //create join statement for this dimension if aggregation level is bigger that filter level --> so build joins until aggregation level (aggregation level depth include filter level)
                        //or aggregation level smaller than filter level and filter level is all level --> aggregation level need joins, filter level does not need joins --> so need build joins until aggregation level
                        listOfJoinStatements.AddRange(CollectAllJoinStatements(new List<string>(), selectedDimension.Dimension, selectedDimension.AggregationDepth - 1));
                    }
                    else
                    {
                        //no aggregation found and filter level is not all level --> build joins until filter level
                        listOfJoinStatements.AddRange(CollectAllJoinStatements(new List<string>(), selectedDimension.Dimension, selectedDimension.LevelDepth - 1));
                    }
                }
            }

            return listOfJoinStatements;
        }

        /// <summary>
        /// Recursively go through all level steps and return a list of all necessary join-statements.
        /// </summary> 
        /// <param name="list">The list that needs to be filled</param> 
        /// <param name="dimension"></param>
        /// <param name="grain"></param> 
        /// <returns></returns> 
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public virtual List<string> CollectAllJoinStatements(List<string> list, Dimension dimension, int grain)
        {
            if (grain < 0)
            {
                return list;
            }

            if (grain > 0 && dimension != null && dimension.GetCoarserDimensionLevel() != null)
                CollectAllJoinStatements(list, dimension.GetCoarserDimensionLevel()[0], --grain);

            if (dimension != null && dimension.ToTable != "")
                list.Add(CreateJoinStatementFromDimension(dimension));

            return list;
        }

        /// <summary>
        /// Creates a Join-statement for a given dimension.
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
        /// <author>Jannik Arndt, Bernhard Bruns, Bernd Nottbeck</author>
        public virtual string CreateJoinStatementFromDimension(Dimension dimension)
        {
            StringBuilder joinStatement = new StringBuilder();
            joinStatement.Append("JOIN ")
                .Append(TableNameFormatter(dimension.ToTable))
                .Append(" ON ")
                .Append(TableColumnFormatter(dimension.FromTable, dimension.FromColumn))
                .Append(" = ")
                .Append(TableColumnFormatter(dimension.ToTable, dimension.ToColumn))
                .Append(" ");
            return joinStatement.ToString();
        }

        /// <summary>
        /// Assembles the join statement.
        /// </summary>
        /// <param name="selectedEventDimensions"></param>
        /// <param name="joinOnEvent">True if join on Event</param>
        /// <param name="selectedFactDimensions"></param>
        /// <returns>The complete JOIN-string, e.g. "JOIN CASE ON FACT.fact_id = CASE.fact_id JOIN DIM_AGE in FACT.age_id = DIM_AGE.age_id "</returns>
        /// <author>Bernd Nottbeck, Jannik Arndt</author>
        public virtual string GetJoinString(List<SelectedDimension> selectedFactDimensions, List<SelectedDimension> selectedEventDimensions, bool joinOnEvent)
        {
            List<string> joinStatements = new List<string>();

            // 4) Create Join-statements ("JOIN `mydimension` ON FACT.dim = mydimension.id ")
            joinStatements.AddRange(CreateDimensionJoinStatements(selectedFactDimensions));
            joinStatements.AddRange(CreateDimensionJoinStatements(selectedEventDimensions));
            if (Settings.Default.JoinAllDimensions)
                joinStatements.AddRange(CreateJoinAllDimensionsStatement());

            // 5) Remove dublicates
            joinStatements = joinStatements.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();

            // 6) Reverse join strings because we have different dimension levels
            //    we have to join the dimension level in direction from low level and high level
            joinStatements.Reverse();

            // "JOIN CASE ON FACT.fact_id = CASE.fact_id "
            StringBuilder result = new StringBuilder();
            result.Append("JOIN ")
                .Append(TableNameFormatter(CaseTableName))
                .Append(" ON ")
                .Append(TableColumnFormatter(FactTableName, FactColumnName))
                .Append(" = ")
                .Append(TableColumnFormatter(CaseTableName, FactColumnName))
                .Append(" \n");

            // "JOIN EVENT ON CASE.case_id = EVENT.case_id "
            if (joinOnEvent)
                result.Append("JOIN ")
                    .Append(TableNameFormatter(EventTableName))
                    .Append(" ON ")
                    .Append(TableColumnFormatter(CaseTableName, CaseColumnName))
                    .Append(" = ")
                    .Append(TableColumnFormatter(EventTableName, CaseColumnName))
                    .Append(" \n");

            // other Joins
            result.Append(string.Join(" \n", joinStatements));

            return result.ToString();
        }

        private IEnumerable<string> CreateJoinAllDimensionsStatement()
        {
            var result = new List<string>();
            foreach (var dimension in DBWorker.MetaData.ListOfFactDimensions)
                if (!dimension.IsEmptyDimension)
                    result.Add(CreateJoinStatementFromDimension(dimension));

            return result;
        }

        #endregion

        #region Where

        /// <summary>
        /// Create where statements for fact
        /// </summary>
        /// <param name="selectedFactDimensions"></param>
        /// <author>Roman Bauer</author>
        private IEnumerable<string> CreateFactWhereStatements(List<SelectedDimension> selectedFactDimensions)
        {
            List<string> listOfWhereStatements = new List<string>();

            if (selectedFactDimensions.Count > 2)
            {
                foreach (SelectedDimension selectedDimension in selectedFactDimensions)
                {
                    if (!selectedDimension.IsAllLevelSelected)
                    {
                        //skip dimension on axis 0 and 1 because they handled later in a special way
                        if (selectedDimension.Axis == 0 || selectedDimension.Axis == 1)
                            continue;

                        Dimension currentDimension;

                        if (selectedDimension.LevelDepth > 1)//selected granularity is bigger than the smallest granularity
                            //find dimension table for the given level
                            currentDimension = getDimensionByLevelDepth(selectedDimension.Dimension, selectedDimension.LevelDepth);

                        else //selected granularity is the smallest granularity
                            currentDimension = selectedDimension.Dimension;

                        if (!currentDimension.IsEmptyDimension && selectedDimension.SelectedFilters != null && selectedDimension.SelectedFilters.Count > 0)
                            listOfWhereStatements.Add(TableColumnFormatter(currentDimension.ToTable, currentDimension.DimensionColumnNames.Col_Content) + " IN " + ListFilters(selectedDimension));
                    }
                }
            }
            return listOfWhereStatements;
        }

        /// <summary>
        /// Create where statements for event
        /// </summary>
        /// <param name="selectedEventDimensions"></param>
        /// <author>Roman Bauer</author>
        private IEnumerable<string> CreateEventWhereStatements(IEnumerable<SelectedDimension> selectedEventDimensions)
        {
            List<string> listOfWhereStatements = new List<string>();

            if (selectedEventDimensions == null)
                return listOfWhereStatements;

            foreach (SelectedDimension selectedDimension in selectedEventDimensions)
            {
                if (!selectedDimension.IsAllLevelSelected)
                {
                    Dimension currentDimension;
                    if (selectedDimension.LevelDepth > 1)//selected granularity is bigger than the smallest granularity
                    {
                        //find dimension table for the given level
                        currentDimension = getDimensionByLevelDepth(selectedDimension.Dimension, selectedDimension.LevelDepth);
                    }
                    else //selected granularity is the smallest granularity
                    {
                        currentDimension = selectedDimension.Dimension;
                    }

                    if (!currentDimension.IsEmptyDimension && selectedDimension.SelectedFilters != null && selectedDimension.SelectedFilters.Count > 0)
                    {
                        listOfWhereStatements.Add(TableColumnFormatter(currentDimension.ToTable, currentDimension.DimensionColumnNames.Col_Content) + " IN " + ListFilters(selectedDimension));
                    }
                }
            }
            return listOfWhereStatements;
        }

        /// <summary>
        /// Gets the tablename at the selected granularity
        /// </summary>
        /// <param name="selectedDimension">The selected dimension (1 or 2)</param>
        /// <returns>The name of the table</returns>
        /// <author>Jannik Arndt</author>
        private string CreateWhereStatements(SelectedDimension selectedDimension)
        {
            if (selectedDimension.Dimension == null || selectedDimension.Dimension.DimensionColumnNames.Col_Content == null)
                return "";

            Dimension dimension = selectedDimension.Dimension;

            // Get dimension at selected granularity
            if (selectedDimension.LevelDepth > 1 && !selectedDimension.IsAllLevelSelected)
                for (int i = 1; i < selectedDimension.LevelDepth; i++)
                    dimension = dimension.GetCoarserDimensionLevel()[0];

            // Get the table name
            if (!selectedDimension.IsAllLevelSelected)
                return TableColumnFormatter(dimension.ToTable, dimension.DimensionColumnNames.Col_Content);

            return "";
        }

        /// <summary>
        /// Assembles the WHERE-Statements with respect to what is actually filled out
        /// </summary>
        /// <param name="selectedFactDimensions"></param>
        /// <param name="selectedEventDimensions"></param>
        /// <param name="field">The field, to extract the column-content</param>
        /// <returns>A complete WHERE-String, e.g. "WHERE DIM_AGE.content = '42' AND DIM_TIME.content = '2014' AND DIM_SEX = 'female'"</returns>
        /// <author>Jannik Arndt</author>
        private string GetWhereString(List<SelectedDimension> selectedFactDimensions, IEnumerable<SelectedDimension> selectedEventDimensions, Field field)
        {
            List<string> additionalWhereStatements = new List<string>();

            string whereTable1 = CreateWhereStatements(selectedFactDimensions[0]);
            string whereTable2 = CreateWhereStatements(selectedFactDimensions[1]);

            // Get the WHERE-statements for the other dimensions
            additionalWhereStatements.AddRange(CreateFactWhereStatements(selectedFactDimensions));
            additionalWhereStatements.AddRange(CreateEventWhereStatements(selectedEventDimensions));

            if (string.IsNullOrEmpty(whereTable1) && string.IsNullOrEmpty(whereTable2) && additionalWhereStatements.Count == 0)
                return "";

            StringBuilder result = new StringBuilder("WHERE ");

            // 1. Dimension (e.g. "DIM_AGE.content" = '42' ")
            if (!string.IsNullOrEmpty(whereTable1))
                result.Append(whereTable1)
                    .Append(" = ")
                    .Append(StringFormatter(field.DimensionContent1.Content));

            // if both dimensions have any content, append an "AND"
            if (!string.IsNullOrEmpty(whereTable1) && !string.IsNullOrEmpty(whereTable2))
                result.Append("\nAND ");

            // 2. Dimension (e.g. "DIM_TIME.content = '2014' ")
            if (!string.IsNullOrEmpty(whereTable2))
                result.Append(whereTable2)
                    .Append(" = ")
                    .Append(StringFormatter(field.DimensionContent2.Content));

            // If the first OR second dimension have any content AND there are more dimensions, append an "AND"
            if ((!string.IsNullOrEmpty(whereTable1) || !string.IsNullOrEmpty(whereTable2)) && additionalWhereStatements.Count > 0)
                result.Append("\nAND ");

            // Additional Dimensions
            if (additionalWhereStatements.Count > 0)
                result.Append(string.Join(" \nAND ", additionalWhereStatements));

            return result.ToString();
        }

        #endregion

        #region Aggregation / Group By

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedEventDimensions"></param>
        /// <returns></returns>
        /// <author>Roman Bauer</author>
        private bool containsAggregation(IEnumerable<SelectedDimension> selectedEventDimensions)
        {
            if (selectedEventDimensions == null)
                return false;

            foreach (SelectedDimension selectedDimension in selectedEventDimensions)
                if (selectedDimension.AggregationDepth > 0)
                    return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedEventDimensions"></param>
        /// <author>Roman Bauer</author>
        private IEnumerable<string> CreateGroupByStatements(IEnumerable<SelectedDimension> selectedEventDimensions)
        {
            List<string> result = new List<string>();
            result.Add(TableColumnFormatter(FactTableName, FactColumnName));
            result.Add(TableColumnFormatter(CaseTableName, CaseColumnName));

            foreach (SelectedDimension selectedDimension in selectedEventDimensions)
            {
                if (selectedDimension.AggregationDepth > 0)
                {
                    //aggregation is selected --> get aggregation level
                    Dimension aggragationLevel = getDimensionByLevelDepth(selectedDimension.Dimension, selectedDimension.AggregationDepth);
                    result.Add(TableColumnFormatter(aggragationLevel.ToTable, aggragationLevel.DimensionColumnNames.Col_Content));
                }
            }
            return result;
        }

        /// <summary>
        /// Creates the GROUP BY-string, if it is needed
        /// </summary>
        /// <param name="selectedEventDimensions"></param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        private string GetGroupByString(List<SelectedDimension> selectedEventDimensions)
        {
            if (!containsAggregation(selectedEventDimensions))
                return "";

            //aggregation selected --> build group by clause
            var groupByStatements = CreateGroupByStatements(selectedEventDimensions);

            //concatenate group by strings to final group by clause
            return "GROUP BY " + string.Join(", \n", groupByStatements);
        }
        #endregion

        #region Order

        /// <summary>
        /// Creates the complete ORDER BY-string
        /// </summary>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        private string GetOrderByString()
        {
            return "ORDER BY " + TableColumnFormatter(CaseTableName, CaseColumnName) + ", " + EventSortArgument;
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Returns the dimension table for the given level
        /// </summary>
        /// <param name="dimension"></param>
        /// /// <param name="levelDepth"></param>
        /// <author>Roman Bauer</author>
        private Dimension getDimensionByLevelDepth(Dimension dimension, int levelDepth)
        {
            if (levelDepth > 1)
                return getDimensionByLevelDepth(dimension.DimensionLevelsList[0], levelDepth - 1);

            return dimension;
        }

        /// <summary>
        /// Creates a string from a SelectedDimension, formed like "(1, 2, 3, 4) ". Perfect to use for SQL's "IN" operation.
        /// </summary>
        /// <param name="selectedDimension"></param>
        /// <returns></returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public virtual string ListFilters(SelectedDimension selectedDimension)
        {
            List<string> temp = new List<string>();

            foreach (DimensionContent dimensionContent in selectedDimension.SelectedFilters)
            {
                String content = dimensionContent.Content;

                //check if apostrophe exists
                //an apostrophe leads to a problem in the sql statement
                //because it marks the end of a string instead of be part of the string
                if (content.Contains("'"))
                {
                    //make a double quote leads to a valid sql statement
                    content = content.Replace("'", "''");
                }

                temp.Add(content);
            }
            return "(" + StringMarker + "" + string.Join("" + StringMarker + ", " + StringMarker + "", temp) + "') ";
        }

        #endregion

        #region Formatter

        /// <summary>
        /// Formats a string for the specific SQL type
        /// </summary>
        /// <param name="text">String</param>
        /// <returns>Formatted string</returns>
        /// <author>Bernd Nottbeck</author>
        public virtual string StringFormatter(string text)
        {
            return StringMarker + text + StringMarker;
        }

        /// <summary>
        /// Formats a table and column for the specific SQL type
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A string representing the table column for the SQL type</returns>
        /// <author>Bernd Nottbeck</author>
        public virtual string TableColumnFormatter(string tableName, string columnName)
        {
            return tableName + "." + columnName;
        }

        /// <summary>
        /// Formats the table name.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <returns>Formatted table name</returns>
        /// <author>Bernd Nottbeck</author>
        public virtual string TableNameFormatter(string tableName)
        {
            return tableName;
        }

        #endregion
    }
}