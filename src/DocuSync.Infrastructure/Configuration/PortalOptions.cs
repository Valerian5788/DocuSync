using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Infrastructure.Configuration
{
    public class PortalOptions
    {
        public OrangePortalOptions Orange { get; set; }
    }

    public class OrangePortalOptions
    {
        public string BaseUrl { get; set; }
        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);
    }
}
