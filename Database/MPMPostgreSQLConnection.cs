using Npgsql;
using pgmpm.Database.Exceptions;
using pgmpm.Database.Model;
using System;
using System.Data;
using System.Data.Common;
namespace pgmpm.Database
{
    /// <summary>
    /// Connector-class for PostgreSQL-databases. Implements the MPMdBConnection-Interface to be compatible to the software.
    /// This class should only throw DBExceptions, NO NpgsqlException, since those will NOT be handled!
    /// </summary>
    /// <author>Jannik Arndt, Bernd Nottbeck</author>
    public class MPMPostgreSQLConnection : MPMdBConnection, IDisposable
    {
        #region connection
        //private NpgsqlConnection Connection;

        /// <summary>
        /// Constructor, initializes a new connection from the ConnectionParameters-object.
        /// </summary>
        /// <param name="connectionParameters">The connection parameters.</param>
        public MPMPostgreSQLConnection(ConnectionParameters connectionParameters)
            : base(connectionParameters)
        {
            DBWorker.SqlCreator = new SQLCreatorPostgres();
        }

        /// <summary>
        /// Initializes a Connection.
        /// </summary>
        /// <param name="conString">String containing Connection Parameter</param>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override void Initialize(String conString)
        {
            Connection = new NpgsqlConnection(conString);
        }

        /// <summary>
        /// Builds a connection string something like "MyConnection ("
        /// </summary>
        /// <returns>Oracle connection string</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override String BuildConnectionString()
        {
            string conString = "Server=" + DbConnectionParameter.Host + ";" + "Port=" + DbConnectionParameter.Port + ";" + "User Id=" + DbConnectionParameter.User + ";" + "Password=" + DbConnectionParameter.Password + ";" + "Database=" + DbConnectionParameter.Database + ";";
            return conString;
        }

        #endregion

        #region content
        /// <summary>
        /// Gets a DataTable containing names of all tables referenced in the specified table.
        /// </summary>
        /// <param name="tablename">Table to check.</param>
        /// <returns>DataTable with all referencing tables.</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override DataTable GetReferencingDataTable(string tablename)
        {
            DataTable tempTable = new DataTable();
            NpgsqlCommand getMetaDataSQL = new NpgsqlCommand(
        "SELECT tc.constraint_name AS FROMCONSTRAINT, tc.table_name AS FROMTABLE, kcu.column_name AS FROMCOLUMN, " +
        "ccu.constraint_name AS TOCONSTRAINT, ccu.table_name AS TOTABLE, ccu.column_name AS TOCOLUMN  " +
        "FROM information_schema.table_constraints AS tc JOIN information_schema.key_column_usage AS kcu ON tc.constraint_name = kcu.constraint_name JOIN information_schema.constraint_column_usage AS ccu ON ccu.constraint_name = tc.constraint_name " +
        "WHERE constraint_type = 'FOREIGN KEY' AND tc.table_name = '" + tablename + "' AND  ccu.table_name != '" + DBWorker.SqlCreator.CaseTableName + "';", (NpgsqlConnection)Connection);

            NpgsqlDataAdapter metaDataAdapter = new NpgsqlDataAdapter(getMetaDataSQL);
            metaDataAdapter.Fill(tempTable);
            return tempTable;
        }

        /// <summary>
        /// Executes a DbDataReader stream getting the column names for a specified table.
        /// </summary>
        /// <param name="table">Table to analyze</param>
        /// <returns>DbDataReader with column names</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override DbDataReader ReadColumnsForTable(String table)
        {
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT column_name FROM information_schema.columns WHERE table_schema = '" + DbConnectionParameter.Database + "' AND table_name = '" + table + "';", (NpgsqlConnection)Connection);
            DbDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Method to load dimension level values for a specific dimension level.
        /// </summary>
        /// <param name="tablename">The name of the TABLE in the FROM-part</param>
        /// <returns>Data Table</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override DataTable GetTableContent(String tablename)
        {
            DataTable tempTable2 = new DataTable();
            NpgsqlCommand getContentSQL = new NpgsqlCommand("SELECT * FROM \"" + DbConnectionParameter.Database + "\".\"" + tablename + "\" ", (NpgsqlConnection)Connection);
            NpgsqlDataAdapter contentAdapter = new NpgsqlDataAdapter(getContentSQL);
            contentAdapter.Fill(tempTable2);
            return tempTable2;

        }

        /// <summary>
        /// Reads raw fact data from a database.
        /// </summary>
        /// <param name="sqlcode">Generates SQL statement</param>
        /// <returns>Generates SQL statement to load facts.</returns>
        public override DataTable GetFactDataTable(string sqlcode)
        {
            DataTable factDataTable = new DataTable();
            NpgsqlCommand getFactsSQL = new NpgsqlCommand(sqlcode, (NpgsqlConnection)Connection);
            NpgsqlDataAdapter factAdapter = new NpgsqlDataAdapter(getFactsSQL);
            factAdapter.Fill(factDataTable);
            return factDataTable;
        }


        #endregion

        #region misc
        /// <summary>
        /// Turns all NpgsqlException into DBExceptions.
        /// Error Codes: http://www.postgresql.org/docs/9.1/static/errcodes-appendix.html
        /// </summary>
        /// <param name="ex">A NpgsqlException to handle.</param>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override void HandleException(DbException ex)
        {
            //NpgsqlException ex
            NpgsqlException exception = ex as NpgsqlException;
            if (exception != null)
            {
                switch (exception.Code)
                {
                    case "01000":
                        throw new DBException("Warning! " + exception.Message, ex);
                    //throw new WarningException(ex.Message, ex);
                    case "08000":
                        throw new NoConnectionException(exception.Message, ex);
                    case "28000":
                        throw new WrongCredentialsException(exception.Message, ex);
                    case "28P01":
                        throw new WrongCredentialsException(exception.Message, ex);
                    case "3D000":
                        throw new DatabaseDoesNotExist(exception.Message, ex);
                    default:
                        throw new DBException(exception.Message + exception.Code, ex);
                }
            }
            throw ex;
        }

        #endregion
    }
}