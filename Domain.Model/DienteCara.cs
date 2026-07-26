using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class DienteCara
    {
        public int DienteNro { get; set; }

        public Diente Diente { get; set; }

        public int CaraId { get; set; }

        public Cara Cara { get; set; }

}
}