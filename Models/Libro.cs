using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EP_CarlosAndresRuizMiranda.Models
{
    public class Libro
    {
        public int ID { get; set; }

        [DisplayName("Título")]
        [MinLength(10, ErrorMessage = "La longitud mínima del título es de 6 caracteres.")]
        [Required(ErrorMessage = "El título del libro es obligatorio.")]
        public string Titulo { get; set; }

        [DisplayName("Publicación")]
        [Required(ErrorMessage = "Debe indicar el año de publicación del libro.")]
        public int Publicacion { get; set; }

        [DisplayName("Autor")]
        [Required(ErrorMessage = "Debe indicar el autor del libro.")]
        public int IdAutor { get; set; }

        [DisplayName("Categoría")]
        [Required(ErrorMessage = "Debe indicar la categoría del libro.")]
        public int IdCategoria { get; set; }

        public Autor Autor { get; set; }
        public Categoria Categoria { get; set; }
    }
}
