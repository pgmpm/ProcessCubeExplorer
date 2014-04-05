using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using pgmpm.Database;
using pgmpm.MiningAlgorithm;

namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// This static class provides helper-methods to easily create interface-elements and add them to the framework.
    /// </summary>
    /// <author>Jannik Arndt</author>
    public static class InterfaceHelpers
    {
        /// <summary>
        /// Help to navigate
        /// </summary>
        public static List<String> PageList = new List<string> { "/Pages/P1connection.xaml", "/Pages/P2metadata.xaml", "/Pages/P3dimensionselection.xaml", "/Pages/P4eventselection.xaml", "/Pages/P5configuration.xaml", "/Pages/P6mining.xaml", "/Pages/P8results.xaml" };
        public static String CurrentPage { get; set; }
        public static bool RestoreData { get; set; }
        public static bool MiningIsCompleted { get; set; }

        /// <summary>
        /// Create a Label and add it to the given DimensionPanel.
        /// </summary>
        /// <param name="text">The text (content) of the Label.</param>
        /// <param name="style">The Name of a style, e.g. "Emphasis" or "Heading2".</param>
        /// <param name="left">Left margin.</param>
        /// <param name="top">Top margin.</param>
        /// <param name="right">Right margin.</param>
        /// <param name="bottom">Bottom margin.</param>
        /// <param name="gridColumn"></param>
        /// <param name="gridRow"></param>
        /// <author>Jannik Arndt</author>
        public static Label CreateLabel(String text, String style = "", double left = 0, double top = 0, double right = 0, double bottom = 0, int gridColumn = -1, int gridRow = -1)
        {
            Label label = new Label
            {
                Content = text,
                Margin = new Thickness(left, top, right, bottom)
            };

            if (!String.IsNullOrEmpty(style))
                label.Style = Application.Current.TryFindResource(style) as Style;

            if (gridColumn >= 0)
                label.SetValue(Grid.ColumnProperty, gridColumn);

            if (gridRow >= 0)
                label.SetValue(Grid.RowProperty, gridRow);

            return label;
        }

        /// <summary>
        /// Create a TextBlock and add it to the given DimensionPanel.
        /// </summary>
        /// <param name="text">The text of the TextBlock.</param>
        /// <param name="style">The Name of a style, e.g. "Emphasis" or "Heading2".</param>
        /// <param name="left">Left margin.</param>
        /// <param name="top">Top margin.</param>
        /// <param name="right">Right margin.</param>
        /// <param name="bottom">Bottom margin.</param>
        /// <param name="gridColumn"></param>
        /// <param name="gridRow"></param>
        /// <author>Jannik Arndt</author>
        public static TextBlock CreateTextBlock(String text, String style = "", double left = 0, double top = 0, double right = 0, double bottom = 0, int gridColumn = -1, int gridRow = -1)
        {
            TextBlock block = new TextBlock
            {
                Text = text,
                Margin = new Thickness(left, top, right, bottom)
            };

            if (!String.IsNullOrEmpty(style))
                block.Style = Application.Current.TryFindResource(style) as Style;

            if (gridColumn >= 0)
                block.SetValue(Grid.ColumnProperty, gridColumn);

            if (gridRow >= 0)
                block.SetValue(Grid.RowProperty, gridRow);

            return block;
        }

        /// <summary>
        /// Create a TextBox and add it to the given DimensionPanel.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text">The text in the TextBox.</param>
        /// <param name="style"></param>
        /// <param name="left">Left margin.</param>
        /// <param name="top">Top margin.</param>
        /// <param name="right">Right margin.</param>
        /// <param name="bottom">Bottom margin.</param>
        /// <param name="gridColumn"></param>
        /// <param name="gridRow"></param>
        /// <author>Jannik Arndt</author>
        public static TextBox CreateTextBox(String name, String text = "", String style = "", double left = 0, double top = 0, double right = 0, double bottom = 0, int gridColumn = -1, int gridRow = -1)
        {
            TextBox box = new TextBox
            {
                Name = name,
                Text = text,
                Margin = new Thickness(left, top, right, bottom),
                Width = 150,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            return box;
        }

        /// <summary>
        /// Creates a Combobox
        /// </summary>
        /// <param name="name">Name of the box</param>
        /// <param name="style">Style</param>
        /// <param name="width">Width</param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="gridColumn">Grid column</param>
        /// <param name="gridRow">Grid row</param>
        /// <returns>A combo box object</returns>
        /// /// <author>Jannik Arndt</author>
        public static ComboBox CreateComboBox(String name, String style = "", double width = 0, double left = 0, double top = 0, double right = 0, double bottom = 0, int gridColumn = -1, int gridRow = -1)
        {
            ComboBox combo = new ComboBox
            {
                Name = name,
                Margin = new Thickness(left, top, right, bottom)
            };

            if (!String.IsNullOrEmpty(style))
                combo.Style = Application.Current.TryFindResource(style) as Style;

            if (width > 0)
                combo.Width = width;

            if (gridColumn >= 0)
                combo.SetValue(Grid.ColumnProperty, gridColumn);

            if (gridRow >= 0)
                combo.SetValue(Grid.RowProperty, gridRow);

            return combo;
        }

        /// <summary>
        /// Creates a button.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="text"></param>
        /// <param name="style"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="gridColumn"></param>
        /// <param name="gridRow"></param>
        /// <returns></returns>
        public static Button CreateButton(String name, String text, String style = "", double left = 0, double top = 0, double right = 0, double bottom = 0, int gridColumn = -1, int gridRow = -1)
        {
            Button button = new Button { Content = text, Name = name };

            if (!String.IsNullOrEmpty(style))
                button.Style = Application.Current.TryFindResource(style) as Style;
            else
                button.Style = new Style();
            button.Margin = new Thickness(left, top, right, bottom);

            if (gridColumn >= 0)
                button.SetValue(Grid.ColumnProperty, gridColumn);

            if (gridRow >= 0)
                button.SetValue(Grid.RowProperty, gridRow);
            return button;
        }

        /// <summary>
        /// Creates a separator
        /// </summary>
        /// <param name="name"></param>
        /// <param name="style"></param>
        /// <param name="height"></param>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <param name="gridColumn"></param>
        /// <param name="gridRow"></param>
        /// <returns></returns>
        public static Separator CreateSeparator(String name = "", String style = "", int height = 4, double left = 10, double top = 10, double right = 10, double bottom = 10, int gridColumn = -1, int gridRow = -1)
        {
            Separator separator = new Separator { Name = name };

            if (!String.IsNullOrEmpty(style))
                separator.Style = Application.Current.TryFindResource(style) as Style;

            separator.Height = height;
            separator.Margin = new Thickness(left, top, right, bottom);
            return separator;
        }

        /// <summary>
        /// Handle the navigation of the workflow
        /// </summary>
        /// <param name="navigateToPage"></param>
        /// <returns></returns>
        /// <author>Bernhard Bruns</author>
        public static bool CheckIfNavigationIsAllowed(String navigateToPage)
        {
            if (PageList.Contains(navigateToPage))
            {
                // [0] P1connection.xaml", 
                // [1] P2metadata.xaml", 
                // [2] P3dimensionselection.xaml", 
                // [3] P4eventselection.xaml", 
                // [4] P5configuration.xaml", 
                // [5] P6mining.xaml", 
                // [6] P8results.xaml"

                var indexPage = PageList.IndexOf(CurrentPage);
                var indexNavigateToPage = PageList.IndexOf(navigateToPage);
                var margin = indexNavigateToPage - indexPage;

                if (DBWorker.MetaData == null && !RestoreData && indexNavigateToPage != 0 && margin != 0 && MiningIsCompleted == false)
                    return false;

                if (indexNavigateToPage <= indexPage) // Allow always navigation to lower index
                    return true;

                if (margin == 1 && indexPage != 4 && indexPage != 5) // Allow navigation to next index, exception: consolidation, configuration  IndexNavigateToPage != 6 &&
                    return true;

                if (indexPage == 0 && indexNavigateToPage == 6 && RestoreData) // Exception: Restore saved results (From P1connection to P6results)
                    return true;

                if (indexPage == 5 && indexNavigateToPage == 6 && MiningIsCompleted) // Exception: Allow navigation only if Mining is complete
                    return true;

                if (indexPage == 4 && margin == 1 && MinerSettings.IsAlgorithmSet) //Exception: Allow navigation from P4configuration to P5mining only, if an algorithm has been selected
                    return true;
                return false;
            }

            return true;
        }
    }
}
