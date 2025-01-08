namespace DocuSync.Domain.Entities.Enums
{
    public enum RequirementStatus
    {
        Pending = 1,      // Waiting for document
        Received = 2,     // Document uploaded
        Validated = 3,    // Document checked
        Completed = 4,    // Fully processed
        Overdue = 5,      // Past due date
        Cancelled = 6     // No longer needed
    }
}
