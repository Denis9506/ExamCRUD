using ExamCRUD.Model;
using ExamCRUD.Models;
using ExamCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExamCRUD.Pages.CRUD.ReportCRUD
{
    public class DeleteModel : PageModel
    {
        private readonly ITableStorageService<Report> _reportService;

        public DeleteModel(ITableStorageService<Report> reportService)
        {
            _reportService = reportService;
        }

        [BindProperty]
        public Report Report { get; set; }
        [BindProperty]
        public Author Author { get; set; }

        public async Task<IActionResult> OnGetAsync(string rowKey)
        {
            Report = await _reportService.GetEntityAsync(rowKey);
            if (Report == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string rowKey)
        {
            var report = await _reportService.GetEntityAsync(rowKey);
            if (report == null)
            {
                return NotFound();
            }

            await _reportService.DeleteEntityAsync(rowKey);
            return RedirectToPage("./Index");
        }
    }
}
