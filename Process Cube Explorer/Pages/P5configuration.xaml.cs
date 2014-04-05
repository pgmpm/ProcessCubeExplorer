using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.MainV2.Utilities;
using pgmpm.MiningAlgorithm;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pgmpm.MainV2.Pages
{
    /// <summary>
    /// Interaction logic for P4configuration.xaml
    /// </summary>
    public partial class P5configuration : IContent
    {
        public P5configuration()
        {
            InitializeComponent();

            MinerList.ItemsSource = MinerFactory.ListOfMiners; 
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
            DBInformationButton.ToolTip = MainWindow.ConnectionName;
            InterfaceHelpers.CurrentPage = e.Source.OriginalString;
        }

        void IContent.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (InterfaceHelpers.CheckIfNavigationIsAllowed(e.Source.OriginalString) == false)
                e.Cancel = true;
        }

        #region Buttons
        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/P4eventselection.xaml", null);
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
            NavigationCommands.GoToPage.Execute("/Pages/P6mining.xaml", null);
        }
        #endregion

        #region Events
        private void Frame_FragmentNavigation(object sender, FragmentNavigationEventArgs e)
        {
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
        }

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {

        }

        private void OnMinerListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MinerSettings.IsAlgorithmSet = true;
            var selectedMiner = (KeyValuePair<String, String>)MinerList.SelectedItem;
            MinerSettings.MinerName = selectedMiner.Key;
            MinerSettings.MinerURI = selectedMiner.Value;
            ContinueButton.IsEnabled = true;
            Frame.Source = new Uri(MinerSettings.MinerURI, UriKind.Relative);
        }
        #endregion
    }
}
