using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class DienteCara
    {


        //FK DE DIENTE
        public int _dienteNro { get; set; }
        public Diente _diente { get; set; }
        //FK DE CARA
        public int _caraId { get; set; }
        public Cara _cara { get; set; }

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
        public int CaraId
        {
            get => _cara?.IdCara ?? _caraId;
            set => _caraId = value;
        }
        public Cara? Cara
        {
            get => _cara;
            set
            {
                _cara = value;
                if (value != null && _caraId != value.IdCara)
                {
                    _caraId = value.IdCara; // Sincronizar automáticamente
                }
            }
        }
    }
}