﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OwnCloud.Resource.Localization;
using OwnCloud.Data;

namespace OwnCloud.View.Page
{
    public partial class CalendarSelectPage : PhoneApplicationPage
    {
        public CalendarSelectPage()
        {
            InitializeComponent();

            

            this.Unloaded += CalendarSelectPage_Unloaded;
        }

        void CalendarSelectPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_context != null)
                Context.Dispose();
        }

        #region Private Fields

        private Guid _userId;
        private Data.OwnCloudDataContext _context;
        private Data.OwnCloudDataContext Context
        {
            get
            {
                if (_context == null)
                    _context = new OwnCloudDataContext();
                return _context;
            }
        }

        private new CalendarListDataContext DataContext
        {
            get { return base.DataContext as CalendarListDataContext; }
        }

        #endregion

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Get userid in query
            if (NavigationContext.QueryString.ContainsKey("uid"))
                _userId = Guid.Parse(NavigationContext.QueryString["uid"]);
            else throw new ArgumentNullException("uid", AppResources.Exception_NoUserID);

            LoadCalendars();

            base.OnNavigatedTo(e);
        }

        /// <summary>
        /// Load all availible Calendars for a Event
        /// </summary>
        private void LoadCalendars()
        {
            base.DataContext = new CalendarListDataContext(_userId);
        }


        private void CalendarEnabledChange(object sender, RoutedEventArgs e)
        {
            var calendar = ((sender as FrameworkElement).DataContext as ServerCalendarDisplayInfo).CalendarInfo;

            if ((sender as ToggleSwitch).IsChecked ?? false)
                DataContext.EnableCalendar(calendar);
            else
                DataContext.DisableCalendar(calendar);

        }
    }
}