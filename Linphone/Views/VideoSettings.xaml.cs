﻿/*
VideoSettings.xaml.cs
Copyright (C) 2015  Belledonne Communications, Grenoble, France
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.
This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
*/

using BelledonneCommunications.Linphone.Native;
using Linphone.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Linphone.Views
{
    /// <summary>
    /// Page displaying the video settings and the video codecs to let the user enable/disable them.
    /// </summary>
    public partial class VideoSettings : Page
    {
        private CallSettingsManager _callSettings = new CallSettingsManager();
        private CodecsSettingsManager _codecsSettings = new CodecsSettingsManager();
        private bool saveSettingsOnLeave = true;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public VideoSettings()
        {
            this.InitializeComponent();

            _callSettings.Load();
            VideoEnabled.IsOn = (bool) _callSettings.VideoEnabled;
            AutomaticallyInitiateVideo.IsOn = (bool) _callSettings.AutomaticallyInitiateVideo;
            AutomaticallyAcceptVideo.IsOn = (bool) _callSettings.AutomaticallyAcceptVideo;
            SelfViewEnabled.IsOn = (bool) _callSettings.SelfViewEnabled;

            foreach(VideoSize size in LinphoneManager.Instance.Core.SupportedVideoSizes)
            {
                Debug.WriteLine(size.Name);
            }

            List<string> videoSizes = new List<string>
            {
                "vga",
                "qvga"
            };
            PreferredVideoSize.ItemsSource = videoSizes;
            PreferredVideoSize.SelectedItem = _callSettings.PreferredVideoSize;

            _codecsSettings.Load();
            H264.IsOn = _codecsSettings.H264;
            VP8.IsOn = _codecsSettings.VP8;
        }

        /// <summary>
        /// Method called when the user is navigation away from this page
        /// </summary>
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (saveSettingsOnLeave)
            {
                Save();
            }
            base.OnNavigatingFrom(e);
        }

        private void Save()
        {
            _codecsSettings.H264 = ToBool(H264.IsOn);
            _codecsSettings.VP8 = ToBool(VP8.IsOn);
            _codecsSettings.Save();

            _callSettings.VideoEnabled = ToBool(VideoEnabled.IsOn);
            _callSettings.AutomaticallyInitiateVideo = ToBool(AutomaticallyInitiateVideo.IsOn);
            _callSettings.AutomaticallyAcceptVideo = ToBool(AutomaticallyAcceptVideo.IsOn);
            _callSettings.SelfViewEnabled = ToBool(SelfViewEnabled.IsOn);
            _callSettings.PreferredVideoSize = PreferredVideoSize.SelectedItem.ToString();
            if (PreferredVideoSize.SelectedItem.ToString() == "vga")
            {
                _callSettings.DownloadBandwidth = _callSettings.UploadBandwidth = 512;
            }
            else
            {
                _callSettings.DownloadBandwidth = _callSettings.UploadBandwidth = 380;
            }
            _callSettings.Save();
        }

        private void cancel_Click_1(object sender, EventArgs e)
        {
            saveSettingsOnLeave = false;
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private bool ToBool(bool? enabled)
        {
            if (!enabled.HasValue) enabled = false;
            return (bool)enabled;
        }

        private void save_Click_1(object sender, EventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}