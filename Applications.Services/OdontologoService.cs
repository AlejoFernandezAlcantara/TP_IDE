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

        public async Task<List<OdontologoDTO>> GetAllAsync()
            => (await _repository.GetAllAsync()).Select(ToDto).ToList();

        public async Task<OdontologoDTO?> GetByMatriculaAsync(string matricula)
        {
            var domain = await _repository.GetByMatriculaAsync(matricula);
            return domain is null ? null : ToDto(domain);
        }

        public async Task CrearAsync(OdontologoDTO odontologo)
        {
            var domain = ToDomain(odontologo);
            await _repository.AddAsync(domain);
        }

        public async Task ActualizarAsync(OdontologoDTO odontologo)
        {
            var domain = ToDomain(odontologo);
            await _repository.AddAsync(domain);
        }

        public async Task EliminarAsync(string matricula) => await _repository.DeleteAsync(matricula);

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