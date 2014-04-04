using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using pgmpm.MainV2.Properties;
using UserControl = System.Windows.Controls.UserControl;


namespace pgmpm.MainV2.Content
{
    /// <summary>
    /// Interaction logic for settingsGeneral.xaml
    /// </summary>
    public partial class settingsGeneral : UserControl
    {
        public static string ImagePath;
        public static string ImageName;
        public static int ImageFileType;
        public static bool AskClosing;
        public static double ProcessModellQuality;
        public static bool OpenFolderAfterExport;

        public settingsGeneral()
        {
            InitializeComponent();

            ShowSettings();

        }

        /// <summary>
        /// Displays the settings
        /// </summary>
        private void ShowSettings()
        {
            closingProgramCheckBox.IsChecked = Settings.Default.AskExitMainWindow;
            openFolderAfterSaveCheckBox.IsChecked = Settings.Default.OpenFolderAfterExport;
            defaultImagePathTextBox.Text = Settings.Default.ImagePath;
            defaultimagenameTextBox.Text = Settings.Default.ImageName;
            setDefaultImageTypeComboBox.SelectedIndex = Settings.Default.ImageFiletype;
        }

        /// <summary>
        /// Save the settings
        /// </summary>
        private void SaveSettings()
        {
            ImagePath = defaultImagePathTextBox.Text;
            ImageName = defaultimagenameTextBox.Text;
            AskClosing = (bool)closingProgramCheckBox.IsChecked;
            OpenFolderAfterExport = (bool)openFolderAfterSaveCheckBox.IsChecked;
            ImageFileType = setDefaultImageTypeComboBox.SelectedIndex;

            Settings.Default.Save();
            MatrixSelection.Properties.Settings.Default.Save();
        }

        /// <summary>
        /// File dialog to set the default file path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DefaultImagePathClick(Object sender, EventArgs e)
        {
            FolderBrowserDialog folderbrowserdialog = new FolderBrowserDialog();
            if (folderbrowserdialog.ShowDialog() == DialogResult.OK)
            {
                defaultImagePathTextBox.Text = folderbrowserdialog.SelectedPath;
            }
        }

        /// <summary>
        /// Saves the setting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosingProgramClick(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Saves the setting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFolderAfterSaveClick(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Saves the setting whenever a text is changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Saves the setting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetDefaultImageTypeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SaveSettings();
        }

        /// <summary>
        /// Check if the Filename contains the right characters
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        /// <author>Bernhard Bruns</author>
        private void DefaultImageName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (Regex.IsMatch(e.Text, @"[\\/:*?""<>|]"))
            {
                e.Handled = true;
                PopupTextBlock.Text = "A file Name can´t contain any of the following characters: / \\ : * ? < > |";
                txtPasswordPopup.IsOpen = true;
            }
            else
            {
                e.Handled = false;
                txtPasswordPopup.IsOpen = false;
            }

        }
    }
}