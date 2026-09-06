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
            var domain = ToDomain(odontologo, hashearPassword: true);
            await _repository.AddAsync(domain);
        }

        public async Task ActualizarAsync(OdontologoDTO odontologo)
        {
            var domain = ToDomain(odontologo, hashearPassword: true);
            await _repository.UpdateAsync(domain); 
        }

        public async Task EliminarAsync(string matricula) => await _repository.DeleteAsync(matricula);

        private static OdontologoDTO ToDto(Odontologo o)
            => new OdontologoDTO
            {
                Matricula = o.Matricula,
                NroDocumento = o.NroDocumento,
                TipoDocumento = o.TipoDocumento,
                Especialidad = o.Especialidad,
                Nombre = o.Nombre,
                Apellido = o.Apellido,
                Email = o.Email,
                Password = string.Empty 
            };

        private static Odontologo ToDomain(OdontologoDTO dto, bool hashearPassword)
        {
            var passwordHash = hashearPassword
                ? BCrypt.Net.BCrypt.HashPassword(dto.Password ?? string.Empty)
                : dto.Password ?? string.Empty;

            return new Odontologo(
                dto.Matricula,
                dto.NroDocumento,
                dto.TipoDocumento,
                dto.Especialidad,
                dto.Nombre,
                dto.Apellido,
                dto.Email,
                passwordHash
            );
        }
    }
}