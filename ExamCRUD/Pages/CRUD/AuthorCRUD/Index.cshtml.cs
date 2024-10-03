using ExamCRUD.Services;
using ExamCRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AzureTeacherStudentSystem;
using System.Text.Json;
using System.Security.Cryptography;
using System.Text;

namespace ExamCRUD.Pages.CRUD.AuthorCRUD
{
    public class IndexModel : PageModel
    {
        private readonly ITableStorageService<Author> _authorService;
        public readonly ICacheService _cacheService;

        public IndexModel(ITableStorageService<Author> authorService, ICacheService cacheService)
        {
            _authorService = authorService;
            _cacheService = cacheService;   
        }

        public List<Author> Authors { get; set; }

        public async Task OnGetAsync()
        {
           Authors = await _authorService.GetAllEntitiesAsync();
        }
    }
}
