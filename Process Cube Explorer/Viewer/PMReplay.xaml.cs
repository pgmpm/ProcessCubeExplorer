using pgmpm.ConformanceChecking;
using pgmpm.Model.PetriNet;

namespace pgmpm.MainV2.Viewer
{
    /// <summary>
    /// Interaction logic for PMInfo.xaml
    /// </summary>
    /// <author>Jannik Arndt</author>
    public partial class PMReplay
    {
        public PMReplay()
        {
            InitializeComponent();

            TokenReplayResult replayResult = TokenReplayAlgorithm.Replay((PetriNet) Viewer.CurrentField.ProcessModel, Viewer.CurrentField.EventLog);
            ReplayResultsView.ItemsSource = replayResult.ToDictionary();
            NotFoundView.ItemsSource = replayResult.GetTransitionsNotFoundAsDictionary();
            NotEnabledView.ItemsSource = replayResult.GetTransitionsNotEnabledAsDictionary();
        }
    }
}