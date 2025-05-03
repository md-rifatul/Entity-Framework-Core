using DbOperationsWithEfCoreApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEfCoreApp.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        private readonly AppDbContext _appDbContext;
        public BookController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpPost("AddBooks")]
        public async Task<IActionResult> AddBooks([FromBody] Book model)
        {
            _appDbContext.Books.Add(model);
            await _appDbContext.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPost("AddBooksUsingList")]
        public async Task<IActionResult> AddBooks([FromBody] List<Book> model)
        {
            _appDbContext.Books.AddRange(model);
            await _appDbContext.SaveChangesAsync();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooks([FromRoute] int id, [FromBody] Book model)
        {
            var book = await _appDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            book.Title = model.Title;
            book.Description = model.Description;
            book.NoOfPages = model.NoOfPages;
            await _appDbContext.SaveChangesAsync();
            return Ok(model);
        }

    }
}
