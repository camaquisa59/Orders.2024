﻿using Orders.Shared.Entities;
using Orders.Shared.Responses;

namespace Orders.Backend.UnitOfWork.interfaces
{
    public interface ICountriesUnitOfWork 
    {

        Task<ActionResponse<Country>> GetAsync(int id);

        Task<ActionResponse<IEnumerable<Country>>> GetAsync();




    }
}
