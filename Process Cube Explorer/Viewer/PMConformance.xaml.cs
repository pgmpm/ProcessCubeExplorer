using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;
using pgmpm.ConformanceChecking;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection.Fields;
using pgmpm.Model.PetriNet;
using pgmpm.Visualization.GUIElements;

namespace pgmpm.MainV2.Viewer
{
    /// <summary>
    /// Interaction logic for PMConformance.xaml
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public partial class PMConformance
    {
        public PMConformance()
        {
            InitializeComponent();
            DrawConformanceModelInCanvas(Viewer.CurrentField, CanvasConformanceChecking);
            PreviewMouseWheel += Zoom;
        }

        /// <summary>
        /// If the Shift-Key is pressed, this zooms into the Canvas, keeping true the scroll-wheel-acceleration.
        /// Functionality: This uses the ScaleTransform-property of the canvas.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Jannik Arndt</author>
        private void Zoom(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
                VisualizationHelpers.ZoomFunction(CanvasConformanceChecking, ScrollViewer, "", e, sender);
        }

        #region Functionality

        /// <summary>
        /// Conformance Check between the eventlog and generated process model.
        /// The criticized transitions are shown in the process model.
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        public void DrawConformanceModelInCanvas(Field field, Canvas canvas)
        {
            try
            {
                // Create Footprints
                ComparingFootprint footPrintEventlog = ComparingFootprintAlgorithm.CreateFootprint(field.EventLog);
                ComparingFootprint footPrintPetrinet = ComparingFootprintAlgorithm.CreateFootprint((PetriNet)field.ProcessModel);
                ComparingFootprintResultMatrix resultFootprint = new ComparingFootprintResultMatrix(footPrintEventlog, footPrintPetrinet);

                // Calculate Fitness
                double fitnessComparingFootprint = ComparingFootprintAlgorithm.CalculateFitness(resultFootprint.GetNumberOfDifferences(), resultFootprint.GetNumberOfOpportunities());
                FitnessComparingFootprints.Content = "Fitness Comparing Footprints: " + Math.Round(fitnessComparingFootprint * 100, 2) + " %";

                // Prepare Canvas & draw model
                RemoveListenerFromCanvas(canvas);
                canvas.Children.Clear();
                VisualizationHelpers.GetOrCreatePetriNetVisualization(field, canvas);
                AddListenertoTransitions(canvas);

                // Color the transitions in
                List<String> listOfGreenTransitions = new List<String>();
                List<String> listOfRedTransitions = new List<String>();
                List<List<String>> listOfLinesToBeRemoved = new List<List<String>>();

                int row = 0;
                foreach (String rowHeaderName in resultFootprint.HeaderWithEventNames)
                {
                    int column = 0;
                    foreach (String columnHeaderName in resultFootprint.HeaderWithEventNames)
                    {
                        if (resultFootprint.ResultMatrix[row, column].Equals(ResultCellType.NoDifferences) && rowHeaderName.Equals(columnHeaderName))
                            listOfGreenTransitions.Add(rowHeaderName);

                        else if (resultFootprint.ResultMatrix[row, column].Equals(ResultCellType.NothingAndRight))
                            listOfLinesToBeRemoved.Add(new List<String> { rowHeaderName, columnHeaderName });

                            // else if (ResultFootprint.ResultMatrix[Row, Column].Equals(ResultCellType.RightAndNothing))
                            // Model enhacement: here you can fill a list of lines that are must to be add to the petrinet
                            // example: listOfLinesToAddTo.Add(new TransitionCombination(yHeaderName, xHeaderName));

                        else if (rowHeaderName.Equals(columnHeaderName))
                            listOfRedTransitions.Add(rowHeaderName);

                        column++;
                    }
                    row++;
                }

                SetColorInCanvas(listOfGreenTransitions, Brushes.Green, canvas);
                SetColorInCanvas(listOfRedTransitions, Brushes.Red, canvas);
                SetColorOnLinesToBeRemoved(listOfLinesToBeRemoved, canvas, (PetriNet)field.ProcessModel);
            }
            catch (Exception Ex)
            {
                ErrorHandling.ReportErrorToUser("Error: " + Ex.Message + Ex.StackTrace);
            }
        }

        /// <summary>
        /// Method to remove a listener from the canvas
        /// </summary>
        /// <param name="conformanceCheckingCanvas"></param>
        /// <autor>Andrej Albrecht</autor>
        private void RemoveListenerFromCanvas(Canvas conformanceCheckingCanvas)
        {
            foreach (Object child in conformanceCheckingCanvas.Children)
            {
                Thumb thumb = child as Thumb;
                if (thumb != null && thumb.Name.Equals("Transition"))
                    thumb.MouseRightButtonDown -= Transition_MouseRightButtonDown;
            }
        }

        /// <summary>
        /// Method to add a listener to the canvas
        /// </summary>
        /// <param name="conformanceCheckingCanvas"></param>
        /// <autor>Andrej Albrecht</autor>
        private void AddListenertoTransitions(Canvas conformanceCheckingCanvas)
        {
            foreach (Object child in conformanceCheckingCanvas.Children)
            {
                Thumb thumb = child as Thumb;
                if (thumb != null && thumb.Name.Equals("Transition"))
                    thumb.MouseRightButtonDown += Transition_MouseRightButtonDown;
            }
        }

        /// <summary>
        /// Method to change the color of the given list of lines in a canvas
        /// </summary>
        /// <param name="listOfLinesToBeRemoved">List of lines that must be removed</param>
        /// <param name="conformanceCheckingCanvas"></param>
        /// <param name="petriNet">Petrinet</param>
        /// <autor>Andrej Albrecht</autor>
        private void SetColorOnLinesToBeRemoved(IEnumerable<List<string>> listOfLinesToBeRemoved, Canvas conformanceCheckingCanvas, PetriNet petriNet)
        {
            foreach (List<String> stringCombination in listOfLinesToBeRemoved)
            {
                if (stringCombination.Count == 2)
                {
                    String transition1 = stringCombination.ElementAt(0);
                    String transition2 = stringCombination.ElementAt(1);

                    IEnumerable<Shape> listWithArrowsToBeMarked = GetArrowsBetweenTransitions(transition1, transition2, conformanceCheckingCanvas, petriNet);

                    foreach (Shape shape in listWithArrowsToBeMarked)
                    {
                        shape.Stroke = Brushes.Red;
                        shape.Fill = Brushes.Red;
                    }
                }
            }
        }

        /// <summary>
        /// This method is not finished yet
        /// 
        /// The method returns a list of shapes that are between two transitions
        /// </summary>
        /// <param name="t1">Transitionname</param>
        /// <param name="t2">Transitionname</param>
        /// <param name="conformanceCheckingCanvas"></param>
        /// <param name="petriNet">Petrinet</param>
        /// <autor>Andrej Albrecht</autor>
        private IEnumerable<Shape> GetArrowsBetweenTransitions(String t1, String t2, Canvas conformanceCheckingCanvas, PetriNet petriNet)
        {
            List<Place> listOfPlacesWithBadLines = GetListOfPlacesBetweenTwoTransitionsFromPetriNet(t1, t2, petriNet);
            List<Point2D> listWithPointsTo = GetListWithPointsToFromCanvas(t1, t2, conformanceCheckingCanvas);
            List<Point2D> listWithPointsFrom = GetListWithPointsFromCanvas(listOfPlacesWithBadLines, conformanceCheckingCanvas);
            List<Shape> listOfArrowsToBeMarked = GetListOfArrowsToBeMarked(conformanceCheckingCanvas, listWithPointsFrom, listWithPointsTo);

            List<Shape> listOfLinesToBeMarked = new List<Shape>();
            foreach (Object kind in conformanceCheckingCanvas.Children)
            {
                if (kind is Line)
                {
                    // Line KindP = (Line)Kind;
                    foreach (Shape shapeArrow in listOfArrowsToBeMarked)
                    {
                        if (shapeArrow is Arrow)
                        {
                            // Arrow Arrow = (Arrow)ShapeArrow;

                            //Place for the method who gets the lines and not only the arrows of the canvas:

                            //Started with the following algorithm that not worked now.
                            /*
                            //if (pointFrom.Equals(new Point2D(arrow.X1, arrow.Y1)) && pointTo.Equals(new Point2D(arrow.X2, arrow.Y2))
                            //    && KindP.X1==arrow.ArrowX1 && KindP.X2==arrow.ArrowX2 && KindP.Y1==arrow.ArrowY1 && KindP.Y2==arrow.ArrowY2)
                            if (arrow.ArrowX1 == KindP.X1 && arrow.ArrowY1 == KindP.Y1 && arrow.ArrowX2 == KindP.X1 && arrow.ArrowY2 == KindP.Y2)
                            {
                                System.Console.WriteLine("BadLine xFrom:" + arrow.ArrowX1 + " yFrom:" + arrow.ArrowY1 + " xTo:" + arrow.ArrowX2 + " yTo:" + arrow.ArrowY2 + " Name:" + arrow.Name);

                                listOfLinesToBeMarked.Add(KindP);
                            }
                            */
                        }
                    }
                }
            }
            return listOfArrowsToBeMarked.Union(listOfLinesToBeMarked).ToList();
        }

        /// <summary>
        /// The method returns a list of shapes that are starting point is in the list "listWithPointsFrom" and that endpoint is in the list "listWithPointsTo"
        /// </summary>
        /// <param name="conformanceCheckingCanvas">Canvas</param>
        /// <param name="listWithPointsFrom">List with starting points</param>
        /// <param name="listWithPointsTo">List with end points</param>
        /// <returns>List of shapes</returns>
        /// <autor>Andrej Albrecht</autor>
        private List<Shape> GetListOfArrowsToBeMarked(Canvas conformanceCheckingCanvas, List<Point2D> listWithPointsFrom, List<Point2D> listWithPointsTo)
        {
            List<Shape> listOfShapeToBeMarked = new List<Shape>();
            foreach (Object child in conformanceCheckingCanvas.Children)
            {
                Arrow arrow = child as Arrow;
                if (arrow != null)
                    foreach (Point2D pointFrom in listWithPointsFrom)
                        foreach (Point2D pointTo in listWithPointsTo)
                            if (pointFrom.Equals(new Point2D(arrow.X1, arrow.Y1)) && pointTo.Equals(new Point2D(arrow.X2, arrow.Y2)))
                                listOfShapeToBeMarked.Add(arrow);
            }

            return listOfShapeToBeMarked;
        }

        /// <summary>
        /// The method returns a list of points for a given list of affected places
        /// </summary>
        /// <param name="listOfPlacesWithBadLines">List fo places</param>
        /// <param name="conformanceCheckingCanvas">Canvas</param>
        /// <returns></returns>
        /// <autor>Andrej Albrecht</autor>
        private List<Point2D> GetListWithPointsFromCanvas(List<Place> listOfPlacesWithBadLines, Canvas conformanceCheckingCanvas)
        {
            List<Point2D> listWithPointsFrom = new List<Point2D>();
            foreach (Object child in conformanceCheckingCanvas.Children)
            {
                ExtendedThumb thumb = child as ExtendedThumb;
                if (thumb != null && thumb.Name.Equals("Place"))
                    foreach (Place place in listOfPlacesWithBadLines)
                        if (thumb.InternName.Equals(place.ToString()))
                            listWithPointsFrom.Add(new Point2D(Canvas.GetLeft(thumb), Canvas.GetTop(thumb)));
            }

            return listWithPointsFrom;
        }

        /// <summary>
        /// Gets a list of points who starting from the transitionname in t1 and ends with the transitionname in t2
        /// </summary>
        /// <param name="t1">Transitionname</param>
        /// <param name="t2">Transitionname</param>
        /// <param name="conformanceCheckingCanvas">Canvas</param>
        /// <returns>Returns a list of points</returns>
        /// <autor>Andrej Albrecht</autor>
        private List<Point2D> GetListWithPointsToFromCanvas(string t1, string t2, Canvas conformanceCheckingCanvas)
        {
            List<Point2D> listWithPointsTo = new List<Point2D>();
            foreach (Object child in conformanceCheckingCanvas.Children)
            {
                Thumb thumb = child as Thumb;
                if (thumb != null && thumb.Name.Equals("Transition"))
                {
                    if ((thumb.GetValue(ContentProperty).ToString()).Contains(t1))
                    {
                        //For extension:
                        //System.Console.WriteLine("TransitionT1Left left:" + Canvas.GetLeft(KindThumb) + " top:" + Canvas.GetTop(KindThumb) + " Content: " + KindThumb.GetValue(Label.ContentProperty).ToString());
                    }
                    else if ((thumb.GetValue(ContentProperty).ToString()).Contains(t2))
                    {
                        listWithPointsTo.Add(new Point2D(Canvas.GetLeft(thumb), Canvas.GetTop(thumb)));
                    }
                }
            }

            return listWithPointsTo;
        }

        /// <summary>
        /// Returns a list of places between two transitions in a petrinet
        /// </summary>
        /// <param name="t1">Transitionname</param>
        /// <param name="t2">Transitionname</param>
        /// <param name="petriNet">Petrinet</param>
        /// <returns>Returns a list of places</returns>
        /// <autor>Andrej Albrecht</autor>
        private List<Place> GetListOfPlacesBetweenTwoTransitionsFromPetriNet(string t1, string t2, PetriNet petriNet)
        {
            Transition transition1 = petriNet.FindTransition(t1);
            Transition transition2 = petriNet.FindTransition(t2);
            HashSet<Place> placesInBetween = new HashSet<Place>();

            foreach (Place outgoingPlace in transition1.OutgoingPlaces)
                if (transition2.IncomingPlaces.Contains(outgoingPlace))
                    placesInBetween.Add(outgoingPlace);

            return placesInBetween.ToList();
        }

        /// <summary>
        /// Change the color of transitions in a canvas who name is in a given list
        /// </summary>
        /// <param name="listOfTransitions">List of trasitions</param>
        /// <param name="solidColorBrush">SolidColorBrush</param>
        /// <param name="conformanceCheckingCanvas">Canvas</param>
        /// <autor>Andrej Albrecht</autor>
        private void SetColorInCanvas(List<String> listOfTransitions, SolidColorBrush solidColorBrush, Canvas conformanceCheckingCanvas)
        {
            foreach (var child in conformanceCheckingCanvas.Children)
            {
                var thumb = child as Thumb;
                if (thumb == null || !thumb.Name.Equals("Transition")) continue;
                if (listOfTransitions.Contains(thumb.GetValue(ContentProperty).ToString()))
                    thumb.Background = solidColorBrush;
            }
        }

        /// <summary>
        /// Method for the right click on a transition.
        /// This method opens a footprintviewer
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        private static void Transition_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Thumb thumb = (Thumb)sender;
                String transitionName = thumb.GetValue(ContentProperty).ToString();

                ModernWindow footprintViewer = new ModernWindow
                {
                    Style = (Style)Application.Current.Resources["EmptyWindow"],
                    Width = 600,
                    Height = 550,
                    Content = new FootprintViewer(transitionName),
                    //Topmost = true
                };
                footprintViewer.ShowDialog();
            }
            catch (Exception Ex)
            {
                ErrorHandling.ReportErrorToUser("Error: " + Ex.Message + Ex.StackTrace);
            }
        }
        #endregion
    }
}