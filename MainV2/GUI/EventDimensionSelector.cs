using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection.Dimensions;

namespace pgmpm.MainV2.GUI
{
    /// <summary>
    /// This class summarize all GUI components which are used to show and select an event dimension.
    /// </summary>
    class EventDimensionSelector
    {
        #region constant
        //Define constants
        private const double Column1Width = 30;
        private const double Column2Width = 130;
        private const double Column3Width = 10;
        private const double Column4Width = 130;
        private const double ComboBoxWidth = Column2Width - 10;
        private const double SearchBoxWidth = Column4Width - 10;
        private const double FilterListWidth = Column4Width - 10;
        private const double FilterListHeight = 80;
        private const double LabelTopMargin = 8;
        #endregion

        #region properties
        //Define variables and properties
        private int _axis;

        public int Axis
        {
            get { return _axis; }
            set { _axis = value; }
        }

        private ComboBox _dimensionComboBox;

        public ComboBox DimensionComboBox
        {
            get { return _dimensionComboBox; }
            set { _dimensionComboBox = value; }
        }

        private ListBox _dimensionContentFilter;

        public ListBox DimensionContentFilter
        {
            get { return _dimensionContentFilter; }
            set { _dimensionContentFilter = value; }
        }

        private TextBox _dimensionContentSearch;

        public TextBox DimensionContentSearch
        {
            get { return _dimensionContentSearch; }
            set { _dimensionContentSearch = value; }
        }

        private ComboBox _dimensionLevelComboBox;

        public ComboBox DimensionLevelComboBox
        {
            get { return _dimensionLevelComboBox; }
            set { _dimensionLevelComboBox = value; }
        }

        private ComboBox _dimensionAggregationComboBox;

        public ComboBox DimensionAggregationComboBox
        {
            get { return _dimensionAggregationComboBox; }
            set { _dimensionAggregationComboBox = value; }
        }

        private Button _selectAllButton;

        public Button SelectAllButton
        {
            get { return _selectAllButton; }
            set { _selectAllButton = value; }
        }

        private Button _selectNoneButton;

        public Button SelectNoneButton
        {
            get { return _selectNoneButton; }
            set { _selectNoneButton = value; }
        }

        private Dimension _showedDimension;

        public Dimension ShowedDimension
        {
            get { return _showedDimension; }
            set { _showedDimension = value; }
        }

        public Grid DimensionSelectorGrid { get; set; }
        #endregion

