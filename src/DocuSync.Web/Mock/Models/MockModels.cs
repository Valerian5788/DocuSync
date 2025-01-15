namespace DocuSync.Web.Mock.Models
{
    public class MockDashboardData
    {
        public int PendingDocuments { get; set; }
        public int OverdueTasks { get; set; }
        public int ProcessedToday { get; set; }
        public decimal SuccessRate { get; set; }
        public List<MockActivity> Activities { get; set; }
    }

    public class MockActivity
    {
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
    }
}
