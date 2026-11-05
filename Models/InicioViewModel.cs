namespace BibliotecaWeb.Models;

public class InicioViewModel
{
    public int TotalLibros { get; set; }
    public int TotalEjemplares { get; set; }
    public int TotalSocios { get; set; }
    public int PrestamosActivos { get; set; }
    public int PrestamosVencidos { get; set; }
    public List<Prestamo> Vencidos { get; set; } = new();
    public List<Prestamo> Recientes { get; set; } = new();
}
