using pgmpm.Model.PetriNet;
using System;
using System.IO;

namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// PNML Exporter
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    class PNMLExporter : IExporter
    {
        private Model.ProcessModel processModel;

        public PNMLExporter(Model.ProcessModel processModel)
        {
            this.processModel = processModel;
        }

        /// <summary>
        /// Exports a processmodel to a pnml file
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <returns>bool if the export was successfully</returns>
        /// <autor>Andrej Albrecht</autor>
        public bool export(String filename)
        {
            String result = ((PetriNet)this.processModel).ConvertToPNML();

            try
            {
                StreamWriter pnmlFile = new StreamWriter(filename);
                pnmlFile.Write(result);
                pnmlFile.Close();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
