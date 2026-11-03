using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Data
{
    public interface IPracticaRepository
    {
        Task<List<Practica>> GetAllAsync();
        Task<Practica?> GetByCodigoAsync(int codigoPractica);
        Task AddAsync(Practica practica);
        Task UpdateAsync(Practica practica);
        Task DeleteAsync(int CodigoPractica);
    }
}
