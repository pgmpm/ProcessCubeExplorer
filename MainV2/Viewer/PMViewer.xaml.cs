using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FirstFloor.ModernUI.Windows.Controls;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.MainV2.Viewer
{
    /// <summary>
    /// Interaction logic for PMViewer.xaml
    /// </summary>
    /// <author>Naby (CloseClick1)</author>
    public partial class PMViewer
    {
        readonly ModernWindow _associatedWindow;
        public Field CurrentField;
        //public bool IsDiff;

        public PMViewer()
        {
            InitializeComponent();
            CurrentField = Viewer.CurrentField;
            ShowProcessModelInCanvas();
            HeadingText.Text = CurrentField.Infotext;
            PreviewKeyDown += HandleShortcuts;
            PreviewMouseWheel += Zoom;
        }

        public PMViewer(Field field, ModernWindow window, bool isDiff = false, string headingtext = "")
        {
            InitializeComponent();

            CurrentField = field;
            _associatedWindow = window;
            _associatedWindow.Title = "Model View";

            ShowProcessModelInCanvas();
            _associatedWindow.PreviewKeyDown += HandleShortcuts;
            _associatedWindow.PreviewMouseWheel += Zoom;
            HeadingText.Text = headingtext != "" ? headingtext : field.Infotext;
            
            //IsDiff = isDiff;
          

            //if (IsDiff)
            //{
                
            //    CloseButton1.Visibility = Visibility.Visible;
            //    ShowLegend();
            //}
            //else
            //{
            //    CloseButton1.Visibility = Visibility.Hidden;
            //}     
        }

        #region Buttons


        private void CloseClick1(object sender, RoutedEventArgs e)
        {
            _associatedWindow.Close();
        }

        private void ExportClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Exporter.Export(ProcessModelCanvas, CurrentField))
                ModernDialog.ShowMessage("File saved successfully!", "Export", MessageBoxButton.OK);
            }
            catch (Exception)
            {
                ModernDialog.ShowMessage("File could not saved!", "Export", MessageBoxButton.OK);
            }
        }

        private void PrintClick(object sender, RoutedEventArgs e)
        {
            Printer.PrintCanvas(ProcessModelCanvas);
        }

        private void RedrawClick(object sender, RoutedEventArgs e)
        {
            ShowProcessModelInCanvas(true);
        }

        private void ZoomInClick(object sender, RoutedEventArgs e)
        {
            VisualizationHelpers.ZoomFunction(ProcessModelCanvas,ScrollViewer,"in");
        }

        private void ZoomOutClick(object sender, RoutedEventArgs e)
        {
            VisualizationHelpers.ZoomFunction(ProcessModelCanvas,ScrollViewer, "out");
        }

        #endregion Buttons


        #region Shortcuts & Mousehandler
        /// <summary>
        /// Catches Key-events (handler is set in the constructor)
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        /// <author>Jannik Arndt, Thomas Meents, Bernd Nottbeck, Bernhard Bruns</author>
        private void HandleShortcuts(object sender, KeyEventArgs e)
        {

            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                switch (e.Key)
                {
                    case Key.P: Printer.PrintCanvas(ProcessModelCanvas); break;
                    case Key.S:
                        {
                            if (Exporter.Export(ProcessModelCanvas, null))
                                ModernDialog.ShowMessage("File saved successfully!", "Export", MessageBoxButton.OK);
                            break;
                        }
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.OemPlus: VisualizationHelpers.ZoomFunction(ProcessModelCanvas, ScrollViewer, "in"); break;
                    case Key.OemMinus: VisualizationHelpers.ZoomFunction(ProcessModelCanvas, ScrollViewer, "out"); break;
                    case Key.Left: VisualizationHelpers.ScrollToDirection(VisualizationHelpers.Direction.Left, ProcessModelCanvas); break;
                    case Key.Right: VisualizationHelpers.ScrollToDirection(VisualizationHelpers.Direction.Right, ProcessModelCanvas); break;
                    case Key.Up: VisualizationHelpers.ScrollToDirection(VisualizationHelpers.Direction.Up, ProcessModelCanvas); break;
                    case Key.Down: VisualizationHelpers.ScrollToDirection(VisualizationHelpers.Direction.Down, ProcessModelCanvas); break;
                }
            }
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
            {
                VisualizationHelpers.ZoomFunction(ProcessModelCanvas,ScrollViewer, "", e, sender);
            }
        }

        #endregion Shortcuts & Mousehandler

        #region Functionality
        private void ShowProcessModelInCanvas(bool forceRedraw = false)
        {
            ProcessModelCanvas.Children.Clear();
            VisualizationHelpers.GetOrCreatePetriNetVisualization(CurrentField, ProcessModelCanvas, forceRedraw: forceRedraw);
        }

        /// <summary>
        /// Method to trigger a search based on the process model.
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        /// <author>Bernd Nottbeck, Markus Holznagel</author>
        private void SearchEventTextBoxTextChanged(object sender, RoutedEventArgs e) //TextChangedEventArgs e)
        {
            SearchPetriNet(sender, e);
        }

        /// <summary>
        /// Search Method to highlight specific transitions on a PetriNet. Checks all Child Thumb Objects of the processModelCanvas 
        /// Object for Filetype Transition and the Search String. 
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        /// <author>Bernd Nottbeck</author>
        private void SearchPetriNet(object sender, RoutedEventArgs e) //TextChangedEventArgs e)
        {
            String searchText = ((TextBox)sender).Text.ToUpper();
            foreach (Object kind in ProcessModelCanvas.Children)
            {
                if (kind is Thumb)
                {
                    Thumb kindT = (Thumb)kind;
                    if (kindT.Name.Equals("Transition"))
                    {
                        if ((kindT.GetValue(ContentProperty).ToString().ToUpper()).Contains(searchText) && searchText != "")
                        {
                            kindT.Background = Brushes.Aquamarine;
                        }
                        else { kindT.Background = Brushes.LightGray; }
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the legend into the window.
        /// </summary>
        /// <author>Thomas Meents</author>
        private void ShowLegend()
        {
            Canvas canvas = new Canvas();
            Rectangle addedelements = new Rectangle { Fill = Brushes.Yellow, Width = 20, Height = 20 };
            Label addedelementslabel = new Label { Content = "Added elements", Margin = new Thickness(30, 0, 0, 0) };
            canvas.Children.Add(addedelements);
            canvas.Children.Add(addedelementslabel);

            Rectangle changedelements = new Rectangle
            {
                Fill = Brushes.Orange,
                Width = 20,
                Height = 20,
                Margin = new Thickness(140, 0, 0, 0)
            };
            Label changedelementslabel = new Label
            {
                Content = "Changed elements (old value) new value",
                Margin = new Thickness(170, 0, 0, 0)
            };
            canvas.Children.Add(changedelements);
            canvas.Children.Add(changedelementslabel);
            Rectangle deletedelements = new Rectangle
            {
                Fill = Brushes.Red,
                Width = 20,
                Height = 20,
                Margin = new Thickness(420, 0, 0, 0)
            };
            Label deletedelementslabel = new Label { Content = "Deleted elements", Margin = new Thickness(450, 0, 0, 0) };
            canvas.Children.Add(deletedelements);
            canvas.Children.Add(deletedelementslabel);

            Legend.Children.Add(canvas);
        }
        #endregion
    }
}