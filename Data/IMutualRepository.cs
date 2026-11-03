using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IMutualRepository
    {
        Task<List<Mutual>> GetAllAsync();
        Task<Mutual?> GetByCuitAsync(int cuit);
        Task AddAsync(Mutual mutual);
        Task UpdateAsync(Mutual mutual);
        Task DeleteAsync(int cuit);
    }
}