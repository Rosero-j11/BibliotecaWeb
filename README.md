# Biblioteca — Aplicación Web MVC con acceso a datos

## ASP.NET Core MVC + Entity Framework Core (Code First) + SQL Server, desplegada en Microsoft Azure

**Desarrollado por:** _[tu nombre]_

**Aplicación en línea:** _https://[nombre-de-tu-app].azurewebsites.net_

---

## 1. Descripción

Sistema web para gestionar una biblioteca: libros, autores, categorías, socios y préstamos. Controla cuántos ejemplares de cada libro hay disponibles y cuáles préstamos están vencidos.

Funcionalidades:

- CRUD completo (crear, listar, ver detalle, editar y eliminar) de **Libros, Autores, Categorías y Socios**.
- **Préstamos**: registrar préstamo, registrar devolución, filtrar por estado (Activo / Vencido / Devuelto) y eliminar del historial.
- **Panel de inicio** con totales, préstamos vencidos (con días de retraso) y últimos préstamos.
- **Búsqueda, filtros y paginación** en todos los listados.
- **Validaciones** en el servidor y en el navegador, con mensajes en español.

Reglas de negocio:

| Regla | Dónde se aplica |
|---|---|
| No se presta un libro sin ejemplares disponibles | `PrestamosController.ValidarReglasAsync` |
| Un socio puede tener máximo 3 préstamos activos | `PrestamosController.ValidarReglasAsync` |
| Un socio con préstamos vencidos no puede pedir otro libro | `PrestamosController.ValidarReglasAsync` |
| Un socio no puede tener dos ejemplares del mismo libro a la vez | `PrestamosController.ValidarReglasAsync` |
| La fecha límite no puede ser anterior a la fecha de préstamo | `PrestamosController.ValidarReglasAsync` |
| ISBN, documento del socio y nombre de categoría son únicos | Índices únicos + validación en los controladores |
| No se eliminan autores, categorías, libros o socios con datos relacionados | `DeleteBehavior.Restrict` + validación en los controladores |
| No se pueden dejar menos ejemplares que los que están prestados | `LibrosController.Edit` |

## 2. Tecnologías

- Visual Studio Code / Visual Studio 2022 y .NET SDK 10
- ASP.NET Core MVC (.NET 10)
- Entity Framework Core 10 — enfoque **Code First** con migraciones
- SQL Server: **LocalDB** en desarrollo y **Azure SQL Database** en producción
- **Azure App Service** (plan gratuito F1) para el despliegue
- Bootstrap 5 y Bootstrap Icons

## 3. Modelo de datos

```
Autor (1) ────< (N) Libro (N) >──── (1) Categoria
                     │
                    (1)
                     │
                    (N)
                  Prestamo (N) >──── (1) Socio
```

- **Autor** 1—N **Libro**
- **Categoría** 1—N **Libro**
- **Socio** N—N **Libro**, resuelta con la entidad **Préstamo**, que guarda `FechaPrestamo`, `FechaLimite` y `FechaDevolucion`.

| Tabla | Columnas principales |
|---|---|
| Autores | AutorId (PK), Nombre, Nacionalidad |
| Categorias | CategoriaId (PK), Nombre (único), Descripcion |
| Libros | LibroId (PK), Titulo, ISBN (único), AnioPublicacion, Ejemplares, AutorId (FK), CategoriaId (FK) |
| Socios | SocioId (PK), Nombre, Documento (único), Email, Telefono, FechaRegistro |
| Prestamos | PrestamoId (PK), LibroId (FK), SocioId (FK), FechaPrestamo, FechaLimite, FechaDevolucion |

El script SQL completo (tablas, llaves, índices y datos iniciales) generado desde las migraciones está en [`Database/BibliotecaDb.sql`](Database/BibliotecaDb.sql).

## 4. Estructura del proyecto

```
BibliotecaWeb/
├── Controllers/      Home, Libros, Autores, Categorias, Socios, Prestamos
├── Data/             BibliotecaDbContext, SeedData (datos iniciales), Paginacion
├── Models/           Entidades (Autor, Categoria, Libro, Socio, Prestamo), PageInfo y ViewModels
├── Migrations/       Migración InitialCreate generada por EF Core
├── Views/            Vistas Razor de cada controlador y parciales compartidas
├── Database/         Script SQL generado desde las migraciones
└── Program.cs        Configuración de servicios, cadena de conexión y migración automática
```

## 5. Paquetes instalados (NuGet)

```
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Tools
```

Herramienta de línea de comandos de EF Core:

```
dotnet tool install --global dotnet-ef
```

## 6. Ejecutar en local

1. La cadena de conexión de desarrollo en `appsettings.json` usa LocalDB:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=BibliotecaDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
   }
   ```

2. Crear la base de datos desde el código (Code First):

   ```
   dotnet build
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

   La aplicación también aplica las migraciones pendientes al arrancar (`db.Database.Migrate()` en `Program.cs`), así que este paso es opcional.

3. Ejecutar:

   ```
   dotnet run
   ```

   y abrir http://localhost:5133

## 7. Despliegue en Azure

Recursos usados (todos en el mismo grupo de recursos y región):

| Recurso | Plan |
|---|---|
| Azure SQL Database | Oferta gratuita (serverless, se pausa sola) |
| Azure App Service (Linux) | F1 gratuito, pila .NET 10 |

Pasos:

1. Crear el **grupo de recursos** `rg-biblioteca`.
2. Crear el **servidor SQL** y la **base de datos** `BibliotecaDb` con la oferta gratuita. En *Redes*, permitir el acceso de los servicios de Azure.
3. Crear la **Web App** con la pila **.NET 10 (LTS)** en Linux y el plan **F1**.
4. En la Web App, en **Configuración → Cadenas de conexión**, agregar `DefaultConnection` (tipo *SQLAzure*) con la cadena ADO.NET de la base de datos. La cadena con la contraseña vive solo en Azure, nunca en el repositorio.
5. En **Centro de implementación**, conectar este repositorio de GitHub (rama `main`). Azure crea un flujo de GitHub Actions que compila y publica en cada `push`.
6. Al primer arranque la app ejecuta las migraciones y crea las tablas y los datos iniciales en Azure SQL.

## 8. Capturas

_(Agregar aquí capturas del panel de inicio, los listados y la aplicación funcionando en Azure.)_

## Referencias

- Código de clase del profesor Walter H. Arboleda: https://github.com/walterarboleda (EFWP1, EFWP2)
- Documentación de EF Core: https://learn.microsoft.com/ef/core/
- Despliegue de ASP.NET Core en Azure App Service: https://learn.microsoft.com/azure/app-service/quickstart-dotnetcore
