using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Helpers;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.DTO;
using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implements
{
    public class StatesRepository :GenericRepository<State>, IStatesRepository
    {
        private readonly DataContext _context;
        public StatesRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<ActionResponse<State>> GetAsync(int id)
        {
            var state = await _context.States
                .Include(s => s.Cities!)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (state == null) {
                return new ActionResponse<State>
                {
                    WasSuceess = false,
                    Message = "Estado / Provincia no encontrado"
                };
                
            }
            return new ActionResponse<State>
            {
                WasSuceess = true,
                Resultado = state
            };
        }

        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync()
        {
            
            var states =  await _context.States
                .OrderBy(x=>x.Name)
                .Include(s=>s.Cities)
                .ToListAsync();

           return new ActionResponse<IEnumerable<State>>
                {
                    WasSuceess = true,
                    Resultado = states
                };
        }



        public override async Task<ActionResponse<IEnumerable<State>>> GetAsync(PaginationDTO pagination)
        {

            var queryable = _context.States
                 .Include(s => s.Cities)
                 .Where(x=>x.Country!.Id == pagination.Id)
                 .AsQueryable();

            return new ActionResponse<IEnumerable<State>>
            {
                WasSuceess = true,
                Resultado = await queryable.OrderBy(x=>x.Name).Paginate(pagination).ToListAsync()
            };
        }

        public  async override Task<ActionResponse<int>> GetTotalPagesAsync(PaginationDTO pagination)
        {

            var queryable = _context.States
                  .Where(x => x.Country!.Id == pagination.Id)
                 .AsQueryable();

            double count = await queryable.CountAsync();
            int totalPages = (int)Math.Ceiling(count/pagination.RecordsNumber);

            return new ActionResponse<int>
            {
                WasSuceess = true,
                Resultado =totalPages
            };
        }


    }
}
