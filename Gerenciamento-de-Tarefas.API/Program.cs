//using Gerenciamento_de_Tarefas.Application.Interfaces;
//using Gerenciamento_de_Tarefas.Application.Services;
//using Gerenciamento_de_Tarefas.Domain.Repositories;
//using Gerenciamento_de_Tarefas.Infrastructure.Repositories;
//using Microsoft.AspNetCore.Http.Json;
//using Npgsql;
//using System.Data;
//using System.Text.Json.Serialization;

//var builder = WebApplication.CreateBuilder(args);

//// convertendo os enum do json
//builder.Services.ConfigureHttpJsonOptions(options =>
//{
//    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
//});

//builder.Services.Configure<JsonOptions>(options =>
//{
//    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
//});

//// Controllers e Swagger
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//// PostgreSQL 
//builder.Services.AddScoped<IDbConnection>(sp =>
//    new NpgsqlConnection(builder.Configuration.GetConnectionString("Postgres")));

//// Repisotories
//builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();

//builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

//builder.Services.AddScoped<ITarefaService, TarefaService>();

//builder.Services.AddScoped<IUsuarioService, UsuarioService>();

//builder.Services.AddScoped<ITokenService, TokenService>();

//var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var repo = scope.ServiceProvider.GetRequiredService<ITarefaRepository>();
//    await repo.CriarTabelaAsync();
//}

//using (var scope = app.Services.CreateScope())
//{
//    var usuarioRepository = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();
//    await usuarioRepository.CriarTabelaAsync();
//}
//app.UseSwagger();
//app.UseSwaggerUI();

//app.UseAuthorization();
//app.MapControllers();



//    app.Run();


using Gerenciamento_de_Tarefas.Application.Interfaces;
using Gerenciamento_de_Tarefas.Application.Services;
using Gerenciamento_de_Tarefas.Domain.Repositories;
using Gerenciamento_de_Tarefas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Convertendo os enum do JSON
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configura��o Swagger com autentica��o JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gerenciamento de Tarefas API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite: Bearer {seu token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Autentica��o JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// PostgreSQL 
builder.Services.AddScoped<IDbConnection>(sp =>
    new NpgsqlConnection(builder.Configuration.GetConnectionString("Postgres")));

// Repositories
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Services
builder.Services.AddScoped<ITarefaService, TarefaService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Cria��o das tabelas
using (var scope = app.Services.CreateScope())
{
    var repo = scope.ServiceProvider.GetRequiredService<ITarefaRepository>();
    await repo.CriarTabelaAsync();

    var usuarioRepository = scope.ServiceProvider.GetRequiredService<IUsuarioRepository>();
    await usuarioRepository.CriarTabelaAsync();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication(); // Adicionado para validar JWT
app.UseAuthorization();

app.MapControllers();

app.Run();
