using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Data;

namespace Applications.Services
{
    public class OdontologoService : IOdontologoService
    {
        private readonly IOdontologoRepository _repository;

        public OdontologoService(IOdontologoRepository repository)
        {
            _repository = repository;
        }

        public List<Odontologo> GetAll() => _repository.GetAll();

        public Odontologo? GetByMatricula(string matricula) => _repository.GetByMatricula(matricula);

        public void Crear(Odontologo odontologo) => _repository.Add(odontologo);

        public void Actualizar(Odontologo odontologo) => _repository.Update(odontologo);

        public void Eliminar(string matricula) => _repository.Delete(matricula);
    }
}