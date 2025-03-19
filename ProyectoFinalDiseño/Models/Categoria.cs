using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProyectoFinalDiseño.Models
{
    [Table("Categorias")]
    public class Categoria
    {
        [Key] 
        public int CategoriaID { get; set; }
        public string NombreCategoria { get; set; }
        
    }
}