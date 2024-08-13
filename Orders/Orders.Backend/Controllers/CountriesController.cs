using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Orders.Backend.Data;
using Orders.Backend.UnitOfWork.interfaces;
using Orders.Shared.Entities;

namespace Orders.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : GenericController<Country>
    {
        public CountriesController(IGenericUnitOfWork<Country> unitOfWork) : base(unitOfWork)
        {
        }
    }
}
