using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Data;


namespace Applications.Services
{
    public interface IPacienteService
    {
        List<Paciente> GetAll();
        Paciente? GetByNroPaciente(int nroPaciente);
        void Crear(Paciente paciente);
        void Actualizar(Paciente paciente);
        void Eliminar(int nroPaciente);
    }
}