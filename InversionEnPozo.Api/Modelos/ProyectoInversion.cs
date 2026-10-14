using InversionEnPozo.Api.Modelos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InversionEnPozo.Api.Modelos
{
    public class ProyectoInversion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostoTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoRecaudado { get; set; }

        [Required]
        public string ConstructoraId { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public ICollection<HitoObra> Hitos { get; set; } = new List<HitoObra>();
        public ICollection<Inversion> Inversiones { get; set; } = new List<Inversion>();
    }
}