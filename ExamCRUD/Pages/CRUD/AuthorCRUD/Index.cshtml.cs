using ExamCRUD.Services;
using ExamCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ExamCRUD.Pages.CRUD.AuthorCRUD
{
    public class IndexModel : PageModel
    {
        private readonly ITableStorageService<Author> _authorService;

        public IndexModel(ITableStorageService<Author> authorService)
        {
            _authorService = authorService;
        }

        public List<Author> Authors { get; set; }

        public async Task OnGetAsync()
        {
            Authors = await _authorService.GetAllEntitiesAsync();
        }
    }
}
