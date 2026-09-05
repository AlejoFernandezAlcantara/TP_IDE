using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface ITurnoRepository
    {
        Task<List<Turno>> GetAllAsync();
        Task<Turno?> GetByCodigoAsync(int codigo);
        Task<List<Turno>> GetByOdontologoAsync(string matricula);
        Task AddAsync(Turno turno);
        Task UpdateAsync(Turno turno);
        Task DeleteAsync(int codigo);
    }
}


