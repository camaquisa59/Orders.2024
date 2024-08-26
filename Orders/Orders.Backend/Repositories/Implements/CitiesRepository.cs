using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTO;
using Orders.Shared.Entities;
using Orders.Shared.Responses;
using System.Runtime.InteropServices;

namespace Orders.Backend.Repositories.Implements
{
    public class CitiesRepository : GenericRepository<City>, ICitiesRepository
    {
        private readonly DataContext _context;

        public CitiesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<IEnumerable<City>>> GetAsync(PaginationDTO pagination)
        {
            var queryable = _context.Cities
                .Where(c => c.State!.Id == pagination.Id)
                .AsQueryable();

            return new ActionResponse<IEnumerable<City>>()
            {
                WasSuceess = true,
                Resultado = await queryable
                .OrderBy(c => c.Name)
                .Paginate(pagination).ToListAsync()
            };
        }

        public override async Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {
            var queryable = _context.Cities
                .Where(c => c.State!.Id == pagination.Id)
                .AsQueryable();

            double count = await queryable.CountAsync();
            int totalpages = (int)Math.Ceiling(count / pagination.RecordsNumber);

            return new ActionResponse<int>()
            {
                WasSuceess = true,
                Resultado = totalpages
            };
        }
    }
}