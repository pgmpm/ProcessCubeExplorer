using pgmpm.Database;
using pgmpm.Database.Exceptions;
using pgmpm.Database.Model;
using pgmpm.MatrixSelection;
using pgmpm.MatrixSelection.Fields;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Windows.Input;

namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// This static class provides helper-methods to get and set easily databaseconnections information in a xml-file
    /// </summary>
    /// <author>Bernhard Bruns, Moritz Eversmann</author>
    public static class DBConnectionHelpers
    {
        public static List<ConnectionParameters> ConnectionParametersList { get; set; }
        public static ConnectionParameters CurrentConnectionParameters { get; set; }
        public static Boolean DefaultEventClassifierIsSelected;

        /// <summary>
        /// Configures and opens a database connection
        /// Builds the metadata repository and loads the xml-serialized metadata-file.
        /// Navigates to page P2Metadata
        /// </summary>
        /// <param name="conParams"></param>
        /// <returns></returns>
        /// <author>Bernhard Bruns, Moritz Eversmann, Bernd Nottbeck</author>
        public static bool EstablishDatabaseConnection(ConnectionParameters conParams)
        {
            try
            {
                if (DBWorker.ConfigureDBConnection(conParams))
                {
                    DBWorker.OpenConnection();

                    MainWindow.ConnectionName = DBWorker.GetConnectionName();

                    DBWorker.BuildMetadataRepository();

                    XMLHelper.SynchronizeFactTableWithXML(conParams);

                    DefaultEventClassifierIsSelected = false;

                    if (DBWorker.MetaData.EventClassifier == "")
                    {
                        DefaultEventClassifierIsSelected = true;
                        if (DBWorker.MetaData.ListOfEventsTableColumnNames.Contains("PROC_DESCRIPTION"))
                            DBWorker.MetaData.EventClassifier = "PROC_DESCRIPTION";
                        else if (DBWorker.MetaData.ListOfEventsTableColumnNames.Contains("activity"))
                            DBWorker.MetaData.EventClassifier = "activity";
                        else if (DBWorker.MetaData.ListOfEventsTableColumnNames.Contains("ACTIVITY"))
                            DBWorker.MetaData.EventClassifier = "ACTIVITY";
                        else DefaultEventClassifierIsSelected = false;
                    }
                    NavigationCommands.GoToPage.Execute("/Pages/P2metadata.xaml", null);
                }

                return true;
            }
            catch (TypeInitializationException ex)
            {
                ErrorHandling.ReportErrorToUser("Error: Type initialization. " + ex.Message);
            }
            catch (NoParamsGivenException)
            {
                ErrorHandling.ReportErrorToUser("Error: No databasefields are filled.");
            }
            catch (DBException ex)
            {
                ErrorHandling.ReportErrorToUser("Database error: " + ex.Message);
            }
            catch (TimeoutException ex)
            {
                ErrorHandling.ReportErrorToUser("Database Timeout: " + ex.Message);
            }
            catch (Exception ex)
            {
                ErrorHandling.ReportErrorToUser("Error: " + ex.Message + " " + ex.StackTrace);
            }
            return false;
        }

        /// <summary>
        /// Deserialize the database settings from a file (DatabaseSettings.mpm)
        /// </summary>
        /// <author>Bernhard Bruns, Moritz Eversmann</author>
        public static bool LoadConnectionParameters()
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\DatabaseSettings.mpm"))
            {
                try
                {
                    ConnectionParametersList = Serializer.Deserialize<List<ConnectionParameters>>("DatabaseSettings.mpm");
                    return true;
                }

                catch (OutOfMemoryException ex)
                {
                    ErrorHandling.ReportErrorToUser("Error loading database settings: The file you are trying to load seems to be damaged. " + ex.Message);
                }
                catch (FileNotFoundException ex)
                {
                    ErrorHandling.ReportErrorToUser("Error loading database settings: The file " + ex.FileName + " cannot be found.");
                }
                catch (SerializationException)
                {
                    ErrorHandling.ReportErrorToUser("Error: The file you are loading was saved with an old version of this software and can't be read. It will automatically be deleted.");
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\DatabaseSettings.mpm");
                }
                catch (Exception ex)
                {
                    if (ex is IOException || ex is NullReferenceException || ex is ArgumentOutOfRangeException)
                        ErrorHandling.ReportErrorToUser("Error loading database settings: " + ex.Message);
                    else
                        throw;
                    return false;
                }
            }
            else
                ConnectionParametersList = new List<ConnectionParameters>();
            return false;
        }

        /// <summary>
        /// Serialize the database settings from a file (DatabaseSettings.mpm)
        /// </summary>
        /// <author>Bernhard Bruns, Moritz Eversmann</author>
        public static void SaveConnectionParameters()
        {
            try
            {
                Serializer.Serialize("DatabaseSettings.mpm", ConnectionParametersList);
            }
            catch (SEHException ex)
            {
                ErrorHandling.ReportErrorToUser("Error while saving: " + ex.Message + "\n" + ex.StackTrace);
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorHandling.ReportErrorToUser("Error while saving: Unauthorized Access: " + ex.Message + "\n" + ex.StackTrace);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is IOException || ex is NullReferenceException || ex is ArgumentOutOfRangeException)
                    ErrorHandling.ReportErrorToUser("Error: " + ex.Message);
                else
                    throw;
            }
        }

        /// <summary>
        /// Add a database connection to the connectionlist
        /// </summary>
        /// <author>Bernhard Bruns</author>
        public static void AddDatabaseConnectionToConnectionList(ConnectionParameters conParams)
        {
            ConnectionParametersList.Add(conParams);
        }

        /// <summary>
        /// Checks if the DatabaseConnection connectionname already exists
        /// </summary>
        /// <param name="connectionName"></param>
        /// <returns>true, if the connectionname not already exists</returns>
        /// <author>Bernhard Bruns</author>
        public static bool CheckIfDatabaseNameExists(String connectionName)
        {
            return ConnectionParametersList.All(conParams => conParams.Name != connectionName);
        }

        /// <summary>
        /// Removes a connectionparameter from the connectionparameterlist 
        /// </summary>
        /// <param name="name">name from the connectionparameter</param>
        /// <author>Bernhard Bruns</author>
        public static void RemoveConnectionParameter(String name)
        {
            foreach (ConnectionParameters conParams in ConnectionParametersList)
            {
                if (conParams.Name != name) continue;
                ConnectionParametersList.Remove(conParams);
                break;
            }
        }

        /// <summary>
        /// Saves the last used databaseconnection
        /// </summary>
        /// <param name="conParams"></param>
        /// <author>Bernhard Bruns</author>
        public static void SaveLastUsedDatabase(ConnectionParameters conParams)
        {
            foreach (ConnectionParameters connectionParameters in ConnectionParametersList)
            {
                connectionParameters.IsLastUsedDatabase = conParams.Name == connectionParameters.Name;
            }
            SaveConnectionParameters();
        }


        /// <summary>
        /// Returns the last used connectionparameter
        /// </summary>
        /// <returns>ConnectionParameter</returns>
        /// <author>Bernhard Bruns</author>
        public static ConnectionParameters LoadLastUsedDatabase()
        {
            return ConnectionParametersList.FirstOrDefault(connectionParameters => connectionParameters.IsLastUsedDatabase);
        }

        /// <summary>
        /// Load a list of facts and eventlogs in the fields.
        /// </summary>
        /// <param name="field"></param>
        /// <author>Bernhard Bruns,Thomas Meents, Moritz Eversmann</author>
        public static bool LoadFactsInField(Field field)
        {
            try
            {
                List<List<Case>> listOfFacts = DBWorker.GetFacts(MainWindow.MatrixSelection.SelectedDimensions, EventSelectionModel.GetInstance().SelectedDimensions, field);

                foreach (List<Case> fact in listOfFacts)
                    if (fact != null)
                        field.EventLog.Cases.AddRange(fact);

                return true;
            }
            catch (TimeoutException ex)
            {
                ErrorHandling.ReportErrorToUser("Error: Database Timeout. " + ex.Message);
            }
            catch (Exception ex)
            {
                if (ex is NoParamsGivenException || ex is NotImplementedException || ex is ArgumentException)
                    ErrorHandling.ReportErrorToUser("Error: " + ex.Message);
                else
                    throw;
            }

            return false;
        }
    }
}