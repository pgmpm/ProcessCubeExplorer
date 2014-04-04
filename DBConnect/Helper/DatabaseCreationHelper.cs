using System;
using System.IO;

namespace pgmpm.Database.Helper
{
    /// <summary>
    /// Helper to generate a Data Ware House with the needed Meta-Data-Repository definition
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public abstract class DatabaseCreationHelper
    {
        protected string DatabaseType;
        protected string SourceFile;

        public DatabaseCreationHelper(string databaseType, string sourceFile)
        {
            DatabaseType = databaseType;
            SourceFile = sourceFile;
        }

        public string CreateInsertSqlStatement()
        {

            switch (DatabaseType)
            {
                case "MySQL": // MySQL
                    return CreateInsertStatementForMySqldwh(SourceFile);
                case "Oracle": // Oracle
                    return CreateInsertStatementForOracleDwh(SourceFile);
                case "PostgreSQL": //PostgreSQL
                    return CreateInsertStatementForPostgreSqldwh(SourceFile);
                case "SQLite": // SQLite
                case "SQLite In-Memory":
                    return CreateInsertStatementForSqLiteDwh(SourceFile);
                case "MS-SQL":
                case "MS-SQL Windows Auth": throw new NotImplementedException();
                default: throw new NotImplementedException();
            }
        }

        public string CreateEmptyDwh()
        {
            switch (DatabaseType)
            {
                case "mysql": // MySQL
                    return CreateEmptyMySqldwh();
                case "oracle": // Oracle
                    return CreateEmptyOracleDwh();
                case "postgresql": //PostgreSQL
                    return CreateEmptyPostgreSqldwh();
                case "sqlite": // SQLite
                    return CreateEmptySqLiteDwh();
                case "mssql": throw new NotImplementedException();
            }

            throw new NotImplementedException();
        }


        protected string CreateEmptyMySqldwh()
        {
            return File.ReadAllText(@"SQLScripts\CreateDWHMySQL.sql");
        }

        protected string CreateEmptyOracleDwh()
        {
            throw new NotImplementedException();
            //return File.ReadAllText(@"SQLScripts\CreateDWHOracle.sql");  
        }

        protected string CreateEmptyPostgreSqldwh()
        {
            throw new NotImplementedException();
            //return File.ReadAllText(@"SQLScripts\CreateDWHPostgreSQL.sql");
        }

        protected string CreateEmptySqLiteDwh()
        {
            throw new NotImplementedException();
            //return File.ReadAllText(@"SQLScripts\CreateDWHSQLite.sql");
        }

        protected abstract string CreateInsertStatementForMySqldwh(string sourceFile);
        protected abstract string CreateInsertStatementForOracleDwh(string sourceFile);
        protected abstract string CreateInsertStatementForPostgreSqldwh(string sourceFile);
        protected abstract string CreateInsertStatementForSqLiteDwh(string sourceFile);
    }
}