        #region build selector
        //Constructor
        public EventDimensionSelector(int axis)
        {
            _axis = axis;

            // Create basic DimensionSelectorGrid with four columns
            DimensionSelectorGrid = new Grid { Name = "dimensionSelectorGrid" + axis };
            var col1 = new ColumnDefinition { Width = new GridLength(Column1Width) };
            var col2 = new ColumnDefinition { Width = new GridLength(Column2Width) };
            var col3 = new ColumnDefinition { Width = new GridLength(Column3Width) };
            var col4 = new ColumnDefinition { Width = new GridLength(Column4Width) };
            DimensionSelectorGrid.ColumnDefinitions.Add(col1);
            DimensionSelectorGrid.ColumnDefinitions.Add(col2);
            DimensionSelectorGrid.ColumnDefinitions.Add(col3);
            DimensionSelectorGrid.ColumnDefinitions.Add(col4);

            // Number on the left
            DimensionSelectorGrid.Children.Add(InterfaceHelpers.CreateTextBlock(axis + "", "Title", gridColumn: 0));

            // Create Dimension, Level and Aggregation combo boxes on the middle
            var panelMiddle = new StackPanel();
            panelMiddle.SetValue(Grid.ColumnProperty, 1);
            panelMiddle.HorizontalAlignment = HorizontalAlignment.Left;
            panelMiddle.VerticalAlignment = VerticalAlignment.Top;
            panelMiddle.Children.Add(InterfaceHelpers.CreateLabel("Dimension"));
            _dimensionComboBox = InterfaceHelpers.CreateComboBox("axis" + axis + "_selector", width: ComboBoxWidth);
            panelMiddle.Children.Add(_dimensionComboBox);

            panelMiddle.Children.Add(InterfaceHelpers.CreateLabel("Level", top: LabelTopMargin));
            _dimensionLevelComboBox = InterfaceHelpers.CreateComboBox("level" + axis + "_selector", width: ComboBoxWidth);
            panelMiddle.Children.Add(_dimensionLevelComboBox);

            panelMiddle.Children.Add(InterfaceHelpers.CreateLabel("Aggregation", top: LabelTopMargin));
            _dimensionAggregationComboBox = InterfaceHelpers.CreateComboBox("aggregation" + axis + "_selector", width: ComboBoxWidth);
            panelMiddle.Children.Add(_dimensionAggregationComboBox);

            DimensionSelectorGrid.Children.Add(panelMiddle);

            // Filters on the right
            var panelRight = new StackPanel();
            panelRight.SetValue(Grid.ColumnProperty, 3);
            panelRight.HorizontalAlignment = HorizontalAlignment.Left;
            panelRight.VerticalAlignment = VerticalAlignment.Top;
            panelRight.Children.Add(InterfaceHelpers.CreateLabel("Filter"));
            _dimensionContentSearch = InterfaceHelpers.CreateTextBox("txtSearchAxis" + axis);
            _dimensionContentSearch.Width = SearchBoxWidth;
            panelRight.Children.Add(_dimensionContentSearch);
            // Filterlist
            _dimensionContentFilter = new ListBox
            {
                Name = "filter" + axis + "_selector",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = FilterListWidth,
                Height = FilterListHeight,
                Style = Application.Current.TryFindResource("ListBox") as Style,
                ItemContainerStyle = Application.Current.TryFindResource("ListBoxItemWithCheckbox") as Style,
                SelectionMode = SelectionMode.Extended
            };
            panelRight.Children.Add(_dimensionContentFilter);

            // Select all / none
            var selectAllNone = new StackPanel { Orientation = Orientation.Horizontal };
            _selectAllButton = InterfaceHelpers.CreateButton("selectAll" + axis, "Select all", "PlainTextButton");
            selectAllNone.Children.Add(_selectAllButton);
            selectAllNone.Children.Add(InterfaceHelpers.CreateLabel("/"));
            _selectNoneButton = InterfaceHelpers.CreateButton("selectNone" + axis, "none", "PlainTextButton");
            selectAllNone.Children.Add(_selectNoneButton);
            panelRight.Children.Add(selectAllNone);

            DimensionSelectorGrid.Children.Add(panelRight);
        }

        /// <summary>
        /// Init selector with the given dimensions.
        /// <param name="allDimensions">All dimensions which the selector could show.</param>
        /// <param name="dimensionToShow">Current dimension to show.</param>
        /// </summary>
        internal void Init(List<Dimension> allDimensions, Dimension dimensionToShow)
        {
            ShowedDimension = dimensionToShow;

            //set dimension
            List<Dimension> temp = new List<Dimension>();
            Dimension emptyDimension = new Dimension("no selection", toTable: "no selection", isEmptyDimension: true);

            temp.Add(emptyDimension);
            temp.AddRange(allDimensions);

            DimensionComboBox.ItemsSource = temp;
            DimensionComboBox.DisplayMemberPath = "Dimensionname";
            DimensionComboBox.SelectedItem = temp[temp.IndexOf(dimensionToShow)];

            //set level
            DimensionLevelComboBox.ItemsSource = dimensionToShow.GetLevel();
            DimensionLevelComboBox.DisplayMemberPath = "Level";
            DimensionLevelComboBox.SelectedIndex = DimensionLevelComboBox.Items.Count - 1;//select highest level by default

            //set aggregation
            temp = new List<Dimension>();
            Dimension emptyLevel = new Dimension("empty level", toTable: "no aggregation", isEmptyDimension: true);

            temp.Add(emptyLevel);
            temp.AddRange(dimensionToShow.GetLevel());

            DimensionAggregationComboBox.Items.Clear();

            //exclude the all level dimension for aggregation level combobox
            foreach (Dimension dim in temp)
            {
                if (dim != temp.Last())
                {
                    DimensionAggregationComboBox.Items.Add(dim);
                }
            }

            DimensionAggregationComboBox.DisplayMemberPath = "Level";
            DimensionAggregationComboBox.SelectedIndex = 0;

            //set content
            var showedLevel = (Dimension)DimensionLevelComboBox.SelectedItem;

            DimensionContentFilter.ItemsSource = showedLevel.DimensionContentsList;
            DimensionContentFilter.DisplayMemberPath = "content";
            DimensionContentFilter.SelectAll();

            DimensionContentSearch.Clear();
        }
        #endregion

