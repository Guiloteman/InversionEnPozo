namespace InversionEnPozo.Api.DTOs
{
    public class SolicitarDesembolsoDto
    {
        public int HitoId { get; set; }
        public string ComentariosAvance { get; set; } = string.Empty;
        public string ConstructoraId { get; set; } = string.Empty;
    }
}
