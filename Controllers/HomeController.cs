using System.Diagnostics;
using BibliotecaWeb.Data;
using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb.Controllers;

public class HomeController : Controller
{
    private readonly BibliotecaDbContext _context;

    public HomeController(BibliotecaDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var hoy = DateTime.Today;
        var pendientes = _context.Prestamos.Where(p => p.FechaDevolucion == null);

        var modelo = new InicioViewModel
        {
            TotalLibros = await _context.Libros.CountAsync(),
            TotalEjemplares = await _context.Libros.SumAsync(l => (int?)l.Ejemplares) ?? 0,
            TotalSocios = await _context.Socios.CountAsync(),
            PrestamosActivos = await pendientes.CountAsync(p => p.FechaLimite >= hoy),
            PrestamosVencidos = await pendientes.CountAsync(p => p.FechaLimite < hoy),
            Vencidos = await pendientes.Where(p => p.FechaLimite < hoy)
                .Include(p => p.Libro).Include(p => p.Socio)
                .OrderBy(p => p.FechaLimite).AsNoTracking().ToListAsync(),
            Recientes = await _context.Prestamos
                .Include(p => p.Libro).Include(p => p.Socio)
                .OrderByDescending(p => p.FechaPrestamo).ThenByDescending(p => p.PrestamoId)
                .Take(5).AsNoTracking().ToListAsync()
        };

        return View(modelo);
    }

    public IActionResult Acerca()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
