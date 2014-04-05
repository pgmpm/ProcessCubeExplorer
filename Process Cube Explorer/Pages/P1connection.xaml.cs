using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using Microsoft.Win32;
using pgmpm.Database;
using pgmpm.Database.Model;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection;

namespace pgmpm.MainV2.Pages
{
    /// <summary>
    /// Interaction logic for P1connection.xaml
    /// </summary>
    public partial class P1connection : IContent
    {
        public P1connection()
        {
            InitializeComponent();

            DBDatabaseTextBox.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(DBDatabaseTextBoxMouseLeftButtonUp), true);
            DBTypeComboBox.ItemsSource = DBWorker.SupportedDbTypes;
        }

        // --------------------- Navigation-Events ---------------------
        void IContent.OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        void IContent.OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        void IContent.OnNavigatedTo(NavigationEventArgs e)
        {
            InterfaceHelpers.CurrentPage = e.Source.OriginalString;

            FillFavouriteDBConnectionsComboBox();

            // Disable "Reconnect to last used Database" if the connection cannot be found
            DBConnectionHelpers.LoadConnectionParameters();
            if (DBConnectionHelpers.LoadLastUsedDatabase() == null)
                ReconnectToLastUsedConnectionButton.IsEnabled = false;
            else
                ReconnectToLastUsedConnectionButton.IsEnabled = true;

            // Disable "Restore saved results" button if no file exists
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\LastMiningResults.mpm"))
                RestoreSavedResultsButton.IsEnabled = false;
            else
                RestoreSavedResultsButton.IsEnabled = true;
        }

        void IContent.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {

            if (DBWorker.MetaData == null && e.Source.OriginalString == "/Pages/P2metadata.xaml")
            {
                ModernDialog.ShowMessage("You have to establish a database connection first.", "Connection", MessageBoxButton.OK);
                e.Cancel = true;
            }

            if (InterfaceHelpers.CheckIfNavigationIsAllowed(e.Source.OriginalString) == false)
                e.Cancel = true;

            InterfaceHelpers.RestoreData = false;
        }

