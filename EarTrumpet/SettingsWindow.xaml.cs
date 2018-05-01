﻿using EarTrumpet.DataModel;
using EarTrumpet.Services;
using EarTrumpet.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace EarTrumpet
{
    public partial class SettingsWindow : Window
    {
        public static SettingsWindow Instance;

        SettingsViewModel _viewModel;

        public SettingsWindow(IAudioDeviceManager manager)
        {
            InitializeComponent();

            Title = Properties.Resources.SettingsWindowText;
            _viewModel = new SettingsViewModel(manager);
            DataContext = _viewModel;

            ThemeService.ThemeChanged += UpdateTheme;

            SourceInitialized += (s, e) => UpdateTheme();

            Instance = this;
            Closing += (s, e) =>
            {
                 Instance = null;
                _viewModel.Save();
            };
        }

        ~SettingsWindow()
        {
            ThemeService.ThemeChanged -= UpdateTheme;
        }

        void UpdateTheme()
        {
            ThemeService.UpdateThemeResources(Resources);
        }

        internal void RaiseWindow()
        {
            Topmost = true;
            Activate();
            Topmost = false;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HotkeySelect_Click(object sender, RoutedEventArgs e)
        {
            var win = new HotkeySelectionWindow(_viewModel.Hotkey);
            win.Owner = this;
            win.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win.ShowDialog();
            _viewModel.Hotkey = win.Hotkey;
        }
    }
}