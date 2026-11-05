using BibliotecaWeb.Data;
using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb.Controllers;

public class AutoresController : Controller
{
    private readonly BibliotecaDbContext _context;

    public AutoresController(BibliotecaDbContext context)
    {
        _context = context;
    }

    // GET: Autores
    public async Task<IActionResult> Index(string? busqueda, int pagina = 1)
    {
        var consulta = _context.Autores.Include(a => a.Libros).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(a => a.Nombre.Contains(busqueda) || (a.Nacionalidad != null && a.Nacionalidad.Contains(busqueda)));
        }

        ViewData["Busqueda"] = busqueda;
        return View(await consulta.OrderBy(a => a.Nombre).PaginarAsync(pagina));
    }

    // GET: Autores/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var autor = await _context.Autores
            .Include(a => a.Libros).ThenInclude(l => l.Categoria)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AutorId == id);

        return autor == null ? NotFound() : View(autor);
    }

    // GET: Autores/Create
    public IActionResult Create()
    {
        return View(new Autor());
    }

    // POST: Autores/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Nacionalidad")] Autor autor)
    {
        if (!ModelState.IsValid) return View(autor);

        _context.Add(autor);
        await _context.SaveChangesAsync();
        TempData["Mensaje"] = $"Autor \"{autor.Nombre}\" creado.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Autores/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var autor = await _context.Autores.FindAsync(id);
        return autor == null ? NotFound() : View(autor);
    }

    // POST: Autores/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("AutorId,Nombre,Nacionalidad")] Autor autor)
    {
        if (id != autor.AutorId) return NotFound();
        if (!ModelState.IsValid) return View(autor);

        try
        {
            _context.Update(autor);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Autores.AnyAsync(a => a.AutorId == id)) return NotFound();
            throw;
        }

        TempData["Mensaje"] = $"Autor \"{autor.Nombre}\" actualizado.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Autores/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var autor = await _context.Autores.Include(a => a.Libros).AsNoTracking()
            .FirstOrDefaultAsync(a => a.AutorId == id);

        return autor == null ? NotFound() : View(autor);
    }

    // POST: Autores/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var autor = await _context.Autores.Include(a => a.Libros).FirstOrDefaultAsync(a => a.AutorId == id);
        if (autor == null) return RedirectToAction(nameof(Index));

        if (autor.Libros.Any())
        {
            TempData["Error"] = $"No se puede eliminar a \"{autor.Nombre}\" porque tiene {autor.Libros.Count} libro(s) registrados.";
            return RedirectToAction(nameof(Index));
        }

        _context.Autores.Remove(autor);
        await _context.SaveChangesAsync();
        TempData["Mensaje"] = $"Autor \"{autor.Nombre}\" eliminado.";
        return RedirectToAction(nameof(Index));
    }
}
