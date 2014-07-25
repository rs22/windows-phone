using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OwnCloud.WebDAV {
    public class WebDAVFile {
        public string FileName { get; set; }
        public Uri FilePath { get; set; }
        public long Size { get; set; }
        public string MimeType { get; set; }
        public string ETag { get; set; }
        public bool IsDirectory { get; set; }
        public DateTime LastModified { get; set; }
        public DateTime Created { get; set; }

        internal static WebDAVFile FromRequestResultItem(DAVRequestResult.Item item) {
            return new WebDAVFile {
                FileName = item.LocalReference,
                FilePath = new Uri(item.Reference, UriKind.Relative),
                Size = item.ContentLength,
                MimeType = item.ContentType,
                Created = item.CreationDate,
                LastModified = item.LastModified,
                IsDirectory = item.ResourceType == ResourceType.Collection
            };
        }
    }
}
