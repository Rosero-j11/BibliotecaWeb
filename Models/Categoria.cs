using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BibliotecaWeb.Models;

public class Categoria
{
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(60, ErrorMessage = "Máximo {1} caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Descripción")]
    public string? Descripcion { get; set; }

    [ValidateNever]
    public ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
