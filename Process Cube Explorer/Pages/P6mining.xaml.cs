using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;
using pgmpm.Database;
using pgmpm.Database.Exceptions;
using pgmpm.MainV2.Utilities;
using pgmpm.MatrixSelection;
using pgmpm.MatrixSelection.Fields;
using pgmpm.MiningAlgorithm;
using pgmpm.MiningAlgorithm.Exceptions;
using pgmpm.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace pgmpm.MainV2.Pages
{
    /// <summary>
    /// Interaction logic for P6mining.xaml
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public partial class P6mining : IContent
    {
        bool _isMinerRunning;
        CancellationToken _cancellationToken;
        CancellationTokenSource _cancellationTokenSource;
        ParallelOptions _parallelOption;

        public P6mining()
        {
            InitializeComponent();
        }

        #region Navigation
        void IContent.OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        void IContent.OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        void IContent.OnNavigatedTo(NavigationEventArgs e)
        {
            DBInformationButton.ToolTip = MainWindow.ConnectionName;
            InterfaceHelpers.CurrentPage = e.Source.OriginalString;

            // If Data selection was skipped or no fields chosen the mining should not try to start.
            if (MainWindow.MatrixSelection.MatrixFields.Count == 0)
            {
                StartCalculationButton.IsEnabled = false;
                MiningInfo.BBCode = "You need to select data before you start mining.";
            }
            // If no miner is chosen, don't try anything stupid.
            else if (!MinerSettings.IsAlgorithmSet)
            {
                StartCalculationButton.IsEnabled = false;
                MiningInfo.BBCode = "You need to select a mining algorithm before you start mining.";
            }
            else
            {
                StartCalculationButton.IsEnabled = true;
                MiningInfo.BBCode = "Click 'Start' to start mining " +
                    MainWindow.MatrixSelection.GetFields().Count +
                    " Fields using the " +
                    MinerSettings.MinerName + ".";
            }
        }

        void IContent.OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (InterfaceHelpers.CheckIfNavigationIsAllowed(e.Source.OriginalString) == false)
                e.Cancel = true;

            if (_isMinerRunning)
            {
                MessageBoxResult doNavigateAway =
                    ModernDialog.ShowMessage("Changing the view will cancel the mining-process. Continue?", "navigate",
                        MessageBoxButton.YesNo);
                if (doNavigateAway == MessageBoxResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (_cancellationTokenSource != null)
                _cancellationTokenSource.Cancel();

            Cursor = Cursors.Arrow;
            _isMinerRunning = false;
            ProgressBar.Visibility = Visibility.Hidden;

            StartCalculationButton.IsEnabled = true;
            CancelCalculationButton.IsEnabled = false;
            MiningInfo.BBCode = "Click 'Start' to start mining " +
                                MainWindow.MatrixSelection.GetFields().Count +
                                " Fields using the " +
                                MinerSettings.MinerName + ".";

            InterfaceHelpers.MiningIsCompleted = false;
        }
        #endregion

        #region Buttons
        private void PreviousClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/P5configuration.xaml", null);
            StartCalculationButton.Content = "Start";
            StartCalculationButton.Click += StartCalculationClick;
            StartCalculationButton.Click -= ContinueClick;
        }

        private void OpenSettingsClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/Settings.xaml", null);
        }
        private void OpenNoteClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/notes.xaml", null);
        }

        private void DBInformationClick(object sender, RoutedEventArgs e)
        {
            ModernDialog.ShowMessage(MainWindow.ConnectionName, "Database information", MessageBoxButton.OK);
        }

        private void ContinueClick(object sender, RoutedEventArgs e)
        {
            NavigationCommands.GoToPage.Execute("/Pages/P8results.xaml", null);
            StartCalculationButton.Content = "Start";
            StartCalculationButton.Click += StartCalculationClick;
            StartCalculationButton.Click -= ContinueClick;
        }

        private void CancelCalculationClick(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
            Cursor = Cursors.Arrow;
            _isMinerRunning = false;
            ProgressBar.Visibility = Visibility.Hidden;

            StartCalculationButton.IsEnabled = true;
            CancelCalculationButton.IsEnabled = false;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Click 'Start' to start mining ");
            stringBuilder.Append(MainWindow.MatrixSelection.GetFields().Count);
            stringBuilder.Append(" Fields using the ");
            stringBuilder.Append(MinerSettings.MinerName);
            stringBuilder.Append(".");

            MiningInfo.BBCode = stringBuilder.ToString();

            //MiningInfo.BBCode = "Click 'Start' to start mining " +
            //    MainWindow.MatrixSelection.GetFields().Count +
            //    " Fields using the " +
            //    MinerSettings.MinerName + ".";
        }
        #endregion


        #region Functionality
        /// <summary>
        /// Method to start the calculation of a miner.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <author>Christopher Licht, Thomas Meents, Bernhard Bruns, Bernd Nottbeck</author>
        private void StartCalculationClick(object sender, RoutedEventArgs e)
        {
            InitializeCalculation();

            //Threading (TPL) 
            Task loadAndMineTask = new Task(() =>
            {
                Calculate();
            }, _cancellationToken);

            //Starting the thread
            loadAndMineTask.Start();
        }


        /// <summary>
        /// Initialize the Mining (set controls etc.)
        /// </summary>
        private void InitializeCalculation()
        {
            Cursor = Cursors.Wait;
            ProgressBar.Visibility = Visibility.Visible;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            _parallelOption = new ParallelOptions();
            _parallelOption.CancellationToken = _cancellationTokenSource.Token;
            _parallelOption.MaxDegreeOfParallelism = System.Environment.ProcessorCount;

            _isMinerRunning = true;
            StartCalculationButton.IsEnabled = false;
            CancelCalculationButton.IsEnabled = true;
            MiningInfo.BBCode += "\nStart mining...";
            MiningInfoScrollViewer.ScrollToEnd();
        }

        private void Calculate()
        {
            try
            {
                if (MainWindow.MatrixSelection.SelectionHasChangedSinceLastLoading)
                    MainWindow.MatrixSelection.BuildMatrixFields();

                SetProgressbarInThread(ProgressBar, 0, MainWindow.MatrixSelection.MatrixFields.Count * 2 + 1, 1);


                if (!_cancellationToken.IsCancellationRequested)
                {
                    LoadDataFromDatabase();

                    if (!_cancellationToken.IsCancellationRequested)
                    {
                        Mine(MainWindow.MatrixSelection.MatrixFields);

                        if (!_cancellationToken.IsCancellationRequested)
                        {
                            SetCalculationControlsInThread();

                            _isMinerRunning = false;
                            InterfaceHelpers.MiningIsCompleted = true;
                        }
                    }
                }
            }
            catch (NoConnectionException Ex)
            {
                ErrorHandling.ReportErrorToUser("Error: " + Ex.Message + Ex.StackTrace);
            }
            catch (NullReferenceException Ex)
            {
                ErrorHandling.ReportErrorToUser("Error: " + Ex.Message + Ex.StackTrace);
            }
            catch (ArgumentNullException Ex)
            {
                ErrorHandling.ReportErrorToUser("Error: " + Ex.Message + Ex.StackTrace);
            }
            catch (Exception Ex)
            {
                ErrorHandling.ReportErrorToUser("Error: " + Ex.Message + Ex.StackTrace);
            }
            finally
            {
                Dispatcher.BeginInvoke((Action)(() =>
                    {
                        ProgressBar.Visibility = Visibility.Hidden;
                        Cursor = Cursors.Arrow;
                    }));
            }
        }

        /// <summary>
        /// Set the progress bar while threading
        /// </summary>
        /// <param name="bar"></param>
        /// <param name="minimum"></param>
        /// <param name="maximum"></param>
        /// <param name="value"></param>
        private void SetProgressbarInThread(ProgressBar bar, int minimum, int maximum, int value)
        {
            Dispatcher.BeginInvoke((Action)(() =>
             {
                 bar.Minimum = minimum;
                 bar.Maximum = maximum;
                 bar.Value = value;
             }));
        }

        /// <summary>
        /// Set the calculation controls while threading
        /// </summary>
        private void SetCalculationControlsInThread()
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                MiningInfo.BBCode += "\nMining completed!";
                MiningInfoScrollViewer.ScrollToEnd();
                StartCalculationButton.Content = "Continue";
                StartCalculationButton.Click -= StartCalculationClick;
                StartCalculationButton.Click += ContinueClick;
                StartCalculationButton.IsEnabled = true;
                CancelCalculationButton.IsEnabled = false;
                ProgressBar.Visibility = Visibility.Hidden;
                Cursor = Cursors.Arrow;
            }));
        }
        /// <summary>
        /// This function loads the data from the database into the fields and update the user interface
        /// </summary>
        /// <author>Thomas Meents, Bernhard Bruns</author>
        private void LoadDataFromDatabase()
        {
            string connectionName = DBWorker.GetConnectionName();

            Dispatcher.BeginInvoke((Action)(() =>
            {
                MiningInfo.BBCode += "\nLoading data from " + connectionName;
                MiningInfoScrollViewer.ScrollToEnd();
            }));

            if (MainWindow.MatrixSelection.SelectionHasChangedSinceLastLoading || EventSelectionModel.GetInstance().SelectionHasChangedSinceLastLoading)
            {
                foreach (Field field in MainWindow.MatrixSelection.MatrixFields)
                {
                    if (_cancellationToken.IsCancellationRequested)
                        break;

                    DBConnectionHelpers.LoadFactsInField(field);

                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        ProgressBar.Value += 1;
                    }));
                }
                MainWindow.MatrixSelection.SelectionHasChangedSinceLastLoading = false;
                EventSelectionModel.GetInstance().SelectionHasChangedSinceLastLoading = false;
            }
        }

        /// <summary>
        /// Start the miner on a list of fields and save the results in a dictionary
        /// </summary>
        /// <param name="fields"></param>
        /// <author>Bernhard Bruns</author>
        public void Mine(List<Field> fields)
        {
            int fieldCounter = 1;


            Parallel.ForEach(fields, field =>
            {
                try
                {
                    field.ResetInformation();
                    IMiner iMiner = MinerFactory.CreateMiner(MinerSettings.MinerName, field);
                    ProcessModel resultingProcessModel = iMiner.Mine();

                    // Save petrinet in the field object
                    field.ProcessModel = resultingProcessModel;
                    Dispatcher.BeginInvoke((Action)(() => { MiningInfo.BBCode += "\n[b]Mining field " + fieldCounter++ + " of " + fields.Count + "[/b]" + "\n[color=#00E600]Successfully mined.[/color]"; MiningInfoScrollViewer.ScrollToEnd(); })).Wait();
                }
                catch (ArgumentNullException Ex)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        MiningInfo.BBCode += "\n[b]Mining field " + fieldCounter++ + " of " + fields.Count + "[/b]" + "\nERROR: Parameter " + Ex.ParamName + " was not given:\n" + Ex.Message; MiningInfoScrollViewer.ScrollToEnd();
                    })).Wait();
                }
                catch (Exception Ex)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        MiningInfo.BBCode += "\n[b]Mining field " + fieldCounter++ + " of " + fields.Count + "[/b]" + "\n[color=#FF0000]ERROR: [/color]" + Ex.Message; MiningInfoScrollViewer.ScrollToEnd();
                    })).Wait();
                }
                finally
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        ProgressBar.Value += 1;
                    })).Wait();

                    _parallelOption.CancellationToken.ThrowIfCancellationRequested();
                }
            });
        }
    }
        #endregion
}