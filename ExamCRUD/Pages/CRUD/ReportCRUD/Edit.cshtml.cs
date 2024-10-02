using ExamCRUD.Model;
using ExamCRUD.Models;
using ExamCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExamCRUD.Pages.CRUD.ReportCRUD
{
    public class EditModel : PageModel
    {
        private readonly ITableStorageService<Report> _reportService;
        private readonly ITableStorageService<Author> _authorService;
        private readonly IBlobService _blobService;

        [BindProperty]
        public Report Report { get; set; }

        [BindProperty]
        public IFormFile ReportFile { get; set; }

        public List<Author> Authors { get; set; } = new();
        public DateTime PublishedDate { get; set; }

        public EditModel(ITableStorageService<Report> reportService, ITableStorageService<Author> authorService, IBlobService blobService)
        {
            _reportService = reportService;
            _authorService = authorService;
            _blobService = blobService;
            PublishedDate = DateTime.Now;
        }

        public async Task<IActionResult> OnGetAsync(string rowKey)
        {
            Report = await _reportService.GetEntityAsync(rowKey);
            Authors = await _authorService.GetAllEntitiesAsync();

            if (Report == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string rowKey)
        {
            Authors = await _authorService.GetAllEntitiesAsync();
            if (ReportFile == null || ReportFile.Length == 0)
            {
                ModelState.Remove("ReportFile");
            }
            if (!ModelState.IsValid)
            {
               
                return Page();
            }

            var currentReport = await _reportService.GetEntityAsync(rowKey);
            if (currentReport == null)
            {
                return NotFound();
            }

            currentReport.Title = Report.Title;
            currentReport.Description = Report.Description;

            var selectedAuthor = Authors.FirstOrDefault(a => a.RowKey == Report.AuthorRowKey);
            if (selectedAuthor != null)
            {
                currentReport.AuthorRowKey = selectedAuthor.RowKey;
            }

            currentReport.PublishedDate = DateTime.SpecifyKind(Report.PublishedDate, DateTimeKind.Utc);

            if (ReportFile != null)
            {
                var fileName = ReportFile.FileName;
                var fileExists = await _blobService.FileExistsAsync("reports", fileName);
                if (fileExists)
                {
                    fileName = GenerateUniqueFileName(fileName);
                }

                var fileUrl = await _blobService.UploadFileAsync("reports", fileName, ReportFile.OpenReadStream());
                currentReport.FileUrl = fileUrl;
            }

            await _reportService.UpdateEntityAsync(currentReport);
            return RedirectToPage("./Index");
        }

        private string GenerateUniqueFileName(string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var baseFileName = Path.GetFileNameWithoutExtension(fileName);
            var uniqueFileName = $"{baseFileName}_{Guid.NewGuid()}{fileExtension}";
            return uniqueFileName;
        }
    }
}
