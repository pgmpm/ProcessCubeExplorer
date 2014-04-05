using pgmpm.MatrixSelection.Fields;
using System;
using System.IO;

namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// MXML Exporter
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public class MXMLExporter : IExporter
    {
        private EventLog eventLog;

        public MXMLExporter(EventLog eventLog)
        {
            this.eventLog = eventLog;
        }

        /// <summary>
        /// Exports an eventlog to a mxml file
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <returns>bool if the export was successfully</returns>
        /// <autor>Andrej Albrecht</autor>
        public bool export(String filename)
        {
            String result = this.eventLog.ConvertToMXML();

            try
            {
                StreamWriter mxmlFile = new StreamWriter(filename);
                mxmlFile.Write(result);
                mxmlFile.Close();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
