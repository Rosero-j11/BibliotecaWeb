using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BibliotecaWeb.Models;

public class Libro
{
    public int LibroId { get; set; }

    [Required(ErrorMessage = "El título es obligatorio.")]
    [StringLength(150, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "El ISBN es obligatorio.")]
    [RegularExpression(@"^\d{10}(\d{3})?$", ErrorMessage = "El ISBN debe tener 10 o 13 dígitos, sin guiones.")]
    public string ISBN { get; set; } = string.Empty;

    [Range(1450, 2100, ErrorMessage = "El año debe estar entre {1} y {2}.")]
    [Display(Name = "Año de publicación")]
    public int AnioPublicacion { get; set; }

    [Range(1, 1000, ErrorMessage = "Debe haber entre {1} y {2} ejemplares.")]
    public int Ejemplares { get; set; } = 1;

    [Display(Name = "Autor")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un autor.")]
    public int AutorId { get; set; }

    [Display(Name = "Categoría")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una categoría.")]
    public int CategoriaId { get; set; }

    [ValidateNever]
    public Autor? Autor { get; set; }

    [ValidateNever]
    public Categoria? Categoria { get; set; }

    [ValidateNever]
    public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
