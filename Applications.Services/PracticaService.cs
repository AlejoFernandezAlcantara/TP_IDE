using Data;
using Domain.Model;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    namespace Applications.Services
    {
        public class PracticaService : IPracticaService
        {
            private readonly IPracticaRepository _repository;

            public PracticaService(IPracticaRepository repository)
            {
                _repository = repository;
            }

            public async Task<List<PracticaDTO>> GetAllAsync() =>
            (await _repository.GetAllAsync()).Select(ToDto).ToList();

            public async Task<PracticaDTO?> GetByCodigoAsync(int codigo)
            {
                var p = await _repository.GetByCodigoAsync(codigo);
                return p is null ? null : ToDto(p);
            }

            public async Task CrearAsync(PracticaDTO dto) =>
                await _repository.AddAsync(new Practica(dto.Detalle, dto.Precio));

            public async Task ActualizarAsync(PracticaDTO dto)
            {
                var practica = new Practica(dto.Detalle, dto.Precio) { CodigoPractica = dto.CodigoPractica };
                await _repository.UpdateAsync(practica);
            }

            public async Task EliminarAsync(int codigo) => await _repository.DeleteAsync(codigo);

            private static PracticaDTO ToDto(Practica p) => new PracticaDTO
            {
                CodigoPractica = p.CodigoPractica,
                Detalle = p.Detalle,
                Precio = p.Precio
            };
        }
    }