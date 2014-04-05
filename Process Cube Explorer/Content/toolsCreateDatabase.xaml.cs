using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using Microsoft.Win32;
using pgmpm.Database;
using pgmpm.Database.Helper;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pgmpm.MainV2.Content
{
    /// <summary>
    /// Interaction logic for toolsCreateDatabase.xaml
    /// </summary>
    /// <autor>Andrej Albrecht</autor>
    public partial class toolsCreateDatabase : UserControl, IContent
    {
        DatabaseCreationHelper DbCreationHelper;
        string _dbType;

        public toolsCreateDatabase()
        {
            InitializeComponent();

            SourceFile.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(SourceFile_Textbox_MouseLeftButtonUp), true);
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

        }

        void IContent.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }


        // --------------------- Functionality ---------------------

        private void DBTypeComboBoxSelectionChanged(object sender, RoutedEventArgs e)
        {
            _dbType = DBTypeComboBox.SelectedValue.ToString();
            
        }

        /// <summary>
        /// Source file handler. Opens a file Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SourceFile_Textbox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("SourceFile_Textbox_MouseLeftButtonUp");

            // Create OpenFileDialog
            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = "mxml",
                Filter = "Mining eXtensible Markup Language (*.mxml)|*.mxml|All files (*.*)|*.*"
            };

            // Set filter for file extension and default file extension

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                string filename = dlg.FileName;
                SourceFile.Text = filename;
            }
        }

        /// <summary>
        /// Creates an empty database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateEmptyDBClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DBTypeComboBox.SelectedIndex < 0) throw new ArgumentNullException("Please select a database type.");

                DbCreationHelper = new DatabaseCreationHelperFromMXML(_dbType, null);

                String result = DbCreationHelper.CreateEmptyDwh();

                Clipboard.SetText(result);
                ModernDialog.ShowMessage("The SQL-Statement was copied to the clipboard.", "Info", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "Error: ", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// Creates the insert statements
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateInsertStatementClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_dbType == null) throw new ArgumentNullException("Please select a database type.");

                if (SourceFile.Text.Equals("")) throw new ArgumentNullException("Please select a source file.");

                string filetype = Path.GetExtension(SourceFile.Text);

                String result = "";

                if (filetype.Equals(".mxml"))
                {
                    DbCreationHelper = new DatabaseCreationHelperFromMXML(_dbType, SourceFile.Text);
                    result = DbCreationHelper.CreateInsertSqlStatement();
                }
                
                Clipboard.SetText(result);
                ModernDialog.ShowMessage("The SQL-Statement was copied to the clipboard.", "Info", MessageBoxButton.OK);
            }
            catch (Exception ex)
            {
                ModernDialog.ShowMessage(ex.Message, "Error: ", MessageBoxButton.OK);
            }
        }
    }
}
