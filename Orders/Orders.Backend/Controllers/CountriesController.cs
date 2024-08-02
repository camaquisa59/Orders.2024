using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Orders.Backend.Data;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly DataContext _dataContext;
        public CountriesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost]
        public async Task<IActionResult> addCountryAsync(Country country)
        {

            _dataContext.Add(country);
            await _dataContext.SaveChangesAsync();
            return Ok(country);

        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok( await _dataContext.Countries.ToListAsync());
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetAsync(int Id)
        {
            var country = await _dataContext.Countries.FindAsync(Id);
            if (country == null) { 
                return NotFound();
            }
            return Ok(country);
        }



        [HttpPut]
        public async Task<IActionResult> PutAsync(Country country)
        {
            _dataContext.Update(country);
            await _dataContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            var country = await _dataContext.Countries.FindAsync(Id);
            if (country == null)
            {
                return NotFound();
            }
            _dataContext.Countries.Remove(country);
            _dataContext.SaveChanges();
            return NoContent();
        }



    }
}
