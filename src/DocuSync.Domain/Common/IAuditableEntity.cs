namespace DocuSync.Domain.Common
{
    /// <summary>
    /// Interface for entities that require audit trails
    /// </summary>
    public interface IAuditableEntity
    {
        DateTime CreatedAt { get; }
        string CreatedBy { get; }
        DateTime? LastModifiedAt { get; }
        string LastModifiedBy { get; }
    }
}
