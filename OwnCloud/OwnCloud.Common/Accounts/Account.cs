using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace OwnCloud.Common.Accounts {
    public class Account {
        public Account() {
            Name = "ownCloud";
            WebDAVPath = "/owncloud/remote.php/webdav/";
            CalDAVPath = "/owncloud/remote.php/caldav/";
            Protocol = "https";
            UsernamePlain = null;
            PasswordPlain = null;
            ServerDomain = "";
        }

        /// <summary>
        /// Primary Key
        /// </summary>
        [DataMember]
        public Guid GUID { get; set; }

        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Server Domain to connect to
        /// </summary>
        ///
        [DataMember]
        public string ServerDomain { get; set; }

        /// <summary>
        /// Only returns the hostname
        /// </summary>
        public string Hostname {
            get {
                return ServerDomain.IndexOf(':') != -1 ? ServerDomain.Substring(0, ServerDomain.IndexOf(':')) : ServerDomain;
            }
        }

        /// <summary>
        /// Returns a port if given in ServerDomain-string or the standard port number depending on context.
        /// </summary>
        public int GetPort(bool secure = false) {
            int p = 0;
            Int32.TryParse(ServerDomain.Split(':').Last(), out p);
            return p == 0 ? (secure ? 443 : 80) : p;
        }

        /// <summary>
        /// The used protocol. http or https only.
        /// </summary>
        [DataMember]
        public string Protocol { get; set; }

        /// <summary>
        /// An username for the account
        /// </summary>
        [DataMember]
        public string Username { get; set; }

        private string _usernamePlain;
        public string UsernamePlain {
            get { return _usernamePlain; }
            set {
                if (!string.IsNullOrEmpty(_usernamePlain))
                    Username = string.Empty;
                _usernamePlain = value;
            }
        }
        /// <summary>
        /// // Password for account
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        private string _passwordPlain;
        public string PasswordPlain {
            get { return _passwordPlain; }
            set {
                if (!string.IsNullOrEmpty(_passwordPlain))
                    Password = string.Empty;
                _passwordPlain = value;
            }
        }


        /// <summary>
        /// Returns a uri from the used server domain and protocol without trailing slash.
        /// </summary>
        /// <returns></returns>
        public Uri GetUri() {
            return new Uri(Protocol + "://" + ServerDomain.Trim('/'), UriKind.Absolute);
        }

        /// <summary>
        /// Returns a uri from the used server
        /// </summary>
        /// <returns></returns>
        public Uri GetUri(string path) {
            return new Uri(Protocol + "://" + ServerDomain.Trim('/') + "/" + path.Trim('/'), UriKind.Absolute);
        }

        /// <summary>
        /// Returns the caldav uri
        /// </summary>
        public Uri GetCalDavUri() {
            return new Uri(GetUri(CalDAVPath).AbsoluteUri + "/" + UsernamePlain + "/");
        }

        /// <summary>
        /// Path to WebDAV-Listening, usually /remote.php/webdav/
        /// </summary>
        [DataMember]
        public string WebDAVPath { get; set; }

        /// <summary>
        /// Path to CalDAV-Listening, usually /remote.php/caldav/
        /// </summary>
        [DataMember]
        public string CalDAVPath { get; set; }

        [DataMember]
        public List<int> Calendars { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsAnonymous { get; set; }

        /// <summary>
        /// Determines if all required settings are set.
        /// If not at least one messagebox is shown to the user.
        /// </summary>
        public bool CanSave() {
            bool canSave = true;
            switch (Protocol) {
                case "http":
                case "https":
                    // ok
                    break;
                default:
                    //MessageBox.Show("Model_Account_Protocol_Unsupported".Translate(Protocol));
                    canSave = false;
                    break;
            }

            if (string.IsNullOrWhiteSpace(ServerDomain)) {
                //MessageBox.Show("Model_Account_ServerDomain_Empty".Translate());
                canSave = false;
            }

            if (!IsAnonymous) {
                if (string.IsNullOrWhiteSpace(Username)) {
                    //MessageBox.Show("Model_Account_Username_Empty".Translate());
                    canSave = false;
                }
            }
            return canSave;
        }

        /// <summary>
        /// Gets a copy.
        /// </summary>
        /// <returns></returns>
        public Account GetCopy() {
            return (Account)this.MemberwiseClone();
        }
    }
}
