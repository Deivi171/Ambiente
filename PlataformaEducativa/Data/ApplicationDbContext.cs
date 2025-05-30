using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using PlataformaEducativa.Models;

namespace PlataformaEducativa.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Materia> Materias { get; set; }
        public DbSet<Unidad> Unidades { get; set; }
        public DbSet<Subtema> Subtemas { get; set; }
        public DbSet<Clase> Clases { get; set; }
        public DbSet<Archivo> Archivos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Bitacora> Bitacoras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuraciones adicionales
            modelBuilder.Entity<Materia>()
                .HasMany(m => m.Unidades)
                .WithOne(u => u.Materia)
                .HasForeignKey(u => u.MateriaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Unidad>()
                .HasMany(u => u.Subtemas)
                .WithOne(s => s.Unidad)
                .HasForeignKey(s => s.UnidadId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subtema>()
                .HasMany(s => s.Clases)
                .WithOne(c => c.Subtema)
                .HasForeignKey(c => c.SubtemaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Clase>()
                .HasMany(c => c.Archivos)
                .WithOne(a => a.Clase)
                .HasForeignKey(a => a.ClaseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}