using System;
using System.Collections.Generic;

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

    public static class OdontogramaConstantes
    {
        // 32 Dientes FDI estándar
        public static readonly int[] Dientes = new int[]
        {
            18, 17, 16, 15, 14, 13, 12, 11,
            21, 22, 23, 24, 25, 26, 27, 28,
            48, 47, 46, 45, 44, 43, 42, 41,
            31, 32, 33, 34, 35, 36, 37, 38
        };

        // 5 Caras anatómicas
        public static readonly (string Codigo, string Nombre)[] Caras = new (string, string)[]
        {
            ("V", "Vestibular"),
            ("L", "Lingual / Palatina"),
            ("O", "Oclusal / Incisal"),
            ("M", "Mesial"),
            ("D", "Distal")
        };
    }
}
