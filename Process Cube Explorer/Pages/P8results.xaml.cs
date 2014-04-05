using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.Consolidation;
using pgmpm.Consolidation.Algorithm;
using pgmpm.Diff.DiffAlgorithm;
using pgmpm.Diff;
using pgmpm.MainV2.Utilities;
using pgmpm.MainV2.Viewer;
using pgmpm.MatrixSelection.Fields;
using pgmpm.Model;
using pgmpm.Model.PetriNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace pgmpm.MainV2.Pages
{
    /// <summary>
    /// Interaction logic for P8results.xaml
    /// </summary>
    public partial class P8Results : IContent
    {
        public static List<ProcessModel> ListOfChoosenProcessModels = new List<ProcessModel>();
        private int _currentPreviewModel;
        int Cols;
        Field _currentlySelectedField;
        private UniformGrid _uniformgrid;
        public List<string> EventUniqueList;
        private HashSet<string> _listOfSelectedOptions;
     

        public P8Results()
        {
            InitializeComponent();
        }

        #region Navigation
        void IContent.OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        void IContent.OnNavigatedFrom(NavigationEventArgs e)
        {
            ListOfChoosenProcessModels.Clear();
        }

        void IContent.OnNavigatedTo(NavigationEventArgs e)
        {
            DBInformationButton.ToolTip = MainWindow.ConnectionName;
            InterfaceHelpers.CurrentPage = e.Source.OriginalString;

            InitializePreviewInformations();

            //Consolidation
            InitializeConsolidationControls();
            FillConsolidationListboxWithEvents();
            ConsolidatorSettings.ProcessModelType = typeof(PetriNet);

            ConsolidatorSettings.ConsolidationType = typeof(StandardConsolidator);
        }

        void IContent.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (InterfaceHelpers.CheckIfNavigationIsAllowed(e.Source.OriginalString) == false)
                e.Cancel = true;
        }
        #endregion

        #region Buttons
        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/P6mining.xaml", null);
        }

        private void OpenSettingsClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/Settings.xaml", null);
        }

        private void OpenNoteClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/notes.xaml", null);
        }

        private void DBInformationClick(object sender, RoutedEventArgs e)
        {
            ModernDialog.ShowMessage(MainWindow.ConnectionName, "Database information", MessageBoxButton.OK);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Jannik Arndt, Markus Holznagel</author>
        private void SaveClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Serializer.Serialize("LastMiningResults.mpm", MainWindow.MatrixSelection))
                    ModernDialog.ShowMessage("The selected and mined data was successfully stored. You can reload it from the connection-tab!", "Save data", MessageBoxButton.OK);
            }
            catch (Exception Ex)
            {
                if (Ex is SEHException)
                    ErrorHandling.ReportErrorToUser("Error while saving: " + Ex.Message + "\n" + (Ex as SEHException).StackTrace);
                else if (Ex is UnauthorizedAccessException)
                    ErrorHandling.ReportErrorToUser("Error while saving: Unauthorized Access: " + Ex.Message + "\n" + (Ex as UnauthorizedAccessException).StackTrace);
                else if (Ex is ArgumentNullException || Ex is IOException || Ex is NullReferenceException || Ex is ArgumentOutOfRangeException)
                    ErrorHandling.ReportErrorToUser("Error: " + Ex.Message);
                else
                    throw;
            }
        }

        /// <summary>
        /// Restart the workflow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Bernhard Bruns, Jannik Arndt, Moritz Eversmann</author>
        private void RestartClick(object sender, RoutedEventArgs e)
        {
            if (ModernDialog.ShowMessage("Do you want to restart the Workflow?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                NavigationCommands.GoToPage.Execute("/Pages/P1connection.xaml", null);
        }

        /// <summary>
        /// Method for exporting a petrinet, eventlog or canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <autor>Andrej Albrecht</autor>
        private void ExportClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Exporter.Export(ProcessModelPreviewCanvas, _currentlySelectedField))
                {
                    ModernDialog.ShowMessage("File saved successfully!", "Export", MessageBoxButton.OK);
                }
            }
            catch (Exception)
            {
                ModernDialog.ShowMessage("file could not be saved!", "Export", MessageBoxButton.OK);
            }
        }

        private void PrintClick(object sender, RoutedEventArgs e)
        {
            Printer.PrintCanvas(ProcessModelPreviewCanvas);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MagnifyClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentlySelectedField != null)
                {
                    ModernWindow viewer = new Viewer.Viewer(_currentlySelectedField);
                    viewer.Owner = Application.Current.MainWindow;
                    viewer.Show();
                }
            }
            catch (ArgumentNullException Ex)
            {
                ErrorHandling.ReportErrorToUser("Error: " + Ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FieldClick(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                ConsolidationExpander.IsExpanded = false;
                LegendExpander.IsExpanded = false;

                _currentlySelectedField = (((Button)sender).Tag as Field);
                try
                {
                    PMInformation.ItemsSource = _currentlySelectedField.Information;
                    ShowPreviewOfField(_currentlySelectedField);
                    EnablePreviewButtons(true);
                }
                catch (Exception Ex)
                {
                    if (Ex is ArgumentNullException)
                    {
                        ProcessModelPreviewCanvas.Width = viewBoxPreview.Width;
                        ProcessModelPreviewCanvas.Height = viewBoxPreview.Height;
                        EnablePreviewButtons(false);

                        TextBlock errorText = new TextBlock { Text = "This field does not have a process model." };
                        ProcessModelPreviewCanvas.Children.Clear();
                        ProcessModelPreviewCanvas.Children.Add(errorText);
                        PMInformation.ItemsSource = new List<KeyValuePair<string, string>>();
                    }
                    else
                        ErrorHandling.ReportErrorToUser("Error: " + Ex.Message + "\n" + Ex.StackTrace);

                    _currentPreviewModel = 0;
                }
            }
        }

        /// <summary>
        /// Opens a PMViewerWindow for the selected field. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FieldDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender.GetType() == typeof(Button))
            {
                _currentlySelectedField = (((Button)sender).Tag as Field);
                try
                {
                    if (_currentlySelectedField.ProcessModel == null)
                        throw new ArgumentNullException("field", @"No processmodel can be found for this field");
                    ModernWindow viewer = new Viewer.Viewer(_currentlySelectedField);
                    viewer.Owner = Application.Current.MainWindow;
                    viewer.Show();
                }
                catch (ArgumentNullException Ex)
                {
                    ErrorHandling.ReportErrorToUser("Error: " + Ex.Message);
                }
            }
        }

        /// <summary>
        /// Compares two or three process models
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Thomas Meents, Christopher Licht</author>
        private void CompareClick(object sender, RoutedEventArgs e)
        {
            switch (ListOfChoosenProcessModels.Count)
            {
                case 2:
                    {
                        try
                        {
                            IDifference diff = DiffFactory.CreateDifferenceObject<SnapshotDiff>();

                            Field diffField = new Field
                            {
                                ProcessModel = diff.CompareProcessModels(ListOfChoosenProcessModels)
                            };
                            if (diffField.ProcessModel != null && diffField.ProcessModel.GetType() == typeof(PetriNet))
                            {
                                String title = String.Join(", ", ListOfChoosenProcessModels.Select(pm => pm.Name));

                                ModernWindow pmDiffWindow = new ModernWindow
                                {
                                    Style = (Style)Application.Current.Resources["EmptyWindow"],
                                    Width = 1000,
                                    Height = 600,
                                    Owner = Application.Current.MainWindow,
                                    Title = title
                                };
                                pmDiffWindow.Content = new PMViewer(diffField, pmDiffWindow, true, title);
                                pmDiffWindow.Show();
                            }
                        }
                        catch (Exception Ex)
                        {
                            ErrorHandling.ReportErrorToUser("Error: " + Ex.Message);
                        }
                    }
                    break;
                case 0:
                    ModernDialog.ShowMessage("No process model checkboxes are checked", "Tip", MessageBoxButton.OK);
                    break;
                default:
                    ModernDialog.ShowMessage("Only two process models can be compared.\nCheck some checkboxes and click Compare to see the differences.", "Tip", MessageBoxButton.OK);
                    break;
            }
        }


        /// <summary>
        /// Fill the list of chosen process models
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Thomas Meents, Bernhard Bruns</author>
        private void GetChoosenProcessModel(object sender, RoutedEventArgs e)
        {
            if (sender.GetType() == typeof(CheckBox))
            {
                try
                {
                    CheckBox checkbox = sender as CheckBox;
                    if (checkbox != null)
                    {
                        Field field = checkbox.Tag as Field;
                        if (checkbox.IsChecked == true)
                        {
                            if (field != null) ListOfChoosenProcessModels.Add(field.ProcessModel.DeepCopy());
                        }
                        if (checkbox.IsChecked == false)
                        {
                            ProcessModel temp = ListOfChoosenProcessModels.Find(k => field != null && k.GetHashCode() == field.ProcessModel.DeepCopyHash);
                            ListOfChoosenProcessModels.Remove(temp);
                        }

                        refreshButtonState();
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show("Exception\n" + Ex);
                }
            }
        }

        /// <summary>
        /// If more then two fields are selected the export button is disabled
        /// </summary>
        /// <autor>Andrej Albrecht</autor>
        private void refreshButtonState()
        {
            if (_currentlySelectedField != null && ListOfChoosenProcessModels.Count <= 1)
            {
                ExportButton.IsEnabled = true;
                PrintButton.IsEnabled = true;
                MagnifyButton.IsEnabled = true;
            }
            else
            {
                ExportButton.IsEnabled = false;
                PrintButton.IsEnabled = false;
                MagnifyButton.IsEnabled = false;
            }
        }

        #endregion

        #region Functionality

        private void AssignUniformGrid(object sender, RoutedEventArgs e)
        {
            _uniformgrid = sender as UniformGrid;
            UpdateMatrix();
        }

        private void UpdateMatrix()
        {
            try
            {
                MatrixVisualizationGrid.ItemsSource = MainWindow.MatrixSelection.MatrixFields;
                // if there is nothing to show, still show one field
                int numberOfColumns = MainWindow.MatrixSelection.SelectedDimensions[0].SelectedFilters.Any() ? MainWindow.MatrixSelection.SelectedDimensions[0].SelectedFilters.Count() : 1;
                if (_uniformgrid != null)
                {
                    _uniformgrid.Columns = Cols;
                    _uniformgrid.Columns = numberOfColumns;
                }
            }
            catch (Exception Ex)
            {
                if (Ex is ArgumentOutOfRangeException)
                    ErrorHandling.ReportErrorToUser("Error: " + Ex.Message);
                else
                    throw;
            }

        }

        /// <summary>
        /// Shows the ProcessModel in the Preview-Box
        /// </summary>
        /// <param name="field"></param>
        /// <author>Jannik Arndt, Bernhard Bruns</author>
        private void ShowPreviewOfField(Field field)
        {
            if (field.ProcessModel == null)
                throw new ArgumentNullException("field", @"No processmodel can be found for this field");
            if (field.ProcessModel.GetHashCode() == _currentPreviewModel)
                return;

            ProcessModelPreviewCanvas.Children.Clear();
            ProcessModelPreviewCanvas.Width = 300;
            ProcessModelPreviewCanvas.Height = 100;
            VisualizationHelpers.GetOrCreatePetriNetVisualization(field, ProcessModelPreviewCanvas, false);
            _currentPreviewModel = field.ProcessModel.GetHashCode();
        }

        /// <summary>
        /// Enable or disable the buttons in the preview
        /// </summary>
        /// <param name="state">Button state</param>
        private void EnablePreviewButtons(bool state)
        {
            ExportButton.IsEnabled = state;
            PrintButton.IsEnabled = state;
            MagnifyButton.IsEnabled = state;
            //refreshButtonState();
        }

        /// <summary>
        /// Initialize the preview Informations
        /// </summary>
        private void InitializePreviewInformations()
        {
            PMInformation.ItemsSource = null;
            ProcessModelPreviewCanvas.Children.Clear();
            ProcessModelPreviewCanvas.Width = viewBoxPreview.Width;
            ProcessModelPreviewCanvas.Height = viewBoxPreview.Height;
            EnablePreviewButtons(false);
        }



        /// <summary>
        /// Initialize the Controls
        /// </summary>
        /// <author>Bernhard Bruns, Christopher Licht, Moritz Eversmann</author>
        private void InitializeConsolidationControls()
        {
            ConsolidationListBox.ItemsSource = ConsolidatorSettings.ConsolidationOptions;


            ConsolidationListBox.Style = (Style)(Application.Current.TryFindResource("ListBox"));
            ConsolidationListBox.ItemContainerStyle = (Style)Application.Current.TryFindResource("ListBoxItemWithCheckbox");

            ConsolidationListBox.SelectionMode = SelectionMode.Extended;

            EventListBox.Style = (Style)(Application.Current.TryFindResource("ListBox"));
            EventListBox.ItemContainerStyle = (Style)Application.Current.TryFindResource("ListBoxItemWithCheckbox");
            EventListBox.SelectionMode = SelectionMode.Extended;



            EventListBox.Visibility = Visibility.Visible;
            EventPanel.Visibility = Visibility.Collapsed;
            SliderPanel.Visibility = Visibility.Collapsed;
            AvailableEventsHeader.Visibility = Visibility.Visible;
            StartConsolidationButton.IsEnabled = false;

            NumberOfEventsSlider.Minimum = 1;
            NumberOfEventsSlider.Maximum = ConsolidationHelper.GetMaximumNumberOfUsedEvents();
        }

        /// <summary>
        /// Set the StartConsolidationButton and listbox with events to visible.
        /// </summary>
        /// <param name="sender">ListBox with available options.</param>
        /// <param name="e"></param>
        /// <author>Christopher Licht, Bernhard Bruns, Moritz Eversmann</author>
        private void OnConsolidationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StartConsolidationButton.IsEnabled = ConsolidationListBox.SelectedItems.Count > 0;
            EventPanel.Visibility = ConsolidationListBox.SelectedItems.Contains(ConsolidatorSettings.ConsolidationOptions[2]) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            if (ConsolidationListBox.SelectedItems.Contains(ConsolidatorSettings.ConsolidationOptions[3]))
                SliderPanel.Visibility = System.Windows.Visibility.Visible;
            else
                SliderPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author>Bernhard Bruns</author>
        private void FillConsolidationListboxWithEvents()
        {
            EventListBox.Items.Clear();

            EventUniqueList = new List<string>();

            foreach (Field field in MainWindow.MatrixSelection.MatrixFields)
            {
                foreach (Event e in field.EventLog.ListOfUniqueEvents)
                {
                    if (EventListBox.Items.Contains(e.Name) == false)
                    {
                        EventListBox.Items.Add(e.Name);
                        EventUniqueList.Add(e.Name);
                    }
                }
            }
            EventListBox.Items.SortDescriptions.Add(
            new System.ComponentModel.SortDescription("",
            System.ComponentModel.ListSortDirection.Ascending));
        }

        /// <summary>
        /// Filters a filterListBox by a search string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Bernhard Bruns</author>
        private void QuickSearchConsolidation(object sender, TextChangedEventArgs e)
        {
            EventListBox.Items.Filter = delegate(object OwnObject)
            {
                String enteredValue = (String)OwnObject;

                foreach (String value in EventListBox.SelectedItems)
                    if (value == enteredValue)
                        return true;

                String finalEnteredValue = enteredValue.ToUpper();
                if (String.IsNullOrEmpty(finalEnteredValue))
                    return false;
                int index = finalEnteredValue.IndexOf(QuicksearchTextField.Text.ToUpper(), 0, StringComparison.Ordinal);
                return (index > -1);
            };
        }

        /// <summary>
        /// Calls the static method of ConsolidationHelper and update the matrix-fields.
        /// </summary>
        /// <param name="sender">Start-Consolidation-Button is the sender.</param>
        /// <param name="e">Current event.</param>
        /// <author>Christopher Licht</author>
        private void StartConsolidationClick(object sender, RoutedEventArgs e)
        {
            HashSet<Field> listOfFieldsWhichFulfilledSelectedOptions = new HashSet<Field>();
            HashSet<String> listOfSelectedEvents = new HashSet<String>();
            HashSet<Field> listOfMatrixFields = new HashSet<Field>();
            int andOrSelection = -1;
            _listOfSelectedOptions = new HashSet<String>();

            andOrSelection = AndRadioButton.IsChecked == true ? 0 : 1;

            foreach (String selectedListViewItem in ConsolidationListBox.SelectedItems)
                _listOfSelectedOptions.Add(selectedListViewItem);

            foreach (String selectedEvent in EventListBox.SelectedItems)
                listOfSelectedEvents.Add(selectedEvent);

            foreach (var matrixField in MainWindow.MatrixSelection.MatrixFields)
                listOfMatrixFields.Add(matrixField);

            if (andOrSelection == 0 && (_listOfSelectedOptions.Count < 2))
                ModernDialog.ShowMessage("Your selected operator is AND. Please select two options minimum from the available options.", "Attention", MessageBoxButton.OK);
            else
            {
                ConsolidatorSettings.AddOrUpdateKey("SelectedOptions", _listOfSelectedOptions);
                ConsolidatorSettings.AddOrUpdateKey("SelectedEvents", listOfSelectedEvents);
                ConsolidatorSettings.AddOrUpdateKey("NumberOfEvents", (int)NumberOfEventsSlider.Value);
                ConsolidatorSettings.AddOrUpdateKey("AndOrSelection", (int)andOrSelection);

                IConsolidator consolidator = ConsolidatorFactory.CreateConsolidator<StandardConsolidator>(listOfMatrixFields);
                HashSet<Field> listOfConsolidateFields = consolidator.Consolidate();

                if (listOfConsolidateFields.Count == 0)
                    ModernDialog.ShowMessage("No results were found.", "Consolidation", MessageBoxButton.OK);
                else
                {
                    MatrixVisualizationGrid.ItemsSource = null;
                    MatrixVisualizationGrid.ItemsSource = listOfConsolidateFields;
                }
            }
        }
        
             
        /// <summary>
        /// Resets the Visualization Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConsolidationResetClick(object sender, RoutedEventArgs e)
        {
            MatrixVisualizationGrid.ItemsSource = null;
            MatrixVisualizationGrid.ItemsSource = MainWindow.MatrixSelection.MatrixFields;
            ListOfChoosenProcessModels.Clear();

            EventListBox.UnselectAll();
            QuicksearchTextField.Text = "";
            NumberOfEventsSlider.Value = 1;
            ConsolidationListBox.UnselectAll();

        }

        /// <summary>
        /// Unchecked every checkbox in the MatrixVisualizationsGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Markus Holznagel</author>
        private void UncheckAllContextClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in MatrixVisualizationGrid.Items)
            {
                var container = MatrixVisualizationGrid.ItemContainerGenerator.ContainerFromItem(item) as FrameworkElement;
                var checkBox = MatrixVisualizationGrid.ItemTemplate.FindName("CompareCheckbox", container) as CheckBox;

                if (checkBox != null)
                    checkBox.IsChecked = false;
            }
        }

        #endregion
    }   
}