using System.Text;
using MySql.Data.MySqlClient;
using pgmpm.Database.Exceptions;
using pgmpm.Database.Model;
using System;
using System.Data;
using System.Data.Common;

namespace pgmpm.Database
{
    /// <summary>
    /// Connector-class for MySQL-databases. Implements the MPMdBConnection-Interface to be compatible to the software.
    /// This class should only throw DBExceptions, NO MySQLExceptions, since those will NOT be handled!
    /// </summary>
    /// <author>Jannik Arndt, Bernd Nottbeck</author>
    public class MPMMySQLConnection : MPMdBConnection, IDisposable
    {
        #region connection
        //private MySqlConnection Connection;

        public MPMMySQLConnection(ConnectionParameters conParams)
            : base(conParams)
        {
            DBWorker.SqlCreator = new SQLCreatorMySQL();
        }

        /// <summary>
        /// Initializes a Connection.
        /// </summary>
        /// <param name="conString">String containing Connection Parameter</param>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override void Initialize(String conString)
        {
            Connection = new MySqlConnection(conString);
        }

        /// <summary>
        /// Builds a connection string.
        /// </summary>
        /// <returns>MySQL connection string</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override String BuildConnectionString()
        {
            string conString = "SERVER=" + DbConnectionParameter.Host + ";" + "DATABASE=" + DbConnectionParameter.Database + ";" + "UID=" + DbConnectionParameter.User + ";" + "PASSWORD=" + DbConnectionParameter.Password + ";PORT=" + DbConnectionParameter.Port + ";";
            return conString;
        }

        #endregion

        #region content

        /// <summary>
        /// Gets a DataTable containing names of all tables referenced in the specified table.
        /// </summary>
        /// <param name="tablename">Table to check.</param>
        /// <returns>DataTable with all referencing tables.</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck, Moritz Eversmann</author>
        public override DataTable GetReferencingDataTable(string tablename)
        {
            DataTable tempTable = new DataTable();

            StringBuilder dataTableString = new StringBuilder();
            dataTableString.Append("SELECT constraint_name as FromConstraint, table_name as FromTable, column_name as FromColumn, constraint_name as ToConstraint, ")
            .Append("referenced_table_name as ToTable, referenced_column_name as ToColumn ")
            .Append("FROM information_schema.key_column_usage ")
            .Append("WHERE constraint_schema = '")
            .Append(DbConnectionParameter.Database)
            .Append("' AND constraint_name != 'PRIMARY' AND table_name = '")
            .Append(tablename)
            .Append("' AND referenced_table_name != '")
            .Append(DBWorker.SqlCreator.CaseTableName)
            .Append("';");
            MySqlCommand getMetaDataSQL = new MySqlCommand(dataTableString.ToString(), (MySqlConnection)Connection);
            MySqlDataAdapter metaDataAdapter = new MySqlDataAdapter(getMetaDataSQL);
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
            MySqlCommand cmd = new MySqlCommand("SELECT column_name FROM information_schema.columns WHERE table_schema = '" + DbConnectionParameter.Database + "' AND table_name = '" + table + "';", (MySqlConnection)Connection);
            DbDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        /// <summary>
        /// Method to load dimension level values for a specific dimension level.
        /// </summary>
        /// <param name="tablename">The name of the TABLE in the FROM-Part</param>
        /// <returns>Data Table</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override DataTable GetTableContent(String tablename)
        {
            DataTable tempTable2 = new DataTable();
            MySqlCommand getContentSQL = new MySqlCommand("SELECT * FROM `" + tablename + "`", (MySqlConnection)Connection);

            /*
             * This SQL-Command seems pretty unsafe, why so? Well, Oracle lets you use parameters for the WHERE-part of the query, 
             * however you can't do the same thing for the FROM-part. God knows why.
             */

            MySqlDataAdapter contentAdapter = new MySqlDataAdapter(getContentSQL);
            contentAdapter.Fill(tempTable2);
            return tempTable2;

        }

        /// <summary>
        /// Reads raw fact data from a database.
        /// </summary>
        /// <param name="sqlcode">Generated SQL statement to load facts.</param>
        /// <returns>DataTable with raw fact data.</returns>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override DataTable GetFactDataTable(string sqlcode)
        {
            DataTable factDataTable = new DataTable();
            MySqlCommand getFactsSQL = new MySqlCommand(sqlcode, (MySqlConnection)Connection);
            MySqlDataAdapter factAdapter = new MySqlDataAdapter(getFactsSQL);
            factAdapter.Fill(factDataTable);
            return factDataTable;
        }

        #endregion

        #region misc
        /// <summary>
        /// Turns all MySqlExceptions into DBExceptions.
        /// Error Codes: http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html
        /// </summary>
        /// <param name="ex">A MySqlException to handle.</param>
        /// <author>Jannik Arndt, Bernd Nottbeck</author>
        public override void HandleException(DbException ex)
        {
            if (ex is MySqlException)
            {
                switch (((MySqlException)ex).Number)
                {
                    case 0:
                        throw new WrongCredentialsException(ex.Message, ex);
                    case 1042:
                        throw new NoConnectionException(ex.Message, ex);
                    case 1045:
                        throw new WrongCredentialsException(ex.Message, ex);
                    case 1046:
                        throw new NoParamsGivenException(ex.Message, ex);
                    default:
                        throw new DBException(ex.Message + ((MySqlException)ex).Number, ex);
                }
            }
        }
        #endregion
    }
}
