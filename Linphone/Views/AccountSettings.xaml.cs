﻿using Linphone.Model;
using Linphone.Resources;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Linphone.Views
{
    /// <summary>
    /// Page used to display SIP account settings
    /// </summary>
    public partial class AccountSettings : BasePage
    {
        private SIPAccountSettingsManager _settings = new SIPAccountSettingsManager();

        private bool _editPassword = true;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public AccountSettings()
        {
            InitializeComponent();
            BuildLocalizedApplicationBar();

            _settings.Load();
            Username.Text = _settings.Username;
            Password.Password = _settings.Password;
            Domain.Text = _settings.Domain;
            Proxy.Text = _settings.Proxy;
            OutboundProxy.IsChecked = _settings.OutboundProxy;
            DisplayName.Text = _settings.DisplayName;
        }

        /// <summary>
        /// Method called when the page is displayed.
        /// </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // Create LinphoneCore if not created yet, otherwise do nothing
            await LinphoneManager.Instance.InitLinphoneCore();
        }

        private void cancel_Click_1(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        private void save_Click_1(object sender, EventArgs e)
        {
            if (Domain.Text.Contains(":"))
            {
                if (Proxy.Text.Length == 0)
                {
                    Proxy.Text = Domain.Text;
                }
                Domain.Text = Domain.Text.Split(':')[0];
            }

            _settings.Username = Username.Text;
            _settings.Password = Password.Password;
            _settings.Domain = Domain.Text;
            _settings.Proxy = Proxy.Text;
            _settings.OutboundProxy = OutboundProxy.IsChecked;
            _settings.DisplayName = DisplayName.Text;
            _settings.Save();

            NavigationService.GoBack();
        }

        private void BuildLocalizedApplicationBar()
        {
            ApplicationBar = new ApplicationBar();

            ApplicationBarIconButton appBarSave = new ApplicationBarIconButton(new Uri("/Assets/AppBar/save.png", UriKind.Relative));
            appBarSave.Text = AppResources.SaveSettings;
            ApplicationBar.Buttons.Add(appBarSave);
            appBarSave.Click += save_Click_1;

            ApplicationBarIconButton appBarCancel = new ApplicationBarIconButton(new Uri("/Assets/AppBar/cancel.png", UriKind.Relative));
            appBarCancel.Text = AppResources.CancelChanges;
            ApplicationBar.Buttons.Add(appBarCancel);
            appBarCancel.Click += cancel_Click_1;
        }

        private void Username_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Password.Focus();
        }

        private void Password_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Domain.Focus();
        }

        private void Domain_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Proxy.Focus();
        }

        private void Proxy_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                DisplayName.Focus();
        }
    }
}