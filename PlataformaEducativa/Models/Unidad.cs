using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlataformaEducativa.Models
{
    public class Unidad
    {
        public int UnidadId { get; set; }

        public int MateriaId { get; set; }

        [Required(ErrorMessage = "El nombre de la unidad es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public int Orden { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime UltimaModificacion { get; set; }

        // Propiedades de navegación
        public virtual Materia Materia { get; set; }
        public virtual ICollection<Subtema> Subtemas { get; set; }
    }
}