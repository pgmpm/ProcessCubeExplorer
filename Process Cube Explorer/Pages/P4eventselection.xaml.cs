using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.Database;
using pgmpm.MainV2.GUI;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection;
using pgmpm.MatrixSelection.Dimensions;

namespace pgmpm.MainV2.Pages
{
    /// <summary>
    /// Interaction logic for P4eventselection.xaml
    /// </summary>
    public partial class P4eventselection : IContent
    {
        private int _lastDbConnectionHash;
        private List<EventDimensionSelector> _listOfDimensionSelectors;

        public P4eventselection()
        {
            InitializeComponent();
        }

        #region Navigation-Events

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {
            DBInformationButton.ToolTip = MainWindow.ConnectionName;
            InterfaceHelpers.CurrentPage = e.Source.OriginalString;

            if (_lastDbConnectionHash != DBWorker.MetaData.GetHashCode())
            {
                BuildEventDimensionsSelectors();
                _lastDbConnectionHash = DBWorker.MetaData.GetHashCode();
            }
        }

        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (InterfaceHelpers.CheckIfNavigationIsAllowed(e.Source.OriginalString) == false)
                e.Cancel = true;
        }
        #endregion

        #region Buttons
        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/P3dimensionselection.xaml", null);
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

        private void ContinueClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/P5configuration.xaml", null);
        }

        #endregion

        #region Init selectors
        /// <summary>
        /// Builds an EventDimensionSelector for each event dimension.
        /// </summary>
        private void BuildEventDimensionsSelectors()
        {
            //reset gui
            EventDimensionList.Children.Clear();

            //clear the the internal representation of the selected dimensions
            EventSelectionModel.GetInstance().Clear();

            _listOfDimensionSelectors = new List<EventDimensionSelector>();

            if (DBWorker.MetaData == null || DBWorker.MetaData.ListOfEventDimensions == null || DBWorker.MetaData.ListOfEventDimensions.Count == 0)
            {
                TextBlock textBlock = new TextBlock {Text = "No events available."};
                EventDimensionList.Children.Add(textBlock);
                return;
            }

            //build EventDimensionSelector for each event dimension
            for (int i = 0; i < DBWorker.MetaData.ListOfEventDimensions.Count; i++)
            {
                //skip empty dimension
                if (DBWorker.MetaData.ListOfEventDimensions[i].IsEmptyDimension)
                    continue;

                //create EventDimensionSelector for dimension
                int axis = i+1;
                EventDimensionSelector dimSelector = new EventDimensionSelector(axis);
                dimSelector.Init(DBWorker.MetaData.ListOfEventDimensions, DBWorker.MetaData.ListOfEventDimensions[i]);
                _listOfDimensionSelectors.Add(dimSelector);
                
                //add selector to panel
                EventDimensionList.Children.Add(dimSelector.DimensionSelectorGrid);
            }

            //build internal representation of the selected dimensions
            foreach (EventDimensionSelector dimSelector in _listOfDimensionSelectors)
            {
                int axis = dimSelector.Axis;
                Dimension dim = dimSelector.ShowedDimension;
                int levelDepth = dimSelector.GetSelectedLevelDepth();
                int aggregationDepth = dimSelector.GetSelectedAggregationDepth();
                List<DimensionContent> dimContent = dimSelector.GetSelectedDimensionContent();
                EventSelectionModel.GetInstance().AddSelectedDimension(new SelectedDimension(axis, dim, levelDepth, aggregationDepth - 1, true, dimContent, true));
            }

            InitEventListener();
        }

        /// <summary>
        /// Set event listener on each EventDimensionSelector. 
        /// </summary>
        private void InitEventListener()
        {
            foreach (EventDimensionSelector dimSelector in _listOfDimensionSelectors)
            {
                dimSelector.DimensionComboBox.SelectionChanged += OnDimensionSelectionChanged;
                dimSelector.DimensionLevelComboBox.SelectionChanged += OnDimensionLevelSelectionChanged;
                dimSelector.DimensionAggregationComboBox.SelectionChanged += OnAggregationSelectionChanged;
                dimSelector.DimensionContentSearch.TextChanged += OnQuickSearchTextChanged;
                dimSelector.DimensionContentFilter.SelectionChanged += OnDimensionContentSelectionChanged;
                dimSelector.SelectAllButton.Click += OnSelectAll;
                dimSelector.SelectNoneButton.Click += OnSelectNone;
            }
        }
        #endregion

        #region Event handler
        private void OnDimensionSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            // EventDimensionSelector which changed its dimension
            EventDimensionSelector affectedDimensionSelector = GetDimensionSelectorByComboBox(sender as ComboBox);

            // previously selected dimension
            Dimension previouslySelectedDimension = affectedDimensionSelector.ShowedDimension;

            //new selected dimension
            ComboBox comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                Dimension newSelectedDimension = (Dimension)comboBox.SelectedItem;

                //update the affected DimensionSelector
                affectedDimensionSelector.UpdateByDimensionChange(newSelectedDimension);

                //look if the new selected dimension is also
                //showed by another DimensionSelector
                if (IsAlreadySelected(affectedDimensionSelector, newSelectedDimension))
                {
                    // get all EventDimensionSelectors which showing the new selected dimension
                    List<EventDimensionSelector> showingSelectors = GetShowingDimensionSelectors(newSelectedDimension);

                    foreach (EventDimensionSelector dimSelector in showingSelectors)
                    {
                        //find the DimensionSelector which shows the new selected dimension and is
                        //not the showingDimensionSelector
                        if (dimSelector != affectedDimensionSelector)
                        {
                            //set dimension to the previously selected dimension of the affected DimensionSelector
                            dimSelector.DimensionComboBox.SelectedItem = previouslySelectedDimension;
                            updateInternalEventSelectionModel(dimSelector.Axis, dimSelector.ShowedDimension, dimSelector.GetSelectedLevelDepth(), dimSelector.GetSelectedAggregationDepth(), dimSelector.GetSelectedDimensionContent());
                        }
                    }
                }
            }

            //update internal event selection model
            updateInternalEventSelectionModel(affectedDimensionSelector.Axis, affectedDimensionSelector.ShowedDimension, affectedDimensionSelector.GetSelectedLevelDepth(), affectedDimensionSelector.GetSelectedAggregationDepth(), affectedDimensionSelector.GetSelectedDimensionContent());
        }

        private void OnDimensionLevelSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EventDimensionSelector affectedDimensionSelector = GetDimensionSelectorByComboBox(sender as ComboBox);
            affectedDimensionSelector.UpdateByLevelChange();

            //update internal event selection model
            updateInternalEventSelectionModel(affectedDimensionSelector.Axis, affectedDimensionSelector.ShowedDimension, affectedDimensionSelector.GetSelectedLevelDepth(), affectedDimensionSelector.GetSelectedAggregationDepth(), affectedDimensionSelector.GetSelectedDimensionContent());
        }

        private void OnAggregationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EventDimensionSelector affectedDimensionSelector = GetDimensionSelectorByComboBox(sender as ComboBox);

            //update internal event selection model
            updateInternalEventSelectionModel(affectedDimensionSelector.Axis, affectedDimensionSelector.ShowedDimension, affectedDimensionSelector.GetSelectedLevelDepth(), affectedDimensionSelector.GetSelectedAggregationDepth(), affectedDimensionSelector.GetSelectedDimensionContent());
        }

        private void OnQuickSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            EventDimensionSelector affectedDimensionSelector = GetDimensionSelectorByTextBox(sender as TextBox);

            affectedDimensionSelector.UpdateByQuickSearchChanged();

            //update internal event selection model
            updateInternalEventSelectionModel(affectedDimensionSelector.Axis, affectedDimensionSelector.ShowedDimension, affectedDimensionSelector.GetSelectedLevelDepth(), affectedDimensionSelector.GetSelectedAggregationDepth(), affectedDimensionSelector.GetSelectedDimensionContent());
        }

        private void OnDimensionContentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            EventDimensionSelector affectedDimensionSelector = GetDimensionSelectorByListBox(sender as ListBox);

            //update internal event selection model
            updateInternalEventSelectionModel(affectedDimensionSelector.Axis, affectedDimensionSelector.ShowedDimension, affectedDimensionSelector.GetSelectedLevelDepth(), affectedDimensionSelector.GetSelectedAggregationDepth(), affectedDimensionSelector.GetSelectedDimensionContent());

        }

        private void OnSelectAll(object sender, RoutedEventArgs e)
        {
            EventDimensionSelector affectedDimensionSelector = GetDimensionSelectorByButton(sender as Button);
            affectedDimensionSelector.DimensionContentFilter.SelectAll();
        }

        private void OnSelectNone(object sender, RoutedEventArgs e)
        {
            EventDimensionSelector affectedDimensionSelector = GetDimensionSelectorByButton(sender as Button);
            affectedDimensionSelector.DimensionContentFilter.UnselectAll();
        }
        #endregion

        #region Getter
        private EventDimensionSelector GetDimensionSelectorByButton(Button button)
        {
            return _listOfDimensionSelectors.FirstOrDefault(dimSelector =>
                Equals(dimSelector.SelectAllButton, button) || Equals(dimSelector.SelectNoneButton, button));
        }

        private EventDimensionSelector GetDimensionSelectorByComboBox(ComboBox comboBox)
        {
            return _listOfDimensionSelectors.FirstOrDefault(dimSelector =>
                Equals(dimSelector.DimensionComboBox, comboBox) || Equals(dimSelector.DimensionLevelComboBox, comboBox) || Equals(dimSelector.DimensionAggregationComboBox, comboBox));
        }

        private EventDimensionSelector GetDimensionSelectorByListBox(ListBox listBox)
        {
            return _listOfDimensionSelectors.FirstOrDefault(dimSelector =>
                Equals(dimSelector.DimensionContentFilter, listBox));
        }

        private EventDimensionSelector GetDimensionSelectorByTextBox(TextBox textBox)
        {
            return _listOfDimensionSelectors.FirstOrDefault(dimSelector =>
                Equals(dimSelector.DimensionContentSearch, textBox));
        }

        private List<EventDimensionSelector> GetShowingDimensionSelectors(Dimension dimension)
        {
            List<EventDimensionSelector> foundSelectors = new List<EventDimensionSelector>();

            foreach (EventDimensionSelector dimSelector in _listOfDimensionSelectors)
            {
                if (dimSelector.DimensionComboBox.SelectedItem == dimension)
                {
                    foundSelectors.Add(dimSelector);
                }
            }
            return foundSelectors;
        }
        
        #endregion

        #region Helper
        /// <summary>
        /// Looks if the given dimension is also selected by another selector as the given one.
        /// <param name="showingDimensionSelector">Selector which currently show the dimension.</param>
        /// <param name="dimension">Dimension to look for.</param>
        /// </summary>
        private bool IsAlreadySelected(EventDimensionSelector showingDimensionSelector, Dimension dimension)
        {
            foreach (EventDimensionSelector dimSelector in _listOfDimensionSelectors)
            {
                if (dimSelector.DimensionComboBox.SelectedItem == dimension && dimSelector != showingDimensionSelector)
                {
                    return true;
                }
            }
            return false;
        }

        private void updateInternalEventSelectionModel(int axis, Dimension dimension, int levelDepth, int aggregatioDepth, List<DimensionContent> dimensionContent)
        {
            EventSelectionModel.GetInstance().Update(axis, dimension, levelDepth, aggregatioDepth, dimensionContent);
        }
        #endregion
    }
}