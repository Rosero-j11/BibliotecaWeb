using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BibliotecaWeb.Models;

public enum EstadoPrestamo
{
    Activo,
    Vencido,
    Devuelto
}

public class Prestamo
{
    public const int DiasPorDefecto = 15;

    public int PrestamoId { get; set; }

    [Display(Name = "Libro")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un libro.")]
    public int LibroId { get; set; }

    [Display(Name = "Socio")]
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione un socio.")]
    public int SocioId { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de préstamo")]
    public DateTime FechaPrestamo { get; set; } = DateTime.Today;

    [DataType(DataType.Date)]
    [Display(Name = "Fecha límite")]
    public DateTime FechaLimite { get; set; } = DateTime.Today.AddDays(DiasPorDefecto);

    [DataType(DataType.Date)]
    [Display(Name = "Fecha de devolución")]
    public DateTime? FechaDevolucion { get; set; }

    [ValidateNever]
    public Libro? Libro { get; set; }

    [ValidateNever]
    public Socio? Socio { get; set; }

    [NotMapped]
    public EstadoPrestamo Estado =>
        FechaDevolucion.HasValue ? EstadoPrestamo.Devuelto
        : FechaLimite.Date < DateTime.Today ? EstadoPrestamo.Vencido
        : EstadoPrestamo.Activo;
}
