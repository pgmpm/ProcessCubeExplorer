using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.Database;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection.Dimensions;
using pgmpm.MatrixSelection.Fields;

namespace pgmpm.MainV2.Pages
{
    /// <summary>
    /// Interaction logic for P3dimensionselection.xaml
    /// </summary>
    public partial class P3dimensionselection : IContent
    {
        #region Functionality

        int _cols;
        int[] _dimensionOnAxis;
        int _lastDbConnectionHash;
        List<ComboBox> _listOfAxisComboBoxes;
        List<ListBox> _listOfFilterComboBoxes;
        List<ComboBox> _listOfLevelComboBoxes;
        List<Boolean> _listOfQuicksearchFirstSearch;
        List<TextBox> _listOfQuicksearchTextfields;
        List<Button> _listOfSelectAllButtons;
        List<Button> _listOfSelectNoneButtons;

        private UniformGrid _uniformgrid;
        private bool _updateMatrixFlag
        {
            get { return updateMatrixFlag; }
            set
            {
                updateMatrixFlag = value;
                UpdateMatrixButton.IsEnabled = value;
            }
        }
        private bool updateMatrixFlag = true;
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
                ShowSelectors();
                InitializeMatrix();
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
            NavigationCommands.GoToPage.Execute("/Pages/P2metadata.xaml", null);
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
            if (_updateMatrixFlag)
            {
                MessageBoxResult answer = ModernDialog.ShowMessage("Would you like to update the matrix before proceeding ?", "Filters changed", MessageBoxButton.OKCancel);
                if (answer == MessageBoxResult.OK)
                {
                    UpdateMatrix();
                }
                else if (answer == MessageBoxResult.Cancel) return;
            }
            NavigationCommands.GoToPage.Execute("/Pages/P4eventselection.xaml", null);
        }


