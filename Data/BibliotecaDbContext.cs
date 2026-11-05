using BibliotecaWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaWeb.Data;

public class BibliotecaDbContext : DbContext
{
    public BibliotecaDbContext(DbContextOptions<BibliotecaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Autor> Autores => Set<Autor>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Libro> Libros => Set<Libro>();
    public DbSet<Socio> Socios => Set<Socio>();
    public DbSet<Prestamo> Prestamos => Set<Prestamo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>().ToTable("Autores");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categorias");
            entity.HasIndex(c => c.Nombre).IsUnique();
        });

        modelBuilder.Entity<Libro>(entity =>
        {
            entity.ToTable("Libros");
            entity.Property(l => l.ISBN).HasMaxLength(13);
            entity.HasIndex(l => l.ISBN).IsUnique();

            // Restrict: no se puede borrar un autor o una categoría que todavía tiene libros.
            entity.HasOne(l => l.Autor).WithMany(a => a.Libros)
                .HasForeignKey(l => l.AutorId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(l => l.Categoria).WithMany(c => c.Libros)
                .HasForeignKey(l => l.CategoriaId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Socio>(entity =>
        {
            entity.ToTable("Socios");
            entity.HasIndex(s => s.Documento).IsUnique();
            entity.Property(s => s.FechaRegistro).HasColumnType("date");
        });

        modelBuilder.Entity<Prestamo>(entity =>
        {
            entity.ToTable("Prestamos");
            entity.Property(p => p.FechaPrestamo).HasColumnType("date");
            entity.Property(p => p.FechaLimite).HasColumnType("date");
            entity.Property(p => p.FechaDevolucion).HasColumnType("date");

            entity.HasOne(p => p.Libro).WithMany(l => l.Prestamos)
                .HasForeignKey(p => p.LibroId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(p => p.Socio).WithMany(s => s.Prestamos)
                .HasForeignKey(p => p.SocioId).OnDelete(DeleteBehavior.Restrict);
        });

        SeedData.Aplicar(modelBuilder);
    }
}
