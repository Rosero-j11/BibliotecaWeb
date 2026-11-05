using BibliotecaWeb.Data;
using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb.Controllers;

public class SociosController : Controller
{
    private readonly BibliotecaDbContext _context;

    public SociosController(BibliotecaDbContext context)
    {
        _context = context;
    }

    // GET: Socios
    public async Task<IActionResult> Index(string? busqueda, int pagina = 1)
    {
        var consulta = _context.Socios.Include(s => s.Prestamos).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(s => s.Nombre.Contains(busqueda) || s.Documento.Contains(busqueda) || s.Email.Contains(busqueda));
        }

        ViewData["Busqueda"] = busqueda;
        return View(await consulta.OrderBy(s => s.Nombre).PaginarAsync(pagina));
    }

    // GET: Socios/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var socio = await _context.Socios
            .Include(s => s.Prestamos).ThenInclude(p => p.Libro)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.SocioId == id);

        return socio == null ? NotFound() : View(socio);
    }

    // GET: Socios/Create
    public IActionResult Create()
    {
        return View(new Socio());
    }

    // POST: Socios/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Documento,Email,Telefono,FechaRegistro")] Socio socio)
    {
        await ValidarDocumentoUnicoAsync(socio);
        if (!ModelState.IsValid) return View(socio);

        _context.Add(socio);
        await _context.SaveChangesAsync();
        TempData["Mensaje"] = $"Socio \"{socio.Nombre}\" registrado.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Socios/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var socio = await _context.Socios.FindAsync(id);
        return socio == null ? NotFound() : View(socio);
    }

    // POST: Socios/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("SocioId,Nombre,Documento,Email,Telefono,FechaRegistro")] Socio socio)
    {
        if (id != socio.SocioId) return NotFound();

        await ValidarDocumentoUnicoAsync(socio);
        if (!ModelState.IsValid) return View(socio);

        try
        {
            _context.Update(socio);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Socios.AnyAsync(s => s.SocioId == id)) return NotFound();
            throw;
        }

        TempData["Mensaje"] = $"Socio \"{socio.Nombre}\" actualizado.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Socios/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var socio = await _context.Socios.Include(s => s.Prestamos).AsNoTracking()
            .FirstOrDefaultAsync(s => s.SocioId == id);

        return socio == null ? NotFound() : View(socio);
    }

    // POST: Socios/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var socio = await _context.Socios.Include(s => s.Prestamos).FirstOrDefaultAsync(s => s.SocioId == id);
        if (socio == null) return RedirectToAction(nameof(Index));

        if (socio.Prestamos.Any())
        {
            TempData["Error"] = $"No se puede eliminar a \"{socio.Nombre}\" porque tiene préstamos en su historial.";
            return RedirectToAction(nameof(Index));
        }

        _context.Socios.Remove(socio);
        await _context.SaveChangesAsync();
        TempData["Mensaje"] = $"Socio \"{socio.Nombre}\" eliminado.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarDocumentoUnicoAsync(Socio socio)
    {
        if (await _context.Socios.AnyAsync(s => s.Documento == socio.Documento && s.SocioId != socio.SocioId))
        {
            ModelState.AddModelError(nameof(Socio.Documento), "Ya existe un socio con ese documento.");
        }
    }
}
