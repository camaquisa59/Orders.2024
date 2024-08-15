using Microsoft.EntityFrameworkCore;
using Orders.Backend.Data;
using Orders.Backend.Repositories.Interfaces;
using Orders.Shared.Responses;

namespace Orders.Backend.Repositories.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        public readonly DataContext _context;
        public readonly DbSet<T> _entity;
        public GenericRepository(DataContext context)
        {
            _context = context; 
            _entity = _context.Set<T>();
        }
        public virtual async Task<ActionResponse<T>> AddAsync(T entity)
        {
           _context.Add(entity);
            try
            {
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WasSuceess = true,
                    Resultado = entity
                };
            }
            catch (DbUpdateException)
            {
                return DbUpdateExceptionActionResponse();

            }
            catch (Exception ex) { 
                return ExceptionActionResponse(ex);            
            }
        }

        

        public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            if (row == null)
            {
                return new ActionResponse<T>
                {
                    WasSuceess = false,
                    Message = "Registro no encontrado"
                };
            }
            //borramos
            try
            {
                _entity.Remove(row);
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WasSuceess = true

                };

            }
            catch  {
                return new ActionResponse<T>
                {
                    WasSuceess = false,
                    Message = "No se puede borrar, porque tiene registros relacionados"

                };
            }





        }

        public virtual async Task<ActionResponse<T>> GetAsync(int id)
        {
            var row = await _entity.FindAsync(id);
            if (row == null)
            {
                return new ActionResponse<T>
                {
                    WasSuceess = false,
                    Message = "Registro no encontrado"
                };
            }

            return new ActionResponse<T>
            {
                WasSuceess = true,
                Resultado = row
            };



        }

        public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync()
        {
            return new ActionResponse<IEnumerable<T>>
            {
                WasSuceess = true,
                Resultado = await _entity.ToListAsync()
            };
        }

        public virtual async Task<ActionResponse<T>> UpdateAsync(T entity)
        {
            _context.Update(entity);
            try
            {
                await _context.SaveChangesAsync();
                return new ActionResponse<T>
                {
                    WasSuceess = true,
                    Resultado = entity
                };
            }
            catch (DbUpdateException)
            {
                return DbUpdateExceptionActionResponse();

            }
            catch (Exception ex)
            {
                return ExceptionActionResponse(ex);
            }
        }


        private ActionResponse<T> DbUpdateExceptionActionResponse()
        {
            return new ActionResponse<T>
            {
                WasSuceess = false,
                Message = "Ya existe el registro que estas intentando crear."
            };
        }

        private ActionResponse<T> ExceptionActionResponse(Exception ex)
        {
            return new ActionResponse<T>
            {
                WasSuceess = false,
                Message = ex.Message
            };
        }
    }
}
