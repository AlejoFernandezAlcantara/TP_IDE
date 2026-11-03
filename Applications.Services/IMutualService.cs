using System.Collections.Generic;
using System.Threading.Tasks;
using DTO;

namespace Applications.Services
{
    public interface IMutualService
    {
        Task<List<MutualDTO>> GetAllAsync();
        Task<MutualDTO?> GetByCuitAsync(int cuit);
        Task CrearAsync(MutualDTO dto);
        Task ActualizarAsync(MutualDTO dto);
        Task EliminarAsync(int cuit);
    }
}