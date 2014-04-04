using System;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection;
using pgmpm.MatrixSelection.Fields;
using Path = System.IO.Path;

namespace MainV2Tests.FunctionsTests
{

    public enum ImageType
    {
        BMP = 0,
        GIF = 1,
        JPEG = 2,
        PNG = 3,
        TIFF = 4,
        DOT = 5,
        PNML = 6,
        MXML = 7
    }

    /// <summary>
    ///Dies ist eine Testklasse für "ExporterTest" und soll
    ///alle ExporterTest Komponententests enthalten.
    ///</summary>
    ///<author>Bernhard Bruns</author>
    [TestClass]
    public class ExporterTest
    {
        /// <summary>
        ///Ruft den Testkontext auf, der Informationen
        ///über und Funktionalität für den aktuellen Testlauf bietet, oder legt diesen fest.
        ///</summary>
        public TestContext TestContext { get; set; }


        /// <summary>
        ///Ein Test für "ExportCanvasToImage"
        ///</summary>
        [TestMethod]
        public void ExportCanvasToImageTestBMP()
        {
            Canvas processModelCanvas = CreateCanvas();
            processModelCanvas.Children.Add(CreateEllipse());


            Exporter.DefaultFileType = (int)ImageType.BMP;
            Exporter.DefaultFileName = "TestImage";
            Exporter.DefaultFilePath = Path.GetTempPath();

            Exporter.Export(processModelCanvas, null, true);

            Assert.IsTrue(File.Exists(Path.GetTempPath() + Exporter.DefaultFileName + ".bmp"));
        }

        /// <summary>
        ///Ein Test für "ExportCanvasToImage"
        ///</summary>
        [TestMethod]
        public void ExportCanvasToImageTestGIF()
        {
            Canvas processModelCanvas = CreateCanvas();
            processModelCanvas.Children.Add(CreateEllipse());

            Exporter.DefaultFileType = (int)ImageType.GIF;
            Exporter.DefaultFileName = "TestImage";
            Exporter.DefaultFilePath = Path.GetTempPath();

            Exporter.Export(processModelCanvas, null, true);

            Assert.IsTrue(File.Exists(Path.GetTempPath() + Exporter.DefaultFileName + ".gif"));
        }


        /// <summary>
        ///Ein Test für "ExportCanvasToImage"
        ///</summary>
        [TestMethod]
        public void ExportCanvasToImageTestJPEG()
        {
            Canvas processModelCanvas = CreateCanvas();
            processModelCanvas.Children.Add(CreateEllipse());


            Exporter.DefaultFileType = (int)ImageType.JPEG;
            Exporter.DefaultFileName = "TestImage";
            Exporter.DefaultFilePath = Path.GetTempPath();

            Exporter.Export(processModelCanvas, null, true);
            Assert.IsTrue(File.Exists(Path.GetTempPath() + Exporter.DefaultFileName + ".jpeg"));
        }

        /// <summary>
        ///Ein Test für "ExportCanvasToImage"
        ///</summary>
        [TestMethod]
        public void ExportCanvasToImageTestPNG()
        {
            Canvas processModelCanvas = CreateCanvas();
            processModelCanvas.Children.Add(CreateEllipse());

            Exporter.DefaultFileType = (int)ImageType.PNG;
            Exporter.DefaultFileName = "TestImage";
            Exporter.DefaultFilePath = Path.GetTempPath();

            Exporter.Export(processModelCanvas, null, true);

            Assert.IsTrue(File.Exists(Path.GetTempPath() + Exporter.DefaultFileName + ".png"));
        }

        /// <summary>
        ///Ein Test für "ExportCanvasToImage"
        ///</summary>
        [TestMethod]
        public void ExportCanvasToImageTestTIFF()
        {
            Canvas processModelCanvas = CreateCanvas();
            processModelCanvas.Children.Add(CreateEllipse());

            Exporter.DefaultFileType = (int)ImageType.TIFF;
            Exporter.DefaultFileName = "TestImage";
            Exporter.DefaultFilePath = Path.GetTempPath();

            Exporter.Export(processModelCanvas, null, true);

            Assert.IsTrue(File.Exists(Path.GetTempPath() + Exporter.DefaultFileName + ".tiff"));
        }


        /// <summary>
        ///Test for the dot-exporter
        ///</summary>
        [TestMethod]
        public void ExportCanvasToDOT()
        {
            Canvas processModelCanvas = CreateCanvas();
            processModelCanvas.Children.Add(CreateEllipse());

            Exporter.DefaultFileType = (int)ImageType.DOT;
            Exporter.DefaultFileName = "TestImage";
            Exporter.DefaultFilePath = Path.GetTempPath();

            Exporter.Export(processModelCanvas, GetField(), true);

            Assert.IsTrue(File.Exists(Path.GetTempPath() + Exporter.DefaultFileName + ".dot"));
        }


        /// <summary>
        ///Test for the pnml exporter
        ///</summary>
        [TestMethod]
        public void ExportCanvasToPNML()
        {
            Canvas processModelCanvas = CreateCanvas();
            processModelCanvas.Children.Add(CreateEllipse());

            Exporter.DefaultFileType = (int)ImageType.PNML;
            Exporter.DefaultFileName = "TestImage";
            Exporter.DefaultFilePath = Path.GetTempPath();

            Exporter.Export(processModelCanvas, GetField(), true);

            Assert.IsTrue(File.Exists(Path.GetTempPath() + Exporter.DefaultFileName + ".pnml"));
        }


        /// <summary>
        ///Test for the mxml-exporter
        ///</summary>
        [TestMethod]
        public void ExportCanvasToMXML()
        {
            Canvas processModelCanvas = CreateCanvas();
            processModelCanvas.Children.Add(CreateEllipse());

            Exporter.DefaultFileType = (int)ImageType.MXML;
            Exporter.DefaultFileName = "TestImage";
            Exporter.DefaultFilePath = Path.GetTempPath();

            Exporter.Export(processModelCanvas, GetField(), true);

            Assert.IsTrue(File.Exists(Path.GetTempPath() + Exporter.DefaultFileName + ".mxml"));
        }


        private Field GetField()
        {
            MatrixSelectionModel model = Serializer.Deserialize<MatrixSelectionModel>(@"Files\MatrixSelection.mpm");
            return model.MatrixFields[0];
        }


        private Canvas CreateCanvas()
        {
            Canvas processModelCanvas = new Canvas { Height = 300, Width = 300 };

            return processModelCanvas;
        }

        private Ellipse CreateEllipse()
        {
            Ellipse el = new Ellipse { Width = 10, Height = 10, Fill = new SolidColorBrush(Colors.Red) };

            return el;
        }

        /// <summary>
        ///Ein Test für "filenameconverter"
        ///</summary>
        [TestMethod]
        public void FilenameconverterTest()
        {
            const string name = "#DATE#_#USER#_Testname";

            string date = DateTime.Now.ToShortDateString();
            date = date.Replace(".", "-");
            StringBuilder stringBuilder = new StringBuilder(name);
            stringBuilder.Replace("#DATE#", date);
            stringBuilder.Replace("#USER#", Environment.UserName);
            stringBuilder.Replace("/", "-");
            stringBuilder.Replace(":", "");
            stringBuilder.Replace(",", "_");

            string expected = stringBuilder.ToString();
            string actual = Exporter.FilenameTagReplace(name);

            Assert.AreEqual(expected, actual);
        }
    }
}
