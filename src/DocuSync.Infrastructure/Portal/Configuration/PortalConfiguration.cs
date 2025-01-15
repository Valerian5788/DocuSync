using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Infrastructure.Portal.Configuration
{
    public class PortalConfiguration
    {
        public string BaseUrl { get; set; }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
        public bool RequireHttps { get; set; } = true;
    }

}
