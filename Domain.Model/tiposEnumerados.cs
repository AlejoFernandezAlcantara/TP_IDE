using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public enum tiposEnumerados
    {
        DNI,
        Pasaporte
    }
    public enum EstadoReserva
    {
        Pendiente,
        Confirmada,
        Cancelada,
        Completada
    }
    public enum EstadoTurno
    {
        Disponible,
        Reservado,
        Cancelado
    }
}