using Newtonsoft.Json;
using OwnCloud.WebDAV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OwnCloud.Common.Connection {
    public class ConnectionService {
        private const string STATUS_PATH = "status.php";
        private const string WEBDAV_PATH = "remote.php/webdav/";

        public async Task<ConnectionInfo> GetConnectionInfo(string host) {
            if (string.IsNullOrEmpty(host))
                return null;

            if (host.StartsWith("https://") || host.StartsWith("http://")) {
                return await TryConnect(new Uri(host));
            }

            var httpsStatus = await TryConnect(new Uri("https://" + host));
            if (httpsStatus != null)
                return httpsStatus;

            // TODO: issslrecoverableexception?
            return await TryConnect(new Uri("http://" + host));
        }

        public async Task<AuthenticationMethod> GetAuthenticationInfo(ConnectionInfo connection) {
            var webdav = new WebDAVClient(connection.WebDAVUrl);
            var method = AuthenticationMethod.Unknown;
            try {
                await webdav.GetRootEntries(false);
                method = AuthenticationMethod.None;
            } catch (WebException webex) {
                if (webex != null && webex.Response is HttpWebResponse) {
                    var webResponse = (HttpWebResponse)webex.Response;
                    if (webResponse.StatusCode == HttpStatusCode.Unauthorized) {
                        var authHeader = webResponse.Headers["www-authenticate"].ToLower().Trim();
                        if (authHeader.StartsWith("basic"))
                            method = AuthenticationMethod.Basic;
                    }
                }
            }
            return method;
        }

        private async Task<ConnectionInfo> TryConnect(Uri url) {
            var status = await GetServerStatus(url);
            if (status == null || !status.Installed || !status.IsVersionValid)
                return null;

            return new ConnectionInfo {
                BaseUrl = url,
                WebDAVUrl = new Uri(url.AbsoluteUri + WEBDAV_PATH),
                Status = status,
                SSLConnect = url.Scheme == "https"
            };
        }

        private async Task<OwnCloudServerStatus> GetServerStatus(Uri server) {
            var statusPage = new Uri(server, STATUS_PATH);
            var http = new HttpClient();
            try {
                var result = await http.GetStringAsync(statusPage);
                var status = JsonConvert.DeserializeObject<OwnCloudServerStatus>(result);                
                return status;
            } catch {
                return null;
            }
        }
    }
}
