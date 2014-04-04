/// <author>Jannik Arndt</author>
using MySql.Data.MySqlClient;
using pgmpm.Diagnostics;
using pgmpm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace pgmpm.Database
{
    /// <summary>
    /// Connector-class for MySQL-databases. Implements the DBConnection-Interface to be compatible to the software.
    /// This class should only throw DBExceptions, NO MySQLExceptions, since those will NOT be handled!
    /// </summary>
    /// <author>Jannik Arndt</author>
    public class MySQLConnection : DBConnection, IDisposable
    {
        private MySqlConnection Connection;

        private ConnectionParameters conParams;

        /// <summary>
        /// Contructor, initializes a new connection from the ConnectionParameters-object.
        /// </summary>
        /// <param Name="ConnectionParameters">The connection parameters.</param>
        public MySQLConnection(ConnectionParameters conParams)
        {
            this.conParams = conParams;
            Initialize(conParams.Host, conParams.Database, conParams.User, conParams.Password, conParams.Port);
        }

        /// <summary>
        /// Initializes a new connection.
        /// </summary>
        /// <param Name="server">The server address.</param>
        /// <param Name="Database">The Database in the mysql-system.</param>
        /// <param Name="User">The username.</param>
        /// <param Name="Password">The Password</param>
        public void Initialize(String server, String database, String user, String password, String port)
        {
            string conString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + user + ";" + "PASSWORD=" + password + ";PORT=" + port + ";";
            Connection = new MySqlConnection(conString);
        }

        /// <summary>
        /// Returns the Name the User has given to the connection.
        /// </summary>
        /// <returns>Given Name + (Database @ Host)</returns>
        public String GetName()
        {
            return conParams.Name + " (" + conParams.Database + "@" + conParams.Host + ")";
        }

        /// <summary>
        /// Checks if the connection state is set to "Open" or not.
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return Connection.State == ConnectionState.Open;
        }

        /// <summary>
        /// Opens and closes the connection
        /// </summary>
        /// <returns>True if succeeded</returns>
        public bool TryConnection()
        {
            try
            {
                if (Open())
                    return true;
                else
                    return false;
            }
            catch (MySqlException ex)
            {
                handleException(ex);
                return false;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        /// <returns>True if succeeded.</returns>
        public bool Open()
        {
            if (Connection == null)
                return false;

            if (Connection.State == ConnectionState.Open)
                return true;
            try
            {
                Connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                handleException(ex);
                return false;
            }
        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <returns>True if succeeded.</returns>
        public bool Close()
        {
            if (Connection == null)
                return false;
            try
            {
                Connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                handleException(ex);
                return false;
            }
        }

        /// <summary>
        /// Returns the columns in a given Table.
        /// </summary>
        /// <param Name="Table">The Name of the Table.</param>
        /// <returns>A List of strings.</returns>
        /// <author>Jannik Arndt</author>
        public List<String> GetColumnNamesOfTable(String tablename)
        {
            List<String> ListOfColoumnames = new List<string>();
            try
            {
                Open();
                MySqlCommand cmd = new MySqlCommand("SELECT column_name FROM information_schema.columns WHERE table_schema = '" + DBWorker.getParams().Database + "' AND table_name = '" + tablename + "';", Connection);
                DbDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    ListOfColoumnames.Add((string)reader["column_name"]);

                return ListOfColoumnames;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Turns all MySqlExceptions into DBExceptions.
        /// Error Codes: http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html
        /// </summary>
        /// <param Name="ex">A MySqlException to handle.</param>
        private void handleException(MySqlException ex)
        {
            //Console.WriteLine("ex.number:" + ex.Number);

            switch (ex.Number)
            {

                case 0:
                    //throw new DBException(ex.Message + ex.Number, ex);
                    throw new WrongCredentialsException(ex.Message, ex);
                case 1042:
                    throw new NoConnectionExpection(ex.Message, ex);
                case 1045:
                    throw new UnauthorizedAccessException(ex.Message, ex);

                default:
                    throw new DBException(ex.Message + ex.Number, ex);
            }
        }

        /// <summary>
        /// This method is call, if an object of this class is disposed.
        /// </summary>
        /// <param Name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
                Connection.Close();
            }
            // free native resources
        }

        /// <summary>
        /// This method is call, if an object of this class is disposed.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public List<Fact> GetFacts(string sqlcode, List<Dimension> selectedDimensions)
        {
            DataTable FactDataTable = new DataTable();
            List<Fact> ListOfFacts = new List<Fact>();

            try
            {
                // Loads a list of all ListOfFacts and then converts it into fact-, case- and event-objects.
                Open();
                MySqlCommand getFactsSQL = new MySqlCommand(sqlcode, Connection);
                MySqlDataAdapter factAdapter = new MySqlDataAdapter(getFactsSQL);
                factAdapter.Fill(FactDataTable);

                string currentFact_id = "";
                string currentCase_id = "";
                Fact currentFact = null;
                Case currentCase = null;

                // iterate over the FactDataTable: object with the same fact_id will be combined into one fact-object.
                foreach (DataRow row in FactDataTable.Rows)
                {
                    // if we receive a new fact
                    if (currentFact_id != row["fact_id"].ToString())
                    {
                        // save the current one
                        if (currentFact != null)
                            ListOfFacts.Add(currentFact);
                        // and create a new one
                        Fact temp_fact = new Fact(row["fact_id"].ToString(), null);


                        currentFact = temp_fact;
                        currentFact_id = row["fact_id"].ToString();
                    }
                    // now if we receive a new case
                    if (currentCase_id != row["case_id"].ToString())
                    {
                        // save the current one
                        //if (currentCase != null)
                        //    currentFact.addCase(currentCase);
                        // and create a new one
                        currentCase = new Case(row["case_id"].ToString());
                        currentCase_id = row["case_id"].ToString();
                        currentFact.addCase(currentCase);
                    }
                    // create an event and add it to the case
                    Event currentEvent = new Event(row["PROC_DESCRIPTION"].ToString());
                    currentEvent.Information.Add("Event ID", row["event_id"].ToString());
                    currentEvent.Information.Add("Case ID", row["case_id"].ToString());
                    currentEvent.Information.Add("Type", row["event"].ToString());
                    currentEvent.Information.Add("Sequence Number", row["PROC_SEQUENCE_NUM"].ToString());
                    currentEvent.Information.Add("Timestamp", row["timestamp"].ToString()); 
                    
                    currentCase.addEvent(currentEvent);
                }
                // since the last case and fact are not saved yet, we need to do this now: TO-DO nochmal anschauen opb das jetzt passt
                //if (currentCase != null)
                //    currentFact.addCase(currentCase);
                if (currentFact_id != null)
                    ListOfFacts.Add(currentFact);

                return ListOfFacts;
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// Returns the columns in a given Table.
        /// </summary>
        /// <param Name="Table">The Name of the Table.</param>
        /// <returns>A List of strings.</returns>
        /// <author>Jannik Arndt</author>
        public FactTable GetMetaData(string factTableName)
        {
            FactTable table = new FactTable(factTableName, GetColumnNamesOfTable("CASE"), GetColumnNamesOfTable("EVENT"));
            table.AddDimension(new Dimension("--no selection--", isEmptyDimension: true));
            table.AddDimensions(GetDimensionsOf(factTableName));
            return table;
        }

        /// <summary>
        /// This takes a tablename (most likely "FACT") and loads all foreign keys, which should point to a dimension (Table + column).
        /// It creates a List of Dimensions and adds a dimension for each row the Database returns. It then goes on recursively 
        /// and grabs the Table + Column of the "next" Table, meaning the one you end up at, if you roll up the cube.
        /// Note that in this piece of code a dimension may have MULTIPLE sub-ListOfDimensions, which should not exist in the model, 
        /// but IF they do, it's not a problem at this point (but rather a GUI-problem, harhar!)
        /// </summary>
        /// <param Name="tablename">The Name of the Table whose ListOfDimensions are to be found.</param>
        /// <returns>A list of ListOfDimensions which, most likely, contain even more ListOfDimensions themselves.</returns>
        /// <author>Jannik Arndt</author>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:SQL-Abfragen auf Sicherheitsrisiken überprüfen")]
        public List<Dimension> GetDimensionsOf(string tablename)
        {
            // the tempTable is used to store the Database-results
            DataTable tempTable = new DataTable();
            DataTable tempTable2 = new DataTable();
            // this list will be filled and returned
            List<Dimension> dimensions = new List<Dimension>();

            try
            {
                Open();
                // see http://swp.offis.uni-oldenburg.de:8082/display/MPMDoku/Metadaten-Repository for sql-explanation
                MySqlCommand getMetaDataSQL = new MySqlCommand(
                    "SELECT constraint_name as FromConstraint, table_name as FromTable, column_name as FromColumn, constraint_name as ToConstraint, " +
                    "referenced_table_name as ToTable, referenced_column_name as ToColumn " +
                    "FROM information_schema.key_column_usage " +
                    "WHERE constraint_schema = '" + DBWorker.getParams().Database + "' AND constraint_name != 'PRIMARY' AND table_name = '" + tablename + "' AND referenced_table_name != 'CASE';", Connection);

                MySqlDataAdapter metaDataAdapter = new MySqlDataAdapter(getMetaDataSQL);
                // Fill the temporally used Table
                metaDataAdapter.Fill(tempTable);

                // every Database-row is a dimension
                foreach (DataRow row in tempTable.Rows)
                {
                    // create a dimension-object (see MetaWorker.Dimension)
                    Dimension d = new Dimension(row["FromColumn"].ToString(), row["FromConstraint"].ToString(),
                        row["FromTable"].ToString(), row["FromColumn"].ToString(),
                        row["ToConstraint"].ToString(), row["ToTable"].ToString(), row["ToColumn"].ToString());

                    // Now get the Data than can be filtered later:
                    MySqlCommand getContentSQL = new MySqlCommand("SELECT * FROM `" + row["ToTable"].ToString() + "` LIMIT 100", Connection);

                    /*
                     * This SQL-Command seems pretty unsafe, why so? Well, Oracle lets you use parameters for the WHERE-part of the query, 
                     * however you can't do the same thing for the FROM-part. God knows why.
                     */

                    MySqlDataAdapter contentAdapter = new MySqlDataAdapter(getContentSQL);
                    // Fill the temporally used Table
                    contentAdapter.Fill(tempTable2);
                    // create DimensionContent-Objects and add them to the current dimension
                    foreach (DataRow row2 in tempTable2.Rows)
                    {
                        string desc = "";
                        if (row2.ItemArray.Count() > 2)
                            desc = row2[2].ToString();
                        d.DimensionContentsList.Add(new DimensionContent(row2[0].ToString(), row2[1].ToString(), desc));
                    }

                    // save the DimensionColumnNames for generated DB-querys
                    d.DimensionColumnNames = new DimensionColumnNames(tempTable2.Columns[0].ColumnName,
                        tempTable2.Columns[1].ColumnName, tempTable2.Columns[2].ColumnName);

                    tempTable2.Reset();
                    // now recursively find all sub-ListOfDimensions of this Table and add them to the current dimension
                    d.AddDimensionLevel(GetDimensionsOf(row["ToTable"].ToString()));

                    // add the current dimension to the list that will be returned eventually
                    dimensions.Add(d);
                }

                return dimensions;

            }
            finally
            {
                Close();
            }
        }
    }
}
