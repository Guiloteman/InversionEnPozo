namespace InversionEnPozo.Movil.Modelos
{
    public class ProyectoInmobiliario
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public decimal CostoTotal { get; set; }
        public decimal MontoRecaudado { get; set; }
        public string ConstructoraId { get; set; } = string.Empty;
    }
}