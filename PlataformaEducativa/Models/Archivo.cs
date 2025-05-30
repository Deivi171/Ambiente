using System;
using System.ComponentModel.DataAnnotations;

namespace PlataformaEducativa.Models
{
    public class Archivo
    {
        public int ArchivoId { get; set; }

        public int ClaseId { get; set; }

        [Required(ErrorMessage = "El nombre del archivo es obligatorio")]
        [StringLength(255)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El tipo de archivo es obligatorio")]
        [StringLength(50)]
        public string Tipo { get; set; } // PDF, VIDEO, POWERPOINT

        [StringLength(100)]
        public string DriveFileId { get; set; }

        [StringLength(500)]
        public string DriveUrl { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime UltimaModificacion { get; set; }

        // Propiedades de navegación
        public virtual Clase Clase { get; set; }
    }
}