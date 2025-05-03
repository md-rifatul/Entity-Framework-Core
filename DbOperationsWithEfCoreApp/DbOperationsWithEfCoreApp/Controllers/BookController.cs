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

        //    [HttpPut("{id}")]
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


        [HttpPut("bulky")]
        public async Task<IActionResult> UpdateBookInBalk()
        {
            var book = await _appDbContext.Books.ToListAsync();
            foreach (var items in book)
            {
                items.Title = "Change all the title";
            }
            await _appDbContext.SaveChangesAsync();
            return Ok(book);
        }

        [HttpPut("bulkyUpdate")]
        public async Task<IActionResult> UpdateBookInBalkUpdate()
        {
            await _appDbContext.Books
                .Where(x => x.NoOfPages == 15)
                .ExecuteUpdateAsync(x => x
                .SetProperty(p => p.Title, "Change the title title titttt"));
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBookUsingIdSoft([FromRoute] int id)
        {
            var book = await _appDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            _appDbContext.Books.Remove(book);
            await _appDbContext.SaveChangesAsync();
            return Ok(book);
        }

        [HttpDelete("BulkDelete")]
        public async Task<IActionResult> DeleteBookUsingBult()
        {
            var book = await _appDbContext.Books.Where(x => x.Id < 11).ExecuteDeleteAsync();
            return Ok(book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> BooksDeleteUsingSoft([FromRoute] int id)
        {
            var book = await _appDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            book.IsActive = false;
            await _appDbContext.SaveChangesAsync();

            return Ok(book);
        }
        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllActiveBooks()
        {
            var book = await _appDbContext.Books.Where(x => x.IsActive != false).AsNoTracking().ToListAsync();
            return Ok(book);
        }
    }
}
