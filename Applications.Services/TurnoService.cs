using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Model;
using Data;
using DTO;

namespace Applications.Services
{
    public class TurnoService : ITurnoService
    {
        private readonly ITurnoRepository _repository;

        public TurnoService(ITurnoRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<TurnoDTO>> GetAllAsync() =>
            (await _repository.GetAllAsync()).Select(ToDto).ToList();

        public async Task<TurnoDTO?> GetByCodigoAsync(int codigo)
        {
            var domain = await _repository.GetByCodigoAsync(codigo);
            return domain is null ? null : ToDto(domain);
        }

        public async Task<List<TurnoDTO>> GetByOdontologoAsync(string matricula) =>
            (await _repository.GetByOdontologoAsync(matricula)).Select(ToDto).ToList();

        public async Task CrearAsync(TurnoDTO dto)
        {
            var turno = new Turno(dto.FechaHoraInicio)
            {
                OdontologoMatricula = dto._odontologoMatricula
            };

            if (dto.Duracion > 0)
                turno.Duracion = dto.Duracion;

            turno.Estado = dto.Estado;

            await _repository.AddAsync(turno);
        }

        public async Task ActualizarAsync(TurnoDTO dto)
        {
            var turno = new Turno(dto.FechaHoraInicio)
            {
                Codigo = dto.Codigo,
                Duracion = dto.Duracion,
                Estado = dto.Estado,
                OdontologoMatricula = dto._odontologoMatricula
            };

            await _repository.UpdateAsync(turno);
        }

        public async Task EliminarAsync(int codigo) => await _repository.DeleteAsync(codigo);

        private static TurnoDTO ToDto(Turno t) => new TurnoDTO
        {
            Codigo = t.Codigo,
            FechaHoraInicio = t.FechaHoraInicio,
            Duracion = t.Duracion,
            Estado = t.Estado,
            _odontologoMatricula = t.OdontologoMatricula
        };
    }
}
