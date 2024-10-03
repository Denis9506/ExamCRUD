using ExamCRUD.Model;
using ExamCRUD.Models;
using ExamCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ExamCRUD.Pages.CRUD.ReportCRUD
{
    public class DeleteModel : PageModel
    {
        private readonly ITableStorageService<Report> _reportService;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(ITableStorageService<Report> reportService, ILogger<DeleteModel> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        [BindProperty]
        public Report Report { get; set; }

        [BindProperty]
        public Author Author { get; set; }

        public async Task<IActionResult> OnGetAsync(string rowKey)
        {
            try
            {
                Report = await _reportService.GetEntityAsync(rowKey);
                if (Report == null)
                {
                    return NotFound();
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the report for deletion with rowKey: {rowKey}", rowKey);
                return StatusCode(500, "Internal server error.");
            }
        }

        public async Task<IActionResult> OnPostAsync(string rowKey)
        {
            try
            {
                var report = await _reportService.GetEntityAsync(rowKey);
                if (report == null)
                {
                    return NotFound();
                }

                await _reportService.DeleteEntityAsync(rowKey);
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the report with rowKey: {rowKey}", rowKey);
                return StatusCode(500, "Internal server error.");
            }
        }
    }
}
