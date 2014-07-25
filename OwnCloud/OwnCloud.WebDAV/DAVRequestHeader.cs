using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OwnCloud.WebDAV {
    internal class DAVRequestHeader {
        /// <summary>
        /// DAV Methods.
        /// </summary>
        internal struct Method {
            /// <summary>
            /// Used to fetch avaiable server options.
            /// </summary>
            internal const string Options = "OPTIONS";

            /// <summary>
            /// Used to fetch resource properties.
            /// </summary>
            internal const string PropertyFind = "PROPFIND";

            /// <summary>
            /// Sets or remove properties.
            /// </summary>
            internal const string PropertyPatch = "PROPPATCH";

            /// <summary>
            /// Creates a new collection resource (a directory).
            /// </summary>
            internal const string MakeCollection = "MKCOL";

            /// <summary>
            /// Deletes a resource.
            /// </summary>
            internal const string Delete = "DELETE";

            /// <summary>
            /// Creates a resource.
            /// </summary>
            internal const string Put = "PUT";

            /// <summary>
            /// Copies a resource to another uri destination.
            /// </summary>
            internal const string Copy = "COPY";

            /// <summary>
            /// Moves a resource from a destination to another.
            /// </summary>
            internal const string Move = "MOVE";

            /// <summary>
            /// Locks a resource.
            /// </summary>
            internal const string Lock = "LOCK";

            /// <summary>
            /// Unlocks a locked resource.
            /// </summary>
            internal const string Unlock = "UNLOCK";
        }
        
        /// <summary>
        /// Additional headers to be used.
        /// </summary>
        internal Dictionary<string, string> Headers { get; set; }

        /// <summary>
        /// Resource to be used.
        /// </summary>
        internal Uri RequestedResource { get; set; }

        /// <summary>
        /// Method to be used.
        /// </summary>
        internal string RequestedMethod { get; set; }

        /// <summary>
        /// Creates a new request header object.
        /// </summary>
        /// <param name="method">The RequestHeader.Method to be used</param>
        /// <param name="resource">A resource URI to work with</param>
        /// <param name="headers">Additional headers this request should have</param>
        private DAVRequestHeader(string method, Uri resource, Dictionary<string, string> headers = null) {
            Headers = headers ?? new Dictionary<string, string>();

            RequestedResource = resource;
            RequestedMethod = method;

            Headers.Add(Header.ContentType, "application/xml; charset=\"utf-8\"");
        }

        /// <summary>
        /// Creates a listening request.
        /// </summary>
        /// <param name="path">A relative path to the resource.</param>
        /// <returns></returns>
        internal static DAVRequestHeader CreateListing(Uri path) {
            return new DAVRequestHeader(Method.PropertyFind, path, new Dictionary<string, string>() {
                { Header.Depth, HeaderAttribute.MethodDepth.ApplyResourceAndChildren }
            });
        }
    }
}
