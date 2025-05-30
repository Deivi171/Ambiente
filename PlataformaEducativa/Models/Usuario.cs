using System;
using System.ComponentModel.DataAnnotations;

namespace PlataformaEducativa.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(20)]
        public string Rol { get; set; } // Admin, Profesor, Estudiante

        public DateTime FechaCreacion { get; set; }

        public DateTime UltimaModificacion { get; set; }
    }
}