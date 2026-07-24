using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Data;
using DTO;


namespace Applications.Services
{
    public interface IPacienteService
    {
        List<PacienteDTO> GetAll();
        PacienteDTO? GetByNroPaciente(int nroPaciente);
        void Crear(PacienteDTO paciente);
        void Actualizar(PacienteDTO paciente);
        void Eliminar(int nroPaciente);
    }
}