        #region update operations
        internal void UpdateByDimensionChange(Dimension newSelectedDimension)
        {
            ShowedDimension = newSelectedDimension;
            
            //update level
            DimensionLevelComboBox.ItemsSource = newSelectedDimension.GetLevel();

            //update aggregation level
            List<Dimension> temp = new List<Dimension>();
            Dimension emptyLevel = new Dimension("empty level", toTable: "no aggregation", isEmptyDimension: true);

            temp.Add(emptyLevel);
            temp.AddRange(newSelectedDimension.GetLevel());

            DimensionAggregationComboBox.Items.Clear();

            //exclude the all level for aggregation level combobox
            foreach (Dimension dim in temp)
            {
                if (dim != temp.Last())
                {
                    DimensionAggregationComboBox.Items.Add(dim);
                }
            }

            DimensionAggregationComboBox.DisplayMemberPath = "Level";
            DimensionAggregationComboBox.SelectedIndex = 0;

            //update content
            DimensionContentSearch.Clear();
        }

        internal void UpdateByLevelChange()
        {
            Dimension selectedLevel = (Dimension)DimensionLevelComboBox.SelectedItem;

            if (selectedLevel == null)
            {
                DimensionLevelComboBox.DisplayMemberPath = "Level";
                DimensionLevelComboBox.SelectedIndex = 0;
            }
            else
            {
                DimensionContentFilter.ItemsSource = selectedLevel.DimensionContentsList;
                DimensionContentFilter.DisplayMemberPath = "content";
                DimensionContentFilter.SelectAll();
            }
        }

        internal void UpdateByQuickSearchChanged()
        {
            if (DimensionContentFilter.Items == null)
                return;

            DimensionContentFilter.Items.Filter = delegate(object obj)
            {
                DimensionContent dc = (DimensionContent)obj;
                string value = dc.Content.ToUpper();
                if (String.IsNullOrEmpty(value))
                    return false;
                var index = value.IndexOf(DimensionContentSearch.Text.ToUpper(), 0, StringComparison.Ordinal);

                return (index > -1);
            };
        }
        #endregion

        #region getter
        internal int GetSelectedLevelDepth()
        {
            if (DimensionLevelComboBox.SelectedItem == null)
                return -1;

            // + 1 because we start to count dimension level by 1 but the combo box starting with index zero
            return DimensionLevelComboBox.SelectedIndex + 1;
        }

        internal int GetSelectedAggregationDepth()
        {
            if (DimensionAggregationComboBox.SelectedItem == null)
                return -1;

            // + 1 because we start to count dimension level by 1 but the combo box starting with index zero
            return DimensionAggregationComboBox.SelectedIndex + 1;
        }

        internal List<DimensionContent> GetSelectedDimensionContent()
        {
            if (DimensionContentFilter == null)
                return null;
            return DimensionContentFilter.SelectedItems.Cast<DimensionContent>().ToList();
        }
        #endregion
    }
}