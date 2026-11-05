using BibliotecaWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb.Data;

// Datos iniciales que inserta la migración para que la app no arranque vacía.
public static class SeedData
{
    public static void Aplicar(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>().HasData(
            new Autor { AutorId = 1, Nombre = "Gabriel García Márquez", Nacionalidad = "Colombiana" },
            new Autor { AutorId = 2, Nombre = "Isabel Allende", Nacionalidad = "Chilena" },
            new Autor { AutorId = 3, Nombre = "Jorge Luis Borges", Nacionalidad = "Argentina" },
            new Autor { AutorId = 4, Nombre = "George Orwell", Nacionalidad = "Británica" },
            new Autor { AutorId = 5, Nombre = "J. R. R. Tolkien", Nacionalidad = "Británica" },
            new Autor { AutorId = 6, Nombre = "Robert C. Martin", Nacionalidad = "Estadounidense" },
            new Autor { AutorId = 7, Nombre = "Laura Restrepo", Nacionalidad = "Colombiana" });

        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { CategoriaId = 1, Nombre = "Novela", Descripcion = "Narrativa de ficción extensa" },
            new Categoria { CategoriaId = 2, Nombre = "Cuento", Descripcion = "Relatos cortos" },
            new Categoria { CategoriaId = 3, Nombre = "Ciencia ficción", Descripcion = "Futuros posibles y distopías" },
            new Categoria { CategoriaId = 4, Nombre = "Fantasía", Descripcion = "Mundos imaginarios" },
            new Categoria { CategoriaId = 5, Nombre = "Tecnología", Descripcion = "Programación e ingeniería de software" });

        modelBuilder.Entity<Libro>().HasData(
            new Libro { LibroId = 1, Titulo = "Cien años de soledad", ISBN = "9780307474728", AnioPublicacion = 1967, Ejemplares = 4, AutorId = 1, CategoriaId = 1 },
            new Libro { LibroId = 2, Titulo = "El amor en los tiempos del cólera", ISBN = "9780307387264", AnioPublicacion = 1985, Ejemplares = 2, AutorId = 1, CategoriaId = 1 },
            new Libro { LibroId = 3, Titulo = "Crónica de una muerte anunciada", ISBN = "9781400034710", AnioPublicacion = 1981, Ejemplares = 3, AutorId = 1, CategoriaId = 1 },
            new Libro { LibroId = 4, Titulo = "La casa de los espíritus", ISBN = "9781501117015", AnioPublicacion = 1982, Ejemplares = 2, AutorId = 2, CategoriaId = 1 },
            new Libro { LibroId = 5, Titulo = "Ficciones", ISBN = "9780802130303", AnioPublicacion = 1944, Ejemplares = 2, AutorId = 3, CategoriaId = 2 },
            new Libro { LibroId = 6, Titulo = "El Aleph", ISBN = "9788499089508", AnioPublicacion = 1949, Ejemplares = 1, AutorId = 3, CategoriaId = 2 },
            new Libro { LibroId = 7, Titulo = "1984", ISBN = "9780451524935", AnioPublicacion = 1949, Ejemplares = 3, AutorId = 4, CategoriaId = 3 },
            new Libro { LibroId = 8, Titulo = "Rebelión en la granja", ISBN = "9780451526342", AnioPublicacion = 1945, Ejemplares = 2, AutorId = 4, CategoriaId = 3 },
            new Libro { LibroId = 9, Titulo = "El hobbit", ISBN = "9780547928227", AnioPublicacion = 1937, Ejemplares = 3, AutorId = 5, CategoriaId = 4 },
            new Libro { LibroId = 10, Titulo = "La comunidad del anillo", ISBN = "9780547928210", AnioPublicacion = 1954, Ejemplares = 2, AutorId = 5, CategoriaId = 4 },
            new Libro { LibroId = 11, Titulo = "Código limpio", ISBN = "9780132350884", AnioPublicacion = 2008, Ejemplares = 2, AutorId = 6, CategoriaId = 5 },
            new Libro { LibroId = 12, Titulo = "Arquitectura limpia", ISBN = "9780134494166", AnioPublicacion = 2017, Ejemplares = 1, AutorId = 6, CategoriaId = 5 },
            new Libro { LibroId = 13, Titulo = "Delirio", ISBN = "9789587045495", AnioPublicacion = 2004, Ejemplares = 2, AutorId = 7, CategoriaId = 1 });

        modelBuilder.Entity<Socio>().HasData(
            new Socio { SocioId = 1, Nombre = "Ana María López", Documento = "1001234567", Email = "ana.lopez@example.com", Telefono = "3001234567", FechaRegistro = new DateTime(2026, 8, 3) },
            new Socio { SocioId = 2, Nombre = "Carlos Andrés Gómez", Documento = "1017654321", Email = "carlos.gomez@example.com", Telefono = "3109876543", FechaRegistro = new DateTime(2026, 8, 20) },
            new Socio { SocioId = 3, Nombre = "Valentina Ríos", Documento = "1036987452", Email = "valentina.rios@example.com", FechaRegistro = new DateTime(2026, 9, 7) },
            new Socio { SocioId = 4, Nombre = "Santiago Herrera", Documento = "1152456789", Email = "santiago.herrera@example.com", Telefono = "3204567890", FechaRegistro = new DateTime(2026, 9, 25) },
            new Socio { SocioId = 5, Nombre = "Laura Martínez", Documento = "1020456123", Email = "laura.martinez@example.com", FechaRegistro = new DateTime(2026, 10, 10) });

        modelBuilder.Entity<Prestamo>().HasData(
            new Prestamo { PrestamoId = 1, LibroId = 1, SocioId = 1, FechaPrestamo = new DateTime(2026, 9, 1), FechaLimite = new DateTime(2026, 9, 16), FechaDevolucion = new DateTime(2026, 9, 14) },
            new Prestamo { PrestamoId = 2, LibroId = 7, SocioId = 2, FechaPrestamo = new DateTime(2026, 10, 5), FechaLimite = new DateTime(2026, 10, 20) },
            new Prestamo { PrestamoId = 3, LibroId = 9, SocioId = 3, FechaPrestamo = new DateTime(2026, 10, 28), FechaLimite = new DateTime(2026, 11, 12) },
            new Prestamo { PrestamoId = 4, LibroId = 11, SocioId = 4, FechaPrestamo = new DateTime(2026, 11, 2), FechaLimite = new DateTime(2026, 11, 17) },
            new Prestamo { PrestamoId = 5, LibroId = 6, SocioId = 1, FechaPrestamo = new DateTime(2026, 10, 15), FechaLimite = new DateTime(2026, 10, 30) },
            new Prestamo { PrestamoId = 6, LibroId = 4, SocioId = 5, FechaPrestamo = new DateTime(2026, 10, 1), FechaLimite = new DateTime(2026, 10, 16), FechaDevolucion = new DateTime(2026, 10, 16) });
    }
}
