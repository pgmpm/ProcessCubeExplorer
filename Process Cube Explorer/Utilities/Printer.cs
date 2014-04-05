using pgmpm.MainV2.Properties;
using System;
using System.Printing;
using System.Windows.Controls;

namespace pgmpm.MainV2.Utilities
{
    public static class Printer
    {
        /// <summary>
        /// Method for print visualization
        /// </summary>
        /// <param name="processModelCanvas">Canvas</param>
        /// <author>Thomas Meents, Moritz Eversmann, Bernhard Bruns</author>  
        public static void PrintCanvas(Canvas processModelCanvas)
        {
            PrintDialog printDialog = new PrintDialog { PrintTicket = { PageOrientation = PageOrientation.Landscape } };

            double factorHeight, factorWidth, temp, scale;
            factorHeight = processModelCanvas.Height / printDialog.PrintableAreaHeight;
            factorWidth = processModelCanvas.Width / printDialog.PrintableAreaWidth;

            if (factorWidth > factorHeight)
                temp = factorWidth;
            else
                temp = factorHeight;

            scale = 100 / temp;

            printDialog.PrintTicket.PageScalingFactor = Convert.ToInt32(scale);

            if (printDialog.ShowDialog() == true)
                printDialog.PrintVisual(processModelCanvas,
                    Exporter.FilenameTagReplace(Settings.Default.ImageName));
        }
    }
}

