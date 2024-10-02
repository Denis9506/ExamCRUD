using ExamCRUD.Model;
using ExamCRUD.Models;
using ExamCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExamCRUD.Pages.CRUD.ReportCRUD
{
    public class IndexModel : PageModel
    {
        private readonly ITableStorageService<Report> _reportService;
        private readonly ITableStorageService<Author> _authorService;

        public IndexModel(ITableStorageService<Report> reportService, ITableStorageService<Author> authorService)
        {
            _reportService = reportService;
            _authorService = authorService;
        }
        public string AuthorNickName { get; set; }
        public List<Report> ReportsFromTable { get; set; }
        public List<ReportDTO> Reports { get; set; } = new List<ReportDTO>();
        public async Task OnGetAsync()
        {
            ReportsFromTable = await _reportService.GetAllEntitiesAsync();
            foreach (var report in ReportsFromTable)
            {
                var author = _authorService.GetEntityAsync(report.AuthorRowKey);
                Reports.Add(new ReportDTO { 
                    Title = report.Title,
                    Description = report.Description,
                    PublishedDate = report.PublishedDate,
                    Author = new() { 
                        RowKey = report.RowKey,
                        NickName = author.Result.NickName   
                    }
                });
            }
        }
    }
}
