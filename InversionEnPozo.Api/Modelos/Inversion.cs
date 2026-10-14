using InversionEnPozo.Api.Modelos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InversionEnPozo.Api.Modelos
{
    public class Inversion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProyectoInversionId { get; set; }

        [ForeignKey("ProyectoInversionId")]
        public ProyectoInversion? ProyectoInversion { get; set; }

        [Required]
        public string InversionistaId { get; set; } = string.Empty; // ID del usuario inversor

        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoAportado { get; set; }

        [MaxLength(100)]
        public string TokensOtorgados { get; set; } = string.Empty; // Cantidad o porcentaje de tokens/metros cuadrados

        [MaxLength(200)]
        public string? TxHashEthereum { get; set; } // Hash de la transacción en la red blockchain (opcional/híbrido)

        public DateTime FechaInversion { get; set; } = DateTime.UtcNow;
    }
}