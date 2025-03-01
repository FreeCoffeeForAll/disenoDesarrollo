using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalDiseño.Models
{
    public class Inventario
    {
        [Key]
        public int ObjetoID { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        [Range(0, int.MaxValue)]
        public int Cantidad { get; set; } = 1;

        [Required]
        [StringLength(50)]
        public string Estado { get; set; }  

        [DataType(DataType.DateTime)]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

       
        public int? CategoriaID { get; set; }
        [ForeignKey("CategoriaID")]
        public Categoria? Categoria { get; set; }
    }
}