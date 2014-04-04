
namespace pgmpm.Database
{
    class SQLCreatorPostgres : SQLCreator
    {
        public SQLCreatorPostgres()
            : base()
        {
            StringMarker = "\'";

        }

        /// <summary>
        /// Formats the table name.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <returns>Formatted table name</returns>
        /// <author>Bernd Nottbeck</author>
        public override string TableNameFormatter(string tableName)
        {
            return "\"" + DBWorker.DbConnectionParameter.Database + "\".\"" + tableName + "\"";
        }

        /// <summary>
        /// Formats a table and column for the specific SQL type
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="columnName">Column name</param>
        /// <returns>A string representing the table column for the SQL type</returns>
        /// <author>Bernd Nottbeck</author>
        public override string TableColumnFormatter(string tableName, string columnName)
        {
            return "\"" + DBWorker.DbConnectionParameter.Database + "\".\"" + tableName + "\".\"" + columnName + "\"";
        }
    }
}
