using InversionEnPozo.Api.Modelos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InversionEnPozo.Api.Modelos
{
    public class HitoObra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProyectoInversionId { get; set; }

        [ForeignKey("ProyectoInversionId")]
        public ProyectoInversion? ProyectoInversion { get; set; }

        [Required]
        [MaxLength(150)]
        public string Titulo { get; set; } = string.Empty;

        [Column(TypeName = "decimal(5,2)")]
        public decimal PorcentajeAsignado { get; set; } // Ejemplo: 20.00% del total de la obra

        public bool Completado { get; set; } = false;

        public DateTime? FechaCompletado { get; set; }

        // --- Propiedades necesarias para los desembolsos y la blockchain ---
        [MaxLength(50)]
        public string Estado { get; set; } = "Pendiente";

        [MaxLength(100)]
        public string? HashTransaccion { get; set; }

        public DateTime? FechaActualizacion { get; set; }
    }
}