using pgmpm.Database;
using pgmpm.Database.Model;
using pgmpm.MatrixSelection.Dimensions;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// This static class provides helper-methods for editing xml-files.
    /// </summary>
    /// <author>Bernhard Bruns, Moritz Eversmann</author>
    public static class XMLHelper
    {
        public static string Path;
        public static MetaDataRepository XmlMetadata;

        /// <summary>
        /// Serialize an object to xml
        /// </summary>
        /// <author>Bernhard Bruns, Moritz Eversmann</author>
        public static void SerializeObjectToXML(String filepath, object objectToSerialize)
        {
            TextWriter writer = null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(objectToSerialize.GetType());
                writer = new StreamWriter(filepath);

                serializer.Serialize(writer, objectToSerialize);
            }
            catch (SEHException ex)
            {
                ErrorHandling.ReportErrorToUser("Error while serialize object to xml: " + ex.Message + "\n" + (ex as SEHException).StackTrace);
            }
            catch (UnauthorizedAccessException ex)
            {
                ErrorHandling.ReportErrorToUser("Error while serialize object to xml: Unauthorized Access: " + ex.Message + "\n" + (ex as UnauthorizedAccessException).StackTrace);
            }
            catch (NotSupportedException ex)
            {
                ErrorHandling.ReportErrorToUser("Error: " + ex.Message + "\nFilename: " + filepath + "\n" + ex.StackTrace);
            }
            catch (Exception ex)
            {
                if (ex is ArgumentNullException || ex is IOException || ex is NullReferenceException || ex is ArgumentOutOfRangeException)
                    ErrorHandling.ReportErrorToUser("Error: " + ex.Message);
                else
                    throw;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }


        /// <summary>
        /// Deserialize a xml-file 
        /// </summary>
        /// <author>Bernhard Bruns, Moritz Eversmann</author>
        public static T DeserializeObjectFromXML<T>(String filepath)
        {
            TextReader reader = null;
            T objectDeserialized = default(T);

            try
            {
                reader = new StreamReader(filepath);

                XmlSerializer deserializer = new XmlSerializer(typeof(T));
                objectDeserialized = (T)deserializer.Deserialize(reader);
            }
            catch (OutOfMemoryException ex)
            {
                ErrorHandling.ReportErrorToUser("Error loading xml: The file you are trying to load seems to be damaged. " + ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                ErrorHandling.ReportErrorToUser("Error loading xml: The file " + (ex as FileNotFoundException).FileName + " cannot be found. You need to generate and save results first!");
            }
            catch (InvalidOperationException ex)
            {
                ErrorHandling.ReportErrorToUser("Error loading xml: The xml-file is invalid. It will automatically be deleted. " + ex.Message);
                reader.Close();
                File.Delete(filepath);
            }
            catch (NotSupportedException ex)
            {
                ErrorHandling.ReportErrorToUser("Error: " + ex.Message + "\nFilepath: " + filepath + "\n" + ex.StackTrace);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException || ex is SerializationException)
                {
                    ErrorHandling.ReportErrorToUser("Error: The file you are loading was saved with an old version of this software and can't be read.");
                }
                else if (ex is IOException || ex is NullReferenceException || ex is ArgumentOutOfRangeException)
                {
                    ErrorHandling.ReportErrorToUser("Error loading xml: " + ex.Message);
                }
                else
                    ErrorHandling.ReportErrorToUser("Error: " + ex.Message);
            }

            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return objectDeserialized;
        }

        //public static bool DeleteMetaDataXML(ConnectionParameters conParams)
        //{
        //    String DBDatabaseWithoutPathName = conParams.Database.Substring(conParams.Database.LastIndexOf(("\\")) + 1);

        //    Path = AppDomain.CurrentDomain.BaseDirectory + @"\Metadata_" + DBDatabaseWithoutPathName + "@" + conParams.Host + ".xml";

        //    if (File.Exists(Path))
        //    {
        //        try
        //        {
        //            File.Delete(Path);
        //        }
        //        catch { 
        //            return false; 
        //        }
        //    }
        //    return true;
        //}

        /// <summary>
       

        /// <summary>
        /// Writes the content from the xml-file to the FactTable if the xml exists
        /// </summary>
        /// <author>Bernhard Bruns, Moritz Eversmann</author>
        public static bool SynchronizeFactTableWithXML(ConnectionParameters conParams)
        {
            try
            {
                //Databasename without Path and without backslashes. The SQLite-Databasename is the directory Path to the database.
                String dbDatabaseWithoutPathName = conParams.Database.Substring(conParams.Database.LastIndexOf(("\\")) + 1);

                Path = AppDomain.CurrentDomain.BaseDirectory + @"\Metadata_" + dbDatabaseWithoutPathName + "@" + conParams.Host + ".xml";

                if (File.Exists(Path))
                {
                    XmlMetadata = DeserializeObjectFromXML<MetaDataRepository>(Path);

                    if (XmlMetadata != null)
                    {
                        if (CompareXMLWithMetadata(DBWorker.MetaData, XmlMetadata))
                        {
                            DBWorker.MetaData.EventClassifier = XmlMetadata.EventClassifier;

                            for (int i = 0; i < DBWorker.MetaData.ListOfFactDimensions.Count; i++)
                                GetMetadataFromXMLAndWriteInFacttable(DBWorker.MetaData.ListOfFactDimensions[i], XmlMetadata.ListOfFactDimensions[i], true);

                            return true;
                        }

                    }
                }
            }
            catch (NullReferenceException ex)
            {
                ErrorHandling.ReportErrorToUser("Error synchronize facttable: " + ex.Message);
            }
            catch (Exception ex)
            {
                ErrorHandling.ReportErrorToUser("Error synchronize facttable: " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Writes the dimension- and levelname from the xml-file to the facttable
        /// </summary>
        /// <param name="originalDimension">Dimension in facttable</param>
        /// <param name="xmlDimension">Dimension in the xml-file</param>
        /// <param name="xmlDimension">true or false</param>
        /// <author>Bernhard Bruns</author>
        private static void GetMetadataFromXMLAndWriteInFacttable(Dimension originalDimension, Dimension xmlDimension, bool firstInvoke)
        {
            //The name of the dimension is only in the first dimension-object
            if (firstInvoke && originalDimension.IsEmptyDimension == false)
                originalDimension.Dimensionname = xmlDimension.Dimensionname;

            originalDimension.Level = xmlDimension.Level;

            if (originalDimension.DimensionLevelsList.Count > 0)
                GetMetadataFromXMLAndWriteInFacttable(originalDimension.DimensionLevelsList[0], xmlDimension.DimensionLevelsList[0], false);
        }

        /// <summary>
        /// Compares the structure of the facttable from the DatabaseConnection and the facttable from the xml
        /// If the structure is the same, the function returns true
        /// </summary>
        /// <author>Bernhard Bruns, Moritz Eversmann</author>
        private static bool CompareXMLWithMetadata(MetaDataRepository ftMetaworker, MetaDataRepository ftXml)
        {
            if (ftMetaworker.ListOfEventsTableColumnNames.Contains(ftXml.EventClassifier) || (ftMetaworker.EventClassifier == ""))
                if (ftMetaworker.ListOfFactDimensions.Count == ftXml.ListOfFactDimensions.Count)
                {
                    for (int i = 0; i < ftMetaworker.ListOfFactDimensions.Count; i++)
                        if (CompareDimensions(ftMetaworker.ListOfFactDimensions[i], ftXml.ListOfFactDimensions[i]) == false)
                            return false;

                    return true;
                }
                else
                    return false;
            return false;
        }

        /// <summary>
        /// Recursive function which compares the dimensions count of the facttable
        /// </summary>
        /// <param name="originalDimension"></param>
        /// <param name="xmlDimension"></param>
        /// <returns></returns>
        /// <author>Bernhard Bruns</author>
        private static bool CompareDimensions(Dimension originalDimension, Dimension xmlDimension)
        {
            if (originalDimension.DimensionLevelsList.Count > 0 && xmlDimension.DimensionLevelsList.Count > 0)
            {
                if (CompareDimensions(originalDimension.DimensionLevelsList[0], xmlDimension.DimensionLevelsList[0]) == false)
                    return false;
                return true;
            }
            if (originalDimension.DimensionLevelsList.Count == 0 && xmlDimension.DimensionLevelsList.Count == 0)
                return true;

            return false;
        }

        ///<summary>
        /// Resets the Metadata to the original names.
        /// </summary>
        /// <author>Bernhard Bruns</author>
        public static void ResetMetadata()
        {
            foreach (Dimension dimension in DBWorker.MetaData.ListOfFactDimensions)
            {
                dimension.Dimensionname = dimension.Name;
                dimension.Level = dimension.ToTable;

                foreach (Dimension cDimension in dimension.GetLevel())
                {
                    cDimension.Dimensionname = cDimension.Name;
                    cDimension.Level = cDimension.ToTable;
                }
            }
        }
    }
}
