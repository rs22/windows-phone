using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OwnCloud.Common.Connection {
    public class OwnCloudServerStatus {
        public string Version { get; set; }
        public string VersionString { get; set; }
        public bool Installed { get; set; }
        public string Edition { get; set; }
        public bool IsVersionValid {
            get {
                if (string.IsNullOrEmpty(Version))
                    return false;

                // TODO: Max dots
                var nums = Version.Split('.');
                foreach (var num in nums) {
                    if (!Regex.IsMatch(num, "\\d+"))
                        return false;
                }
                return true;
            }
        }
    }
}
