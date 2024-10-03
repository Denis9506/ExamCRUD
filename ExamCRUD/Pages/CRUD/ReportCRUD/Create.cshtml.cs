using ExamCRUD.Model;
using ExamCRUD.Models;
using ExamCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ExamCRUD.Pages.CRUD.ReportCRUD
{
    public class CreateModel : PageModel
    {
        private readonly ITableStorageService<Report> _reportService;
        private readonly ITableStorageService<Author> _authorService;
        private readonly IBlobService _blobService;
        private readonly ILogger<CreateModel> _logger;

        [BindProperty]
        public Report Report { get; set; }

        [BindProperty]
        public IFormFile ReportFile { get; set; }

        public DateTime PublishedDate { get; set; }
        public List<Author> Authors { get; set; }

        public CreateModel(
            ITableStorageService<Report> reportService,
            ITableStorageService<Author> authorService,
            IBlobService blobService,
            ILogger<CreateModel> logger)
        {
            _reportService = reportService;
            _authorService = authorService;
            _blobService = blobService;
            _logger = logger;
            PublishedDate = DateTime.Now;
        }

        public async Task OnGetAsync()
        {
            try
            {
                Authors = await _authorService.GetAllEntitiesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading authors.");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                Authors = await _authorService.GetAllEntitiesAsync();

                if (!ModelState.IsValid)
                {
                    if (Report.PublishedDate > DateTime.Now)
                    {
                        ModelState.AddModelError("Report.PublishedDate", "The published date cannot be in the future.");
                    }
                    return Page();
                }
           
                var selectedAuthor = Authors.FirstOrDefault(a => a.RowKey == Report.AuthorRowKey);

                if (selectedAuthor != null)
                {
                    Report.AuthorRowKey = selectedAuthor.RowKey;
                    Report.PublishedDate = DateTime.SpecifyKind(Report.PublishedDate, DateTimeKind.Utc);
                }

                if (ReportFile != null)
                {
                    var fileName = ReportFile.FileName;
                    var fileExists = await _blobService.FileExistsAsync("reports", fileName);
                    if (fileExists)
                    {
                        var uniqueFileName = GenerateUniqueFileName(fileName);
                        fileName = uniqueFileName;
                    }

                    var fileUrl = await _blobService.UploadFileAsync("reports", fileName, ReportFile.OpenReadStream());
                    Report.FileUrl = fileUrl;
                }

                await _reportService.AddEntityAsync(Report);

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the report.");
                return Page();
            }
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
