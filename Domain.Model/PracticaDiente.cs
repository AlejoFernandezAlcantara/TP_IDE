using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    internal class PracticaDiente
    {
        //FK DE PRACTICA
        public int _practicaCodigo { get; set; }
        public Practica _practica { get; set; }
        //FK DE DIENTE
        public int _dienteNro { get; set; }  
        public Diente _diente { get; set; }
        //GET Y SET DE LAS 2FK
        public int PracticaCodigo
        {
            get => _practica?.CodigoPractica ?? _practicaCodigo;
            set => _practicaCodigo = value;
        }
        public Practica? Practica
        {
            get => _practica;
            set
            {
                _practica = value;
                if (value != null && _practicaCodigo != value.CodigoPractica)
                {
                    _practicaCodigo = value.CodigoPractica; // Sincronizar automáticamente
                }
            }
        }
        public int DienteNro
        {
            get => _diente?.NroDiente ?? _dienteNro;
            set => _dienteNro = value;
        }
        public Diente? Diente
        {
            get => _diente;
            set
            {
                _diente = value;
                if (value != null && _dienteNro != value.NroDiente)
                {
                    _dienteNro = value.NroDiente; // Sincronizar automáticamente
                }
            }
        }

    }
}
