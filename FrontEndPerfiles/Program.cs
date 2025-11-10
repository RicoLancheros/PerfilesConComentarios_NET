using FrontEndPerfiles;
using FrontEndPerfiles.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Registrar servicios
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PerfilService>();
builder.Services.AddScoped<ComentarioService>();

await builder.Build().RunAsync();
