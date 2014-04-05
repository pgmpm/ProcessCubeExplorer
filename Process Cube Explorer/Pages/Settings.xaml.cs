using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.Database;
using pgmpm.MainV2.Content;
using pgmpm.MainV2.Utilities;
using System.Windows;
using System.Windows.Input;

namespace pgmpm.MainV2.Pages
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : IContent
    {
        public Settings()
        {
            InitializeComponent();
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
        }
        #endregion

        #region Buttons
        /// <summary>
        /// save the Settings and return to the last page
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void OkayClick(object sender, RoutedEventArgs e)
        {
            SaveSettings();

            NavigationCommands.BrowseBack.Execute(null, null);
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.BrowseBack.Execute(null, null);
        }
        #endregion

        #region Functionality
        private void SaveSettings()
        {
            Properties.Settings.Default.AskExitMainWindow = settingsGeneral.AskClosing;
            Properties.Settings.Default.OpenFolderAfterExport = settingsGeneral.OpenFolderAfterExport;
            Properties.Settings.Default.ImageName = settingsGeneral.ImageName;
            Properties.Settings.Default.ImagePath = settingsGeneral.ImagePath;
            Properties.Settings.Default.ImageFiletype = settingsGeneral.ImageFileType;
            MatrixSelection.Properties.Settings.Default.ProcessModelPercentOfQuality = settingsGeneral.ProcessModellQuality;

            Properties.Settings.Default.Save();
            Visualization.Properties.Settings.Default.Save();
            Database.Properties.Settings.Default.Save();
        }
        #endregion
    }
}