        private void UpdateMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateMatrix();
        }

        #endregion

        public P3dimensionselection()
        {
            InitializeComponent();
        }

        // --------------------- Functionality ---------------------

        private void ShowSelectors()
        {
            // initialize lists of interface-objects
            _listOfAxisComboBoxes = new List<ComboBox>();
            _listOfLevelComboBoxes = new List<ComboBox>();
            _listOfFilterComboBoxes = new List<ListBox>();
            _listOfSelectAllButtons = new List<Button>();
            _listOfSelectNoneButtons = new List<Button>();
            _listOfQuicksearchTextfields = new List<TextBox>();
            _listOfQuicksearchFirstSearch = new List<Boolean>();

            DimensionList.Children.Clear();

            // create and view Grids, Separators and Headings
            DimensionList.Children.Add(InterfaceHelpers.CreateTextBlock("DIMENSIONS", "Heading2", 0, 0, 0, 8));
            DimensionList.Children.Add(CreateSelectorGrid(1));
            DimensionList.Children.Add(InterfaceHelpers.CreateSeparator());
            DimensionList.Children.Add(CreateSelectorGrid(2));
            DimensionList.Children.Add(InterfaceHelpers.CreateSeparator());

            DimensionList.Children.Add(InterfaceHelpers.CreateTextBlock("FILTERS", "Heading2", 0, 0, 0, 8));

            for (var filternumber = 3; filternumber < DBWorker.MetaData.ListOfFactDimensions.Count; filternumber++)
            {
                DimensionList.Children.Add(CreateSelectorGrid(filternumber));
                DimensionList.Children.Add(InterfaceHelpers.CreateSeparator());
            }
        }

        /// <summary>
        /// Creates a grid with dimension-, level- and filter-selection to be displayed right of the matrix.
        /// </summary>
        /// <param name="number">Number of the dimension (starting from 1)</param>
        /// <returns>A Grid that should be added to the interface.</returns>
        /// <author>Jannik Arndt</author>
        private Grid CreateSelectorGrid(int number)
        {
            const double column1Width = 30;
            const double column2Width = 130;
            const double column3Width = 10;
            const double column4Width = 130;
            const double comboBoxWidth = column2Width - 10;
            const double searchBoxWidth = column4Width - 10;
            const double filterListWidth = column4Width - 10;
            const double filterListHeight = 80;
            const double labelTopMargin = 8;

            // Create basic SelectorGrid with four columns
            var selectorGrid = new Grid { Name = "selectorGrid" + number };
            var col1 = new ColumnDefinition { Width = new GridLength(column1Width) };
            var col2 = new ColumnDefinition { Width = new GridLength(column2Width) };
            var col3 = new ColumnDefinition { Width = new GridLength(column3Width) };
            var col4 = new ColumnDefinition { Width = new GridLength(column4Width) };
            selectorGrid.ColumnDefinitions.Add(col1);
            selectorGrid.ColumnDefinitions.Add(col2);
            selectorGrid.ColumnDefinitions.Add(col3);
            selectorGrid.ColumnDefinitions.Add(col4);

            // Number on the left
            selectorGrid.Children.Add(InterfaceHelpers.CreateTextBlock(number + "", "Title", gridColumn: 0));

            // Dimension and Level-Selectors in the middle
            var panelMiddle = new StackPanel();
            panelMiddle.SetValue(Grid.ColumnProperty, 1);
            panelMiddle.HorizontalAlignment = HorizontalAlignment.Left;
            panelMiddle.VerticalAlignment = VerticalAlignment.Top;
            panelMiddle.Children.Add(InterfaceHelpers.CreateLabel("Dimension"));
            var axisComboBox = InterfaceHelpers.CreateComboBox("axis" + number + "_selector", width: comboBoxWidth);
            axisComboBox.SelectionChanged += OnDimensionComboBoxSelectionChanged;
            panelMiddle.Children.Add(axisComboBox);

            panelMiddle.Children.Add(InterfaceHelpers.CreateLabel("Level", top: labelTopMargin));
            var levelComboBox = InterfaceHelpers.CreateComboBox("level" + number + "_selector", width: comboBoxWidth);
            levelComboBox.SelectionChanged += OnLevelComboBoxSelectionChanged;
            panelMiddle.Children.Add(levelComboBox);

            selectorGrid.Children.Add(panelMiddle);

            // Filters on the right
            var panelRight = new StackPanel();
            panelRight.SetValue(Grid.ColumnProperty, 3);
            panelRight.HorizontalAlignment = HorizontalAlignment.Left;
            panelRight.VerticalAlignment = VerticalAlignment.Top;
            panelRight.Children.Add(InterfaceHelpers.CreateLabel("Filter"));
            var quickSearchTextBox = InterfaceHelpers.CreateTextBox("txtSearchAxis" + number);
            quickSearchTextBox.TextChanged += QuickSearch;

            quickSearchTextBox.Width = searchBoxWidth;
            panelRight.Children.Add(quickSearchTextBox);
            //  Filterlist
            var filterListBox = new ListBox
            {
                Name = "filter" + number + "_selector",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = filterListWidth,
                Height = filterListHeight,
                Style = Application.Current.TryFindResource("ListBox") as Style,
                ItemContainerStyle = Application.Current.TryFindResource("ListBoxItemWithCheckbox") as Style,
                SelectionMode = SelectionMode.Extended
            };

            filterListBox.SelectionChanged += OnFilterListBoxSelectionChanged;
            panelRight.Children.Add(filterListBox);

            // Select all / none
            var selectAllNone = new StackPanel { Orientation = Orientation.Horizontal };
            var selectAllButton = InterfaceHelpers.CreateButton("selectAll" + number, "Select all", "PlainTextButton");
            selectAllButton.Click += FilterListBoxSelectAll;
            selectAllNone.Children.Add(selectAllButton);
            selectAllNone.Children.Add(InterfaceHelpers.CreateLabel("/"));
            var selectNoneButton = InterfaceHelpers.CreateButton("selectNone" + number, "none", "PlainTextButton");
            selectNoneButton.Click += FilterListBoxSelectNone;
            selectAllNone.Children.Add(selectNoneButton);
            panelRight.Children.Add(selectAllNone);

            selectorGrid.Children.Add(panelRight);

            _listOfAxisComboBoxes.Add(axisComboBox);
            _listOfLevelComboBoxes.Add(levelComboBox);
            _listOfFilterComboBoxes.Add(filterListBox);
            _listOfSelectAllButtons.Add(selectAllButton);
            _listOfSelectNoneButtons.Add(selectNoneButton);
            _listOfQuicksearchTextfields.Add(quickSearchTextBox);
            _listOfQuicksearchFirstSearch.Add(true);

            return selectorGrid;
        }

        /// <summary>
        /// Initialize the MatrixSelection-object, Reset the ComboBoxes, view the Dimensions in the ComboBoxes and update the Matrix.
        /// </summary>
        /// <author>Jannik Arndt</author>
        public void InitializeMatrix()
        {
            _dimensionOnAxis = new int[_listOfAxisComboBoxes.Count()];

            MainWindow.MatrixSelection.Init(_listOfAxisComboBoxes.Count());

            ResetComboBoxes(_listOfAxisComboBoxes);
            ResetComboBoxes(_listOfLevelComboBoxes);
            ResetListBoxes(_listOfFilterComboBoxes);

            _updateMatrixFlag = false;
            ShowDimensionsInComboBox(_listOfAxisComboBoxes);
            _updateMatrixFlag = true;

            UpdateMatrix();
        }

        /// <summary>
        /// Returns a list of the selected dimension in the data selection
        /// </summary>
        /// <returns></returns>
        /// <author>Bernhard Bruns</author>
        private List<Dimension> GetSelectedFactDimensions()
        {
            List<Dimension> listSelectedDimensions = new List<Dimension>();
            List<Dimension> listDimensions = DBWorker.MetaData.ListOfFactDimensions;

            for (int axisNumber = 0; axisNumber < listDimensions.Count - 1; axisNumber++)
            {
                Dimension dimension = (Dimension)_listOfAxisComboBoxes[axisNumber].SelectedItem;

                if (dimension != null && dimension.IsEmptyDimension == false)
                    listSelectedDimensions.Add(dimension);
            }
            return listSelectedDimensions;
        }

        /// <summary>
        /// This fills the given ComboBox with the names of ALL ListOfDimensions and selects the one specified by parameter dimension. 
        /// This is called after the MetaWorker is built.
        /// </summary>
        /// <param name="boxes"></param>
        /// <author>Jannik Arndt</author>
        private void ShowDimensionsInComboBox(List<ComboBox> boxes)
        {
            List<Dimension> list = DBWorker.MetaData.ListOfFactDimensions;
            for (int dimensionNumber = 0; dimensionNumber < list.Count() && dimensionNumber < boxes.Count(); dimensionNumber++)
            {
                _listOfAxisComboBoxes[dimensionNumber].ItemsSource = list;
                _listOfAxisComboBoxes[dimensionNumber].DisplayMemberPath = "Dimensionname";
                _listOfAxisComboBoxes[dimensionNumber].SelectedIndex = dimensionNumber + 1;
                _dimensionOnAxis[dimensionNumber] = dimensionNumber + 1;
            }
        }

        /// <summary>
        /// Resets the ItemsSource of a given list of ComboBoxes
        /// </summary>
        /// <param name="boxes"></param>
        /// <author>Jannik Arndt</author>
        private void ResetComboBoxes(IEnumerable<ComboBox> boxes)
        {
            foreach (ComboBox comboBox in boxes)
                comboBox.ItemsSource = null;
        }

        /// <summary>
        /// Resets the ItemsSource of a given list of ListBoxes
        /// </summary>
        /// <param name="boxes"></param>
        /// <author>Jannik Arndt</author>
        private void ResetListBoxes(IEnumerable<ListBox> boxes)
        {
            foreach (ListBox listBox in boxes)
                listBox.ItemsSource = null;
        }

        /// <summary>
        /// When you change the dimension in one of the ComboBoxes, this is being called. It loads the dimension that is stored in the selected ListItem and loads its levels.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Jannik Arndt</author>
        private void OnDimensionComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the dimension that is now selected
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                Dimension selectedDimension = (Dimension)comboBox.SelectedItem;
                // Get the right ID from the selectors-list and assign various things to the corresponding selectors.
                int senderId = _listOfAxisComboBoxes.IndexOf(comboBox);

                if (selectedDimension != null && MainWindow.MatrixSelection.SelectedDimensions.Count() >= senderId)
                {
                    // Update internal data model
                    MainWindow.MatrixSelection.SelectedDimensions[senderId].Axis = senderId;
                    MainWindow.MatrixSelection.SelectedDimensions[senderId].Dimension = selectedDimension;
                    //MainWindow.MatrixSelection.SelectedDimensions[SenderID].IsActivePath = (!SelectedDimension.IsEmptyDimension);
                    // Connect the ComboBox to the list of level steps, display the "ToTable" attribute and select the first item
                    _listOfLevelComboBoxes[senderId].ItemsSource = selectedDimension.GetLevel();
                    _listOfLevelComboBoxes[senderId].DisplayMemberPath = "Level";
                    _listOfLevelComboBoxes[senderId].SelectedIndex = _listOfLevelComboBoxes[senderId].Items.Count - 1;
                    // If needed, swap ListOfDimensions with another ComboBox
                    if (!selectedDimension.IsEmptyDimension)
                    {
                        ComboBox swapCb = CheckIfDimensionIsAlreadySelected(selectedDimension, comboBox);
                        if (swapCb != null)
                            swapCb.SelectedIndex = _dimensionOnAxis[senderId];
                    }

                    // Update which Dimension is displayed on which axis
                    _dimensionOnAxis[senderId] = comboBox.SelectedIndex;

                    DBWorker.SelectedDimensions = GetSelectedFactDimensions();
                }
            }
        }

        /// <summary>
        /// Go through all Axis-Selector-ComboBoxes except the given one and check, if the dimension is already displayed somewhere
        /// </summary>
        /// <param name="dimension">The dimension that this method looks for</param>
        /// <param name="except">If this ComboBox displays the dimension, it is being ignored</param>
        /// <returns></returns>
        /// <author>Jannik Arndt</author>
        public ComboBox CheckIfDimensionIsAlreadySelected(Dimension dimension, ComboBox except)
        {
            return _listOfAxisComboBoxes.FirstOrDefault(comboBox => comboBox.SelectedItem == dimension && !comboBox.Equals(except));
        }

        /// <summary>
        /// Same as above: If you change the level in one of the ComboBoxes, this loads the corresponding filters and displays them.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Jannik Arndt</author>
        private void OnLevelComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the level that is now selected
            var comboBox = sender as ComboBox;
            if (comboBox != null)
            {
                Dimension selectedLevel = (Dimension)comboBox.SelectedItem;

                if (selectedLevel != null)
                {
                    // Get the right ID from the selectors-list an assign various things to the corresponding selectors.
                    int senderId = _listOfLevelComboBoxes.IndexOf(comboBox);
                    if (MainWindow.MatrixSelection.SelectedDimensions.Count() > senderId && senderId >= 0)
                    {
                        MainWindow.MatrixSelection.SelectedDimensions[senderId].LevelDepth = _listOfLevelComboBoxes[senderId].SelectedIndex + 1;
                        _listOfFilterComboBoxes[senderId].ItemsSource = selectedLevel.DimensionContentsList;
                        _listOfFilterComboBoxes[senderId].DisplayMemberPath = "content";
                        _listOfFilterComboBoxes[senderId].SelectAll();
                        if (selectedLevel.FromConstraint == "MPMALLAGGREGATION")
                        {
                            MainWindow.MatrixSelection.SelectedDimensions[senderId].IsAllLevelSelected = true;
                        }
                        else
                        {
                            MainWindow.MatrixSelection.SelectedDimensions[senderId].IsAllLevelSelected = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This is called everytime the filters for a dimension are changed. It saves the selection to the MatrixSelection-Object and updates the Matrix.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Jannik Arndt</author>
        private void OnFilterListBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get the right ID from the selectors-list and assign various things to the corresponding selectors.
            int senderId = _listOfFilterComboBoxes.IndexOf((sender as ListBox));

            if (MainWindow.MatrixSelection.SelectedDimensions.Count() > senderId && senderId >= 0)
            {
                MainWindow.MatrixSelection.SelectedDimensions[senderId].SelectedFilters.Clear();

                // write the items into the Preview
                foreach (DimensionContent dimensionContent in ((ListBox)sender).SelectedItems)
                {
                    MainWindow.MatrixSelection.SelectedDimensions[senderId].SelectedFilters.Add(dimensionContent);
                }
                // if not all items are selected, copy the MatrixPreview-List into the SelectedDimensions-List
                MainWindow.MatrixSelection.SelectedDimensions[senderId].AreFiltersSelected = _listOfFilterComboBoxes[senderId].SelectedItems.Count != _listOfFilterComboBoxes[senderId].Items.Count;
            }

            // If this is true, data needs to be reloaded from DatabaseConnection, otherwise cached data may be used
            MainWindow.MatrixSelection.SelectionHasChangedSinceLastLoading = true;
            _updateMatrixFlag = true;
            ////if (_updateMatrixFlag)
            //    UpdateMatrix();
        }

        /// <summary>
        /// This is called from the button below the filter lists, it figures out the corresponding list an then selects all of the items in that list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Jannik Arndt</author>
        private void FilterListBoxSelectAll(object sender, RoutedEventArgs e)
        {
            int senderId = _listOfSelectAllButtons.IndexOf((sender as Button));
            if (_listOfFilterComboBoxes.Count() > senderId && senderId >= 0)
                _listOfFilterComboBoxes[senderId].SelectAll();
        }

        /// <summary>
        /// This is called from the button below the filter lists, it figures out the corresponding list an then UNselects all of the items in that list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Jannik Arndt</author>
        private void FilterListBoxSelectNone(object sender, RoutedEventArgs e)
        {
            int senderId = _listOfSelectNoneButtons.IndexOf((sender as Button));
            if (_listOfFilterComboBoxes.Count() > senderId && senderId >= 0)
                _listOfFilterComboBoxes[senderId].UnselectAll();
        }


        /// <summary>
        /// This updates the binding to the MatrixPreviewGrid (wpf) by letting the MatrixSelection-class calculate the fields.
        /// </summary>
        private void UpdateMatrix()
        {
            _updateMatrixFlag = false;
            //   SQLCreator.buildFieldSQLCache(MainWindow.MatrixSelection.SelectedDimensions);
            MainWindow.MatrixSelection.BuildMatrixFields();
            MatrixPreviewGrid.ItemsSource = MainWindow.MatrixSelection.GetFields();
            // if there is nothing to show, still show one field
            _cols = MainWindow.MatrixSelection.SelectedDimensions[0].SelectedFilters.Any()
                ? MainWindow.MatrixSelection.SelectedDimensions[0].SelectedFilters.Count()
                : 1;
            if (_uniformgrid != null)
                _uniformgrid.Columns = _cols;
        }


        private void AssignUniformGrid(object sender, RoutedEventArgs e)
        {
            _uniformgrid = sender as UniformGrid;
            if (_uniformgrid != null)
                _uniformgrid.Columns = _cols;

            if (_lastDbConnectionHash != DBWorker.MetaData.GetHashCode())
                UpdateMatrix();
        }

        /// <summary>
        /// Reacts to a click on a field in the matrix
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Jannik Arndt, Bernhard Bruns</author>
        private void FieldClick(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;

            if (sender.GetType() == typeof(Button))
            {
                Field field = (((Button)sender).Tag as Field);

                if (field != null)
                {
                    field.ContentCounterTextChanged = "\nEvents: " + DBWorker.GetCountFromSqlStatement(DBWorker.CreateEventCountSqlStatement(MainWindow.MatrixSelection.SelectedDimensions, field)) + " (Unique: " + DBWorker.GetCountFromSqlStatement(DBWorker.CreateUniqueEventCountSqlStatement(MainWindow.MatrixSelection.SelectedDimensions, field)) + ")";
                    field.ContentCounterTextChanged += "\nCases: " + DBWorker.GetCountFromSqlStatement(DBWorker.CreateCaseCountSqlStatement(MainWindow.MatrixSelection.SelectedDimensions, field));
                }
            }

            Cursor = Cursors.Arrow;
        }


        /// <summary>
        /// Filters a filterListBox by a searchstring
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Bernhard Bruns</author>
        private void QuickSearch(object sender, TextChangedEventArgs e)
        {

            int senderId = _listOfQuicksearchTextfields.IndexOf((sender as TextBox));

            if (_listOfQuicksearchFirstSearch[senderId])
            {
                _listOfFilterComboBoxes[senderId].UnselectAll();
                _listOfQuicksearchFirstSearch[senderId] = false;
            }


            _listOfFilterComboBoxes[senderId].Items.Filter = delegate(object obj)
            {
                DimensionContent dimensionContent = (DimensionContent)obj;

                foreach (DimensionContent dc in _listOfFilterComboBoxes[senderId].SelectedItems)
                    if (dc.Id == dimensionContent.Id)
                        return true;


                string value = dimensionContent.Content.ToUpper();
                if (String.IsNullOrEmpty(value))
                    return false;
                int index = value.IndexOf(_listOfQuicksearchTextfields[senderId].Text.ToUpper(), 0, StringComparison.Ordinal);

                return (index > -1);
            };
        }
    }
        #endregion
}