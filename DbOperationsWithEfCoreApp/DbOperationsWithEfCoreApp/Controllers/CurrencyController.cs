using DbOperationsWithEfCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DbOperationsWithEfCoreApp.Controllers
{
    [Route("api/currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CurrencyController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        public IActionResult GetAllCurrencies()
        {
            //var result = _appDbContext.Currencies.ToList();
            var result = (from currencies in _appDbContext.Currencies
                          select currencies).ToList();

            return Ok(result);
        }

        [HttpGet("langulage")]
        public async Task<IActionResult> GetAllLanguagesAsync()
        {
            //var result = await _appDbContext.Languages.ToListAsync();
            var result = await (from languages in _appDbContext.Languages
                                select languages).ToListAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllCurrencyByIdAsync([FromRoute] int id)
        {
            var result = await _appDbContext.Currencies.FindAsync(id);
            return Ok(result);
        }

        [HttpGet("{name}/{description}")]
        public async Task<IActionResult> GetAllCurrencyByNameAsync([FromRoute] string name, [FromQuery] string? description)
        {
            var result = await _appDbContext.Currencies.FirstOrDefaultAsync(x => x.Title == name && (string.IsNullOrEmpty(description) || x.Description == description));
            return Ok(result);
        }


        [HttpGet("{name}")]
        public async Task<IActionResult> GetAllLanguageByTitleAsync([FromRoute] string name)
        {
            var result = await _appDbContext.Languages.Where(x => x.Title == name).ToListAsync();
            return Ok(result);
        }


        [HttpPost("allWithAnonimous")]
        public async Task<IActionResult> GetAllWithNonAnonimous()
        {
            var ids = new List<int> { 1, 4, 3 };
            var result = await _appDbContext.Currencies
                .Where(x => ids.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Title
                })
                .ToListAsync();
            return Ok(result);
        }

        [HttpPost("allWithNonAnonimous")]
        public async Task<IActionResult> GetAllWithAnonimous()
        {
            var ids = new List<int> { 1, 4, 3 };
            var result = await _appDbContext.Currencies
                .Where(x => ids.Contains(x.Id))
                .Select(x => new Currency()
                {
                    Id = x.Id,
                    Title = x.Title
                })
                .ToListAsync();
            return Ok(result);
        }



    }


}
