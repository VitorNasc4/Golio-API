using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Golio.Core.Entities;

namespace Golio.Core.Repositories
{
    public interface IPriceRepository
    {
        Task AddPriceAsync(Price price);
        Task UpdatePriceAsync(Price updatedPrice);
        Task<Price> GetPriceByIdAsync(int priceId);
        Task SaveChangesAsync();
    }
}
