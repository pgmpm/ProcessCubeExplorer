/*
 * Copyright 2014 Projektgruppe MPM. All Rights Reserved.
 *
 * This file is part of Process Cube Explorer.
 *
 * Process Cube Explorer is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Process Cube Explorer is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Process Cube Explorer. If not, see <http://www.gnu.org/licenses/>.
 *
 */

using FirstFloor.ModernUI.Windows.Controls;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection;
using pgmpm.MiningAlgorithm;
using System;
using System.Windows;
using System.Windows.Input;

namespace pgmpm.MainV2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        /// <summary>
        /// Statically stores the selected Dimensions.
        /// </summary>
        public static MatrixSelectionModel MatrixSelection = new MatrixSelectionModel();
        public static string ConnectionName;

        public MainWindow()
        {
            InitializeComponent();
            // Initialize the ErrorHandler as a singleton
            ErrorHandling.Init();

            PreviewKeyDown += HandleShortcuts;
        }

        #region shortcuts
        /// <summary>
        /// handle Shortcuts
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        /// <author>Thomas Meents, Bernd Nottbeck, Jannik Arndt</author>
        private void HandleShortcuts(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                switch (e.Key)
                {
                    case Key.R:
                        Cursor = Cursors.Wait;
                        try
                        {
                            DBConnectionHelpers.LoadConnectionParameters();
                            DBConnectionHelpers.EstablishDatabaseConnection(DBConnectionHelpers.LoadLastUsedDatabase());
                        }
                        finally
                        {
                            Cursor = Cursors.Arrow;
                        }
                        break;
                }
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Enter:
                        switch (ContentSource.ToString())
                        {
                            case ("/Pages/P1connection.xaml"):
                                ContentSource = new Uri("/Pages/P2metadata.xaml", UriKind.RelativeOrAbsolute);
                                break;
                            case ("/Pages/P2metadata.xaml"):
                                ContentSource = new Uri("/Pages/P3dimensionselection.xaml", UriKind.RelativeOrAbsolute);
                                break;
                            case ("/Pages/P3dimensionselection.xaml"):
                                ContentSource = new Uri("/Pages/P4eventselection.xaml", UriKind.RelativeOrAbsolute);
                                break;
                            case ("/Pages/P4eventselection.xaml"):
                                ContentSource = new Uri("/Pages/P5configuration.xaml", UriKind.RelativeOrAbsolute);
                                break;
                            case ("/Pages/P5configuration.xaml"):
                                if (MinerSettings.IsAlgorithmSet)
                                    ContentSource = new Uri("/Pages/P6mining.xaml", UriKind.RelativeOrAbsolute);
                                break;
                            case ("/Pages/P6mining.xaml"):
                                // Start / Continue
                                break;
                            case ("/Pages/P7consolidation.xaml"):
                                ContentSource = new Uri("/Pages/P8results.xaml", UriKind.RelativeOrAbsolute);
                                break;
                        }
                        break;

                    case Key.F11:
                        if (WindowState.Equals(WindowState.Maximized))
                            ViewRegularscreen();
                        else
                            ViewFullscreen();
                        break;
                }
            }
        }
        #endregion


        #region Screen
        /// <summary>
        /// Shows the framework in fullscreen
        /// </summary>
        /// <author>Thomas Meents, Bernd Nottbeck</author>
        private void ViewFullscreen()
        {
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Minimized;
            WindowState = WindowState.Maximized;
            Activate();
        }

        /// <summary>
        /// Shows the framework in regular screen
        /// </summary>
        /// <author>Thomas Meents, Bernd Nottbeck</author>
        private void ViewRegularscreen()
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Normal;
        }
        #endregion
    }
}
