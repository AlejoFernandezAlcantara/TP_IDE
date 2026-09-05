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
        private int _mutualCuit;
        private Mutual? _mutual;
        //FK ODONTOLOGO
        private string _odontologoMatricula = string.Empty;
        private Odontologo? _odontologo;

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