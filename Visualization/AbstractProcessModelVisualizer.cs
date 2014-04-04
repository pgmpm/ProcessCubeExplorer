using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using pgmpm.Visualization.GUIElements;
using pgmpm.Visualization.Properties;

namespace pgmpm.Visualization
{
    /// <summary>
    ///     This is the superclass for all process model visualizer. Each process model visualizer have to extend this class!
    ///     This class contains all fields and methods, which are independent of a concrete visualizer.
    /// </summary>
    /// <author>Roman Bauer</author>
    public abstract class AbstractProcessModelVisualizer
    {
        /// <summary>
        ///     Creates an arrow (line and head) and connects two Thumbs in the given canvas
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="startElement"></param>
        /// <param name="destinationElement"></param>
        /// <param name="headWidth"></param>
        /// <param name="headHeight"></param>
        /// <author>Krystian Zielonka,Thomas Meents</author>
        protected void ConnectByArrow(Canvas canvas, Thumb startElement, Thumb destinationElement, int headWidth = 10,
            int headHeight = 4)
        {
            // Create the arrow
            Arrow arrow = new Arrow(startElement, destinationElement)
            {
                HeadWidth = headWidth,
                HeadHeight = headHeight,
                Stroke = Settings.Default.DefaultStrokeColor,
                StrokeThickness = Settings.Default.DefaultStrokeThickness
            };

            Binding x1 = new Binding();
            Binding x2 = new Binding();
            Binding y1 = new Binding();
            Binding y2 = new Binding();
            x1.Path = new PropertyPath(Canvas.LeftProperty);
            x2.Path = new PropertyPath(Canvas.LeftProperty);
            y1.Path = new PropertyPath(Canvas.TopProperty);
            y2.Path = new PropertyPath(Canvas.TopProperty);
            x1.Source = startElement;
            y1.Source = startElement;
            x2.Source = destinationElement;
            y2.Source = destinationElement;
            arrow.SetBinding(Arrow.X1Property, x1);
            arrow.SetBinding(Arrow.Y1Property, y1);
            arrow.SetBinding(Arrow.X2Property, x2);
            arrow.SetBinding(Arrow.Y2Property, y2);

            // Add the arrow to the canvas
            canvas.Children.Add(arrow);
        }

        /// <summary>
        ///     This event listener is called when a thumb was dragged and is now being dropped.
        ///     It updates the Left and Top values and also checks if the canvas has to be enlarged for the Thumb to fit.
        ///     It also stores the new values in the Nodes for future drawing.
        /// </summary>
        /// <autor>Krystian Zielonka, Jannik Arndt</autor>
        protected void DragAndDropThumb(object sender, DragDeltaEventArgs e)
        {
            Canvas parent = ((Thumb) sender).Parent as Canvas;
            if (parent == null) return;

            ExtendedThumb thumb = (ExtendedThumb) sender;

            // If the element is dragged beyond the bottom border
            if (Canvas.GetTop(thumb) > parent.Height)
                parent.Height = parent.Height + thumb.Height + 20;

            // If the element is dragged beyond the right border
            if (Canvas.GetLeft(thumb) > parent.Width)
                parent.Width = parent.Width + thumb.Width + 20;

            // If the element is dragged beyond the left border
            if (Canvas.GetLeft(thumb) < 0)
                parent.Width = parent.Width + (Canvas.GetLeft(thumb)*-1) + thumb.Width + 20;

            // Calculate the point in the grid, where the Thumb snaps to.
            int newX = NearestMultiple(Canvas.GetLeft(thumb) + e.HorizontalChange, Settings.Default.ColumnWidth/2);
            int newY = NearestMultiple(Canvas.GetTop(thumb) + e.VerticalChange, Settings.Default.RowHeight/2);

            // Place Thumb in Canvas
            Canvas.SetLeft(thumb, newX);
            Canvas.SetTop(thumb, newY);

            // Update values in the original Node
            thumb.BaseNode.PositionX = newX;
            thumb.BaseNode.PositionY = newY;
        }

        /// <summary>
        ///     Calculates the nearest multiple, or rounds to a factor, e.g. if the factor is 8, 4-11 are rounded to 8, 12-19 are
        ///     rounded to 16, and so on.
        /// </summary>
        /// <param name="number">The number that is supposed to be rounded.</param>
        /// <param name="factor">The "stops" it is rounded to.</param>
        /// <returns>An integer and multiple of the factor.</returns>
        /// <author>Jannik Arndt</author>
        protected int NearestMultiple(double number, int factor)
        {
            return (int) Math.Round((number/factor), MidpointRounding.AwayFromZero)*factor;
        }
    }
}