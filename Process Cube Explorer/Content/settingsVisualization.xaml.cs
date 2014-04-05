using System.Windows;
using System.Windows.Controls;
using pgmpm.Visualization.Properties;


namespace pgmpm.MainV2.Content
{
    /// <summary>
    /// Interaction logic for settingsVisualization.xaml
    /// </summary>
    public partial class settingsVisualization : UserControl
    {
        public settingsVisualization()
        {
            InitializeComponent();
        }

        private void ResetToDefaultClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.Reset();
        }
    }
}