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
    public class OdontologoService : IOdontologoService
    {
        private readonly IOdontologoRepository _repository;

        public OdontologoService(IOdontologoRepository repository)
        {
            _repository = repository;
        }

        public List<OdontologoDTO> GetAll()
            => _repository.GetAll().Select(ToDto).ToList();

        public OdontologoDTO? GetByMatricula(string matricula)
        {
            var domain = _repository.GetByMatricula(matricula);
            return domain is null ? null : ToDto(domain);
        }

        public void Crear(OdontologoDTO odontologo)
        {
            var domain = ToDomain(odontologo);
            _repository.Add(domain);
        }

        public void Actualizar(OdontologoDTO odontologo)
        {
            var domain = ToDomain(odontologo);
            _repository.Update(domain);
        }

        public void Eliminar(string matricula) => _repository.Delete(matricula);

        private static OdontologoDTO ToDto(Odontologo o) // Mapeo entre Odontologo y  OdontologoDTO
            => new OdontologoDTO
            {
                Matricula = o.Matricula,
                NroDocumento = o.NroDocumento,
                TipoDocumento = o.TipoDocumento,
                Especialidad = o.Especialidad,
                Nombre = o.Nombre,
                Apellido = o.Apellido,
                Email = o.Email,
                Password = string.Empty  //devuelve vacio por seguridad
            };

        private static Odontologo ToDomain(OdontologoDTO dto)
            => new Odontologo(
                dto.Matricula,
                dto.NroDocumento,
                dto.TipoDocumento,
                dto.Especialidad,
                dto.Nombre,
                dto.Apellido,
                dto.Email,
                dto.Password ?? string.Empty //lo mismo
            );
    }
}