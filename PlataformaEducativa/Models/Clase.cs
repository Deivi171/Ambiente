﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PlataformaEducativa.Models
{
    public class Clase
    {
        public int ClaseId { get; set; }

        public int SubtemaId { get; set; }

        [Required(ErrorMessage = "El nombre de la clase es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public int Orden { get; set; }

        public DateTime FechaCreacion { get; set; }

        public DateTime UltimaModificacion { get; set; }

        // Propiedades de navegación
        public virtual Subtema Subtema { get; set; }
        public virtual ICollection<Archivo> Archivos { get; set; }
    }

    /* Estilos personalizados para la Plataforma Educativa */

    body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    background-color: #f8f9fa;
}

.navbar - brand {
    font - weight: bold;
    font - size: 1.5rem;
}

.card {
    border: none;
box - shadow: 0 0.125rem 0.25rem rgba(0, 0, 0, 0.075);
transition: box - shadow 0.15s ease-in-out;
}

.card: hover {
    box - shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
}

.card - header {
    border - bottom: 1px solid rgba(0, 0, 0, 0.125);
    font - weight: 600;
}

.btn {
    border-radius: 0.375rem;
font - weight: 500;
}

.btn - group.btn {
    border - radius: 0;
}

.btn - group.btn:first - child {
    border - top - left - radius: 0.375rem;
    border - bottom - left - radius: 0.375rem;
}

.btn - group.btn:last - child {
    border - top - right - radius: 0.375rem;
    border - bottom - right - radius: 0.375rem;
}

.list - group - item {
border: 1px solid rgba(0, 0, 0, 0.125);
}

.list - group - item:hover {
    background-color: #f8f9fa;
}

.breadcrumb {
    background-color: #e9ecef;
    border - radius: 0.375rem;
}

.jumbotron {
    background: linear - gradient(135deg, #667eea 0%, #764ba2 100%);
    color: white;
border - radius: 0.5rem;
}

.footer {
    background-color: #343a40;
    color: white;
padding: 1rem 0;
margin - top: 2rem;
}

/* Estilos para el explorador de archivos */
#explorer .list-group-item {
    padding: 0.75rem 1rem;
}

#explorer .list-group-item .list-group {
    margin - top: 0.5rem;
margin - left: 1rem;
}

#explorer .list-group-item .list-group .list-group-item {
    padding: 0.5rem 0.75rem;
border: none;
border - left: 2px solid #dee2e6;
}

/* Estilos para formularios */
.form - group {
    margin - bottom: 1rem;
}

.form - control {
    border - radius: 0.375rem;
}

.form - control:focus {
    border-color: #80bdff;
    box - shadow: 0 0 0 0.2rem rgba(0, 123, 255, 0.25);
}

/* Estilos para alertas */
.alert {
    border-radius: 0.375rem;
}

/* Estilos para modales */
.modal - content {
    border - radius: 0.5rem;
}

.modal - header {
    border - bottom: 1px solid #dee2e6;
}

.modal - footer {
    border - top: 1px solid #dee2e6;
}

/* Estilos responsivos */
@media(max - width: 768px) {
    .btn - group {
        flex - direction: column;
    }
    
    .btn - group.btn {
        border - radius: 0.375rem;
        margin - bottom: 0.25rem;
    }
    
    .card - footer.btn - group {
    display: flex;
        flex - direction: column;
    }
}

/* Animaciones */
@keyframes fadeIn
{
    from
    {
    opacity: 0;
    transform: translateY(20px);
    }
    to
    {
    opacity: 1;
    transform: translateY(0);
    }
}

.card {
    animation: fadeIn 0.5s ease-in-out;
}

/* Estilos para iconos */
.fas, .far {
    margin-right: 0.5rem;
}

/* Estilos para el área de carga de archivos */
.file - upload - area {
border: 2px dashed #dee2e6;
    border - radius: 0.5rem;
padding: 2rem;
    text - align: center;
    background - color: #f8f9fa;
    transition: border - color 0.15s ease-in-out;
}

.file - upload - area:hover {
    border-color: #007bff;
    background - color: #e7f3ff;
}

.file - upload - area.dragover {
    border - color: #28a745;
    background - color: #d4edda;
}
}


