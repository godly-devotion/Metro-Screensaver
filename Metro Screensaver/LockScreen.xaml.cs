using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;

namespace Metro_Screensaver
{
    /// <summary>
    /// Interaction logic for LockScreen.xaml
    /// </summary>
    public partial class LockScreen : UserControl
    {
        private SettingsClass config = new SettingsClass();
        public event EventHandler Unlocked;
        private DispatcherTimer timer;

        public LockScreen()
        {
            InitializeComponent();
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            
            if (Convert.ToBoolean(config.getConfig("BlackBackground")))
            {
                mainGrid.Background = new SolidColorBrush(Colors.Black);
            }
            else
            {
                // set font colors
                if (Convert.ToBoolean(config.getConfig("BlackText")))
                    this.Foreground = new SolidColorBrush(Colors.Black);
                
                // set wallpaper
                var wpReg = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);
                var wallpaperPath = wpReg.GetValue("WallPaper").ToString();
                wpReg.Close();
                
                var bi = new BitmapImage();
                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnLoad;
                bi.UriSource = new Uri(wallpaperPath);
                bi.EndInit();
                ////Windows 7
                //if (Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 1)
                //{
                //    var wpReg = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Desktop\\General\\", false);
                //    wallpaperPath = wpReg.GetValue("WallpaperSource").ToString();
                //    wpReg.Close();
                //}
                //else
                //{
                //    //Windows XP
                //    var wpReg = Registry.CurrentUser.OpenSubKey("Control Panel\\Desktop", false);
                //    wallpaperPath = wpReg.GetValue("Wallpaper").ToString();
                //    wpReg.Close();
                //}
                LockScreenBg.Source = bi;// new BitmapImage(new Uri(wallpaperPath));
            }
      
            Day.Text = DateTime.Now.ToString("dddd");
            Day.Text = char.ToUpper(Day.Text[0]) + Day.Text.Substring(1);
            Month.Text = DateTime.Now.ToString("MMMM") + " " + DateTime.Now.Day;
            Time.Text = DateTime.Now.ToShortTimeString();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            timer.Start();
            Translate.Y = -SystemParameters.PrimaryScreenHeight;

            var s = (Storyboard)Resources["MoveBackAnim"];
            s.Begin();
        }

        void TimerTick(object sender, EventArgs e)
        {
            Time.Text = DateTime.Now.ToShortTimeString();
        }

        private double mouseY;

        private void UserControlMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(this);
            mouseY = e.GetPosition((IInputElement)this.Parent).Y;
        }

        private void UserControlMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
            mouseY = 0;
            if (Translate.Y == 0)
            {
                var s = (Storyboard)Resources["JumpAnim"];
                s.Begin();
                return;
            }
            if (Translate.Y > -120)
            {
                var s = (Storyboard)Resources["MoveBackAnim"];
                s.Begin();
                return;
            }
            Unlock();
        }

        private void UserControlMouseMove(object sender, MouseEventArgs e)
        {
            if (Mouse.Captured != this || e.LeftButton == MouseButtonState.Released)
                return;
            var y = e.GetPosition((IInputElement) this.Parent).Y;
            if (y >= mouseY)
                return;
            Translate.Y = y - mouseY;
        }

        private void UnlockAnimCompleted(object sender, EventArgs e)
        {
            if (Unlocked != null)
                Unlocked(this, EventArgs.Empty);
            Application.Current.Shutdown();
        }

        private void UserControlUnloaded(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        private void MoveBackAnimCompleted(object sender, EventArgs e)
        {
            Translate.Y = 0;
        }

        public void Unlock()
        {
            var s = (Storyboard)Resources["UnlockAnim"];
            ((DoubleAnimation)s.Children[0]).To = -SystemParameters.PrimaryScreenHeight;
            s.Begin();
            timer.Stop();
        }

        private void JumpAnimCompleted(object sender, EventArgs e)
        {
            Translate.Y = 0;
        }
    }
}
