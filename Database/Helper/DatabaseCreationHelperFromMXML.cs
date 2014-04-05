using System.Text;
using pgmpm.MatrixSelection.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace pgmpm.Database.Helper
{
    /// <summary>
    /// Helper to generate a Data Ware House with the needed Meta-Data-Repository definition from a mxml file
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public class DatabaseCreationHelperFromMXML : DatabaseCreationHelper
    {
        public DatabaseCreationHelperFromMXML(string databaseType, string sourceFile)
            : base(databaseType, sourceFile)
        {
        }

        protected override string CreateInsertStatementForMySqldwh(string sourceFile)
        {
            EventLog eventLog = createEventLogFrom(sourceFile);
            String insertStatment = createInsertStatementForMySQLFrom(eventLog);
            return insertStatment;
        }

        private string createInsertStatementForMySQLFrom(EventLog eventLog)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("-- PG MPM Insert Statement\n");
            sql.Append("SET AUTOCOMMIT=0;\n");
            sql.Append("START TRANSACTION;\n");
            sql.Append("\nINSERT INTO `DIM_PROCESS` (`content`, `description`) VALUES ('" + eventLog.Name + "', '" + eventLog.Name + "');\n\n");

            //Insert Time if not exists
            //sql += "\nINSERT INTO `DIM_DATETIME` (`content`, `description`) VALUES (CAST(NOW() AS char), CAST(NOW() AS char));\n\n";
            sql.Append("\nINSERT INTO `DIM_TIME_DAY` (`content`, `description`) VALUES ('2000-01-01 00:00:00', '2000-01-01 00:00:00');\n\n");

            sql.Append("\nINSERT INTO `FACT` (`process`, `time`) VALUES (\n"
                + "(SELECT COALESCE(MAX(id),\n"
                + "  0 ) as maxID \n"
                + "    FROM DIM_PROCESS),\n"
                + "(SELECT COALESCE(MAX(id),\n"
                + "  0 ) as maxID \n"
                + "    FROM DIM_DATETIME));\n\n");

            foreach (Case caseEV in eventLog.Cases)
            {
                sql.Append("\n\n");
                sql.Append("-- Events for Case:\n");

                //sql += "Case: " + caseEV.Name + " \n";
                sql.Append("INSERT INTO `CASE` (`fact_id`) VALUES ((SELECT COALESCE(MAX(`fact_id`),0) as maxID FROM `FACT`)); \n\n");
                sql.Append("\n");

                int i = 0;
                foreach (Event eventCA in caseEV.EventList)
                {
                    String timestamp = "";
                    //Parse the dictionary with additional informations:
                    foreach (KeyValuePair<string, string> info in eventCA.Information)
                    {
                        switch (info.Key.ToLower())
                        {
                            case "timestamp":
                                //Console.WriteLine("timestamp: " + info.Value);
                                String dateString = info.Value;
                                DateTime dateValue = new DateTime();
                                if (DateTime.TryParse(dateString, out dateValue))
                                {
                                    timestamp = dateValue.ToString("yyyy-MM-dd HH:mm:ss");
                                    // "1976-04-12T22:10:00"
                                    //mysqlDateTime = dateValue.ToString(isoDateTimeFormat.SortableDateTimePattern);
                                    //Console.WriteLine(" timestamp:" + timestamp);
                                }
                                else
                                {
                                    throw new Exception("Can't convert timestamp");
                                }
                                break;
                        }
                    }

                    //sql += "Event: " + eventCA.Name + "\n";
                    sql.Append("INSERT INTO `EVENT` (`case_id`, `event`, `activity`, `sequence`, `timestamp`) VALUES "
                        + "((SELECT COALESCE(MAX(`case_id`),0) as maxID FROM `CASE`), 'PROCEDURE', '" + eventCA.Name + "', " + i + ", '" + timestamp + "');\n");
                    i++;
                }


            }


            //sql += "ROLLBACK;\n";

            sql.Append("COMMIT;\n");

            return sql.ToString();
        }

        private EventLog createEventLogFrom(string sourceFile)
        {
            //The Method should return a list with EventLogs!

            EventLog eventLog = new EventLog(Path.GetFileName(sourceFile));
            Case caseFound = new Case();
            Event eventFound = new Event();

            String theActualElement = "";

            XmlTextReader reader = new XmlTextReader(sourceFile);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        theActualElement = reader.Name.ToLower();

                        if (reader.Name.ToLower().Equals("processinstance"))
                        {
                            //A new Case starts
                            caseFound = new Case(reader.GetAttribute("id"));
                        }
                        else if (reader.Name.ToLower().Equals("audittrailentry"))
                        {
                            //A new transition starts
                            eventFound = new Event();
                        }
                        
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        if (theActualElement.Equals("workflowmodelelement"))
                        {
                            eventFound.Name = reader.Value;
                        }
                        else if (theActualElement.Equals("timestamp"))
                        {
                            eventFound.Information.Add("TIMESTAMP", reader.Value);
                        }
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        if (reader.Name.ToLower().Equals("processinstance"))
                        {
                            //The case ends in the mxml file and the case must put to the eventlog
                            eventLog.Cases.Add(caseFound);
                        }
                        else if (reader.Name.ToLower().Equals("audittrailentry"))
                        {
                            //The transition ends here
                            caseFound.EventList.Add(eventFound);
                        }
                        
                        break;
                }
            }

            return eventLog;
        }

        protected override string CreateInsertStatementForOracleDwh(string sourceFile)
        {
            throw new NotImplementedException();
        }

        protected override string CreateInsertStatementForPostgreSqldwh(string sourceFile)
        {
            throw new NotImplementedException();
        }

        protected override string CreateInsertStatementForSqLiteDwh(string sourceFile)
        {
            throw new NotImplementedException();
        }
    }
}