using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace pgmpm.MainV2.Utilities
{
    /// <summary>
    /// Provides static methods for error handling. This class is an abstraction layer, making the actual functionality exchangeable. 
    /// </summary>
    /// <author>Jannik Arndt</author>
    public class ErrorHandling
    {
        /// <summary>
        /// Initializes the Listener. This is called in MainWindow
        /// </summary>
        /// <author>Jannik Arndt</author>
        public static void Init()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("ErrorLog.log", "myListener"));
            Log("Program started.");
        }

        /// <summary>
        /// Opens a Popup-Window that prints out the error-message contained in the given exception
        /// </summary>
        /// <param Name="errorMessage">The message that will be displayed.</param>
        /// <author>Jannik Arndt, Bernhard Bruns, Bernd Nottbeck, Moritz Eversmann</author>
        public static void ReportErrorToUser(String errorMessage)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                Window activeWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
                if (activeWindow != null)
                    activeWindow.Topmost = false;
                ModernDialog.ShowMessage(errorMessage, "Error", MessageBoxButton.OK);
            })).Wait();
            LogError(errorMessage);
        }

        /// <summary>
        /// Appends an error to the log file (ErrorLog.log) in the same folder as the application.
        /// </summary>
        /// <param Name="ex">The exception that should be logged.</param>
        /// <author>Jannik Arndt</author>
        public static void LogError(Exception ex)
        {
            Log(ex.ToString());
        }

        /// <summary>
        /// Appends an error to the log file (ErrorLog.log) in the same folder as the application.
        /// </summary>
        /// <param Name="error">The error message. Exceptions are always preferred!</param>
        /// <author>Jannik Arndt</author>
        public static void LogError(String error)
        {
            Log(error);
        }

        /// <summary>
        /// Appends a string to the log file (ErrorLog.log) in the same folder as the application.
        /// </summary>
        /// <param name="message"></param>
        /// <author>Jannik Arndt</author>
        public static void Log(String message)
        {
            Trace.TraceInformation(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "  " + message);
            Trace.Flush();
        }
    }
}