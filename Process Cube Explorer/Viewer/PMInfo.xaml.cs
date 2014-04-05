using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows.Controls;
using pgmpm.MatrixSelection.Fields;
using pgmpm.Model.PetriNet;

namespace pgmpm.MainV2.Viewer
{
    /// <summary>
    /// Interaction logic for PMInfo.xaml
    /// </summary>
    public partial class PMInfo
    {
        readonly ModernWindow _parentWindow;

        public PMInfo()
        {
            InitializeComponent();
            PMInformationList.ItemsSource = Viewer.CurrentField.Information;

            foreach (var Case in Viewer.CurrentField.EventLog.Cases)
                EventLogViewerListView.Items.Add(Case);
        }
        public PMInfo(Dictionary<string, string> information, ModernWindow window)
        {
            InitializeComponent();
            PMInformationList.ItemsSource = information;

            _parentWindow = window;
            _parentWindow.PreviewKeyDown += HandleShortcuts;
        }

        #region Short cuts
        private void HandleShortcuts(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    _parentWindow.Close();
                    break;
            }
        }
        #endregion


        /// <summary>
        /// Reloads the events for the selected case
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <autor>Andrej Albrecht</autor>
        private void EventLogViewer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EventListView.Items.Clear();
            var Case = (Case)EventLogViewerListView.SelectedItem;
            if (Case == null) return;

            CaseAdditionalInfoListView.ItemsSource = Case.AdditionalData;

            foreach (var Event in Case.EventList)
                EventListView.Items.Add(Event);
        }

        private void EventSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var _event = (Event) EventListView.SelectedItem;
            if(_event == null) return;
            EventAdditionalInfoListView.ItemsSource = _event.Information;
        }
    }
}
