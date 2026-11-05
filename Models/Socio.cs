using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BibliotecaWeb.Models;

public class Socio
{
    public const int MaxPrestamosActivos = 3;

    public int SocioId { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Nombre completo")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El documento es obligatorio.")]
    [RegularExpression(@"^\d{5,15}$", ErrorMessage = "El documento debe tener entre 5 y 15 dígitos.")]
    public string Documento { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo no es válido.")]
    [StringLength(120)]
    [Display(Name = "Correo")]
    public string Email { get; set; } = string.Empty;

    [Phone(ErrorMessage = "El teléfono no es válido.")]
    [StringLength(20)]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.Today;

    [ValidateNever]
    public ICollection<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
}
