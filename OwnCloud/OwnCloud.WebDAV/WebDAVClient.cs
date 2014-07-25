using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace OwnCloud.WebDAV {
    /// <summary>
    /// Provides methods and objects to deal with any WebDAV implementation
    /// </summary>
    public class WebDAVClient {
        ICredentials _creds;
        Uri _host;
        Uri _relativeHost;

        /// <summary>
        /// Creates a new object.
        /// </summary>
        /// <param name="host">A valid host</param>
        /// <param name="credentials">Username and Password if necessary</param>
        public WebDAVClient(Uri host, ICredentials credentials = null) {
            _creds = credentials;
            _host = host;
        }

        public Task<List<WebDAVFile>> GetRootEntries(bool fetchAllProperties) {
            return GetEntries("", fetchAllProperties);
        }

        public async Task<List<WebDAVFile>> GetEntries(string path, bool fetchAllProperties) {
            var requestBody = fetchAllProperties ? DAVRequestBody.CreateAllPropertiesListing() : null;
            var result = await SendDAVRequestAsync(
                DAVRequestHeader.CreateListing(new Uri(path, UriKind.Relative)), requestBody);
            if (result.Status == ServerStatus.MultiStatus) {
                return result.Items.Select(x => WebDAVFile.FromRequestResultItem(x)).ToList();
            } else {
                throw new Exception("Error downloading entries");
            }
        }
        
        /// <summary>
        /// Starts an asynchronous DAV-HTTP-Request.
        /// </summary>
        /// <param name="header">The DAV-Request header to used.</param>
        /// <param name="body">The DAV-Request-body to used.</param>
        private async Task<DAVRequestResult> SendDAVRequestAsync(DAVRequestHeader header, DAVRequestBody body) {
            _relativeHost = new Uri(_host, header.RequestedResource);

            HttpWebRequest request = WebRequest.CreateHttp(_relativeHost);
            request.Method = header.RequestedMethod.ToString();
            foreach (KeyValuePair<string, string> current in header.Headers) {
                // API restriction
                switch (current.Key) {
                    case "Accept":
                        request.Accept = current.Value;
                        break;
                    case Header.ContentType:
                        request.ContentType = current.Value;
                        break;
                    default:
                        request.Headers[current.Key] = current.Value;
                        break;
                }
            }

            if (_creds != null)
                request.Credentials = _creds;

            if (body != null) {
                using (var requestStream = await Task<Stream>.Factory
                    .FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, request)) {
                    body.WriteToStream(requestStream);
                }
            }

            return await Task<WebResponse>.Factory
                .FromAsync(request.BeginGetResponse, request.EndGetResponse, request)
                .ContinueWith(response => new DAVRequestResult(this, response.Result as HttpWebResponse, _relativeHost));
        }
    }
}
