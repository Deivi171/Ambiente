using System;
using System.ComponentModel.DataAnnotations;

namespace PlataformaEducativa.Models
{
    public class Bitacora
    {
        public int BitacoraId { get; set; }

        public int? UsuarioId { get; set; }

        [Required]
        [StringLength(50)]
        public string Entidad { get; set; } // Materia, Unidad, Subtema, Clase, Archivo

        [Required]
        public int EntidadId { get; set; }

        [Required]
        [StringLength(50)]
        public string Accion { get; set; } // Crear, Modificar, Eliminar

        public string Detalles { get; set; }

        public DateTime FechaAccion { get; set; }

        // Propiedades de navegación
        public virtual Usuario Usuario { get; set; }
    }
}