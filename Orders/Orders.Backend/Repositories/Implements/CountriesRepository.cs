using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTO;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implements
{
    public class CountriesRepository : GenericRepository<Country>, ICountriesRepository
    {
        private readonly DataContext _context;

        public CountriesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<Country>> GetAsync(int id)
        {
           var country = await _context.Countries
                .Include(c=>c.States!)
                .ThenInclude(c=>c.Cities)
                .FirstOrDefaultAsync(c=>c.Id == id);

            if (country == null) {
                return new ActionResponse<Country>
                {
                    WasSuceess = false,
                    Message = "País no encontrado"
                };

            }

            return new ActionResponse<Country>
            {
                WasSuceess = true,
                Resultado = country
            };

        }

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync()
        {
            var countries = await _context.Countries
                .OrderBy(c=>c.Name)
                .ToListAsync();

            return new ActionResponse<IEnumerable<Country>>()
            {
                WasSuceess = true,
                Resultado = countries

            };
          

        }

        public override async Task<ActionResponse<IEnumerable<Country>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Countries
                .Include(c => c.States)
                .AsQueryable();

            return new ActionResponse<IEnumerable<Country>>()
            {
                WasSuceess = true,
                Resultado = await queryable.OrderBy(c=>c.Name).Paginate(pagination).ToListAsync()

            };


        }
    }
}
