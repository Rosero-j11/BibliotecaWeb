using BibliotecaWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb.Data;

public static class Paginacion
{
    public const int TamanoPagina = 8;

    public static async Task<Paginado<T>> PaginarAsync<T>(this IQueryable<T> consulta, int pagina, int tamano = TamanoPagina)
    {
        var total = await consulta.CountAsync();
        var info = new PageInfo { TotalItems = total, ItemsPerPage = tamano };
        info.CurrentPage = Math.Clamp(pagina, 1, info.TotalPages);

        var items = await consulta.Skip((info.CurrentPage - 1) * tamano).Take(tamano).ToListAsync();
        return new Paginado<T> { Items = items, PageInfo = info };
    }
}
