using pgmpm.Database.Exceptions;
using pgmpm.Database.Model;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace pgmpm.Database
{
    class MPMMicrosoftSQL : MPMdBConnection
    {
        private bool NoWindowsAuth;
        #region connection
        public MPMMicrosoftSQL(ConnectionParameters conParams, bool noWindowsAuth = true)
            : base(conParams)
        {
            NoWindowsAuth = noWindowsAuth;

            Initialize(BuildConnectionString());

            DBWorker.SqlCreator = new SQLCreatorMicrosoftSQL();
        }

        public override void Initialize(string conString)
        {
            Connection = new SqlConnection(conString);
        }

        public override string BuildConnectionString()
        {

            StringBuilder conString = new StringBuilder();
            conString.Append("Server=")
                .Append(DbConnectionParameter.Host)
                .Append(", ")
                .Append(DbConnectionParameter.Port)
                .Append(";")
                .Append("Database=")
                .Append(DbConnectionParameter.Database)
                .Append(";Network Library=DBMSSOCN;");

            if (NoWindowsAuth)
            {
                conString.Append("User Id=")
                .Append(DbConnectionParameter.User)
                .Append(";")
                .Append("Password=")
                .Append(DbConnectionParameter.Password)
                .Append(";Integrated Security=false;");
            }
            else
            {
                conString.Append("Integrated Security=SSPI;");
            }
            return conString.ToString();
        }

        #endregion

        #region content
        public override DataTable GetReferencingDataTable(string tablename)
        {
            DataTable tempTable = new DataTable();

            StringBuilder sqlStatement = new StringBuilder();
            sqlStatement.Append("select xx.name as FromConstraint ,ft.name as FromTable,fc.name as FromColumn, xx.name as ToConstraint, tt.name as ToTable ,tc.name as ToColumn ")
                .Append("from sys.foreign_key_columns as fk ")
                .Append("inner join sys.tables as ft on fk.parent_object_id = ft.object_id ")
                .Append("inner join sys.columns as fc on fk.parent_object_id = fc.object_id and fk.parent_column_id = fc.column_id ")
                .Append("inner join sys.tables as tt on fk.referenced_object_id = tt.object_id ")
                .Append("inner join sys.columns as tc on fk.referenced_object_id = tc.object_id and fk.referenced_column_id = tc.column_id ")
                .Append("inner join sys.foreign_keys as xx on fk.constraint_object_id=xx.object_id ")
                .Append("where fk.parent_object_id = (select object_id from sys.tables where name = '")
                .Append(tablename)
                .Append("')")
                .Append(" AND tt.object_id != ( select object_id from sys.tables where name = '")
                .Append(DBWorker.SqlCreator.CaseTableName)
                .Append("')");

            SqlCommand cmd = new SqlCommand(sqlStatement.ToString(), (SqlConnection)Connection);
            SqlDataAdapter metaDataAdapter = new SqlDataAdapter(cmd);
            metaDataAdapter.Fill(tempTable);
            return tempTable;

            //Alternative EXEC sp_fkeys "TableName"           
        }

        public override DbDataReader ReadColumnsForTable(string table)
        {
            //exec sp_columns [tablename]
            SqlCommand cmd = new SqlCommand("SELECT * FROM INFORMATION_SCHEMA.COLUMNS where TABLE_NAME='" + table + "'", (SqlConnection)Connection);
            DbDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        public override DataTable GetTableContent(string tablename)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM " + DBWorker.SqlCreator.TableNameFormatter(tablename) + "", (SqlConnection)Connection);

            //SqlCommand cmd = new SqlCommand("SELECT TOP " + MaxRows + " * FROM " + DBWorker.SqlCreator.TableNameFormatter(tablename) + "", (SqlConnection)Connection);
            SqlDataAdapter contentAdapter = new SqlDataAdapter(cmd);
            DataTable tempTable2 = new DataTable();
            contentAdapter.Fill(tempTable2);
            return tempTable2;
        }

        public override DataTable GetFactDataTable(string sqlcode)
        {
            DataTable factDataTable = new DataTable();
            SqlCommand getFactsSQL = new SqlCommand(sqlcode, (SqlConnection)Connection);
            SqlDataAdapter factAdapter = new SqlDataAdapter(getFactsSQL);
            factAdapter.Fill(factDataTable);
            return factDataTable;
        }
        #endregion

        #region misc
        public override void HandleException(DbException ex)
        {
            if (ex is SqlException)
            {
                switch (((SqlException)ex).Number)
                {
                    case 10061:
                        throw new NoConnectionException(ex.Message, ex);
                    case 18456:
                        throw new WrongCredentialsException(ex.Message, ex);
                    default:
                        throw new DBException(ex.Message + ((SqlException)ex).Number, ex);
                }
            }
        }
        #endregion
    }
}