        #region Buttons Bottom
        private void OpenSettingsClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/Settings.xaml", null);
        }

        private void OpenNoteClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/notes.xaml", null);
        }

        #endregion

        #region Buttons Middle

        /// <summary>
        /// Restore the Metadata, selected Dimensions and mined data, if they were previously saved in the Results-view
        /// </summary>
        /// <author>Jannik Arndt, Bernhard Bruns</author>
        private void RestoreSavedResultsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.MatrixSelection = Serializer.Deserialize<MatrixSelectionModel>("LastMiningResults.mpm");

                MainWindow.ConnectionName = "No database connection available. \nThese are restored results.";

                InterfaceHelpers.RestoreData = true; // Allows the navigation from connection to results
                P8Results.ListOfChoosenProcessModels.Clear();

                NavigationCommands.GoToPage.Execute("/Pages/P8results.xaml", null);
            }
            catch (Exception Ex)
            {
                if (Ex is OutOfMemoryException)
                    ErrorHandling.ReportErrorToUser("Error loading last results: The file you are trying to load seems to be damaged. " + Ex.Message);
                else if (Ex is FileNotFoundException)
                    ErrorHandling.ReportErrorToUser("Error loading last results: The file " + (Ex as FileNotFoundException).FileName + " cannot be found. You need to generate and save results first!");
                else if (Ex is TargetInvocationException || Ex is SerializationException)
                {
                    ErrorHandling.ReportErrorToUser("Error: The file you are loading was saved with an old version of this software and can't be read. It will automatically be deleted.");
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + "LastMiningResults.mpm");
                    RestoreSavedResultsButton.IsEnabled = false;
                }
                else if (Ex is IOException || Ex is NullReferenceException || Ex is ArgumentOutOfRangeException)
                    ErrorHandling.ReportErrorToUser("Error loading last results: " + Ex.Message);
                else
                    throw;
            }
        }

        /// <summary>
        /// Reconnect to the Last used database connection
        /// </summary>
        /// <author>Bernhard Bruns</author>
        private void ReconnectToLastUsedConnectionClick(object sender, RoutedEventArgs e)
        {
            DBConnectionHelpers.LoadConnectionParameters();
            ConnectToDb(DBConnectionHelpers.LoadLastUsedDatabase());
        }

        /// <summary>
        /// Connect to the chosen favorite-connection
        /// </summary>
        /// <author>Bernhard Bruns</author>
        private void ConnectToFavouriteClick(object sender, RoutedEventArgs e)
        {
            if (FavouriteDBConnectionsComboBox.SelectedIndex == -1)
                ErrorHandling.ReportErrorToUser("Error: No connection is selected.");
            else
            {
                ConnectionParameters connectionParameters = (ConnectionParameters)FavouriteDBConnectionsComboBox.Items[FavouriteDBConnectionsComboBox.SelectedIndex];
                ConnectToDb(connectionParameters);

                DBConnectionHelpers.SaveLastUsedDatabase(connectionParameters);
            }
        }

        /// <summary>
        /// Show or hide the grid where the User can input details for a new connection.
        /// </summary>
        /// <author>Jannik Arndt</author>
        private void AddConnectionClick(object sender, RoutedEventArgs e)
        {
            // Toggle visibility
            AddConnectionGrid.Visibility = AddConnectionGrid.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        /// <summary>
        /// Add a new connection to the DatabaseSettings.xml and connect to the Database
        /// </summary>
        /// <author>Bernhard Bruns</author>
        private void SaveAndConnectClick(object sender, RoutedEventArgs e)
        {
            DBConnectionHelpers.LoadConnectionParameters();

            ConnectionParameters connectionParameters = new ConnectionParameters(DBTypeComboBox.Text, DBConnectionnameTextBox.Text, DBHostnameTextBox.Text, DBDatabaseTextBox.Text, DBUsernameTextBox.Text, DBPasswordTextBox.Password, DBPortTextBox.Text);

            if (connectionParameters.IsComplete())
                if (DBConnectionHelpers.CheckIfDatabaseNameExists(connectionParameters.Name))
                {
                    DBConnectionHelpers.AddDatabaseConnectionToConnectionList(connectionParameters);

                    if (ConnectToDb(connectionParameters))
                    {
                        DBConnectionHelpers.SaveConnectionParameters();
                        DBConnectionHelpers.SaveLastUsedDatabase(connectionParameters);
                        AddConnectionGrid.Visibility = Visibility.Collapsed;
                    }
                    else
                        ModernDialog.ShowMessage("Can't connect to database!", "New Connection", MessageBoxButton.OK);
                }
                else
                    ModernDialog.ShowMessage("Connection Name \"" + connectionParameters.Name + "\" already exists! \r\nPlease choose a new Name for your connection.", "New Connection", MessageBoxButton.OK);
            else
                ModernDialog.ShowMessage("Please fill out all fields to add the new connection!", "New Connection", MessageBoxButton.OK);
        }
        #endregion

        // --------------------- Functionality ---------------------
        /// <summary>
        /// Connect to the DB
        /// </summary>
        /// <author>Moritz Eversmann, Bernd Nottbeck</author>
        /// <param name="conParams"></param>
        /// <returns></returns>
        public bool ConnectToDb(ConnectionParameters conParams)
        {
            Cursor = Cursors.Wait;
            Boolean connected = DBConnectionHelpers.EstablishDatabaseConnection(conParams);
            Cursor = Cursors.Arrow;

            return connected;
        }

        /// <summary>
        /// Read the DatabaseSettings xml-file and write it in the connectionlist
        /// </summary>
        /// <author>Bernhard Bruns</author>
        public void FillFavouriteDBConnectionsComboBox()
        {
            DBConnectionHelpers.LoadConnectionParameters();

            if (DBConnectionHelpers.ConnectionParametersList != null)
            {
                // Binding
                FavouriteDBConnectionsComboBox.ItemsSource = DBConnectionHelpers.ConnectionParametersList;
                FavouriteDBConnectionsComboBox.DisplayMemberPath = "Name";

                if (DBConnectionHelpers.ConnectionParametersList.Count == 0)
                {
                    ConnectToFavouriteButton.IsEnabled = false;
                    FavouriteDBConnectionsComboBox.IsEnabled = false;
                }
                else
                {
                    ConnectToFavouriteButton.IsEnabled = true;
                    FavouriteDBConnectionsComboBox.IsEnabled = true;
                    FavouriteDBConnectionsComboBox.SelectedIndex = 0;
                }
            }
        }

        /// <summary>
        /// This is called, whenever the Database-Type changes (even if selected from the saved entries on the right) and updates the labels, standard ports etc. accordingly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Jannik Arndt</author>
        private void DBTypeComboBoxChanged(object sender, SelectionChangedEventArgs e)
        {
            //Restore UI-Elements if not visible
            if (!AddConnectionGrid.Children.Contains(DBHostnameLabel)) AddConnectionGrid.Children.Add(DBHostnameLabel);
            if (!AddConnectionGrid.Children.Contains(DBUsernameLabel)) AddConnectionGrid.Children.Add(DBUsernameLabel);
            if (!AddConnectionGrid.Children.Contains(DBPasswordLabel)) AddConnectionGrid.Children.Add(DBPasswordLabel);
            if (!AddConnectionGrid.Children.Contains(DBPortLabel)) AddConnectionGrid.Children.Add(DBPortLabel);
            if (!AddConnectionGrid.Children.Contains(DBHostnameTextBox)) AddConnectionGrid.Children.Add(DBHostnameTextBox);
            if (!AddConnectionGrid.Children.Contains(DBUsernameTextBox)) AddConnectionGrid.Children.Add(DBUsernameTextBox);
            if (!AddConnectionGrid.Children.Contains(DBPasswordTextBox)) AddConnectionGrid.Children.Add(DBPasswordTextBox);
            if (!AddConnectionGrid.Children.Contains(DBPortTextBox)) AddConnectionGrid.Children.Add(DBPortTextBox);

            switch (DBTypeComboBox.SelectedItem.ToString())
            {
                case "MySQL": // MySQL
                    DBDatabaseLabel.Content = "Database:";
                    DBDatabaseTextBox.Text = "";
                    DBHostnameTextBox.Text = "";
                    DBUsernameTextBox.Text = "";
                    DBPasswordTextBox.Password = "";
                    DBPortTextBox.Text = "3306";
                    break;
                case "Oracle": // Oracle
                    DBDatabaseLabel.Content = "Service:";
                    DBDatabaseTextBox.Text = "";
                    DBHostnameTextBox.Text = "";
                    DBUsernameTextBox.Text = "";
                    DBPasswordTextBox.Password = "";
                    DBPortTextBox.Text = "1521";
                    break;
                case "PostgreSQL": //PostgreSQL
                    DBDatabaseLabel.Content = "Database:";
                    DBDatabaseTextBox.Text = "";
                    DBHostnameTextBox.Text = "";
                    DBUsernameTextBox.Text = "";
                    DBPasswordTextBox.Password = "";
                    DBPortTextBox.Text = "5432";

                    break;
                case "MS-SQL": // MS-SQL
                case "MS-SQL Windows Auth":
                    DBDatabaseLabel.Content = "Database:";
                    DBDatabaseTextBox.Text = "";
                    DBHostnameTextBox.Text = "";
                    DBUsernameTextBox.Text = "";
                    DBPasswordTextBox.Password = "";
                    DBPortTextBox.Text = "1433";
                    break;
                case "SQLite": // SQLite
                    DBDatabaseLabel.Content = "Database:";
                    DBDatabaseTextBox.Text = "";
                    DBHostnameTextBox.Text = "SQLite";
                    DBUsernameTextBox.Text = "-";
                    DBPasswordTextBox.Password = "-";
                    DBPortTextBox.Text = "-";

                    // Hide not required UI-Elements
                    if (AddConnectionGrid.Children.Contains(DBHostnameLabel)) AddConnectionGrid.Children.Remove(DBHostnameLabel);
                    if (AddConnectionGrid.Children.Contains(DBUsernameLabel)) AddConnectionGrid.Children.Remove(DBUsernameLabel);
                    if (AddConnectionGrid.Children.Contains(DBPasswordLabel)) AddConnectionGrid.Children.Remove(DBPasswordLabel);
                    if (AddConnectionGrid.Children.Contains(DBPortLabel)) AddConnectionGrid.Children.Remove(DBPortLabel);
                    if (AddConnectionGrid.Children.Contains(DBHostnameTextBox)) AddConnectionGrid.Children.Remove(DBHostnameTextBox);
                    if (AddConnectionGrid.Children.Contains(DBUsernameTextBox)) AddConnectionGrid.Children.Remove(DBUsernameTextBox);
                    if (AddConnectionGrid.Children.Contains(DBPasswordTextBox)) AddConnectionGrid.Children.Remove(DBPasswordTextBox);
                    if (AddConnectionGrid.Children.Contains(DBPortTextBox)) AddConnectionGrid.Children.Remove(DBPortTextBox);

                    break;
                case "SQLite In-Memory": // SQLite In-Memory
                    DBDatabaseLabel.Content = "Database:";
                    DBDatabaseTextBox.Text = "";
                    DBHostnameTextBox.Text = "SQLite In-Memory";
                    DBUsernameTextBox.Text = "-";
                    DBPasswordTextBox.Password = "-";
                    DBPortTextBox.Text = "-";

                    // Hide not required UI-Elements
                    if (AddConnectionGrid.Children.Contains(DBHostnameLabel)) AddConnectionGrid.Children.Remove(DBHostnameLabel);
                    if (AddConnectionGrid.Children.Contains(DBUsernameLabel)) AddConnectionGrid.Children.Remove(DBUsernameLabel);
                    if (AddConnectionGrid.Children.Contains(DBPasswordLabel)) AddConnectionGrid.Children.Remove(DBPasswordLabel);
                    if (AddConnectionGrid.Children.Contains(DBPortLabel)) AddConnectionGrid.Children.Remove(DBPortLabel);
                    if (AddConnectionGrid.Children.Contains(DBHostnameTextBox)) AddConnectionGrid.Children.Remove(DBHostnameTextBox);
                    if (AddConnectionGrid.Children.Contains(DBUsernameTextBox)) AddConnectionGrid.Children.Remove(DBUsernameTextBox);
                    if (AddConnectionGrid.Children.Contains(DBPasswordTextBox)) AddConnectionGrid.Children.Remove(DBPasswordTextBox);
                    if (AddConnectionGrid.Children.Contains(DBPortTextBox)) AddConnectionGrid.Children.Remove(DBPortTextBox);

                    break;
            }
        }

        /// <summary>
        /// Opens a OpenFileDialog to pick a SQLite-Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <autor>Andrej Albrecht</autor>
        private void DBDatabaseTextBoxMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DBTypeComboBox.Text.Equals("SQLite") || DBTypeComboBox.Text.Equals("SQLite In-Memory"))
            {
                // Create OpenFileDialog and set filter for file extension and default file extension
                OpenFileDialog dialog = new OpenFileDialog
                {
                    DefaultExt = "db",
                    Filter = "SQLite-Databasefile (*.db)|*.db|SQLite-Databasefile (*.sqlite)|*.sqlite|All files (*.*)|*.*"
                };

                // Display OpenFileDialog by calling ShowDialog method
                bool? result = dialog.ShowDialog();

                // Get the selected file name and display in a TextBox
                if (result == true)
                {
                    // Open document
                    string filename = dialog.FileName;
                    DBDatabaseTextBox.Text = filename;
                }
            }
        }
    }
}