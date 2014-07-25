﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OwnCloud.Data;
using OwnCloud.View.Controls;
using OwnCloud.Extensions;
using OwnCloud.WebDAV;
using System.Threading.Tasks;

namespace OwnCloud.View.Page
{
    public partial class RemoteFiles : PhoneApplicationPage
    {

        private Account _workingAccount;
        private FileListDataContext _context;
        private string[] _views = { "detail", "tile" };
        private int _viewIndex = 0;
        private string _workingPath = "";

        public RemoteFiles()
        {
            InitializeComponent();
            _context = new FileListDataContext();
            DataContext = _context;
            // Translate unsupported XAML bindings
            // ApplicationBar.TranslateButtons();
        }

        private void ToggleTray()
        {
            Dispatcher.BeginInvoke(() =>
            {
                SystemTray.SetIsVisible(this, !SystemTray.IsVisible);
            });
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _workingAccount = App.DataContext.LoadAccount(NavigationContext.QueryString["account"]);
                _workingAccount.RestoreCredentials();
            }
            catch (Exception)
            {
                // should not happen
            }
            ApplicationBar = (ApplicationBar)Resources["DefaultAppBar"];
            ApplicationBar.TranslateButtons();
            FetchStructure(_workingAccount.WebDAVPath);
        }

        private void ChangeView(object sender, EventArgs e)
        {
            if (_viewIndex + 1 < _views.Length)
            {
                ++_viewIndex;
            }
            else
            {
                _viewIndex = 0;
            }
            FetchStructure(_workingPath);
        }

        private WebDAVFile _item;

        /// <summary>
        /// Tries to fetch a given path and refreshes the views.
        /// </summary>
        /// <param name="path"></param>
        private void FetchStructure(string path)
        {
            string viewMode = _views[_viewIndex];
            _workingPath = path;

            progress.IsVisible = true;
            progress.Text = "Fetching Structure...";

            switch (viewMode)
            {
                case "tile":
                    DetailList.Hide();
                    DetailList.Items.Clear();
                    TileViewContainer.Show();

                    // fadeout existig from tile view
                    if (TileView.Children.Count == 0)
                    {
                        StartRequest();
                    }
                    else
                    {
                        int itemsLeft = TileView.Children.Count;
                        foreach (FrameworkElement item in TileView.Children)
                        {
                            item.FadeOut(100, () =>
                            {
                                --itemsLeft;
                                if (itemsLeft <= 0)
                                {
                                    TileView.Children.Clear();
                                    StartRequest();
                                }
                            });
                        }
                    }
                    break;

                case "detail":
                    TileViewContainer.Hide();
                    DetailList.Show();
                    // fadeout existing from detail view
                    if (DetailList.Items.Count == 0)
                    {
                        StartRequest();
                    }
                    else
                    {
                        int detailItemsLeft = DetailList.Items.Count;
                        foreach (FrameworkElement item in DetailList.Items)
                        {
                            item.FadeOut(100, () =>
                            {
                                --detailItemsLeft;
                                if (detailItemsLeft <= 0)
                                {
                                    DetailList.Items.Clear();
                                    StartRequest();
                                }
                            });
                        }
                    }
                    break;
            }

            // start overlay
        }

        private async Task StartRequest()
        {
            var dav = new WebDAVClient(_workingAccount.GetUri(), _workingAccount.GetCredentials());
            try {
                var entries = await dav.GetEntries(_workingPath, true);
                if (entries.Count == 0) throw new Exception("No entries found");
                bool _firstItem = false;
                // display all items linear
                // we cannot wait till an item is displayed, instead for a fluid
                // behaviour we should calculate fadeIn-delays.
                int delayStart = 0;
                int delayStep = 50; // ms

                foreach (var item in entries) {
                    File fileItem = new File() {
                        FileName = item.FileName,
                        FilePath = item.FilePath.ToString(),
                        FileSize = item.Size,
                        FileType = item.MimeType,
                        FileCreated = item.Created,
                        FileLastModified = item.LastModified,
                        IsDirectory = item.IsDirectory
                    };

                    bool display = true;

                    Dispatcher.BeginInvoke(() => {

                        switch (_views[_viewIndex]) {
                            case "detail":
                                if (!_firstItem) {
                                    _firstItem = true;

                                    // Root
                                    if (fileItem.IsDirectory) {
                                        if (item.FilePath.ToString() == _workingAccount.WebDAVPath) {
                                            // cannot go up further
                                            display = false;
                                        } else {
                                            fileItem.IsRootItem = true;
                                            fileItem.FilePath = fileItem.FileParentPath;
                                        }
                                    }
                                }

                                if (display) {
                                    FileDetailViewControl detailControl = new FileDetailViewControl() {
                                        DataContext = fileItem,
                                        Opacity = 0,
                                        Background = new SolidColorBrush() { Color = Colors.Transparent },
                                    };

                                    DetailList.Items.Add(detailControl);
                                    detailControl.Delay(delayStart, () => {
                                        detailControl.FadeIn(100);
                                    });
                                    delayStart += delayStep;
                                }
                                break;

                            case "tile":
                                if (!_firstItem) {
                                    _firstItem = true;

                                    // Root
                                    if (fileItem.IsDirectory) {
                                        if (item.FilePath.ToString() == _workingAccount.WebDAVPath) {
                                            // cannot go up further
                                            display = false;
                                        } else {
                                            fileItem.IsRootItem = true;
                                            fileItem.FilePath = fileItem.FileParentPath;
                                        }
                                    }
                                }

                                if (display) {
                                    FileMultiTileViewControl multiControl = new FileMultiTileViewControl(_workingAccount, fileItem, true) {
                                        Width = 200,
                                        Height = 200,
                                        Opacity = 0,
                                        Margin = new Thickness(0, 0, 10, 10),
                                    };
                                    multiControl.Tap += new EventHandler<System.Windows.Input.GestureEventArgs>(TileListSelectionChanged);

                                    // sometimes the exception "wrong parameter" is thrown - but why???
                                    TileView.Children.Add(multiControl);
                                    multiControl.Delay(delayStart, () => {
                                        multiControl.FadeIn(100);
                                    });
                                }

                                break;
                        }
                    });
                }

                Dispatcher.BeginInvoke(() => {
                    progress.IsVisible = false;
                });
            } catch (Exception ex) {
                Dispatcher.BeginInvoke(() => {
                    progress.IsVisible = false;
                    var webException = ex as WebException;
                    var webResponse = webException != null ? webException.Response as HttpWebResponse : null;
                    if (webException != null && 
                        webException.Status == WebExceptionStatus.ProtocolError &&
                        webResponse != null &&
                        webResponse.StatusCode == HttpStatusCode.Unauthorized) {
                        MessageBox.Show("FetchFile_Unauthorized".Translate(), "Error_Caption".Translate(), MessageBoxButton.OK);
                    } else {
                        MessageBox.Show("FetchFile_Unexpected_Result".Translate(), "Error_Caption".Translate(), MessageBoxButton.OK);
                    }
                });
            }
        }

        private void TileListSelectionChanged(object sender, EventArgs e)
        {
            FileMultiTileViewControl item = sender as FileMultiTileViewControl;
            OpenItem(item.FileProperties);
        }

        private void DetailListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DetailList.Items.Count == 0) return;

            FileDetailViewControl item = (FileDetailViewControl)(DetailList.SelectedItem);
            OpenItem(item.FileProperties);
        }

        private void OpenItem(File item)
        {
            if (item.IsDirectory)
            {
                FetchStructure(item.FilePath);
            }
            else
            {
                //todo: win8 file+uri associations callers
                //todo: open file
            }
        }
    }
}