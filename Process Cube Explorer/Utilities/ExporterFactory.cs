using System;
using System.Collections.Generic;
using System.Text;

namespace pgmpm.MainV2.Utilities
{
    public static class ExporterFactory
    {
        private readonly static Dictionary<String, String> _ListOfExportFormats = new Dictionary<String, String>()
        {
            { "Petri Net Markup Language (PNML)", ".pnml" },
            { "Mining eXtensible Markup Language (MXML)", ".mxml" },
            { "DOT (GraphViz)", ".dot" },
            { "PNG Image", ".png" },
            { "JPEG Image", ".jpeg" },
            { "Bitmap Image", ".bmp" },
            { "GIF Image", ".gif" },
            { "TIFF Image", ".tiff" }
        };

        public static Dictionary<String, String> ListOfExportFormats
        {
            get { return _ListOfExportFormats; }
        }

        /// <summary>
        /// Creates a string for the filter in a save file dialog
        /// </summary>
        /// <returns></returns>
        /// <autor>Andrej Albrecht</autor>
        public static String getFilterString()
        {
            return "PNG Image|*.png|JPEG Image|*.jpeg|Bitmap Image|*.bmp|GIF Image|*.gif|TIFF Image|*.tiff|DOT (GraphViz)|*.dot|Petri Net Markup Language (PNML)|*.pnml|Mining eXtensible Markup Language (MXML)|*.mxml";
        }

        internal static string getFilterString(System.Windows.Controls.Canvas processModelCanvas, MatrixSelection.Fields.Field selectedField)
        {
            StringBuilder filterString = new StringBuilder("");
            
            //PNG, JPEG, BMP, GIF, TIFF
            if (processModelCanvas!=null)
            {
                filterString.Append("PNG Image|*.png|JPEG Image|*.jpeg|Bitmap Image|*.bmp|GIF Image|*.gif|TIFF Image|*.tiff");
            }

            //DOT, PNML
            if (selectedField != null && selectedField.ProcessModel != null)
            {
                if (filterString.Length > 0) filterString.Append("|");
                filterString.Append("DOT (GraphViz)|*.dot|Petri Net Markup Language (PNML)|*.pnml");
            }

            //MXML
            if (selectedField != null && selectedField.EventLog != null)
            {
                if (filterString.Length > 0) filterString.Append("|");
                filterString.Append("Mining eXtensible Markup Language (MXML)|*.mxml");
            }

            return filterString.ToString();
        }

        public static IExporter createExporter(String filetype, System.Windows.Controls.Canvas canvas, MatrixSelection.Fields.Field selectedField)
        {
            switch (filetype)
            {
                case ".bmp":
                case ".png":
                case ".jpeg":
                case ".gif":
                case ".tiff":
                    return new ImageExporter(filetype, canvas);
                case ".mxml":
                    return new MXMLExporter(selectedField.EventLog);
                case ".pnml":
                    return new PNMLExporter(selectedField.ProcessModel);
                case ".dot":
                    return new DotExporter(selectedField.ProcessModel);
            }

            throw new NotImplementedException("No exporter available for the filetype: "+filetype);
        }

        
    }
}
