using System.Globalization;
using BibliotecaWeb.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    var mensajes = options.ModelBindingMessageProvider;
    mensajes.SetValueMustNotBeNullAccessor(_ => "Este campo es obligatorio.");
    mensajes.SetMissingBindRequiredValueAccessor(_ => "Este campo es obligatorio.");
    mensajes.SetAttemptedValueIsInvalidAccessor((valor, _) => $"El valor '{valor}' no es válido.");
    mensajes.SetValueMustBeANumberAccessor(_ => "Debe ser un número.");
});

// En Azure la cadena "DefaultConnection" se define en la configuración del App Service
// y reemplaza la de appsettings.json (LocalDB), así no queda ninguna contraseña en el repositorio.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Falta la cadena de conexión 'DefaultConnection'.");

builder.Services.AddDbContext<BibliotecaDbContext>(options =>
    options.UseSqlServer(connectionString, sql =>
        // La base gratuita de Azure SQL se pausa sola; los reintentos cubren el tiempo que tarda en despertar.
        sql.EnableRetryOnFailure(maxRetryCount: 6, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)));

var app = builder.Build();

// Crea o actualiza la base de datos (tablas y datos iniciales) al arrancar, tanto en local como en Azure.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BibliotecaDbContext>();
    db.Database.Migrate();
}

var cultura = new CultureInfo("es-CO");
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(cultura),
    SupportedCultures = new[] { cultura },
    SupportedUICultures = new[] { cultura }
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
