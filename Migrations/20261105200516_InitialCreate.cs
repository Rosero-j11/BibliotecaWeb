using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BibliotecaWeb.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Autores",
                columns: table => new
                {
                    AutorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Nacionalidad = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autores", x => x.AutorId);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "Socios",
                columns: table => new
                {
                    SocioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socios", x => x.SocioId);
                });

            migrationBuilder.CreateTable(
                name: "Libros",
                columns: table => new
                {
                    LibroId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    AnioPublicacion = table.Column<int>(type: "int", nullable: false),
                    Ejemplares = table.Column<int>(type: "int", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Libros", x => x.LibroId);
                    table.ForeignKey(
                        name: "FK_Libros_Autores_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Autores",
                        principalColumn: "AutorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Libros_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Prestamos",
                columns: table => new
                {
                    PrestamoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LibroId = table.Column<int>(type: "int", nullable: false),
                    SocioId = table.Column<int>(type: "int", nullable: false),
                    FechaPrestamo = table.Column<DateTime>(type: "date", nullable: false),
                    FechaLimite = table.Column<DateTime>(type: "date", nullable: false),
                    FechaDevolucion = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prestamos", x => x.PrestamoId);
                    table.ForeignKey(
                        name: "FK_Prestamos_Libros_LibroId",
                        column: x => x.LibroId,
                        principalTable: "Libros",
                        principalColumn: "LibroId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prestamos_Socios_SocioId",
                        column: x => x.SocioId,
                        principalTable: "Socios",
                        principalColumn: "SocioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Autores",
                columns: new[] { "AutorId", "Nacionalidad", "Nombre" },
                values: new object[,]
                {
                    { 1, "Colombiana", "Gabriel García Márquez" },
                    { 2, "Chilena", "Isabel Allende" },
                    { 3, "Argentina", "Jorge Luis Borges" },
                    { 4, "Británica", "George Orwell" },
                    { 5, "Británica", "J. R. R. Tolkien" },
                    { 6, "Estadounidense", "Robert C. Martin" },
                    { 7, "Colombiana", "Laura Restrepo" }
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "CategoriaId", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Narrativa de ficción extensa", "Novela" },
                    { 2, "Relatos cortos", "Cuento" },
                    { 3, "Futuros posibles y distopías", "Ciencia ficción" },
                    { 4, "Mundos imaginarios", "Fantasía" },
                    { 5, "Programación e ingeniería de software", "Tecnología" }
                });

            migrationBuilder.InsertData(
                table: "Socios",
                columns: new[] { "SocioId", "Documento", "Email", "FechaRegistro", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "1001234567", "ana.lopez@example.com", new DateTime(2026, 8, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ana María López", "3001234567" },
                    { 2, "1017654321", "carlos.gomez@example.com", new DateTime(2026, 8, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carlos Andrés Gómez", "3109876543" },
                    { 3, "1036987452", "valentina.rios@example.com", new DateTime(2026, 9, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Valentina Ríos", null },
                    { 4, "1152456789", "santiago.herrera@example.com", new DateTime(2026, 9, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Santiago Herrera", "3204567890" },
                    { 5, "1020456123", "laura.martinez@example.com", new DateTime(2026, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laura Martínez", null }
                });

            migrationBuilder.InsertData(
                table: "Libros",
                columns: new[] { "LibroId", "AnioPublicacion", "AutorId", "CategoriaId", "Ejemplares", "ISBN", "Titulo" },
                values: new object[,]
                {
                    { 1, 1967, 1, 1, 4, "9780307474728", "Cien años de soledad" },
                    { 2, 1985, 1, 1, 2, "9780307387264", "El amor en los tiempos del cólera" },
                    { 3, 1981, 1, 1, 3, "9781400034710", "Crónica de una muerte anunciada" },
                    { 4, 1982, 2, 1, 2, "9781501117015", "La casa de los espíritus" },
                    { 5, 1944, 3, 2, 2, "9780802130303", "Ficciones" },
                    { 6, 1949, 3, 2, 1, "9788499089508", "El Aleph" },
                    { 7, 1949, 4, 3, 3, "9780451524935", "1984" },
                    { 8, 1945, 4, 3, 2, "9780451526342", "Rebelión en la granja" },
                    { 9, 1937, 5, 4, 3, "9780547928227", "El hobbit" },
                    { 10, 1954, 5, 4, 2, "9780547928210", "La comunidad del anillo" },
                    { 11, 2008, 6, 5, 2, "9780132350884", "Código limpio" },
                    { 12, 2017, 6, 5, 1, "9780134494166", "Arquitectura limpia" },
                    { 13, 2004, 7, 1, 2, "9789587045495", "Delirio" }
                });

            migrationBuilder.InsertData(
                table: "Prestamos",
                columns: new[] { "PrestamoId", "FechaDevolucion", "FechaLimite", "FechaPrestamo", "LibroId", "SocioId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 9, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 2, null, new DateTime(2026, 10, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 7, 2 },
                    { 3, null, new DateTime(2026, 11, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), 9, 3 },
                    { 4, null, new DateTime(2026, 11, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 11, 4 },
                    { 5, null, new DateTime(2026, 10, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 1 },
                    { 6, new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Libros_AutorId",
                table: "Libros",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_CategoriaId",
                table: "Libros",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Libros_ISBN",
                table: "Libros",
                column: "ISBN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_LibroId",
                table: "Prestamos",
                column: "LibroId");

            migrationBuilder.CreateIndex(
                name: "IX_Prestamos_SocioId",
                table: "Prestamos",
                column: "SocioId");

            migrationBuilder.CreateIndex(
                name: "IX_Socios_Documento",
                table: "Socios",
                column: "Documento",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prestamos");

            migrationBuilder.DropTable(
                name: "Libros");

            migrationBuilder.DropTable(
                name: "Socios");

            migrationBuilder.DropTable(
                name: "Autores");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
