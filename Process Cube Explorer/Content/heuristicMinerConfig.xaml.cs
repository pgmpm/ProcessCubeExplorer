using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.MiningAlgorithm;
using System.Windows.Controls;

namespace pgmpm.MainV2.Content
{
    /// <summary>
    /// Interaction logic for heuristicMinerConfig.xaml
    /// </summary>
    public partial class heuristicMinerConfig : UserControl, IContent
    {
        public heuristicMinerConfig()
        {
            InitializeComponent();
        }

        // --------------------- Navigation-Events ---------------------
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

        /// <summary>
        /// Save miner settings.
        /// </summary>
        /// <param name="e"></param>
        void IContent.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            MinerSettings.AddOrUpdateKey("AdjacencyThresholdSlider", AdjacencyThresholdSlider.Value);
            MinerSettings.AddOrUpdateKey("MaximumRecursionDepthSlider", MaximumRecursionDepthSlider.Value);
        }
    }
}
