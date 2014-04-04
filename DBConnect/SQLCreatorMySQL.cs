
namespace pgmpm.Database
{
    class SQLCreatorMySQL : SQLCreator
    {
        /// <summary>
        /// Formats the table name.
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <returns>Formatted table name</returns>
        /// <author>Bernd Nottbeck</author>
        public override string TableNameFormatter(string tableName)
        {
            return "`" + tableName + "`";
        }
    }
}
