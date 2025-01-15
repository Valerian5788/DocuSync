using DocuSync.Application.Common.Results;
using DocuSync.Infrastructure.Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Infrastructure.Portal.Interfaces
{
    public interface IPortalService
    {
        Task<Result<bool>> TestConnectionAsync(CancellationToken cancellation = default);
        Task<Result<IEnumerable<PortalDocument>>> GetDocumentsAsync(CancellationToken cancellation = default);
        Task<Result<Stream>> DownloadDocumentAsync(string documentId, CancellationToken cancellation = default);
    }
}
