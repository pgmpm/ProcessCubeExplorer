
namespace pgmpm.Database
{
    class SQLCreatorSQLite : SQLCreator
    {
        public SQLCreatorSQLite()
            : base()
        {
            StringMarker = "\'";
            //FieldSqlObject.select = "SELECT * FROM " + TableNameFormatter(FactTable) + " ";
            //FieldSqlObject.groupby = "";
            //FieldSqlObject.order = "ORDER BY " + TableColumnFormatter(CaseTable, CaseColumn) + ", " + TableColumnFormatter(EventTable, EventSortArgument);

        }

        /// <summary>
        /// Formats the table name.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <returns>Formatted table name</returns>
        /// <author>Bernd Nottbeck</author>
        public override string TableNameFormatter(string tableName)
        {
            return "'" + tableName + "'";
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
            return "'" + tableName + "'." + columnName + "";
        }
    }
}
