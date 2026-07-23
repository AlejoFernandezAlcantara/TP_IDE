using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;

namespace Data
{
    public interface IOdontologoRepository
    {
        List<Odontologo> GetAll();
        Odontologo? GetByMatricula(string matricula);
        void Add(Odontologo odontologo);
        void Update(Odontologo odontologo);
        void Delete(string matricula);
    }
}