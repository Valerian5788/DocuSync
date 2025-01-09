using DocuSync.Domain.Entities.Enums;

namespace DocuSync.Web.Components.Admin.Models
{
    public class ClientViewModel
    {
        public string Name { get; set; }
        public string HorusEmail { get; set; }
        public ClientStatus Status { get; set; }
    }
}