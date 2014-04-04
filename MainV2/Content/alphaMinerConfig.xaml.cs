using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.MiningAlgorithm;
using System.Windows.Controls;

namespace pgmpm.MainV2.Content
{
    /// <summary>
    /// Interaction logic for alphaMinerConfig.xaml
    /// </summary>
    public partial class alphaMinerConfig : UserControl, IContent
    {
        public alphaMinerConfig()
        {
            InitializeComponent();
        }

        void IContent.OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        void IContent.OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// Alpha miner was selected.
        /// </summary>
        /// <param name="e"></param>
        void IContent.OnNavigatedTo(NavigationEventArgs e)
        {
            MinerSettings.IsAlgorithmSet = true;
        }

        /// <summary>
        /// Sets the AlphamMiner Type
        /// </summary>
        /// <param name="e"></param>
        void IContent.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (AlphaMinerRadioButton.IsChecked == true)
                MinerSettings.AddOrUpdateKey("Alphaminer", AlphaMinerRadioButton.Name);
            else if (AlphaMinerPlusRadioButton.IsChecked == true)
                MinerSettings.AddOrUpdateKey("AlphaMiner", AlphaMinerPlusRadioButton.Name);
            else if (AlphaMinerPlusPlusRadioButton.IsChecked == true)
                MinerSettings.AddOrUpdateKey("AlphaMiner", AlphaMinerPlusPlusRadioButton.Name);
        }
    }
}
