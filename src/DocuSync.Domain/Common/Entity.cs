namespace DocuSync.Domain.Common
{
    /// <summary>
    /// Base class for all domain entities
    /// </summary>
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; private set; }
        public string CreatedBy { get; private set; }
        public DateTime? LastModifiedAt { get; private set; }
        public string LastModifiedBy { get; private set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public void SetAuditFields(string creator)
        {
            if (string.IsNullOrEmpty(CreatedBy))
            {
                CreatedAt = DateTime.UtcNow;
                CreatedBy = creator;
            }
        }

        public void UpdateAuditFields(string modifier)
        {
            LastModifiedAt = DateTime.UtcNow;
            LastModifiedBy = modifier;
        }
    }
}
