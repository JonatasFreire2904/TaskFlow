using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TaskFlow.Blazor;
using TaskFlow.Blazor.Services;
using System;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient público (sem token) - útil pro login
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// TokenProvider
builder.Services.AddScoped<TokenProvider>();

// registrar AuthService
builder.Services.AddScoped<AuthService>();

await builder.Build().RunAsync();