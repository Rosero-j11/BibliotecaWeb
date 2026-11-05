using BibliotecaWeb.Data;
using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb.Controllers;

public class PrestamosController : Controller
{
    private readonly BibliotecaDbContext _context;

    public PrestamosController(BibliotecaDbContext context)
    {
        _context = context;
    }

    // GET: Prestamos
    public async Task<IActionResult> Index(string? busqueda, EstadoPrestamo? estado, int pagina = 1)
    {
        var hoy = DateTime.Today;
        var consulta = _context.Prestamos.Include(p => p.Libro).Include(p => p.Socio).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(p => p.Libro!.Titulo.Contains(busqueda) || p.Socio!.Nombre.Contains(busqueda) || p.Socio!.Documento.Contains(busqueda));
        }

        consulta = estado switch
        {
            EstadoPrestamo.Activo => consulta.Where(p => p.FechaDevolucion == null && p.FechaLimite >= hoy),
            EstadoPrestamo.Vencido => consulta.Where(p => p.FechaDevolucion == null && p.FechaLimite < hoy),
            EstadoPrestamo.Devuelto => consulta.Where(p => p.FechaDevolucion != null),
            _ => consulta
        };

        ViewData["Busqueda"] = busqueda;
        ViewData["Estado"] = estado;
        return View(await consulta.OrderByDescending(p => p.FechaPrestamo).ThenByDescending(p => p.PrestamoId).PaginarAsync(pagina));
    }

    // GET: Prestamos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var prestamo = await _context.Prestamos
            .Include(p => p.Libro).ThenInclude(l => l!.Autor)
            .Include(p => p.Socio)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PrestamoId == id);

        return prestamo == null ? NotFound() : View(prestamo);
    }

    // GET: Prestamos/Create?libroId=1&socioId=2
    public async Task<IActionResult> Create(int? libroId, int? socioId)
    {
        var prestamo = new Prestamo { LibroId = libroId ?? 0, SocioId = socioId ?? 0 };
        await CargarListasAsync(prestamo);
        return View(prestamo);
    }

    // POST: Prestamos/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LibroId,SocioId,FechaPrestamo,FechaLimite")] Prestamo prestamo)
    {
        await ValidarReglasAsync(prestamo);
        if (!ModelState.IsValid)
        {
            await CargarListasAsync(prestamo);
            return View(prestamo);
        }

        _context.Add(prestamo);
        await _context.SaveChangesAsync();

        var libro = await _context.Libros.FindAsync(prestamo.LibroId);
        TempData["Mensaje"] = $"Préstamo de \"{libro?.Titulo}\" registrado. Debe devolverse el {prestamo.FechaLimite:dd/MM/yyyy}.";
        return RedirectToAction(nameof(Index));
    }

    // POST: Prestamos/Devolver/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Devolver(int id, string? volverA)
    {
        var prestamo = await _context.Prestamos.Include(p => p.Libro).FirstOrDefaultAsync(p => p.PrestamoId == id);
        if (prestamo == null) return NotFound();

        if (prestamo.FechaDevolucion.HasValue)
        {
            TempData["Error"] = "Este préstamo ya estaba devuelto.";
        }
        else
        {
            var estabaVencido = prestamo.Estado == EstadoPrestamo.Vencido;
            prestamo.FechaDevolucion = DateTime.Today;
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = $"\"{prestamo.Libro?.Titulo}\" devuelto" +
                (estabaVencido ? $" con {(DateTime.Today - prestamo.FechaLimite.Date).Days} día(s) de retraso." : " a tiempo.");
        }

        return Url.IsLocalUrl(volverA) ? LocalRedirect(volverA) : RedirectToAction(nameof(Index));
    }

    // GET: Prestamos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var prestamo = await _context.Prestamos.Include(p => p.Libro).Include(p => p.Socio).AsNoTracking()
            .FirstOrDefaultAsync(p => p.PrestamoId == id);

        return prestamo == null ? NotFound() : View(prestamo);
    }

    // POST: Prestamos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var prestamo = await _context.Prestamos.FindAsync(id);
        if (prestamo != null)
        {
            _context.Prestamos.Remove(prestamo);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Préstamo eliminado del historial.";
        }

        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarReglasAsync(Prestamo prestamo)
    {
        if (prestamo.FechaPrestamo.Date > DateTime.Today)
        {
            ModelState.AddModelError(nameof(Prestamo.FechaPrestamo), "La fecha de préstamo no puede ser futura.");
        }

        if (prestamo.FechaLimite.Date < prestamo.FechaPrestamo.Date)
        {
            ModelState.AddModelError(nameof(Prestamo.FechaLimite), "La fecha límite debe ser igual o posterior a la fecha de préstamo.");
        }

        var libro = await _context.Libros.Include(l => l.Prestamos).AsNoTracking()
            .FirstOrDefaultAsync(l => l.LibroId == prestamo.LibroId);
        if (libro != null && libro.Prestamos.Count(p => p.FechaDevolucion == null) >= libro.Ejemplares)
        {
            ModelState.AddModelError(nameof(Prestamo.LibroId), $"No quedan ejemplares disponibles de \"{libro.Titulo}\".");
        }

        var activosSocio = await _context.Prestamos.AsNoTracking()
            .Where(p => p.SocioId == prestamo.SocioId && p.FechaDevolucion == null)
            .ToListAsync();

        if (activosSocio.Any(p => p.Estado == EstadoPrestamo.Vencido))
        {
            ModelState.AddModelError(nameof(Prestamo.SocioId), "El socio tiene préstamos vencidos; debe devolverlos antes de llevar otro libro.");
        }
        else if (activosSocio.Count >= Socio.MaxPrestamosActivos)
        {
            ModelState.AddModelError(nameof(Prestamo.SocioId), $"El socio ya tiene {Socio.MaxPrestamosActivos} préstamos activos (máximo permitido).");
        }

        if (activosSocio.Any(p => p.LibroId == prestamo.LibroId))
        {
            ModelState.AddModelError(nameof(Prestamo.LibroId), "El socio ya tiene prestado un ejemplar de este libro.");
        }
    }

    private async Task CargarListasAsync(Prestamo prestamo)
    {
        var libros = await _context.Libros
            .OrderBy(l => l.Titulo)
            .Select(l => new { l.LibroId, l.Titulo, Disponibles = l.Ejemplares - l.Prestamos.Count(p => p.FechaDevolucion == null) })
            .ToListAsync();

        ViewData["LibroId"] = new SelectList(
            libros.Select(l => new { l.LibroId, Texto = $"{l.Titulo} ({l.Disponibles} disponible(s))" }),
            "LibroId", "Texto", prestamo.LibroId);

        ViewData["SocioId"] = new SelectList(
            await _context.Socios.OrderBy(s => s.Nombre).Select(s => new { s.SocioId, Texto = s.Nombre + " - " + s.Documento }).ToListAsync(),
            "SocioId", "Texto", prestamo.SocioId);
    }
}
