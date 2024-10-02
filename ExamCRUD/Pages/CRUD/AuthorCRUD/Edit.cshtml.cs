using ExamCRUD.Models;
using ExamCRUD.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExamCRUD.Pages.CRUD.AuthorCRUD
{
    public class EditModel : PageModel
    {
        private readonly ITableStorageService<Author> _authorService;

        public EditModel(ITableStorageService<Author> authorService)
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

        public async Task<IActionResult> OnPostAsync(string rowKey)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var currentAuthor = await _authorService.GetEntityAsync(rowKey);
            if (currentAuthor == null)
            {
                return NotFound();
            }
            currentAuthor.NickName = Author.NickName;
            currentAuthor.Role = Author.Role;
            await _authorService.UpdateEntityAsync(currentAuthor);
            return RedirectToPage("./Index");
        }
    }
}
