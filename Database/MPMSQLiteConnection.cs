using pgmpm.Database.Exceptions;
using pgmpm.Database.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Text;

namespace pgmpm.Database
{
    /// <summary>
    /// Connector-class for SQLite-databases. 
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public class MPMSQLiteConnection : MPMdBConnection, IDisposable
    {
        bool InMemory = false;
        #region connection

        public MPMSQLiteConnection(ConnectionParameters conParams, bool _inMemory = false)
            : base(conParams)
        {
            DBWorker.SqlCreator = new SQLCreatorSQLite();

            if (_inMemory)
            {
                //ReInitialize for In-Memory
                this.InMemory = true;
                if (Connection != null) Connection.Close();
                Initialize(BuildConnectionString());
            }
        }

        public override void Initialize(String conString)
        {
            if (InMemory)
            {
                try
                {
                    SQLiteConnection sourceConnection = new SQLiteConnection(conString);
                    SQLiteConnection destinationConnection = new SQLiteConnection("Data Source=:memory:");

                    sourceConnection.Open();
                    destinationConnection.Open();

                    sourceConnection.BackupDatabase(destinationConnection, "main", "main", -1, Callback, 0);

                    sourceConnection.Close();
                    destinationConnection.Close();

                    Connection = destinationConnection;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            else
            {
                Connection = new SQLiteConnection(conString);
            }
        }

        /// <summary>
        /// Callback-Method for a Progressbar, while the sqlite database is copying to the memory
        /// </summary>
        /// <param name="srcConnection"></param>
        /// <param name="srcName"></param>
        /// <param name="destConnection"></param>
        /// <param name="destName"></param>
        /// <param name="pages"></param>
        /// <param name="remaining"></param>
        /// <param name="pageCount"></param>
        /// <param name="retry"></param>
        /// <returns></returns>
        /// <autor>Andrej Albrecht</autor>
        protected virtual bool Callback(SQLiteConnection srcConnection, string srcName, SQLiteConnection destConnection, string destName,
                                      int pages, int remaining, int pageCount, bool retry)
        {
            //Console.WriteLine("pages:"+pages+" remaining:"+remaining+" pageCount:"+pageCount+" retry:"+retry+"");
            //Completion = 100% * (pagecount() - remaining()) / pagecount()

            return true;
        }

        public override String BuildConnectionString()
        {
            StringBuilder conString = new StringBuilder("Data Source=" + DbConnectionParameter.Database + ";FailIfMissing=True;Read Only=True;UTF8Encoding=True;");

            if (!DbConnectionParameter.Password.Equals("") && !DbConnectionParameter.Password.Equals("-"))
            {
                conString.Append("Password=");
                conString.Append(DbConnectionParameter.Password);
                conString.Append(";");
            }
            return conString.ToString();
        }
        #endregion

        #region content

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        /// <autor>Andrej Albrecht</autor>
        public override DataTable GetReferencingDataTable(string tablename)
        {
            //A subquery on PRAGMA FOREIGN_KEY_LIST('<tablename>') is not supported by SQLite
            //
            String sql = "PRAGMA FOREIGN_KEY_LIST('" + tablename + "');";

            DataTable tempTable = new DataTable();
            SQLiteCommand getMetaDataSQL = new SQLiteCommand(
            sql, (SQLiteConnection)Connection);

            SQLiteDataAdapter metaDataAdapter = new SQLiteDataAdapter(getMetaDataSQL);
            metaDataAdapter.Fill(tempTable);

            DataTable tableForeignKeys = new DataTable();
            DataColumn columnID = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "id" };

            DataColumn columnTABLE = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "ToTable" };

            DataColumn columnFROM = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "FromColumn" };

            DataColumn columnTO = new DataColumn { DataType = Type.GetType("System.String"), ColumnName = "ToColumn" };

            DataColumn columnFromTable = new DataColumn
            {
                DataType = Type.GetType("System.String"),
                ColumnName = "FromTable"
            };

            DataColumn columnFromConstraint = new DataColumn
            {
                DataType = Type.GetType("System.String"),
                ColumnName = "FromConstraint"
            };

            DataColumn columnToConstraint = new DataColumn
            {
                DataType = Type.GetType("System.String"),
                ColumnName = "ToConstraint"
            };

            tableForeignKeys.Columns.Add(columnID);
            tableForeignKeys.Columns.Add(columnTABLE);
            tableForeignKeys.Columns.Add(columnFROM);
            tableForeignKeys.Columns.Add(columnTO);
            tableForeignKeys.Columns.Add(columnFromTable);
            tableForeignKeys.Columns.Add(columnFromConstraint);
            tableForeignKeys.Columns.Add(columnToConstraint);

            foreach (DataRow ReferencedTableRow in tempTable.Rows)
            {
                DataRow newRow = tableForeignKeys.NewRow();
                newRow["id"] = ReferencedTableRow["id"].ToString();
                newRow["ToTable"] = ReferencedTableRow["table"].ToString();
                newRow["FromColumn"] = ReferencedTableRow["from"].ToString();
                newRow["ToColumn"] = ReferencedTableRow["to"].ToString();
                newRow["FromTable"] = tablename;
                newRow["FromConstraint"] = "";
                newRow["ToConstraint"] = "";

                tableForeignKeys.Rows.Add(newRow);
            }

            return tableForeignKeys;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public override DbDataReader ReadColumnsForTable(String table)
        {
            try
            {
                SQLiteCommand cmd = new SQLiteCommand("pragma table_info('" + table + "');", (SQLiteConnection)Connection);
                SQLiteDataReader reader = cmd.ExecuteReader();
                return reader;
            }          
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public override List<String> GetColumnNamesOfTable(String table)
        {
            List<String> listOfColoumnames = new List<String>();
            try
            {
                Open();

                SQLiteDataReader reader = (SQLiteDataReader)ReadColumnsForTable(table);

                while (reader.Read())
                {
                    listOfColoumnames.Add((String)reader["name"]);
                }
                return listOfColoumnames;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public override DataTable GetTableContent(String tablename)
        {
            DataTable tempTable2 = new DataTable();
            SQLiteCommand getContentSQL = new SQLiteCommand("SELECT * FROM '" + tablename + "'", (SQLiteConnection)Connection);

            SQLiteDataAdapter contentAdapter = new SQLiteDataAdapter(getContentSQL);
            contentAdapter.Fill(tempTable2);
            return tempTable2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlcode"></param>
        /// <returns></returns>
        public override DataTable GetFactDataTable(string sqlcode)
        {
            DataTable factDataTable = new DataTable();
            SQLiteCommand getFactsSQL = new SQLiteCommand(sqlcode, (SQLiteConnection)Connection);
            SQLiteDataAdapter factAdapter = new SQLiteDataAdapter(getFactsSQL);
            factAdapter.Fill(factDataTable);
            return factDataTable;
        }

        #endregion

        #region misc

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public override void HandleException(DbException ex)
        {
            Console.WriteLine("SQLite DbException: " + ex);

            if (ex is SQLiteException)
            {
                switch (((SQLiteException)ex).ErrorCode)
                {
                    default:
                        throw new DBException(ex.Message + "\nErrorCode: " + ((SQLiteException)ex).ErrorCode, ex);
                }
            }
        }

        #endregion
    }
}
