using ExamCRUD.Model;
using ExamCRUD.Models;
using ExamCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ExamCRUD.Pages.CRUD.ReportCRUD
{
    public class IndexModel : PageModel
    {
        private readonly ITableStorageService<Report> _reportService;
        private readonly ITableStorageService<Author> _authorService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(
            ITableStorageService<Report> reportService,
            ITableStorageService<Author> authorService,
            ILogger<IndexModel> logger)
        {
            _reportService = reportService;
            _authorService = authorService;
            _logger = logger;
        }

        public string AuthorNickName { get; set; }
        public List<Report> ReportsFromTable { get; set; }
        public List<ReportDTO> Reports { get; set; } = new List<ReportDTO>();

        public async Task OnGetAsync()
        {
            try
            {
                ReportsFromTable = await _reportService.GetAllEntitiesAsync();

                foreach (var report in ReportsFromTable)
                {
                    try
                    {
                        var author = await _authorService.GetEntityAsync(report.AuthorRowKey);

                        Reports.Add(new ReportDTO
                        {
                            Title = report.Title,
                            Description = report.Description,
                            PublishedDate = report.PublishedDate,
                            Author = new Author
                            {
                                RowKey = report.RowKey,
                                NickName = author.NickName
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"An error occurred while fetching author for report: {report.Title}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading reports.");
            }
        }
    }
}
