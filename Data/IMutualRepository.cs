using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IMutualRepository
    {
        Task<List<Mutual>> GetAllAsync();
        Task AddAsync(Mutual mutual);  
        Task UpdateAsync(Mutual mutual);
        Task DeleteAsync(int cuil);
    }
}
