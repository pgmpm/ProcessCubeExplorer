using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.MiningAlgorithm;
using System.Windows.Controls;

namespace pgmpm.MainV2.Content
{
    /// <summary>
    /// Interaktionslogik für InductiveMinerConfig.xaml
    /// </summary>
    public partial class InductiveMinerConfig : UserControl, IContent
    {
        public InductiveMinerConfig()
        {
            InitializeComponent();
            InductiveThresholdSliderPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        void IContent.OnFragmentNavigation(FragmentNavigationEventArgs e)
        {

        }

        void IContent.OnNavigatedFrom(NavigationEventArgs e)
        {

        }

        /// <summary>
        /// Miner was selected
        /// </summary>
        /// <param name="e"></param>
        void IContent.OnNavigatedTo(NavigationEventArgs e)
        {
            MinerSettings.IsAlgorithmSet = true;
        }

        void IContent.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (InductiveminerRadioButton.IsChecked == true)
                MinerSettings.AddOrUpdateKey("InductiveMiner", "InductiveMiner");
            else if (InductiveminerInfrequentRadioButton.IsChecked == true)
            {
                MinerSettings.AddOrUpdateKey("InductiveMiner", "InductiveMinerInfrequent");
                MinerSettings.AddOrUpdateKey("InductiveThresholdSlider", InductiveThresholdSlider.Value);
            }

        }

        private void InductiveminerInfrequentRadioButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            InductiveThresholdSliderPanel.Visibility = System.Windows.Visibility.Visible;
        }

        private void InductiveminerRadioButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            InductiveThresholdSliderPanel.Visibility = System.Windows.Visibility.Collapsed;
        }




    }
}
