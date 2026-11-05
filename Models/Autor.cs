using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BibliotecaWeb.Models;

public class Autor
{
    public int AutorId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(60, ErrorMessage = "Máximo {1} caracteres.")]
    public string? Nacionalidad { get; set; }

    [ValidateNever]
    public ICollection<Libro> Libros { get; set; } = new List<Libro>();
}
