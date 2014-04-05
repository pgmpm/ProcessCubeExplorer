using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using pgmpm.Database.Model;
using pgmpm.Database.Properties;
using pgmpm.MatrixSelection.Dimensions;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.Database
{
    public abstract class MPMdBConnection
    {
        #region definition

        public string ColumnNameCaseID = Settings.Default.CaseColumnName; // "case_id"
        public string ColumnNameFactID = Settings.Default.FactColumnName; // "fact_id"
        protected DbConnection Connection;
        public ConnectionParameters DbConnectionParameter;

        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public MPMdBConnection(ConnectionParameters connectionParameter)
        {
            DbConnectionParameter = connectionParameter;
            Initialize(BuildConnectionString());
        }

        #endregion

        #region abstract methods
        /// <summary>
        /// Creates an DB specific connection object
        /// </summary>
        /// <param name="conString">Connection parameters</param>
        public abstract void Initialize(String conString);

        /// <summary>
        /// Builds an DB specific connection string
        /// </summary>
        /// <returns>Connection string</returns>
        public abstract String BuildConnectionString();

        /// <summary>
        /// Gets a DataTable containing names of all tables referencing specified table.
        /// </summary>
        /// <param name="tablename">Analyzed table</param>
        /// <returns>DataTable with all referencing tables.</returns>
        public abstract DataTable GetReferencingDataTable(String tablename);

        /// <summary>
        /// Executes a DbDataReader stream getting the column names for a specified table
        /// </summary>
        /// <param name="table">Specified table</param>
        /// <returns>DbDataReader</returns>
        public abstract DbDataReader ReadColumnsForTable(String table);

        /// <summary>
        /// Method to load dimension level values for a specific dimension level.
        /// "Select * from" the specified table
        /// </summary>
        /// <param name="tablename">Table to load values from</param>
        /// <returns>Data table</returns>
        public abstract DataTable GetTableContent(String tablename);

        /// <summary>
        /// Reads raw fact data from a database.
        /// </summary>
        /// <param name="sqlcode">DB specific SQL statement to Load Fact</param>
        /// <returns>Raw data table</returns>
        public abstract DataTable GetFactDataTable(String sqlcode);

        /// <summary>
        /// Generates universal MPM DBexceptions from DB Type specific exceptions.
        /// </summary>
        /// <param name="ex">DB type specific exception</param>
        public abstract void HandleException(DbException ex);
        #endregion

        #region common methods
        /// <summary>
        /// Tries to establish a connection to the Database.
        /// </summary>
        /// <returns>True if connection</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public bool TryConnection()
        {
            try
            {
                Open();
            }
            catch (DbException ex)
            {
                HandleException(ex);
                return false;
            }
            finally
            {
                Close();
            }
            return true;
        }

        /// <summary>
        /// Checks if the connection state is set to "Open" or not.
        /// </summary>
        /// <returns></returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public bool IsOpen()
        {
            return Connection.State == ConnectionState.Open;
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <returns>True if succeeded.</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public bool Open()
        {
            if (Connection == null)
                return false;

            if (Connection.State == ConnectionState.Open)
            {
                return true;
            }
            try
            {
                Connection.Open();
                return true;
            }
            catch (DbException ex)
            {
                HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <returns>True if succeeded.</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public bool Close()
        {
            if (Connection == null)
                return false;
            try
            {
                Connection.Close();
                return true;
            }
            catch (DbException ex)
            {
                HandleException(ex);
                return false;
            }
        }

        /// <summary>
        /// Returns the connection name.
        /// </summary>
        /// <returns>Given Name + (Database @ Host)</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public String GetName()
        {
            return DbConnectionParameter.Name + " (" + DbConnectionParameter.Database + "@" + DbConnectionParameter.Host + ")";
        }

        /// <summary>
        /// Returns the columns in a given Table.
        /// </summary>
        /// <param name="table">The Name of the Table.</param>
        /// <returns>A List of strings.</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public virtual List<String> GetColumnNamesOfTable(String table)
        {
            var listOfColoumnames = new List<string>();
            try
            {
                Open();

                var reader = ReadColumnsForTable(table);

                while (reader.Read())
                    listOfColoumnames.Add((string)reader["column_name"]);

                return listOfColoumnames;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Method to get a table containing all dimensions.
        /// </summary>
        /// <param name="factTableName"></param>
        /// <param name="eventTableName"></param>
        /// <returns>FactTable containing the dimensions</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public virtual MetaDataRepository GetMetaData(string factTableName, string eventTableName)
        {
            MetaDataRepository table = new MetaDataRepository(factTableName);

            table.ListOfCaseTableColumnNames.AddRange(GetColumnNamesOfTable(DBWorker.SqlCreator.CaseTableName));
            table.ListOfEventsTableColumnNames.AddRange(GetColumnNamesOfTable(DBWorker.SqlCreator.EventTableName));

            //Add dimensions from table fact
            table.ListOfFactDimensions.Add(new Dimension("--no selection--", isEmptyDimension: true));
            table.ListOfFactDimensions.AddRange(GetDimensionsOf(factTableName));

            //Add dimensions from table event
            table.ListOfEventDimensions.AddRange(GetDimensionsOf(eventTableName));

            //Add logical all level for each dimension
            AddAllLevel(table.ListOfFactDimensions);
            AddAllLevel(table.ListOfEventDimensions);
            return table;
        }

        /// <summary>
        /// Method to Add the logical dimension ALL to every dimension "axis"
        /// </summary>
        /// <param name="dimensionList">dimension "axis" list</param>
        public void AddAllLevel(List<Dimension> dimensionList)
        {
            foreach (var dimension in dimensionList)
            {
                if (dimension.IsEmptyDimension)
                    continue;

                var temp = dimension;
                while (temp.DimensionLevelsList.Count != 0)
                    temp = temp.DimensionLevelsList[0];

                var allDimension = new Dimension("ALL", "MPMALLAGGREGATION", "", "", "MPMALLAGGREGATION", "ALL");

                allDimension.DimensionContentsList.Add(new DimensionContent("0", "ALL", "ALL"));
                temp.AddDimensionLevel(allDimension);
            }
        }

        /// <summary>
        /// Reads the Events from the Database and returns a DataSet.
        /// </summary>
        /// <returns>A list of Fact-objects.</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public List<List<Case>> GetFacts(string sqlcode)
        {
            var listOfFacts = new List<List<Case>>();

            try
            {
                // Loads a list of all facts and then converts it into fact-, case- and event-objects.
                Open();

                var factDataTable = GetFactDataTable(sqlcode);

                var currentFactID = "";
                var currentCaseID = "";
                List<Case> currentFact = null;
                Case currentCase = null;

                // iterate over the FactDataTable: object with the same fact_id will be combined into one fact-object.
                foreach (DataRow row in factDataTable.Rows)
                {
                    // 1. Did a new fact start?
                    if (currentFactID != row[ColumnNameFactID].ToString())
                    {
                        // save the current one
                        if (currentFact != null)
                            listOfFacts.Add(currentFact);

                        // and create a new one
                        currentFactID = row[ColumnNameFactID].ToString();
                        currentFact = new List<Case>();
                    }

                    // 2. Did a new case start?
                    if (currentCaseID != row[ColumnNameCaseID].ToString())
                    {
                        currentCaseID = row[ColumnNameCaseID].ToString();
                        currentCase = new Case(currentCaseID);
                        currentCase.AdditionalData.Add(ColumnNameFactID, row[ColumnNameFactID].ToString());
                        if (Settings.Default.JoinAllDimensions)
                            foreach (var dimension in DBWorker.MetaData.ListOfFactDimensions)
                                if (!dimension.IsEmptyDimension)
                                    currentCase.AdditionalData.Add(dimension.ToTable, row[dimension.ToTable].ToString());

                        if (currentFact != null)
                            currentFact.Add(currentCase);
                    }

                    // 3. create an event and add it to the case
                    var currentEvent = new Event(row[DBWorker.MetaData.EventClassifier].ToString());
                    foreach (var columnName in DBWorker.MetaData.ListOfEventsTableColumnNames)
                        currentEvent.Information.Add(columnName, row[columnName].ToString());

                    if (currentCase != null)
                        currentCase.EventList.Add(currentEvent);
                }
                // since the last case and fact are not saved yet, we need to do this now.
                listOfFacts.Add(currentFact);

                return listOfFacts;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// This takes a tablename (most likely "FACT") and loads all foreign keys, which should point to a dimension (Table + column).
        /// It creates a List of Dimensions and adds a dimension for each row the Database returns. It then goes on recursively 
        /// and grabs the Table + Column of the "next" Table, meaning the one you end up at, if you roll up the cube.
        /// Note that in this piece of code a dimension may have MULTIPLE sub-ListOfDimensions, which should not exist in the model, 
        /// but IF they do, it's not a problem at this point (but rather a GUI-problem)
        /// </summary>
        /// <param name="tablename">The Name of the Table whose ListOfDimensions are to be found.</param>
        /// <returns>A list of ListOfDimensions which, most likely, contain even more ListOfDimensions themselves.</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public List<Dimension> GetDimensionsOf(String tablename)
        {
            DataTable referencedTables;
            DataTable dimensionContentDataTable;
            List<Dimension> resultingListOfDimensions = new List<Dimension>();

            try
            {
                Open();

                // every table row is a dimension
                referencedTables = GetReferencingDataTable(tablename);

                foreach (DataRow referencedTableRow in referencedTables.Rows)
                {
                    // create a dimension-object (see MetaWorker.Dimension)
                    Dimension newDimension = new Dimension(referencedTableRow["FromColumn"].ToString(), referencedTableRow["FromConstraint"].ToString(),
                        referencedTableRow["FromTable"].ToString(), referencedTableRow["FromColumn"].ToString(),
                        referencedTableRow["ToConstraint"].ToString(), referencedTableRow["ToTable"].ToString(), referencedTableRow["ToColumn"].ToString());

                    // Load the content of this table
                    dimensionContentDataTable = GetTableContent(referencedTableRow["ToTable"].ToString());

                    // create DimensionContent-Objects and add them to the current dimension
                    foreach (DataRow dimensionContentRow in dimensionContentDataTable.Rows)
                    {
                        string description = "";
                        if (dimensionContentRow.ItemArray.Count() > 2)
                            description = dimensionContentRow[2].ToString();
                        newDimension.DimensionContentsList.Add(new DimensionContent(dimensionContentRow[0].ToString(), dimensionContentRow[1].ToString(), description));
                    }

                    // save the DimensionColumnNames for generated DB-querys
                    newDimension.DimensionColumnNames = new DimensionColumnNames(dimensionContentDataTable.Columns[0].ColumnName,
                        dimensionContentDataTable.Columns[1].ColumnName, dimensionContentDataTable.Columns[2].ColumnName);

                    dimensionContentDataTable.Reset();
                    // now recursively find all sub-ListOfDimensions of this Table and add them to the current dimension
                    newDimension.AddDimensionLevel(GetDimensionsOf(referencedTableRow["ToTable"].ToString()));

                    // add the current dimension to the list that will be returned eventually
                    resultingListOfDimensions.Add(newDimension);
                }
                return resultingListOfDimensions;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Return the count of events per field
        /// </summary>
        /// <param name="sqlcode"></param>
        /// <returns></returns>
        /// <author>Bernhard Bruns</author>
        public string GetFieldCount(string sqlcode)
        {
            try
            {
                var factDataTable = GetFactDataTable(sqlcode);
                return factDataTable.Rows[0].ItemArray[0].ToString();
            }
            catch (Exception)
            {
                return "Error while loading field count.";
            }
        }

        #endregion

        #region misc
        /// <summary>
        /// This method is call, if an object of this class is disposed.
        /// </summary>
        /// <param name="disposing"></param>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                Connection.Close();
            }
        }

        /// <summary>
        /// This method is call, if an object of this class is disposed.
        /// </summary>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Sets Connection = null for test reasons
        /// </summary>
        public void KillConnection()
        {
            Connection.Close();
            Connection = null;
        }


        #endregion
    }
}
