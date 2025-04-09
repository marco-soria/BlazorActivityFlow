using ActivityFlow.Client;
using ActivityFlow.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Obtener la URL base de la API desde la configuración
var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? 
    throw new InvalidOperationException("La URL base de la API no está configurada en appsettings.json");

// Configurar el HttpClient con la URL base
builder.Services.AddScoped(sp => new HttpClient 
{ 
    BaseAddress = new Uri(apiBaseUrl) 
});

// Registrar los servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IActivityService, ActivityService>();

await builder.Build().RunAsync();
