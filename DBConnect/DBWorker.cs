using System;
using System.Collections.Generic;
using pgmpm.Database.Exceptions;
using pgmpm.Database.Model;
using pgmpm.MatrixSelection.Dimensions;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.Database
{
    /// <summary>
    /// The worker manages the Database-connection and offers methods to establish a connection. It functions as a layer of abstraction towards the actual implementation.
    /// </summary>
    /// <author>Jannik Arndt, Bernd Nottbeck</author>
    public static class DBWorker
    {
        public readonly static String[] SupportedDbTypes =
        {
            "MySQL",
            "Oracle",
            "PostgreSQL",
            "MS-SQL",
            "MS-SQL Windows Auth",
            "SQLite",
            "SQLite In-Memory"
        };

        internal static MPMdBConnection DatabaseConnection;
        public static List<Dimension> SelectedDimensions;
        public static SQLCreator SqlCreator;
        public static MetaDataRepository MetaData;
        public static ConnectionParameters DbConnectionParameter { get; set; }

        /// <summary>
        /// Configures a new Database-connection that can be accessed from everywhere.
        /// </summary>
        /// <param name="connectionParameter">An Object that stores the connection-details.</param>
        /// <returns>True if the connection configuration was successful.</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public static bool ConfigureDBConnection(ConnectionParameters connectionParameter)
        {
            if (String.IsNullOrEmpty(connectionParameter.Type))
                throw new ConnectionTypeNotGivenException();

            if (connectionParameter.Type.Equals("SQLite") || connectionParameter.Type.Equals("SQLite In-Memory"))
            {
                if (String.IsNullOrEmpty(connectionParameter.Database))
                    throw new NoParamsGivenException();
            }
            else
            {
                if (String.IsNullOrEmpty(connectionParameter.Database) || String.IsNullOrEmpty(connectionParameter.Host) ||
                String.IsNullOrEmpty(connectionParameter.Name) || String.IsNullOrEmpty(connectionParameter.Port))
                    throw new NoParamsGivenException();
            }

            DbConnectionParameter = connectionParameter;

            switch (connectionParameter.Type)
            {
                case "MySQL":
                    DatabaseConnection = new MPMMySQLConnection(connectionParameter);
                    return true;
                case "Oracle":
                    DatabaseConnection = new MPMOracleSQLConnection(connectionParameter);
                    return true;
                case "PostgreSQL":
                    DatabaseConnection = new MPMPostgreSQLConnection(connectionParameter);
                    return true;
                case "MS-SQL":
                    DatabaseConnection = new MPMMicrosoftSQL(connectionParameter);
                    return true;
                case "MS-SQL Windows Auth":
                    DatabaseConnection = new MPMMicrosoftSQL(connectionParameter, false);
                    return true;
                case "SQLite":
                    DatabaseConnection = new MPMSQLiteConnection(connectionParameter);
                    return true;
                case "SQLite In-Memory":
                    DatabaseConnection = new MPMSQLiteConnection(connectionParameter, true);
                    return true;
                default:
                    throw new DatabaseDoesNotExist();
            }
        }

        /// <summary>
        /// Closes the current connection, if there is one.
        /// </summary>
        /// <returns>True if connection is closed</returns>
        /// <author>Bernhard Bruns, Moritz Eversmann, Bernd Nottbeck</author>
        public static bool CloseConnection()
        {
            if (DatabaseConnection != null)
            {
                if (DatabaseConnection.IsOpen())
                    return DatabaseConnection.Close();
                return true;
            }
            return true;
        }

        /// <summary>
        /// Returns the given Name of the current connection.
        /// </summary>
        /// <returns>The Name as a string or "No connection".</returns>
        /// <author>Jannik Arndt, Bernhard Bruns, Moritz Eversmann, Bernd Nottbeck</author>
        public static String GetConnectionName()
        {
            if (DatabaseConnection != null)
                return DatabaseConnection.GetName();
            return "No connection";
        }

        /// <summary>
        /// Returns whether there is a connection or not. This actually opens the connection! If you want
        /// to check if the connection is Open or not, User IsOpen()!
        /// </summary>
        /// <returns>Whether there is a connection or not</returns>
        /// <author>Jannik Arndt, Bernhard Bruns, Moritz Eversmann, Bernd Nottbeck</author>
        public static bool OpenConnection()
        {
            if (DatabaseConnection != null)
                return DatabaseConnection.Open();
            return false;
        }

        /// <summary>
        /// Returns if the connection is Open right now. This does not check for connectivity!
        /// </summary>
        /// <returns>True if connection is open</returns>
        /// <author>Jannik Arndt, Bernhard Bruns, Moritz Eversmann, Bernd Nottbeck</author>
        public static bool IsOpen()
        {
            if (DatabaseConnection != null)
                return DatabaseConnection.IsOpen();
            return false;
        }

        /// <summary>
        /// Returns a DataTable of the Fact-Table.
        /// </summary>
        /// <returns>A list of Facts.</returns>
        /// <author>Jannik Arndt, Bernhard Bruns, Moritz Eversmann, Bernd Nottbeck</author>
        public static List<List<Case>> GetFacts(List<SelectedDimension> selectedFactDimensions, List<SelectedDimension> selectedEventDimensions, Field field)
        {
            if (field == null)
                throw new NoParamsGivenException("No field given");

            OpenConnection();

            field.SqlCode = SqlCreator.GetSQLCode(selectedFactDimensions, selectedEventDimensions, field);

            return DatabaseConnection.GetFacts(field.SqlCode.Replace("\n", " "));
        }


        /// <summary>
        /// Return the count of the events per field
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <returns></returns>
        /// <author>Bernhard Bruns</author>
        public static string GetCountFromSqlStatement(String sqlStatement)
        {
            return DatabaseConnection.GetFieldCount(sqlStatement);
        }


        /// <summary>
        /// Return a string which contains a sql-statement to count the event per field
        /// </summary>
        /// <param name="selectedDimensionList"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        /// <author>Bernhard Bruns</author>
        public static string CreateEventCountSqlStatement(List<SelectedDimension> selectedDimensionList, Field field)
        {
            return SqlCreator.GetSQLCodeCountEvents(selectedDimensionList, null, field);
        }


        /// <summary>
        /// Return a string which contains a sql-statement to count only unique events per field
        /// </summary>
        /// <param name="selectedDimensionList"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        /// <author>Bernhard Bruns</author>
        public static string CreateUniqueEventCountSqlStatement(List<SelectedDimension> selectedDimensionList, Field field)
        {
            return SqlCreator.GetSQLCodeCountUniqueEvents(selectedDimensionList, null, field);
        }

        /// <summary>
        /// Return a string which contains a sql-statement to count the cases per field
        /// </summary>
        /// <param name="selectedDimensionList"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        /// <author>Bernhard Bruns</author>
        public static string CreateCaseCountSqlStatement(List<SelectedDimension> selectedDimensionList, Field field)
        {
            return SqlCreator.GetSQLCodeCountCases(selectedDimensionList, null, field);
        }

        /// <summary>
        /// Returns MetaData
        /// </summary>
        /// <returns>A FactTable-Object, already filled with ListOfDimensions and everything you need.</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public static MetaDataRepository GetMetaData(string factTableName, string eventTableName)
        {
            return DatabaseConnection.GetMetaData(factTableName, eventTableName);
        }

        /// <summary>
        /// Resets the internal DatabaseConnection-connection and the connection parameters.
        /// </summary>
        public static void Reset()
        {
            DatabaseConnection = null;
            DbConnectionParameter = null;
        }

        /// <summary>
        /// Dispose a connection.
        /// </summary>
        /// <returns></returns>
        public static bool DisposeConnection()
        {
            DatabaseConnection.Dispose();
            return true;
        }

        /// <summary>
        /// Try a connection.
        /// </summary>
        /// <returns></returns>
        public static bool TryConnection()
        {
            return DatabaseConnection.TryConnection();
        }

        /// <summary>
        /// Builds the metadata-repository from the Database.
        /// </summary>
        /// <returns>True if the repository is built, false if the operation failed.</returns>
        public static bool BuildMetadataRepository()
        {
            if (!IsOpen())
                return false;

            MetaData = GetMetaData(SqlCreator.FactTableName, SqlCreator.EventTableName);

            return MetaData != null;
        }
    }
}
