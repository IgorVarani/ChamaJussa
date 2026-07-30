using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ChamaJussaAPI.Contexts;
using ChamaJussaAPI.Interfaces;
using ChamaJussaAPI.Repositories;
using ChamaJussaAPI.Applications.Services;
using ChamaJussaAPI.Applications.Autenticacao;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Configura política de CORS ampla para acesso do frontend
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configura Swagger para suportar JWT Bearer Token nos testes manuais
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT desta forma: Bearer {seu_token}"
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

// Registrar DbContext com a conexão SQL Server scaffolded
builder.Services.AddDbContext<ChamaJussaContext>(options =>
    options.UseSqlServer("Server=DESKTOP-LAO5MIJ\\SQLEXPRESSTEC;Database=ChamaJussa;user=sa;pwd=abc123;TrustServerCertificate=True"));

// Registros de Injeção de Dependência (DI)
// Repositórios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();

// Serviços
builder.Services.AddScoped<IStorageService, LocalStorageService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<OrdemServicoService>();
builder.Services.AddScoped<AutenticacaoService>();

// JWT Helper
builder.Services.AddScoped<GeradorTokenJwt>();

// Configuração do JWT Bearer Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var chave = builder.Configuration["Jwt:Key"]!;
        var issuer = builder.Configuration["Jwt:Issuer"]!;
        var audience = builder.Configuration["Jwt:Audience"]!;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave))
        };
    });

var app = builder.Build();

// ATENÇÃO: O middleware UseCors DEVE vir no topo do pipeline, ANTES de StaticFiles, Authentication e Authorization!
app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Habilita a servir arquivos estáticos (como as imagens salvas na pasta wwwroot)
app.UseStaticFiles();

// Ativação da autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
