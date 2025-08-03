using Gerenciamento_de_Tarefas.Application.Services;
using Gerenciamento_de_Tarefas.Domain.Repositories;
using Gerenciamento_de_Tarefas.Infrastructure.Repositories;
using Npgsql;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL (Dapper)
builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("Postgres")));

// Injeção do Repositório
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

builder.Services.AddScoped<ITarefaService, TarefaService>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<ITarefaRepository>();
    await repo.CriarTabelaAsync();
}

using (var scope = app.Services.CreateScope())
{
    var usuarioRepository = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();
    await usuarioRepository.CriarTabelaAsync();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();



    app.Run();
