using Microsoft.AspNetCore.Mvc;
using Orders.Backend.UnitOfWork.interfaces;
using Orders.Shared.DTO;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : GenericController<City>
    {
         private readonly ICitiesUnitOfWork _citiesUnitOfWork;

        public CitiesController(IGenericUnitOfWork<City> unitOfWork, ICitiesUnitOfWork citiesUnitOfWork) : base(unitOfWork)
        {
           
            _citiesUnitOfWork = citiesUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var response  =  await _citiesUnitOfWork.GetAsync(pagination);
            if (response.WasSuceess) { 
                return Ok(response.Resultado);  
            
            }
            return BadRequest();
        }

        [HttpGet("totalPages")]
        public override async Task<IActionResult> GetPagesAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _citiesUnitOfWork.GetTotalPagesAsync(pagination);
            if (action.WasSuceess) { 
                return Ok(action.Resultado);
            }
            return BadRequest();    
        }

    }
}
