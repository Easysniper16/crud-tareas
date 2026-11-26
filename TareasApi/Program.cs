using System.Reflection;
using TareasApi;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

var endpoints = Assembly.GetExecutingAssembly().GetTypes()
    .Where(t => t is { IsClass: true, IsAbstract: false } && typeof(IEndpoint).IsAssignableFrom(t));

foreach (var tipo in endpoints)
{
    ((IEndpoint)Activator.CreateInstance(tipo)!).Map(app);
}

app.Run();

public partial class Program { }
