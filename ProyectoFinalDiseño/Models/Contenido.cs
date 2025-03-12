using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoFinalDiseño.Models
{
    public class Contenido
    {
        [Key]
        public int ContenidoID { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string URL { get; set; } // Puede ser un enlace a un archivo o recurso externo

        [Required]
        public bool Habilitado { get; set; } = false; // Indica si el contenido está habilitado

        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? FechaModificacion { get; set; }
    }
}
