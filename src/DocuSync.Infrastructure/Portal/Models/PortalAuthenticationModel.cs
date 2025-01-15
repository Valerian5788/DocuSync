using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Infrastructure.Portal.Models
{
    public class PortalAuthenticationModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string PortalUrl { get; set; }
    }
}
