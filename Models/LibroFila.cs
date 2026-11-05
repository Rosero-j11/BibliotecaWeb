namespace BibliotecaWeb.Models;

// Fila del listado de libros con los ejemplares que están prestados en este momento.
public class LibroFila
{
    public int LibroId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int AnioPublicacion { get; set; }
    public int Ejemplares { get; set; }
    public int Prestados { get; set; }
    public int Disponibles => Ejemplares - Prestados;
}
