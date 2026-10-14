namespace InversionEnPozo.Api.DTOs
{
    public class CrearProyectoDto
    {
        public string NombreProyecto { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal PresupuestoTotal { get; set; }
        public decimal MontoRecaudado { get; set; } = 0;
        public string ConstructoraId { get; set; } = string.Empty;
    }
}
