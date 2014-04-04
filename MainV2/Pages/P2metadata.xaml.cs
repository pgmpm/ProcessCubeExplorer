using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.Database;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection.Dimensions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pgmpm.MainV2.Pages
{
    /// <summary>
    /// Interaction logic for P2metadata.xaml
    /// </summary>
    public partial class P2metadata : UserControl, IContent
    {
        public P2metadata()
        {
            InitializeComponent();
        }

        int _lastDbConnectionHash;

        // --------------------- Navigation-Events ---------------------
        void IContent.OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        void IContent.OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        void IContent.OnNavigatedTo(NavigationEventArgs e)
        {
            DBInformationButton.ToolTip = MainWindow.ConnectionName;
            InterfaceHelpers.CurrentPage = e.Source.OriginalString;

            if (DBWorker.MetaData != null)
            {
                if (_lastDbConnectionHash != DBWorker.MetaData.GetHashCode())
                {
                    LoadDimensionMetaData();
                    _lastDbConnectionHash = DBWorker.MetaData.GetHashCode();
                }

            }
            else
                throw new NullReferenceException("FactTable is null. There is no database connection");
        }

        void IContent.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (DBWorker.MetaData.EventClassifier == "")
            {
                e.Cancel = true;
                ModernDialog.ShowMessage("Please select a classifier", "Metadata", MessageBoxButton.OK);
            }

            if (InterfaceHelpers.CheckIfNavigationIsAllowed(e.Source.OriginalString) == false)
                e.Cancel = true;
        }

        // --------------------- Buttons ---------------------
        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/P1connection.xaml", null);
        }

        private void OpenSettingsClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/Settings.xaml", null);
        }

        private void DBInformationClick(object sender, RoutedEventArgs e)
        {
            ModernDialog.ShowMessage(MainWindow.ConnectionName, "Database information", MessageBoxButton.OK);
        }

        private void ContinueClick(object sender, RoutedEventArgs e)
        {
            if (SaveNewMetadataInMetaWorker())
            {
                XMLHelper.SerializeObjectToXML(XMLHelper.Path, DBWorker.MetaData);
                NavigationCommands.GoToPage.Execute("/Pages/P3dimensionselection.xaml", null);
            }

            else
                ModernDialog.ShowMessage("Please fill out all fields before you continue!", "Metadata", MessageBoxButton.OK);
        }

        private void OpenNoteClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/notes.xaml", null);
        }

        private void ResetClick(object sender, RoutedEventArgs e)
        {

            if (ModernDialog.ShowMessage("Do you want to reset the database names ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                XMLHelper.ResetMetadata();
                LoadDimensionMetaData();
            }
        }

        // --------------------- Functionality ---------------------
        readonly List<string> _registeredNames = new List<string>();


        /// <summary>
        /// Creates for each dimension a column with textboxes(dimension, level) where the metadata can be modified
        /// </summary>
        /// <author>Jannik Arndt, Bernhard Bruns</author>
        private void LoadDimensionMetaData()
        {
            DimensionGrid.Children.Clear();
            DimensionGrid.ColumnDefinitions.Clear();

            foreach (string registeredName in _registeredNames)
                DimensionGrid.UnregisterName(registeredName);

            _registeredNames.Clear();

            // Load the columnnames of the event table into the combobox to let the user decide which one to use as a classifier
            ClassifierComboBox.ItemsSource = DBWorker.MetaData.ListOfEventsTableColumnNames;

            if (DBWorker.MetaData.ListOfEventsTableColumnNames.Contains(DBWorker.MetaData.EventClassifier))
                ClassifierComboBox.SelectedItem = DBWorker.MetaData.EventClassifier;
            else if (DBWorker.MetaData.ListOfEventsTableColumnNames.Count > 0)
            {
                ClassifierComboBox.SelectedIndex = 0;
            }

         
            int dimensionNumber = 0;
            foreach (Dimension dimension in DBWorker.MetaData.ListOfFactDimensions)
            {
                DimensionGrid.ColumnDefinitions.Add(new ColumnDefinition());

                if (!dimension.IsEmptyDimension && dimension.Name != "--no selection--")
                {
                    StackPanel dimensionPanel = new StackPanel();
                    dimensionPanel.SetValue(Grid.ColumnProperty, dimensionNumber);
                    dimensionPanel.Width = 150;

                    dimensionPanel.Children.Add(InterfaceHelpers.CreateTextBlock("DIMENSION " + (dimensionNumber + 1), "Emphasis", bottom: 8));
                    dimensionPanel.Children.Add(InterfaceHelpers.CreateTextBlock("Original Name: \n" + dimension.Name, bottom: 8));
                    dimensionPanel.Children.Add(InterfaceHelpers.CreateTextBlock("New Name:", bottom: 8));

                    TextBox dimensionNumberTextBox = InterfaceHelpers.CreateTextBox("Dimension" + (dimensionNumber + 1) + "newName", dimension.Dimensionname, bottom: 8);
                    dimensionNumberTextBox.GotKeyboardFocus += SelectTextInTextbox;

                    DimensionGrid.RegisterName(dimensionNumberTextBox.Name, dimensionNumberTextBox);
                    _registeredNames.Add(dimensionNumberTextBox.Name);
                    dimensionPanel.Children.Add(dimensionNumberTextBox);

                    dimensionPanel.Children.Add(InterfaceHelpers.CreateTextBlock("LEVELS", "Emphasis", top: 20, bottom: 8));

                    int j = 0;
                    foreach (Dimension cDim in dimension.GetLevel())
                    {
                        if (j >= 0)
                        {
                            dimensionPanel.Children.Add(InterfaceHelpers.CreateTextBlock(cDim.ToTable, bottom: 8));

                            TextBox txtLevel = InterfaceHelpers.CreateTextBox("level" + (dimensionNumber + 1) + "_" + j + "newName", cDim.Level, bottom: 15);
                            txtLevel.GotKeyboardFocus += SelectTextInTextbox;

                            DimensionGrid.RegisterName(txtLevel.Name, txtLevel);
                            _registeredNames.Add(txtLevel.Name);
                            dimensionPanel.Children.Add(txtLevel);
                        }
                        j++;
                    }

                    DimensionGrid.Children.Add(dimensionPanel);
                    dimensionNumber++;
                }
            }

            if (DBConnectionHelpers.DefaultEventClassifierIsSelected)
            {
                ModernDialog.ShowMessage("Auto selected " + ClassifierComboBox.SelectedItem + " as classifier.", "Metadata", MessageBoxButton.OK);
                DBConnectionHelpers.DefaultEventClassifierIsSelected = false;
            }

        }

        /// <summary>
        /// Saves the new edited metadata from the textboxes in the metaworker
        /// </summary>
        /// <author>Bernhard Bruns</author>
        private bool SaveNewMetadataInMetaWorker()
        {
            int dimensionNumber = 0;
            foreach (Dimension dimension in DBWorker.MetaData.ListOfFactDimensions)
            {
                if (!dimension.IsEmptyDimension && dimension.Dimensionname != "--no selection--")
                {
                    var textBox = (TextBox)DimensionGrid.FindName("Dimension" + (dimensionNumber + 1) + "newName");
                    if (textBox != null)
                        dimension.DimensionnameChanged = textBox.Text;

                    if (dimension.Dimensionname == "")
                    {
                        return false;
                    }

                    int levelNumber = 0;
                    foreach (Dimension cDim in dimension.GetLevel())
                    {
                        if (levelNumber >= 0)
                        {
                            TextBox temporaryTextBox = (TextBox)DimensionGrid.FindName("level" + (dimensionNumber + 1) + "_" + levelNumber + "newName");
                            if (temporaryTextBox != null && !string.IsNullOrEmpty(temporaryTextBox.Text))
                                cDim.LevelChanged = temporaryTextBox.Text;
                            else
                                return false;
                        }
                        levelNumber++;
                    }
                    dimensionNumber++;
                }
            }
            return true;
        }

        /// <summary>
        /// Select the text from the sender textbox (for faster editing)
        /// </summary>
        /// <param name="sender">Textbox</param>
        /// <param name="e"></param>
        /// <author>Bernhard Bruns</author>
        private void SelectTextInTextbox(object sender, KeyboardFocusChangedEventArgs e)
        {
            ((TextBox)sender).SelectAll();
        }

        /// <summary>
        /// Immediately sets the EventClassifier in the MetaWorker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClassifierComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClassifierComboBox.SelectedItem != null)
                DBWorker.MetaData.EventClassifier = (ClassifierComboBox.SelectedItem as string);
        }

    }
}