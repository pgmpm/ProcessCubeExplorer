using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Controls;
using Microsoft.Win32;
using pgmpm.MainV2.Properties;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// Export Canvas to a File
    /// </summary>
    /// <author>Moritz Eversmann, Bernhard Bruns</author>
    public static class Exporter
    {
        public static string DefaultFileName;
        public static string DefaultFilePath;
        public static int DefaultFileType;
        public static bool OpenFolderAferExport;


        /// <summary>
        /// Load Properties in Static field
        /// </summary>
        /// <author>Moritz Eversmann, Bernhard Bruns</author>
        private static void GetProperties()
        {
            DefaultFileName = Settings.Default.ImageName;
            DefaultFilePath = Settings.Default.ImagePath;
            DefaultFileType = Settings.Default.ImageFiletype;
            OpenFolderAferExport = Settings.Default.OpenFolderAfterExport;
        }

        /// <summary>
        /// Saves the visualization, petrinet or the eventlog in the required format
        /// </summary>
        /// <param name="processModelCanvas">Canvas with the processmodel</param>
        /// <param name="selectedField">The field</param>
        /// <param name="unitTest">true if a unit test calls this method</param>
        /// <author>Thomas Meents, Bernhard Bruns, Moritz Eversmann, Naby M. Sow, Andrej Albrecht</author>
        public static bool Export(Canvas processModelCanvas, Field selectedField, bool unitTest = false)
        {
            if (!unitTest)
                GetProperties();

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                ValidateNames = true,
                AddExtension = true,
                FileName = FilenameTagReplace(DefaultFileName, selectedField),
                InitialDirectory = DefaultFilePath,
                Filter = ExporterFactory.getFilterString(processModelCanvas, selectedField),
                FilterIndex = DefaultFileType + 1,
                RestoreDirectory = true,
                OverwritePrompt = true
            };

            //// for unit test ////////////////
            if (unitTest)
            {
                string filetype = "";

                switch (DefaultFileType)
                {
                    case 0:
                        filetype = ".bmp";
                        break;
                    case 1:
                        filetype = ".gif";
                        break;
                    case 2:
                        filetype = ".jpeg";
                        break;
                    case 3:
                        filetype = ".png";
                        break;
                    case 4:
                        filetype = ".tiff";
                        break;
                    case 5:
                        filetype = ".dot";
                        break;
                    case 6:
                        filetype = ".pnml";
                        break;
                    case 7:
                        filetype = ".mxml";
                        break;
                }

                IExporter exporter = ExporterFactory.createExporter(filetype, processModelCanvas, selectedField);
                if (exporter.export(DefaultFilePath + DefaultFileName + filetype))
                    return true;
                return false;
            }
            //// /for unit test ////////////////

            if (saveFileDialog.ShowDialog() == true)
            {
                string filetype = Path.GetExtension(saveFileDialog.FileName);
                string filename = saveFileDialog.FileName;
                string filePath = Path.GetDirectoryName(saveFileDialog.FileName);

                IExporter exporter = ExporterFactory.createExporter(filetype, processModelCanvas, selectedField);
                if (exporter.export(filename))
                {
                    if (OpenFolderAferExport)
                        Process.Start("explorer", filePath);

                    return true;
                }
                return false;
            }

            return false;
        }

        /// <summary>
        /// Replaces tags with current values
        /// </summary>
        /// <param name="name">Filename</param>
        /// <param name="field">Selected field</param>
        /// <returns>Renamed filename</returns>
        /// <author>Thomas Meents</author>
        public static string FilenameTagReplace(String name, Field field = null)
        {
            string date = DateTime.Now.ToShortDateString();
            date = date.Replace(".", "-");
            StringBuilder stringBuilder = new StringBuilder(name);
            stringBuilder.Replace("#DATE#", date);
            stringBuilder.Replace("#USER#", Environment.UserName);
            if (field != null)
            {
                stringBuilder.Replace("#NAME#", field.Infotext);
                stringBuilder.Replace("#QUALITY#", field.ProcessModelQualityColor);
            }
            stringBuilder.Replace("/", "-");
            stringBuilder.Replace(":", "");
            stringBuilder.Replace(",", "_");
            return stringBuilder.ToString();
        }
    }
}