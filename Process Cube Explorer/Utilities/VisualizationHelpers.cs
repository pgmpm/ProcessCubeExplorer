using pgmpm.MatrixSelection.Fields;
using pgmpm.Model.PetriNet;
using pgmpm.Visualization.PetriNetVisualization;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// This static class provides helper-methods for the visualization.
    /// </summary>
    /// <author>Bernhard Bruns</author>
    public static class VisualizationHelpers
    {
        public enum Direction : int
        {
            Left = 1,
            Right = 2,
            Up = 3,
            Down = 4
        }

        const double Zoomfactor = 1.25;

        /// <summary>
        /// Zoom in and out the process model.
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="scrollviewer"></param>
        /// <param name="zoomdirection">"in" or "out"</param>
        /// <param name="e"></param>
        /// <param name="sender"></param>
        /// <author>Thomas Meents, based on Jannik Arndt's implementation</author>
        public static void ZoomFunction(Canvas canvas, ScrollViewer scrollviewer, string zoomdirection, MouseWheelEventArgs e = null, object sender = null)
        {
            if (sender != null || e != null)
            {
                Point currentMousePosition = Mouse.GetPosition(scrollviewer);
                (canvas.LayoutTransform as ScaleTransform).CenterX = currentMousePosition.X;
                (canvas.LayoutTransform as ScaleTransform).CenterY = currentMousePosition.Y;
                (canvas.LayoutTransform as ScaleTransform).ScaleX *= 1 + (e.Delta / 1000.0);
                (canvas.LayoutTransform as ScaleTransform).ScaleY *= 1 + (e.Delta / 1000.0);
                e.Handled = true;
            }
            else
            {
                switch (zoomdirection)
                {
                    case "in":
                        (canvas.LayoutTransform as ScaleTransform).ScaleX *= Zoomfactor;
                        (canvas.LayoutTransform as ScaleTransform).ScaleY *= Zoomfactor;
                        break;
                    case "out":
                        (canvas.LayoutTransform as ScaleTransform).ScaleX /= Zoomfactor;
                        (canvas.LayoutTransform as ScaleTransform).ScaleY /= Zoomfactor;
                        break;
                    default:
                        return;
                }
            }
        }



        /// <summary>
        /// This method scrolls the canvas in one zoom direction
        /// </summary>
        /// <author>Thomas Meents, Bernhard Bruns</author>
        public static void ScrollToDirection(Direction direction, Canvas canvas)
        {
            for (int i = 0; i < canvas.Children.Count; ++i)
            {
                if (canvas.Children[i] is Canvas)
                {
                    ScrollViewer sv = canvas.Children[i] as ScrollViewer;
                    if (direction == Direction.Left)
                        sv.LineLeft();
                    else if (direction == Direction.Right)
                        sv.LineRight();
                    else if (direction == Direction.Up)
                        sv.LineUp();
                    else if (direction == Direction.Down)
                        sv.LineDown();
                }
            }
        }

        /// <summary>
        /// Returns the Visualization of the field, creates on if it does not already exist.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="canvas"></param>
        /// <param name="dragAndDrop"></param>
        /// <param name="forceRedraw"></param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public static Canvas GetOrCreatePetriNetVisualization(Field field, Canvas canvas, bool dragAndDrop = true, bool forceRedraw = false)
        {
            PetriNetVisualizer petriNetVisualizer = new PetriNetVisualizer();
            try
            {
                return petriNetVisualizer.DrawPetriNet((PetriNet)field.ProcessModel, canvas, dragAndDrop, forceRedraw);
            }
            catch (KeyNotFoundException)
            {
                ErrorHandling.ReportErrorToUser("This process model cannot be viewed, probably because a loop was not properly marked.");
            }
            return null;
        }
    }
}