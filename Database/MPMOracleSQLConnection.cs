using System.Text;
using Oracle.DataAccess.Client;
using pgmpm.Database.Exceptions;
using pgmpm.Database.Model;
using System;
using System.Data;
using System.Data.Common;

namespace pgmpm.Database
{
    /// <summary>
    /// The implementation of MPMdBConnection for Oracle-databases. This class contains all the sql-code and only returns non-oracle-specific 
    /// objects like DataTable or DataSet. Its methods should NEVER be called directly but only via corresponding DBWorker-methods, that 
    /// are generalized for all Database-connections and may handle oracle-specific quirks.
    /// </summary>
    /// <author>Jannik Arndt, Bernd Nottbeck</author>
    public class MPMOracleSQLConnection : MPMdBConnection, IDisposable
    {
        #region connection
        //private OracleConnection Connection;

        /// <summary>
        /// Creates a new connection to an oracle-Database with the given parameters.
        /// </summary>
        /// <param name="connectionParameters"></param>
        public MPMOracleSQLConnection(ConnectionParameters connectionParameters)
            : base(connectionParameters)
        {
            DBWorker.SqlCreator = new SQLCreator();
        }

        /// <summary>
        /// Initializes a Connection.
        /// </summary>
        /// <param name="conString">String containing Connection Parameter</param>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override void Initialize(String conString)
        {
            Connection = new OracleConnection(conString);
        }

        /// <summary>
        /// Builds a connection string something like "MyConnection (User@127.0.0.1:1521/myService)"
        /// </summary>
        /// <returns>Oracle connection string</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override String BuildConnectionString()
        {
            return "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=" + DbConnectionParameter.Host + ")(PORT=" + DbConnectionParameter.Port + ")))" +
                   "(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=" + DbConnectionParameter.Database + ")));User Id=" + DbConnectionParameter.User + ";Password=" + DbConnectionParameter.Password;
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
            StringBuilder sqlStatement = new StringBuilder();
            sqlStatement.Append("SELECT a.constraint_name FromConstraint, b.table_name FromTable, b.column_name FromColumn, ");
            sqlStatement.Append("a.r_constraint_name ToConstraint, c.table_name ToTable, c.column_name ToColumn ");
            sqlStatement.Append("FROM all_constraints a ");
            sqlStatement.Append("JOIN all_cons_columns b ON a.constraint_name = b.constraint_name ");
            sqlStatement.Append("JOIN all_cons_columns c ON a.r_constraint_name = c.constraint_name ");
            sqlStatement.Append("WHERE constraint_type = 'R' AND a.table_name = :tablename" + " AND c.table_name != '" + DBWorker.SqlCreator.TableNameFormatter(DBWorker.SqlCreator.CaseTableName) + "' AND UPPER(a.OWNER)=UPPER('" + DbConnectionParameter.User + "')");
            DataTable tempTable = new DataTable();
            OracleCommand getMetaDataSQL = new OracleCommand(sqlStatement.ToString(), (OracleConnection)Connection);
            getMetaDataSQL.Parameters.Add(new OracleParameter("tablename", tablename));

            OracleDataAdapter metaDataAdapter = new OracleDataAdapter(getMetaDataSQL);
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
            OracleCommand cmd = new OracleCommand("SELECT column_name FROM user_tab_columns WHERE table_name = :tablename", (OracleConnection)Connection);
            cmd.Parameters.Add(new OracleParameter("tablename", table));
            DbDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Method to load dimension level values for a specific dimension level.
        /// </summary>
        /// <param name="tablename">The Tablename for the FROM-Part</param>
        /// <returns>Data Table</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override DataTable GetTableContent(String tablename)
        {
            DataTable resultingDataTable = new DataTable();
            OracleCommand getContentSql = new OracleCommand("SELECT * FROM " + tablename + "", (OracleConnection)Connection);
            OracleDataAdapter contentAdapter = new OracleDataAdapter(getContentSql);
            contentAdapter.Fill(resultingDataTable);
            return resultingDataTable;
        }

        /// <summary>
        /// Reads raw fact data from a database.
        /// </summary>
        /// <param name="sqlcode">Generated SQL statement</param>
        /// <returns>Raw data table</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override DataTable GetFactDataTable(string sqlcode)
        {
            DataTable factDataTable = new DataTable();
            OracleCommand getFactsSql = new OracleCommand(sqlcode, (OracleConnection)Connection);
            OracleDataAdapter factAdapter = new OracleDataAdapter(getFactsSql);
            factAdapter.Fill(factDataTable);
            return factDataTable;
        }

        #endregion

        #region misc

        /// <summary>
        /// Turns all OracleException into DBExceptions.
        /// </summary>
        /// <param name="ex">A OracleException to handle.</param>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override void HandleException(DbException ex)
        {
            OracleException exception = ex as OracleException;
            if (exception != null)
            {
                switch (exception.Number)
                {
                    case 0:
                        throw new WrongCredentialsException(ex.Message, ex);
                    case 1042:
                        throw new NoConnectionException(ex.Message, ex);
                    case 1045:
                        throw new WrongCredentialsException(ex.Message, ex);
                    case 0918:
                        throw new DBException("Columns are ambiguously defined. Do not use one dimension multiple times!", ex);
                    default:
                        throw new DBException(ex.Message + exception.Number, ex);
                }
            }
        }

        #endregion
    }
}
