using DbOperationsWithEfCoreApp.Data;
using Microsoft.AspNetCore.Mvc;

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

    }
}
