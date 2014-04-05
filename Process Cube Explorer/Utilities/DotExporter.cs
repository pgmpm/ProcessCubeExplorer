using pgmpm.Model.PetriNet;
using System;
using System.IO;

namespace pgmpm.MainV2.Utilities
{
    class DotExporter : IExporter
    {
        private Model.ProcessModel processModel;

        public DotExporter(Model.ProcessModel processModel)
        {
            this.processModel = processModel;
        }
        public bool export(String filename)
        {
            String result = ((PetriNet)processModel).ConvertToDot();

            try
            {
                StreamWriter dotFile = new StreamWriter(filename);
                dotFile.Write(result);
                dotFile.Close();

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
