using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class OdontologoMutual
    {
        public int NroAfiliado { get; set; }


        //FK MUTUAL
        public int _mutualCuit { get; set; }
        public Mutual _mutual { get; set; }
        //FK ODONTOLOGO
        public string _odontologoMatricula { get; set; }
        public Odontologo _odontologo { get; set; }
        //GET Y SET DE LAS 2FK
        public string OdontologoMatricula
        {
            get => _odontologo?.Matricula ?? _odontologoMatricula;
            set => _odontologoMatricula = value;
        }
        public Odontologo? Odontologo
        {
            get => _odontologo;
            set
            {
                _odontologo = value;
                if (value != null && _odontologoMatricula != value.Matricula)
                {
                    _odontologoMatricula = value.Matricula; // Sincronizar automáticamente
                }
            }
        }
        public int MutualCuit
        {
            get => _mutual?.Cuit ?? _mutualCuit;
            set => _mutualCuit = value;
        }
        public Mutual? Mutual
        {
            get => _mutual;
            set
            {
                _mutual = value;
                if (value != null && _mutualCuit != value.Cuit)
                {
                    _mutualCuit = value.Cuit; // Sincronizar automáticamente
                }
            }
        }
        public OdontologoMutual(int nroAfiliado)
        {
            SetNroAfiliado(nroAfiliado);
        }
        public void SetNroAfiliado(int nroAfiliado)
        {
            if (nroAfiliado <= 0)
            {
                throw new ArgumentException("El número de afiliado debe ser mayor que cero.");
            }
            NroAfiliado = nroAfiliado;
        }
    }
}
