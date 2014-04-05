using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;


namespace pgmpm.MainV2.Pages
{
    /// <summary>
    /// Interaction logic for P7consolidation.xaml
    /// </summary>
    public partial class P7consolidation : IContent
    {
        Field _currentlySelectedField;
        private UniformGrid _uniformgrid;
        private HashSet<string> _listOfSelectedOptions;
        public List<string> EventUniqueList;

        public P7consolidation()
        {
            InitializeComponent();

            FillListboxWithEvents();
            InitializeControls();

        }

        #region Navigation
        void IContent.OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        void IContent.OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        void IContent.OnNavigatedTo(NavigationEventArgs e)
        {
            DBConnectButton.ToolTip = MainWindow.ConnectionName;
            InterfaceHelpers.CurrentPage = e.Source.OriginalString;
        }

        void IContent.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (InterfaceHelpers.CheckIfNavigationIsAllowed(e.Source.OriginalString) == false)
                e.Cancel = true;
        }
        #endregion

        #region Buttons
        private void Previous(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/P5configuration.xaml", null);
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/Settings.xaml", null);
        }
        private void OpenNote(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/notes.xaml", null);
        }

        private void DBInformation(object sender, RoutedEventArgs e)
        {
            ModernDialog.ShowMessage(MainWindow.ConnectionName, "Database information", MessageBoxButton.OK);
        }

        private void Continue(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/P8results.xaml", null);
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
                _currentlySelectedField = ((sender as Button).Tag as Field);
                try
                {
                    ModernWindow viewer = new Viewer.Viewer(_currentlySelectedField);
                    viewer.Owner = Application.Current.MainWindow;
                    viewer.Show();
                }
                catch (ArgumentNullException ex)
                {
                    ErrorHandling.ReportErrorToUser("Error: " + ex.Message);
                }
            }
        }

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
                int numberOfColumns = MainWindow.MatrixSelection.SelectedDimensions[1].SelectedFilters.Any() ? MainWindow.MatrixSelection.SelectedDimensions[1].SelectedFilters.Count() : 1;
                if (_uniformgrid != null)
                    _uniformgrid.Columns = numberOfColumns;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentOutOfRangeException)
                    ErrorHandling.ReportErrorToUser("Error: " + ex.Message);
                else
                    throw;
            }
        }

        /// <summary>
        /// Calls the static method of ConsolidationHelper and update the matrix-fields.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Christopher Licht</author>
        private void StartConsolidationClick(object sender, RoutedEventArgs e)
        {
            HashSet<Field> _ListOfNetWithSelectedOptions = new HashSet<Field>();
            HashSet<String> listOfEvents = new HashSet<String>();
            HashSet<Field> listOfMatrixFields = new HashSet<Field>();
            int andOrSelection = -1;
            _listOfSelectedOptions = new HashSet<string>();

            andOrSelection = AndRadioButton.IsChecked == true ? 0 : 1;

            foreach (String selectedListViewItem in ConsolidationListBox.SelectedItems)
                _listOfSelectedOptions.Add(selectedListViewItem);

            foreach (String selectedEvent in EventListBox.SelectedItems)
                listOfEvents.Add(selectedEvent);

            foreach (var matrixField in MainWindow.MatrixSelection.MatrixFields)
                listOfMatrixFields.Add(matrixField);

            _ListOfNetWithSelectedOptions = ConsolidationHelper.StartConsolidation(listOfMatrixFields, _listOfSelectedOptions, listOfEvents, andOrSelection,(int) NumberOfEvents.Value);

            if (_ListOfNetWithSelectedOptions.Count == 0)
                ModernDialog.ShowMessage("No results were found.", "Attention", MessageBoxButton.OK);
            else
            {
                MatrixVisualizationGrid.ItemsSource = null;
                MatrixVisualizationGrid.ItemsSource = _ListOfNetWithSelectedOptions;
            }
        }

        /// <summary>
        /// Initialize the Controls
        /// </summary>
        /// <author>Bernhard Bruns, Christopher Licht, Moritz Eversmann</author>
        private void InitializeControls()
        {
            ConsolidationListBox.Style = (Style)(Application.Current.TryFindResource("ListBox"));
            ConsolidationListBox.ItemContainerStyle = (Style)Application.Current.TryFindResource("ListBoxItemWithCheckbox");
            ConsolidationListBox.SelectionMode = SelectionMode.Extended;

            EventListBox.Style = (Style)(Application.Current.TryFindResource("ListBox"));
            EventListBox.ItemContainerStyle = (Style)Application.Current.TryFindResource("ListBoxItemWithCheckbox");
            EventListBox.SelectionMode = SelectionMode.Extended;

            ConsolidationListBox.Items.Add("Loop");
            ConsolidationListBox.Items.Add("Parallelism");
            ConsolidationListBox.Items.Add("Events");
            ConsolidationListBox.Items.Add("Min. Number of Events");

            EventListBox.Visibility = Visibility.Visible;
            EventPanel.Visibility = Visibility.Collapsed;
            SliderPanel.Visibility = Visibility.Collapsed;
            AvailableEventsHeader.Visibility = Visibility.Visible;
            StartConsolidation.IsEnabled = false;
        }

        /// <summary>
        /// Set the StartConsolidationButton and listbox with events to visible.
        /// </summary>
        /// <param name="sender">ListBox with available options.</param>
        /// <param name="e"></param>
        /// <author>Christopher Licht, Bernhard Bruns, Moritz Eversmann</author>
        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StartConsolidation.IsEnabled = ConsolidationListBox.SelectedItems.Count > 0;
            EventPanel.Visibility = ConsolidationListBox.SelectedItems.Contains("Events") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            if (ConsolidationListBox.SelectedItems.Contains("Min. Number of Events"))
                SliderPanel.Visibility = System.Windows.Visibility.Visible;
            else
                SliderPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author>Bernhard Bruns</author>
        private void FillListboxWithEvents()
        {
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
        /// Filters a filterListBox by a searchstring
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Bernhard Bruns</author>
        private void QuickSearch(object sender, TextChangedEventArgs e)
        {
            EventListBox.Items.Filter = delegate(object ownObject)
            {
                String enteredValue = (String)ownObject;
                String finalEnteredValue = enteredValue.ToUpper();
                if (String.IsNullOrEmpty(finalEnteredValue))
                    return false;
                int index = finalEnteredValue.IndexOf(QuicksearchTextField.Text.ToUpper(), 0, StringComparison.Ordinal);
                return (index > -1);
            };
        }
        #endregion
    }
}

