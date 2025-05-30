using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlataformaEducativa.Models
{
    public class Materia
    {
        public int MateriaId { get; set; }

        [Required(ErrorMessage = "El nombre de la materia es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime UltimaModificacion { get; set; }

        // Propiedades de navegación
        public virtual ICollection<Unidad> Unidades { get; set; }
    }
}