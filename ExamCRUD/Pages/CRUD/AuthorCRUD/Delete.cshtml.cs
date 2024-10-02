using ExamCRUD.Models;
using ExamCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExamCRUD.Pages.CRUD.AuthorCRUD
{
    public class DeleteModel : PageModel
    {
        private readonly ITableStorageService<Author> _authorService;

        public DeleteModel(ITableStorageService<Author> authorService)
        {
            _authorService = authorService;
        }

        [BindProperty]
        public Author Author { get; set; }

        public async Task<IActionResult> OnGetAsync(string rowKey)
        {
            Author = await _authorService.GetEntityAsync(rowKey);

            if (Author == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await _authorService.DeleteEntityAsync(Author.RowKey);
            return RedirectToPage("./Index");
        }
    }
}
