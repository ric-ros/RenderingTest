using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RenderingTest.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<DataService>();

await builder.Build().RunAsync();
