using pgmpm.MatrixSelection.Fields;
using System;
using System.Windows;
using System.Windows.Input;

namespace pgmpm.MainV2.Viewer
{
            

    /// <summary>
    /// Interaction logic for Viewer.xaml
    /// </summary>
    public partial class Viewer
    {
        public static Field CurrentField;
        public Viewer(Field field, bool isDiff = false, string headingtext = "")
        {
            if (field.ProcessModel == null)
                throw new ArgumentNullException("field", @"No processmodel can be found for this field");

            InitializeComponent();

            CurrentField = field;

            PreviewKeyDown += HandleShortcuts;

            //Topmost = false;
        }

        public void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void HandleShortcuts(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
                Close();
        }

    }
}
