using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using Microsoft.Win32;
using pgmpm.Database;
using pgmpm.Database.Exceptions;
using pgmpm.Database.Model;
using pgmpm.MainV2.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pgmpm.MainV2.Content
{
    /// <summary>
    /// Interaction logic for settingsDatabase.xaml
    /// </summary>
    public partial class settingsDatabase : UserControl, IContent
    {
        ConnectionParameters ConnectionParameters;

        public settingsDatabase()
        {
            InitializeComponent();

            DBDatabaseTextBox.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(DBDatabaseTextBox_Textbox_MouseLeftButtonUp), true);
            DBTypeComboBox.ItemsSource = DBWorker.SupportedDbTypes;
        }

        // --------------------- Navigation-Events ---------------------
        void IContent.OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        void IContent.OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// Calls FillListboxWithDatabaseConections.
        /// </summary>
        /// <param name="e"></param>
        void IContent.OnNavigatedTo(NavigationEventArgs e)
        {
            FillListboxWithDatabaseConections();
        }

        void IContent.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        /// <summary>
        /// Fills the listbox with saved database connections
        /// </summary>
        /// <author>Bernhard Bruns</author>
        private void FillListboxWithDatabaseConections()
        {

            DBConnectionHelpers.LoadConnectionParameters();
            ConnectionsListBox.ItemsSource = DBConnectionHelpers.ConnectionParametersList;

            if (ConnectionsListBox.Items.Count > 0)
            {
                ConnectionsListBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// This is called, whenever the Database-Type changes (even if selected from the saved entries on the right) and updates the labels, standard ports etc. accordingly.
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        /// <author>Jannik Arndt</author>
        private void DBTypeComboBoxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Restore UI-Elements if not visible
            if (!AddConnectionGrid.Children.Contains(DBHostnameLabel)) AddConnectionGrid.Children.Add(DBHostnameLabel);
            if (!AddConnectionGrid.Children.Contains(DBUsernameLabel)) AddConnectionGrid.Children.Add(DBUsernameLabel);
            if (!AddConnectionGrid.Children.Contains(DBPasswordLabel)) AddConnectionGrid.Children.Add(DBPasswordLabel);
            if (!AddConnectionGrid.Children.Contains(DBPortLabel)) AddConnectionGrid.Children.Add(DBPortLabel);
            if (!AddConnectionGrid.Children.Contains(DBHostnameTextBox)) AddConnectionGrid.Children.Add(DBHostnameTextBox);
            if (!AddConnectionGrid.Children.Contains(DBUsernameTextBox)) AddConnectionGrid.Children.Add(DBUsernameTextBox);
            if (!AddConnectionGrid.Children.Contains(DBPasswordPasswordBox)) AddConnectionGrid.Children.Add(DBPasswordPasswordBox);
            if (!AddConnectionGrid.Children.Contains(DBPortTextBox)) AddConnectionGrid.Children.Add(DBPortTextBox);
            if (DBTypeComboBox.SelectedItem == null) DBTypeComboBox.SelectedIndex = 0;
            switch (DBTypeComboBox.SelectedItem.ToString())
            {
                case "MySQL": // MySQL
                    DBDatabaseLabel.Content = "Database:";
                    DBPortTextBox.Text = "3306";
                    break;
                case "Oracle": // Oracle
                    DBDatabaseLabel.Content = "Service:";
                    DBPortTextBox.Text = "1521";
                    break;
                case "PostgreSQL": //PostgreSQL
                    DBDatabaseLabel.Content = "Database:";
                    DBPortTextBox.Text = "5432";
                    break;
                case "MS-SQL": //MS-SQL
                case "MS-SQL Windows Auth":
                    DBDatabaseLabel.Content = "Database:";
                    DBPortTextBox.Text = "1433";
                    break;
                case "SQLite": //SQLite
                    DBDatabaseLabel.Content = "Database:";
                    DBDatabaseTextBox.Text = "";
                    DBHostnameTextBox.Text = "SQLite";
                    DBUsernameTextBox.Text = "-";
                    DBPasswordPasswordBox.Password = "-";
                    DBPortTextBox.Text = "-";

                    // Hide not required UI-Elements
                    if (AddConnectionGrid.Children.Contains(DBHostnameLabel)) AddConnectionGrid.Children.Remove(DBHostnameLabel);
                    if (AddConnectionGrid.Children.Contains(DBUsernameLabel)) AddConnectionGrid.Children.Remove(DBUsernameLabel);
                    if (AddConnectionGrid.Children.Contains(DBPasswordLabel)) AddConnectionGrid.Children.Remove(DBPasswordLabel);
                    if (AddConnectionGrid.Children.Contains(DBPortLabel)) AddConnectionGrid.Children.Remove(DBPortLabel);
                    if (AddConnectionGrid.Children.Contains(DBHostnameTextBox)) AddConnectionGrid.Children.Remove(DBHostnameTextBox);
                    if (AddConnectionGrid.Children.Contains(DBUsernameTextBox)) AddConnectionGrid.Children.Remove(DBUsernameTextBox);
                    if (AddConnectionGrid.Children.Contains(DBPasswordPasswordBox)) AddConnectionGrid.Children.Remove(DBPasswordPasswordBox);
                    if (AddConnectionGrid.Children.Contains(DBPortTextBox)) AddConnectionGrid.Children.Remove(DBPortTextBox);

                    break;
                case "SQLite In-Memory": //SQLite In-Memory
                    DBDatabaseLabel.Content = "Database:";
                    DBDatabaseTextBox.Text = "";
                    DBHostnameTextBox.Text = "SQLite In-Memory";
                    DBUsernameTextBox.Text = "-";
                    DBPasswordPasswordBox.Password = "-";
                    DBPortTextBox.Text = "-";

                    // Hide not required UI-Elements
                    if (AddConnectionGrid.Children.Contains(DBHostnameLabel)) AddConnectionGrid.Children.Remove(DBHostnameLabel);
                    if (AddConnectionGrid.Children.Contains(DBUsernameLabel)) AddConnectionGrid.Children.Remove(DBUsernameLabel);
                    if (AddConnectionGrid.Children.Contains(DBPasswordLabel)) AddConnectionGrid.Children.Remove(DBPasswordLabel);
                    if (AddConnectionGrid.Children.Contains(DBPortLabel)) AddConnectionGrid.Children.Remove(DBPortLabel);
                    if (AddConnectionGrid.Children.Contains(DBHostnameTextBox)) AddConnectionGrid.Children.Remove(DBHostnameTextBox);
                    if (AddConnectionGrid.Children.Contains(DBUsernameTextBox)) AddConnectionGrid.Children.Remove(DBUsernameTextBox);
                    if (AddConnectionGrid.Children.Contains(DBPasswordPasswordBox)) AddConnectionGrid.Children.Remove(DBPasswordPasswordBox);
                    if (AddConnectionGrid.Children.Contains(DBPortTextBox)) AddConnectionGrid.Children.Remove(DBPortTextBox);

                    break;

            }
        }

        /// <summary>
        /// Resets the textboxes to create a new connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearConnectionClick(object sender, RoutedEventArgs e)
        {
            if (ModernDialog.ShowMessage("Do you want to clear the database fields ?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                DBConnectionnameTextBox.Text = "";
                DBHostnameTextBox.Text = "";
                DBDatabaseTextBox.Text = "";
                DBUsernameTextBox.Text = "";
                DBPasswordPasswordBox.Password = "";
                DBPortTextBox.Text = "";
                DBTypeComboBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Adds a new/changed connection to the connection list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddConnectionClick(object sender, RoutedEventArgs e)
        {
            ConnectionParameters conParams = new ConnectionParameters(DBTypeComboBox.Text, DBConnectionnameTextBox.Text, DBHostnameTextBox.Text, DBDatabaseTextBox.Text, DBUsernameTextBox.Text, DBPasswordPasswordBox.Password, DBPortTextBox.Text);

            if (conParams.IsComplete())
            {
                if (DBConnectionHelpers.CheckIfDatabaseNameExists(conParams.Name))
                    DBConnectionHelpers.AddDatabaseConnectionToConnectionList(conParams);

                else
                {
                    if (ModernDialog.ShowMessage("Connection Name \"" + conParams.Name + "\" already exists! \r\nDo you want to override the existing connection?", "New Connection", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        DBConnectionHelpers.RemoveConnectionParameter(conParams.Name);
                        DBConnectionHelpers.AddDatabaseConnectionToConnectionList(conParams);
                    }
                }
            }
            else
                ModernDialog.ShowMessage("Please fill out all fields to add the new connection!", "New Connection", MessageBoxButton.OK);

            ReloadListbox();
        }

        /// <summary>
        /// Reloads the connection listbox
        /// </summary>
        private void ReloadListbox()
        {
            ConnectionsListBox.ItemsSource = null;
            ConnectionsListBox.ItemsSource = DBConnectionHelpers.ConnectionParametersList;
            DBConnectionHelpers.SaveConnectionParameters();
        }

        /// <summary>
        /// The selected item will be removed.
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        /// <author>Bernhard Bruns</author>
        private void RemoveConnectionClick(object sender, RoutedEventArgs e)
        {
            if (ConnectionsListBox.Items.Count > 0 && ConnectionsListBox.SelectedIndex >= 0)
            {
                ConnectionParameters = (ConnectionParameters)ConnectionsListBox.Items[ConnectionsListBox.SelectedIndex];

                if (ModernDialog.ShowMessage("Are you sure you want to remove the connection \"" + ConnectionParameters.Name + "\"?", "Remove", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    DBConnectionHelpers.ConnectionParametersList.Remove(ConnectionParameters);
                    ReloadListbox();

                    if (ConnectionsListBox.Items.Count > 0)
                    {
                        ConnectionsListBox.SelectedIndex = 0;
                    }
                    else
                    {
                        DBTypeComboBox.Text = "";
                        DBConnectionnameTextBox.Text = "";
                        DBUsernameTextBox.Text = "";
                        DBPasswordPasswordBox.Password = "";
                        DBHostnameTextBox.Text = "";
                        DBPortTextBox.Text = "";
                        DBDatabaseTextBox.Text = "";
                    }
                }

            }
        }

        /// <summary>
        /// If the selectedindex of the listbox changes, the textboxes get refreshed
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        /// <author>Bernhard Bruns</author>
        private void ConnectionsSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ConnectionsListBox.SelectedIndex >= 0)
                {
                    ConnectionParameters = (ConnectionParameters)ConnectionsListBox.Items[ConnectionsListBox.SelectedIndex];

                    DBTypeComboBox.Text = ConnectionParameters.Type;
                    DBConnectionnameTextBox.Text = ConnectionParameters.Name;
                    DBHostnameTextBox.Text = ConnectionParameters.Host;
                    DBDatabaseTextBox.Text = ConnectionParameters.Database;
                    DBPasswordPasswordBox.Password = ConnectionParameters.Password;
                    DBPortTextBox.Text = ConnectionParameters.Port;
                    DBUsernameTextBox.Text = ConnectionParameters.User;
                }
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "Error: ", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Enables the "Test"-Button as soon as all textboxes are filled.
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        /// <author>Jannik Arndt, Markus Holznagel</author>
        private void EnabledTestButton(object sender, TextChangedEventArgs e)
        {
            if (DBConnectionnameTextBox.Text != "" && DBUsernameTextBox.Text != "" && DBHostnameTextBox.Text != "" && DBPortTextBox.Text != "" && DBDatabaseTextBox.Text != "")
            {
                TestConnectionButton.IsEnabled = true;
                AddConnectionButton.IsEnabled = true;
            }
            else
            {
                if (TestConnectionButton != null)
                    TestConnectionButton.IsEnabled = false;

                if (AddConnectionButton != null)
                    AddConnectionButton.IsEnabled = false;
            }
        }

        /// <summary>
        /// Test the Database-Connection.
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        /// <author>Bernhard Bruns</author>
        private void TestConnectionClick(object sender, RoutedEventArgs e)
        {

            Cursor = Cursors.Wait;

            ConnectionParameters connectionParameters = new ConnectionParameters();
            connectionParameters.Type = DBTypeComboBox.SelectedItem.ToString();
            connectionParameters.Name = DBConnectionnameTextBox.Text;
            connectionParameters.User = DBUsernameTextBox.Text;
            connectionParameters.Password = DBPasswordPasswordBox.Password;
            connectionParameters.Host = DBHostnameTextBox.Text;
            connectionParameters.Database = DBDatabaseTextBox.Text;
            connectionParameters.Port = DBPortTextBox.Text;


            try
            {
                if (DBWorker.ConfigureDBConnection(connectionParameters))
                {
                    if (DBWorker.OpenConnection())
                    {
                        ModernDialog.ShowMessage("The connection to " + connectionParameters.Host + " is established.", "Connection", MessageBoxButton.OK);
                    }
                    else
                        ErrorHandling.ReportErrorToUser("The connection could not be established.");
                }
                else
                    ModernDialog.ShowMessage("The connection could not be established. It's probably not implemented yet.", "Error: Implementation missing", MessageBoxButton.OK);
            }
            catch (NoParamsGivenException ex)
            {
                ModernDialog.ShowMessage(ex.ExceptionMessage, "Error: Missing parameters", MessageBoxButton.OK);
            }
            catch (ConnectionTypeNotGivenException ex)
            {
                ModernDialog.ShowMessage(ex.ExceptionMessage, "Error: No connection Type", MessageBoxButton.OK);
            }
            catch (NoConnectionException ex)
            {
                ModernDialog.ShowMessage(ex.ExceptionMessage, "Error: No connection", MessageBoxButton.OK);
            }

            catch (UnauthorizedAccessException ex)
            {
                ModernDialog.ShowMessage(ex.Message, "Error: Access denied ", MessageBoxButton.OK);
            }

            catch (WrongCredentialsException ex)
            {
                ModernDialog.ShowMessage(ex.ExceptionMessage, "Error: Wrong credentials", MessageBoxButton.OK);
            }
            catch (DatabaseDoesNotExist ex)
            {
                ModernDialog.ShowMessage(ex.ExceptionMessage, "Error: Wrong database", MessageBoxButton.OK);
            }
            catch (TimeoutException ex)
            {
                ModernDialog.ShowMessage(ex.Message, "Error: Timeout", MessageBoxButton.OK);
            }
            catch (DBException ex)
            {
                ModernDialog.ShowMessage(ex.Message, "Error: Unknown", MessageBoxButton.OK);
            }

            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Opens a file dialog window to select a sqlite file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <autor>Andrej Albrecht</autor>
        private void DBDatabaseTextBox_Textbox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (DBTypeComboBox.Text.Equals("SQLite") || DBTypeComboBox.Text.Equals("SQLite In-Memory"))
            {

                // Create OpenFileDialog
                OpenFileDialog dlg = new OpenFileDialog
                {
                    DefaultExt = "db",
                    Filter =
                        "SQLite-Databasefile (*.db)|*.db|SQLite-Databasefile (*.sqlite)|*.sqlite|All files (*.*)|*.*"
                };

                // Set filter for file extension and default file extension
                //"Text files (*.txt)|*.txt|All files (*.*)|*.*";

                // Display OpenFileDialog by calling ShowDialog method
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox
                if (result == true)
                {
                    // Open document
                    string filename = dlg.FileName;
                    DBDatabaseTextBox.Text = filename;
                }
            }
        }
    }
}
