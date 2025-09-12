using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Carrega o ocelot.json (faça isso ANTES de AddOcelot para garantir que a config esteja disponível)
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

// Registra o Ocelot e outros serviços
builder.Services.AddOcelot();

var app = builder.Build();

// Opcional: rotas ou middlewares customizados podem vir antes ou depois do UseOcelot dependendo do caso.
// Aqui deixamos o Ocelot como middleware principal de proxy.
await app.UseOcelot();

app.MapGet("/", () => "Hello from ApiGateway");

app.Run();
