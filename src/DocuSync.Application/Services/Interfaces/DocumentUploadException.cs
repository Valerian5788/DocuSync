namespace DocuSync.Application.Services.Interfaces
{
    public class DocumentUploadException : Exception 
    {
        public DocumentUploadException(string message, Exception inner = null) 
            : base(message, inner) { }
    }

    public interface IDocumentStorage
    {
        /// <summary>
        /// Uploads a document to temporary storage and returns its tracking ID
        /// </summary>
        Task<string> UploadAsync(
            Stream content, 
            string filename, 
            Guid clientId, 
            Guid requirementId,
            CancellationToken cancellation = default);
            
        /// <summary>
        /// Gets a temporary read URL for a document
        /// </summary>
        Task<string> GetTemporaryUrlAsync(
            string blobId,
            CancellationToken cancellation = default);
    }
}