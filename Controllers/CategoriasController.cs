using BibliotecaWeb.Data;
using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb.Controllers;

public class CategoriasController : Controller
{
    private readonly BibliotecaDbContext _context;

    public CategoriasController(BibliotecaDbContext context)
    {
        _context = context;
    }

    // GET: Categorias
    public async Task<IActionResult> Index(string? busqueda, int pagina = 1)
    {
        var consulta = _context.Categorias.Include(c => c.Libros).AsNoTracking();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(c => c.Nombre.Contains(busqueda));
        }

        ViewData["Busqueda"] = busqueda;
        return View(await consulta.OrderBy(c => c.Nombre).PaginarAsync(pagina));
    }

    // GET: Categorias/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var categoria = await _context.Categorias
            .Include(c => c.Libros).ThenInclude(l => l.Autor)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoriaId == id);

        return categoria == null ? NotFound() : View(categoria);
    }

    // GET: Categorias/Create
    public IActionResult Create()
    {
        return View(new Categoria());
    }

    // POST: Categorias/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Descripcion")] Categoria categoria)
    {
        await ValidarNombreUnicoAsync(categoria);
        if (!ModelState.IsValid) return View(categoria);

        _context.Add(categoria);
        await _context.SaveChangesAsync();
        TempData["Mensaje"] = $"Categoría \"{categoria.Nombre}\" creada.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Categorias/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var categoria = await _context.Categorias.FindAsync(id);
        return categoria == null ? NotFound() : View(categoria);
    }

    // POST: Categorias/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,Nombre,Descripcion")] Categoria categoria)
    {
        if (id != categoria.CategoriaId) return NotFound();

        await ValidarNombreUnicoAsync(categoria);
        if (!ModelState.IsValid) return View(categoria);

        try
        {
            _context.Update(categoria);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Categorias.AnyAsync(c => c.CategoriaId == id)) return NotFound();
            throw;
        }

        TempData["Mensaje"] = $"Categoría \"{categoria.Nombre}\" actualizada.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Categorias/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var categoria = await _context.Categorias.Include(c => c.Libros).AsNoTracking()
            .FirstOrDefaultAsync(c => c.CategoriaId == id);

        return categoria == null ? NotFound() : View(categoria);
    }

    // POST: Categorias/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var categoria = await _context.Categorias.Include(c => c.Libros).FirstOrDefaultAsync(c => c.CategoriaId == id);
        if (categoria == null) return RedirectToAction(nameof(Index));

        if (categoria.Libros.Any())
        {
            TempData["Error"] = $"No se puede eliminar \"{categoria.Nombre}\" porque tiene {categoria.Libros.Count} libro(s) asignados.";
            return RedirectToAction(nameof(Index));
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        TempData["Mensaje"] = $"Categoría \"{categoria.Nombre}\" eliminada.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarNombreUnicoAsync(Categoria categoria)
    {
        if (await _context.Categorias.AnyAsync(c => c.Nombre == categoria.Nombre && c.CategoriaId != categoria.CategoriaId))
        {
            ModelState.AddModelError(nameof(Categoria.Nombre), "Ya existe una categoría con ese nombre.");
        }
    }
}
