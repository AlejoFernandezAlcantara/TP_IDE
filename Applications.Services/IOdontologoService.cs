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
        List<OdontologoDTO> GetAll();
        OdontologoDTO? GetByMatricula(string matricula);
        void Crear(OdontologoDTO odontologo);
        void Actualizar(OdontologoDTO odontologo);
        void Eliminar(string matricula);
    }
}