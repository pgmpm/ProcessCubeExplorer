using System.Text;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.Database;
using pgmpm.MainV2.Utilities;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pgmpm.MainV2.Pages
{
    /// <summary>
    /// Interaktionslogik für notes.xaml
    /// </summary>
    /// <author>Thomas Meents</author>
    public partial class notes : UserControl, IContent
    {
        string Filename = AppDomain.CurrentDomain.BaseDirectory + "notes.txt";

        public notes()
        {
            InitializeComponent();
            ReadNotes();
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
            if (DBWorker.MetaData == null && e.Source.OriginalString == "/Pages/P2metadata.xaml")
            {
                ModernDialog.ShowMessage("You have to establish a database connection first.", "Connection", MessageBoxButton.OK);
                e.Cancel = true;
            }

            if (InterfaceHelpers.CheckIfNavigationIsAllowed(e.Source.OriginalString) == false)
                e.Cancel = true;
        }


        // --------------------- Buttons ---------------------
        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.BrowseBack.Execute(null, null);
        }

        private void OpenSettingsClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/Settings.xaml", null);
        }

        private void SaveNotesClick(object sender, RoutedEventArgs e)
        {
            SaveNotes();
            NavigationCommands.BrowseBack.Execute(null, null);
        }

        // --------------------- Functionality ---------------------

        /// <summary>
        /// shows the notes from the txt-file.
        /// </summary>
        /// <author>Thomas Meents</author>
        public void ReadNotes()
        {
            if (File.Exists(Filename))
            {
                StreamReader notes = new StreamReader(Filename, Encoding.Default);
                personalnotes.Text = notes.ReadToEnd();
                notes.Close();
            }
            else
            {
                StreamWriter notes = File.CreateText(Filename);
                notes.Write(personalnotes.Text);
                notes.Close();
            }
        }

        /// <summary>
        /// save the notes in a txt-file
        /// </summary>
        /// <author>Thomas Meents</author>
        public void SaveNotes()
        {
            if (File.Exists(Filename))
            {
                StreamWriter notes = new StreamWriter(Filename);
                notes.Write(personalnotes.Text);
                notes.Close();
            }
            else
            {
                ModernDialog.ShowMessage("Unable to write notes!", "Error", MessageBoxButton.OK);
            }
        }
    }
}
