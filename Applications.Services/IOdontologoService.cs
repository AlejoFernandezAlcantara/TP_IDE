using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Data;


namespace Applications.Services
{
    public interface IOdontologoService
    {
        List<Odontologo> GetAll();
        Odontologo? GetByMatricula(string matricula);
        void Crear(Odontologo odontologo);
        void Actualizar(Odontologo odontologo);
        void Eliminar(string matricula);
    }
}