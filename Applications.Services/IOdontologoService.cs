using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Data;
using DTO;

//hacer asincrono
namespace Applications.Services
{
    public interface IOdontologoService
    {
        Task<List<OdontologoDTO>> GetAllAsync();
        Task<OdontologoDTO?> GetByMatriculaAsync(string matricula);
        Task CrearAsync(OdontologoDTO odontologo);
        Task ActualizarAsync(OdontologoDTO odontologo);
        Task EliminarAsync(string matricula);
    }
}