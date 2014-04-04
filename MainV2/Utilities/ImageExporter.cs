using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// Image exporter for petrinet's
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    class ImageExporter : IExporter
    {
        private string filetype;
        private Canvas processModelCanvas;

        public ImageExporter(string filetype, Canvas processModelCanvas)
        {
            this.filetype = filetype;
            this.processModelCanvas = processModelCanvas;
        }

        /// <summary>
        /// Image export method
        /// Encodes a RenderTargetBitmap as a graphic-file (bmp, gif, jpeg, png or tiff) and saves it with the given filename.
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <returns>bool if the export was successfully</returns>
        /// <author>Thomas Meents, Bernhard Bruns, Andrej Albrecht</author>
        public bool export(String filename)
        {
            try
            {
                processModelCanvas.Background = Brushes.White;
                RenderTargetBitmap render = new RenderTargetBitmap((int)processModelCanvas.Width, (int)processModelCanvas.Height, 96d, 96d, PixelFormats.Pbgra32);
                render.Clone();
                processModelCanvas.Measure(new Size((int)processModelCanvas.Width, (int)processModelCanvas.Height));
                processModelCanvas.Arrange(new Rect(new Size((int)processModelCanvas.Width, (int)processModelCanvas.Height)));

                render.Render(processModelCanvas);

                switch (this.filetype)
                {
                    case ".bmp":
                        BmpBitmapEncoder bmpEncoder = new BmpBitmapEncoder();
                        bmpEncoder.Frames.Add(BitmapFrame.Create(render));
                        using (FileStream bmpFile = File.Create(filename))
                        {
                            bmpEncoder.Save(bmpFile);
                        }
                        break;
                    case ".gif":
                        GifBitmapEncoder gifEncoder = new GifBitmapEncoder();
                        gifEncoder.Frames.Add(BitmapFrame.Create(render));
                        using (FileStream gifFile = File.Create(filename))
                        {
                            gifEncoder.Save(gifFile);
                        }
                        break;
                    case ".jpeg":
                        JpegBitmapEncoder jpegEncoder = new JpegBitmapEncoder();
                        jpegEncoder.Frames.Add(BitmapFrame.Create(render));
                        using (FileStream jpegFile = File.Create(filename))
                        {
                            jpegEncoder.Save(jpegFile);
                        }
                        break;
                    case ".png":
                        PngBitmapEncoder pngEncoder = new PngBitmapEncoder();
                        pngEncoder.Frames.Add(BitmapFrame.Create(render));
                        using (FileStream pngFile = File.Create(filename))
                        {
                            pngEncoder.Save(pngFile);
                        }
                        break;
                    case ".tiff":
                        TiffBitmapEncoder tiffEncoder = new TiffBitmapEncoder();
                        tiffEncoder.Frames.Add(BitmapFrame.Create(render));
                        using (FileStream tiffFile = File.Create(filename))
                        {
                            tiffEncoder.Save(tiffFile);
                        }
                        break;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
