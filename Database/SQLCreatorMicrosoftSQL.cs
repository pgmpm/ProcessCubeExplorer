
namespace pgmpm.Database
{
    class SQLCreatorMicrosoftSQL: SQLCreator
    {
        public override string TableNameFormatter(string tableName)
        {
            return "[" + DBWorker.DbConnectionParameter.Database + "].[dbo].[" + tableName + "]";
        }
        public override string TableColumnFormatter(string tableName, string columnName)
        {
            return "[" + DBWorker.DbConnectionParameter.Database + "].[dbo].[" + tableName + "].["+columnName+"]";
        }
    }
}
