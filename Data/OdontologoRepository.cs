using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public class OdontologoRepository : IOdontologoRepository
    {
        private readonly List<Odontologo> _odontologos = new();

        public List<Odontologo> GetAll() => _odontologos;

        public Odontologo? GetByMatricula(string matricula) =>
            _odontologos.FirstOrDefault(o => o.Matricula == matricula);

        public void Add(Odontologo odontologo)
        {
            if (GetByMatricula(odontologo.Matricula) != null)
                throw new InvalidOperationException("Ya existe un odontólogo con esa matrícula.");
            _odontologos.Add(odontologo);
        }

        public void Update(Odontologo odontologo)
        {
            var existente = GetByMatricula(odontologo.Matricula);
            if (existente == null)
                throw new InvalidOperationException("Odontólogo no encontrado.");
            _odontologos.Remove(existente);
            _odontologos.Add(odontologo);
        }

        public void Delete(string matricula)
        {
            var existente = GetByMatricula(matricula);
            if (existente == null)
                throw new InvalidOperationException("Odontólogo no encontrado.");
            _odontologos.Remove(existente);
        }
    }
}