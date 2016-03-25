using System;
using System.Windows;

namespace Metro_Screensaver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SettingsClass config = new SettingsClass();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void WindowSourceInitialized(object sender, EventArgs e)
        {
            this.blackCheckbox.IsChecked = Convert.ToBoolean(config.getConfig("BlackBackground"));
            this.blackTextCheckbox.IsChecked = Convert.ToBoolean(config.getConfig("BlackText"));
            this.blackTextCheckbox.IsEnabled = !Convert.ToBoolean(config.getConfig("BlackBackground"));
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void blackCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            config.setConfig(true, "BlackBackground");
            blackTextCheckbox.IsEnabled = false;
            saveSettings();
        }

        private void blackCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            config.setConfig(false, "BlackBackground");
            blackTextCheckbox.IsEnabled = true;
            saveSettings();
        }

        private void blackTextCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            config.setConfig(true, "BlackText");
            saveSettings();
        }

        private void blackTextCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            config.setConfig(false, "BlackText");
            saveSettings();
        }

        private void saveSettings()
        {
            config.saveConfig();
        }
    }
}
