using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;

namespace Applications.Services
{
    public interface IPracticaService
    {
        Task<List<PracticaDTO>> GetAllAsync();
        Task<PracticaDTO?> GetByCodigoAsync(int codigoPractica);
        Task CrearAsync(PracticaDTO dto);
        Task ActualizarAsync(PracticaDTO dto);
        Task EliminarAsync(int codigoPractica);
    }
}
