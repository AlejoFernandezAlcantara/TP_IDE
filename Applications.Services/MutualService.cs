using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Domain.Model;
using DTO;

namespace Applications.Services
{
    public class MutualService : IMutualService
    {
        private readonly IMutualRepository _repository;

        public MutualService(IMutualRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MutualDTO>> GetAllAsync()
        {
            var mutuales = await _repository.GetAllAsync();
            return mutuales.Select(ToDTO).ToList();
        }

        public async Task<MutualDTO?> GetByCuitAsync(int cuit)
        {
            var mutual = await _repository.GetByCuitAsync(cuit);
            return mutual == null ? null : ToDTO(mutual);
        }


        public async Task CrearAsync(MutualDTO dto)
        {
            var mutual = new Mutual(dto.Cuit, dto.Nombre);
            await _repository.AddAsync(mutual);
        }

        public async Task ActualizarAsync(MutualDTO dto)
        {
            var mutual = new Mutual(dto.Cuit, dto.Nombre);
            await _repository.UpdateAsync(mutual);
        }

        public async Task EliminarAsync(int cuit)
        {
            await _repository.DeleteAsync(cuit);
        }

        private static MutualDTO ToDTO(Mutual m) => new MutualDTO
        {
            Cuit = m.Cuit,
            Nombre = m.Nombre
        };
    }
}
