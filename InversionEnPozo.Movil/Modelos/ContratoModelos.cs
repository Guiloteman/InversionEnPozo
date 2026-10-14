using System;
using System.Collections.Generic;
using System.Text;

namespace InversionEnPozo.Movil.Modelos
{
    public class CrearProyectoRequestDto
    {
        public string NombreProyecto { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PresupuestoTotal { get; set; }
        public string ConstructoraId { get; set; } = "UsuarioConstructoraDefault";
    }

    public class ProyectoResponseDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal CostoTotal { get; set; }
        public decimal MontoRecaudado { get; set; }
        public decimal Porcentaje { get; set; }
    }

    public class AprobarHitoResponseDto
    {
        public string Mensaje { get; set; } = string.Empty;
        public int HitoId { get; set; }
        public string HashTransaccionHito { get; set; } = string.Empty;
        public DateTime FechaLiberacion { get; set; }
    }

    public class CrearProyectoResponseDto
    {
        public string Mensaje { get; set; } = string.Empty;
        public int ProyectoId { get; set; }
        public string HashContratoGenesis { get; set; } = string.Empty;
    }
}