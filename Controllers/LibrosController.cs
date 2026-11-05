using BibliotecaWeb.Data;
using BibliotecaWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb.Controllers;

public class LibrosController : Controller
{
    private readonly BibliotecaDbContext _context;

    public LibrosController(BibliotecaDbContext context)
    {
        _context = context;
    }

    // GET: Libros
    public async Task<IActionResult> Index(string? busqueda, int? categoriaId, bool soloDisponibles = false, int pagina = 1)
    {
        var consulta = _context.Libros.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            consulta = consulta.Where(l => l.Titulo.Contains(busqueda) || l.ISBN.Contains(busqueda) || l.Autor!.Nombre.Contains(busqueda));
        }

        if (categoriaId.HasValue)
        {
            consulta = consulta.Where(l => l.CategoriaId == categoriaId);
        }

        var filas = consulta.Select(l => new LibroFila
        {
            LibroId = l.LibroId,
            Titulo = l.Titulo,
            ISBN = l.ISBN,
            Autor = l.Autor!.Nombre,
            Categoria = l.Categoria!.Nombre,
            AnioPublicacion = l.AnioPublicacion,
            Ejemplares = l.Ejemplares,
            Prestados = l.Prestamos.Count(p => p.FechaDevolucion == null)
        });

        if (soloDisponibles)
        {
            filas = filas.Where(f => f.Ejemplares > f.Prestados);
        }

        ViewData["Busqueda"] = busqueda;
        ViewData["SoloDisponibles"] = soloDisponibles;
        ViewData["CategoriaId"] = new SelectList(await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync(), "CategoriaId", "Nombre", categoriaId);
        return View(await filas.OrderBy(f => f.Titulo).PaginarAsync(pagina));
    }

    // GET: Libros/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var libro = await _context.Libros
            .Include(l => l.Autor)
            .Include(l => l.Categoria)
            .Include(l => l.Prestamos).ThenInclude(p => p.Socio)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.LibroId == id);

        return libro == null ? NotFound() : View(libro);
    }

    // GET: Libros/Create
    public async Task<IActionResult> Create()
    {
        await CargarListasAsync();
        return View(new Libro { AnioPublicacion = DateTime.Today.Year });
    }

    // POST: Libros/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Titulo,ISBN,AnioPublicacion,Ejemplares,AutorId,CategoriaId")] Libro libro)
    {
        await ValidarIsbnUnicoAsync(libro);
        if (!ModelState.IsValid)
        {
            await CargarListasAsync(libro);
            return View(libro);
        }

        _context.Add(libro);
        await _context.SaveChangesAsync();
        TempData["Mensaje"] = $"Libro \"{libro.Titulo}\" creado.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Libros/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var libro = await _context.Libros.FindAsync(id);
        if (libro == null) return NotFound();

        await CargarListasAsync(libro);
        return View(libro);
    }

    // POST: Libros/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("LibroId,Titulo,ISBN,AnioPublicacion,Ejemplares,AutorId,CategoriaId")] Libro libro)
    {
        if (id != libro.LibroId) return NotFound();

        await ValidarIsbnUnicoAsync(libro);

        var prestados = await _context.Prestamos.CountAsync(p => p.LibroId == id && p.FechaDevolucion == null);
        if (libro.Ejemplares < prestados)
        {
            ModelState.AddModelError(nameof(Libro.Ejemplares), $"Hay {prestados} ejemplar(es) prestados; no puede dejar menos que eso.");
        }

        if (!ModelState.IsValid)
        {
            await CargarListasAsync(libro);
            return View(libro);
        }

        try
        {
            _context.Update(libro);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await _context.Libros.AnyAsync(l => l.LibroId == id)) return NotFound();
            throw;
        }

        TempData["Mensaje"] = $"Libro \"{libro.Titulo}\" actualizado.";
        return RedirectToAction(nameof(Index));
    }

    // GET: Libros/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var libro = await _context.Libros
            .Include(l => l.Autor)
            .Include(l => l.Categoria)
            .Include(l => l.Prestamos)
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.LibroId == id);

        return libro == null ? NotFound() : View(libro);
    }

    // POST: Libros/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var libro = await _context.Libros.Include(l => l.Prestamos).FirstOrDefaultAsync(l => l.LibroId == id);
        if (libro == null) return RedirectToAction(nameof(Index));

        if (libro.Prestamos.Any())
        {
            TempData["Error"] = $"No se puede eliminar \"{libro.Titulo}\" porque tiene préstamos en su historial.";
            return RedirectToAction(nameof(Index));
        }

        _context.Libros.Remove(libro);
        await _context.SaveChangesAsync();
        TempData["Mensaje"] = $"Libro \"{libro.Titulo}\" eliminado.";
        return RedirectToAction(nameof(Index));
    }

    private async Task ValidarIsbnUnicoAsync(Libro libro)
    {
        if (await _context.Libros.AnyAsync(l => l.ISBN == libro.ISBN && l.LibroId != libro.LibroId))
        {
            ModelState.AddModelError(nameof(Libro.ISBN), "Ya existe un libro con ese ISBN.");
        }
    }

    private async Task CargarListasAsync(Libro? libro = null)
    {
        ViewData["AutorId"] = new SelectList(await _context.Autores.OrderBy(a => a.Nombre).ToListAsync(), "AutorId", "Nombre", libro?.AutorId);
        ViewData["CategoriaId"] = new SelectList(await _context.Categorias.OrderBy(c => c.Nombre).ToListAsync(), "CategoriaId", "Nombre", libro?.CategoriaId);
    }
}
