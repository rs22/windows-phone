using FluentAssertions;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace OwnCloud.WebDAV.Tests {
    public class WebDAVTest {

        private WebDAVClient GetDemoWebDAV() {
            var baseUri = new Uri("https://www.crushftp.com/");
            var relativeUri = new Uri("/demo/", UriKind.Relative);
            return new WebDAVClient(new Uri(baseUri, relativeUri),
                new NetworkCredential("demo", "demo"));
        }

        [Fact]
        public async Task DirectoryEntriesShouldBeListedCorrectly() {
            var webDAV = GetDemoWebDAV();

            var result = await webDAV.GetRootEntries(true);
            result.Count.Should().BeGreaterThan(0);
        }
    }
}
