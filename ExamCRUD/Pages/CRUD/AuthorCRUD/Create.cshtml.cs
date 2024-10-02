using ExamCRUD.Models;
using ExamCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExamCRUD.Pages.CRUD.AuthorCRUD
{
    public class CreateModel : PageModel
    {
        private readonly ITableStorageService<Author> _authorService;

        public CreateModel(ITableStorageService<Author> authorService)
        {
            _authorService = authorService;
        }

        [BindProperty]
        public Author Author { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            await _authorService.AddEntityAsync(Author);
            return RedirectToPage("./Index");
        }
    }
}
