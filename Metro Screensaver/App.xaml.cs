using System;
using System.Windows;
using System.Windows.Media;

namespace Metro_Screensaver
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            bool ok;
            var m = new System.Threading.Mutex(true, "Joshua Park", out ok);

            if (!ok)
                Application.Current.Shutdown(); // another instance already running

            base.OnStartup(e);

            if (e.Args.Length > 0)
            {
                // Get the 2 character command line argument
                string arg = e.Args[0].ToLowerInvariant().Trim().Substring(0, 2);
                switch (arg)
                {
                    case "/c":
                        // Show the options dialog
                        ShowOptions();
                        break;
                    case "/p":
                        // Don't do anything for preview
                        Application.Current.Shutdown();
                        break;
                    case "/s":
                        // Show screensaver form
                        ShowScreenSaver();
                        break;
                    case "/d":
                        // debug mode
                        MessageBox.Show("Debug mode has not been implemented!", "Warning");
                        break;
                    default:
                        MessageBox.Show("Invalid command line argument: " + arg, "Invalid Command Line Argument");
                        break;
                }
            }
            else
            {
                ShowScreenSaver();
            }
            // keep m free from garbage collection
            GC.KeepAlive(m);
        }

        public static void ShowOptions()
        {
            new MainWindow().ShowDialog();
        }

        public static void ShowScreenSaver()
        {
            var LockWndw = new LockWindow {
                Topmost = true,
                AllowsTransparency = true,
                Background =
                    new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00000000"))
            };
            var LockScrn = new LockScreen();
            //hubContent.Unlocked += HubContentUnlocked;
            LockWndw.Content = LockScrn;
            //LockWndw.KeyUp += LockScrn.u;

            LockWndw.ShowDialog();
        }
    }
}
