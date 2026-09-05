using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class PacienteMutual
    {
        public int NroAfiliado { get; set; }
        public bool Cubre { get; set; }

        //FK MUTUAL
        private int _mutualCuit;
        private Mutual? _mutual;

        //FK PACIENTE
        private int _pacienteId;
        private Paciente? _paciente;

        //GET Y SET DE LAS 2FK
        public int PacienteId
        {
            get => _paciente?.NroPaciente ?? _pacienteId;
            set => _pacienteId = value;
        }

        public Paciente? Paciente
        {
            get => _paciente;
            set
            {
                _paciente = value;
                if (value != null && _pacienteId != value.NroPaciente)
                {
                    _pacienteId = value.NroPaciente; // Sincronizar automáticamente
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

        //CONSTRUCTOR
        public PacienteMutual(int nroAfiliado, bool cubre)
        {
            SetNroAfiliado(nroAfiliado);
            SetCubre(cubre);
        }
        public void SetNroAfiliado(int nroAfiliado)
        {
            if (nroAfiliado <= 0)
            {
                throw new ArgumentException("El número de afiliado debe ser un número positivo.");
            }
            NroAfiliado = nroAfiliado;
        }
        public void SetCubre(bool cubre)
        {
            Cubre = cubre;
        }
    }
}