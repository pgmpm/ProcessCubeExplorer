using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;
using pgmpm.Model.PetriNet;
using pgmpm.Visualization.GUIElements;
using pgmpm.Visualization.Properties;
using pgmpm.MiningAlgorithm.InductiveV2;

namespace pgmpm.Visualization.PetriNetVisualization
{
    /// <summary>
    ///     The PetriNetVisualizer draw a petri net on a canvas.
    /// </summary>
    /// <author>Roman Bauer</author>
    public class PetriNetVisualizer : AbstractProcessModelVisualizer
    {
        /// <summary>
        ///     Slices a PetriNet into columns and then draws it on the given canvas.
        /// </summary>
        /// <param name="petriNet">A petrinet.</param>
        /// <param name="canvas">The canvas the net is drawn upon.</param>
        /// <param name="isDragAndDropAllowed">Enables drag and drop for all nodes. Default: true</param>
        /// <param name="forceRedraw"></param>
        /// <returns>The given canvas but with a net on it.</returns>
        /// <author>Jannik Arndt (based on Roman Bauers implementation)</author>
        public Canvas DrawPetriNet(PetriNet petriNet, Canvas canvas, bool isDragAndDropAllowed = true,
            bool forceRedraw = false)
        {
            Dictionary<Node, Thumb> nodesDictionary = new Dictionary<Node, Thumb>();

            List<Column> listOfColumns = ColumnBuilder.Build(petriNet);

            if (Settings.Default.AutomaticallyAlignNodes)
                listOfColumns = RowBuilder.CalculateRows(listOfColumns);

            // 1. Draw Transitions and Places
            foreach (Column column in listOfColumns)
            {
                int positionX = (Settings.Default.ColumnWidth*column.ColumnNumber) + Settings.Default.PlaceRadius;
                int positionY;

                foreach (Node node in column.HashSetOfNodes)
                {
                    // If the node has been drawn before, use the saved values
                    if (node.IsDrawn && forceRedraw == false)
                    {
                        positionX = node.PositionX;
                    }
                    else
                    {
                        node.PositionX = positionX;
                        positionY = Convert.ToInt32(node.Row*Settings.Default.RowHeight);
                        node.PositionY = positionY;
                        node.IsDrawn = true;
                    }

                    Thumb nodeThumb = DrawNode(canvas, node, isDragAndDropAllowed);

                    //Draw the place and cache the drawing.
                    if (!nodesDictionary.ContainsKey(node))
                        nodesDictionary.Add(node, nodeThumb);
                }
            }

            // 2. Draw arrows between the nodes
            foreach (KeyValuePair<Node, Thumb> node in nodesDictionary)
                foreach (Node followingNode in node.Key.OutgoingNodes)
                    ConnectByArrow(canvas, node.Value, nodesDictionary[followingNode]);
            return canvas;
        }

        /// <summary>
        ///     Draws a node (place or transition) on the given canvas and returns the Thumb
        /// </summary>
        /// <param name="canvas"></param>
        /// <param name="node"></param>
        /// <param name="isDragAndDropAllowed"></param>
        /// <returns></returns>
        /// <author>Jannik Arndt, Thomas Meents, Krystian Zielonka</author>
        private Thumb DrawNode(Canvas canvas, Node node, bool isDragAndDropAllowed)
        {
            // 1. get the specifics (transition or place?)
            ExtendedThumb nodeThumb;

            if (node.GetType() == typeof (Place))
                nodeThumb = DrawPlace((Place) node);
            else if (node.GetType() == typeof (Transition))
                nodeThumb = DrawTransition((Transition) node);
            
            else
                throw new PetriNetVisualizerException("Node is neither Place nor Transition.");

            // 2. do what they have in common
            if (node.DiffStatus == DiffState.Added)
                nodeThumb.Background = Settings.Default.AddedNodeColor;
            else if (node.DiffStatus == DiffState.Changed)
                nodeThumb.Background = Settings.Default.ChangedNodeColor;
            else if (node.DiffStatus == DiffState.Deleted)
                nodeThumb.Background = Settings.Default.DeletedNodeColor;
            else
                nodeThumb.Background = Settings.Default.DefaultBackgroundColor;

            nodeThumb.BorderBrush = Settings.Default.DefaultBorderColor;
            nodeThumb.BaseNode = node;


            if (isDragAndDropAllowed)
            {
                nodeThumb.DragDelta += DragAndDropThumb;
                nodeThumb.Cursor = Settings.Default.DefaultMoveCursor;
            }

            Canvas.SetTop(nodeThumb, node.PositionY);
            Canvas.SetLeft(nodeThumb, node.PositionX);

            if (node.PositionX > canvas.Width)
                canvas.Width = node.PositionX + nodeThumb.Width + Settings.Default.PositionCorrection;

            if (node.PositionY > canvas.Height)
                canvas.Height = node.PositionY + nodeThumb.Height + Settings.Default.PositionCorrection;

            canvas.Children.Add(nodeThumb);

            return nodeThumb;
        }

