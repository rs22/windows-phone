using System;

namespace OwnCloud.Common.Connection {
    public class ConnectionInfo {
        public OwnCloudServerStatus Status { get; set; }
        public bool SSLConnect { get; set; }
        public Uri BaseUrl { get; set; }
        public Uri WebDAVUrl { get; set; }
    }
}