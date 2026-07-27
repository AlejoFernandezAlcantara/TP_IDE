using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IOdontologoRepository
    {
        Task<List<Odontologo>> GetAllAsync();
        Task<Odontologo?> GetByMatriculaAsync(string matricula);
        Task AddAsync(Odontologo odontologo);
        Task UpdateAsync(Odontologo odontologo);
        Task DeleteAsync(string matricula);
        
    }
}