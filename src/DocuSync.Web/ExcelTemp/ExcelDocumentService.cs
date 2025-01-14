using System.Collections.Concurrent;
using System.ComponentModel;
using System.Globalization;
using OfficeOpenXml;


namespace DocuSync.Web.ExcelTemp
{
    public interface IExcelDocumentService
    {
        Task<IEnumerable<MissingDocumentDto>> GetMissingDocumentsAsync();
    }

    public class MissingDocumentDto
    {
        public string CompanyName { get; set; }
        public string DocumentType { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }

    public class ExcelDocumentService : IExcelDocumentService
    {
        private static readonly ConcurrentDictionary<string, IEnumerable<MissingDocumentDto>> _cache = new();
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ExcelDocumentService> _logger;

        public ExcelDocumentService(IWebHostEnvironment environment, ILogger<ExcelDocumentService> logger)
        {
            _environment = environment;
            _logger = logger;
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
        }

        public async Task<IEnumerable<MissingDocumentDto>> GetMissingDocumentsAsync()
        {
            const string cacheKey = "missingDocs";

            if (_cache.TryGetValue(cacheKey, out var cached))
                return cached;

            var filePath = Path.Combine(_environment.ContentRootPath, "ExcelTemp", "missing-documents.xlsx");

            try
            {
                using var package = new ExcelPackage(new FileInfo(filePath));
                var worksheet = package.Workbook.Worksheets[0];

                var documents = new List<MissingDocumentDto>();
                var rowCount = worksheet.Dimension.Rows;

                // Skip header row
                for (int row = 2; row <= rowCount; row++)
                {
                    var doc = new MissingDocumentDto
                    {
                        CompanyName = worksheet.Cells[row, 1].Text,
                        DocumentType = worksheet.Cells[row, 2].Text,
                        DueDate = ParseExcelDate(worksheet.Cells[row, 3]),
                        Status = worksheet.Cells[row, 4].Text,
                        Description = worksheet.Cells[row, 5].Text
                    };

                    documents.Add(doc);
                }

                _cache.TryAdd(cacheKey, documents);
                return documents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load missing documents from Excel");
                throw;
            }
        }

        private DateTime ParseExcelDate(ExcelRange cell)
        {
            try
            {
                return cell.GetValue<DateTime>();
            }
            catch
            {
                // Fallback for string dates
                string dateStr = cell.Text?.Trim();
                if (DateTime.TryParseExact(dateStr,
                    new[] { "yyyyMMdd", "dd/MM/yyyy", "yyyy-MM-dd" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime result))
                {
                    return result;
                }

                _logger.LogWarning($"Could not parse date: {dateStr}");
                return DateTime.MinValue;
            }
        }
    }
}
