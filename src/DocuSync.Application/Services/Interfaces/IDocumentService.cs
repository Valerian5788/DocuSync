using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSync.Application.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<DocumentUploadResult> UploadForRequirementAsync(
            Guid requirementId,
            Stream content,
            string filename,
            CancellationToken cancellation = default);

        Task<DocumentStatus> GetStatusAsync(
            Guid requirementId,
            CancellationToken cancellation = default);
    }

    public record DocumentUploadResult(
        bool Success,
        string TrackingId,
        string Error = null);

    public record DocumentStatus(
        Guid RequirementId,
        bool HasDocument,
        DateTimeOffset? UploadedAt,
        string Status);
}
