﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using OwnCloud;
using OwnCloud.Data;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Ocwp.Controls
{
    public partial class CalendarDayControl : UserControl
    {
        private DateTime _targetDate;

        public CalendarDayControl()
        {
            InitializeComponent();
        }

        public DateTime TargetDate
        {
            get { return _targetDate; }
            set { _targetDate = value.Date; }
        }

        public Guid AccountID { get; set; }
        
        private void ContextMenu_OnOpened(object sender, RoutedEventArgs e)
        {
            var context = new OwnCloudDataContext();

            var events = context.Events.Where(o => o.To > _targetDate && o.From < _targetDate.AddDays(1)).ToArray();
            context.Dispose();

            if (events.Length == 0) return;

            CmMenu.Items.Clear();

            foreach (var tableEvent in events)
            {
                var item = new MenuItem {Header = tableEvent.Title, DataContext = tableEvent};
                item.Click += EventClick;

                CmMenu.Items.Add(item);
            }
            if (CmMenu.Items.Count == 0)
                CmMenu.IsEnabled = false;
        }

        void EventClick(object sender, RoutedEventArgs e)
        {
            var dbEvent = (sender as FrameworkElement).DataContext as TableEvent;

            Guid accountID = Guid.Empty;
            using (var context = new OwnCloudDataContext())
            {
                accountID = context.Calendars.Where(o => o.Id == dbEvent.CalendarId).Single()._accountId ?? Guid.Empty;
            }

            //TODO: Import Edit Page..
            App.Current.RootFrame.Navigate(new Uri("/View/Page/AppointmentPage.xaml?url=" + dbEvent.Url + "&uid=" + accountID.ToString(), UriKind.Relative));
        }


        #region Styling Functions

        private void BeginOverStyle()
        {
            this.LayoutRoot.Background = new SolidColorBrush(Color.FromArgb(150, 255, 255, 255));
        }

        private void EndOverStyle()
        {
            this.LayoutRoot.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void CalendarDayControl_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BeginOverStyle();
        }

        private void CalendarDayControl_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            EndOverStyle();
        }

        private void CalendarDayControl_OnMouseLeave(object sender, MouseEventArgs e)
        {
            EndOverStyle();
        }

        private void CalendarDayControl_OnLostMouseCapture(object sender, MouseEventArgs e)
        {
            EndOverStyle();
        }

        #endregion

        private void LayoutRoot_OnTap(object sender, GestureEventArgs e)
        {
            App.Current.RootFrame.Navigate(new Uri("/View/Page/CalendarDayPage.xaml?uid=" + AccountID.ToString() + "&startDate="+TargetDate.ToShortDateString(),
                                                   UriKind.Relative));
        }
    }
}