        /// <summary>
        ///     Draw a Rectangle
        /// </summary>
        /// <param name="transition"></param>
        /// <autor>Thomas Meents, Krystian Zielonka</autor>
        private ExtendedThumb DrawTransition(Transition transition)
        {
            String name = transition.Name.Trim();
            ExtendedThumb transitionThumb = new ExtendedThumb
            {
                Name = "Transition",
                Width = Settings.Default.TransitionWidth,
                Height = Settings.Default.TransitionHeight
            };
            transitionThumb.Margin = new Thickness(-transitionThumb.Width/2, -transitionThumb.Height/2, 0, 0);
            transitionThumb.Template = GetTransitionTemplate();

            transitionThumb.ToolTip = name;

            transitionThumb.SetValue(ContentControl.ContentProperty, name);

            ContextMenu contextMenu = new ContextMenu();
            Label labelText = new Label {Content = name};
            contextMenu.Items.Add(labelText);
            transitionThumb.ContextMenu = contextMenu;

            return transitionThumb;
        }

        /// <summary>
        ///     Draw a Ellipse
        /// </summary>
        /// <param Name="myCanvas"></param>
        /// <param name="place"></param>
        /// <autor>Thomas Meents, Krystian Zielonka</autor>
        private ExtendedThumb DrawPlace(Place place)
        {
            String name = place.Name.Trim();
            ExtendedThumb placeThumb = new ExtendedThumb
            {
                Name = "Place",
                Width = Settings.Default.PlaceRadius,
                Height = Settings.Default.PlaceRadius
            };
            placeThumb.Margin = new Thickness(-placeThumb.Width/2);
            placeThumb.Template = GetPlaceTemplate();
            placeThumb.SetValue(ContentControl.ContentProperty, name);
            placeThumb.InternName = place.ToString();

            return placeThumb;
        }

        /// <summary>
        ///     Creates the Template for a Transition
        /// </summary>
        /// <returns></returns>
        private ControlTemplate GetTransitionTemplate()
        {
            ControlTemplate transitionTemplate = new ControlTemplate();
            FrameworkElementFactory canvasElement = new FrameworkElementFactory(typeof (Canvas));
            FrameworkElementFactory rectangleElement = new FrameworkElementFactory(typeof (Rectangle));
            FrameworkElementFactory labelElement = new FrameworkElementFactory(typeof (Label));

            labelElement.SetValue(FrameworkElement.WidthProperty,
                new TemplateBindingExtension(FrameworkElement.WidthProperty));
            labelElement.SetValue(FrameworkElement.HeightProperty,
                new TemplateBindingExtension(FrameworkElement.HeightProperty));
            labelElement.SetValue(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
            labelElement.SetValue(Control.VerticalContentAlignmentProperty, VerticalAlignment.Center);
            labelElement.SetValue(ContentControl.ContentProperty,
                new TemplateBindingExtension(ContentControl.ContentProperty));
            labelElement.SetValue(Control.FontSizeProperty, new TemplateBindingExtension(Control.FontSizeProperty));

            rectangleElement.SetValue(Shape.FillProperty, new TemplateBindingExtension(Control.BackgroundProperty));
            rectangleElement.SetValue(Shape.StrokeProperty, new TemplateBindingExtension(Control.BorderBrushProperty));
            rectangleElement.SetValue(FrameworkElement.WidthProperty,
                new TemplateBindingExtension(FrameworkElement.WidthProperty));
            rectangleElement.SetValue(FrameworkElement.HeightProperty,
                new TemplateBindingExtension(FrameworkElement.HeightProperty));

            canvasElement.AppendChild(rectangleElement);
            canvasElement.AppendChild(labelElement);
            transitionTemplate.VisualTree = canvasElement;

            return transitionTemplate;
        }

        /// <summary>
        ///     Creates a template for a Place
        /// </summary>
        /// <returns></returns>
        private ControlTemplate GetPlaceTemplate()
        {
            ControlTemplate placeTemplate = new ControlTemplate();
            FrameworkElementFactory canvasElement = new FrameworkElementFactory(typeof (Canvas));
            FrameworkElementFactory ellipseElement = new FrameworkElementFactory(typeof (Ellipse));
            FrameworkElementFactory labelElement = new FrameworkElementFactory(typeof (Label));

            labelElement.SetValue(FrameworkElement.WidthProperty,
                new TemplateBindingExtension(FrameworkElement.WidthProperty));
            labelElement.SetValue(FrameworkElement.HeightProperty,
                new TemplateBindingExtension(FrameworkElement.HeightProperty));
            labelElement.SetValue(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Center);
            labelElement.SetValue(Control.VerticalContentAlignmentProperty, VerticalAlignment.Center);
            labelElement.SetValue(ContentControl.ContentProperty,
                new TemplateBindingExtension(ContentControl.ContentProperty));

            ellipseElement.SetValue(Shape.FillProperty, new TemplateBindingExtension(Control.BackgroundProperty));
            ellipseElement.SetValue(Shape.StrokeProperty, new TemplateBindingExtension(Control.BorderBrushProperty));
            ellipseElement.SetValue(FrameworkElement.WidthProperty,
                new TemplateBindingExtension(FrameworkElement.WidthProperty));
            ellipseElement.SetValue(FrameworkElement.HeightProperty,
                new TemplateBindingExtension(FrameworkElement.HeightProperty));

            canvasElement.AppendChild(ellipseElement);
            canvasElement.AppendChild(labelElement);
            placeTemplate.VisualTree = canvasElement;

            return placeTemplate;
        }
    }
}