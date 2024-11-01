using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Data;
using Services;
using Data.Context;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;
using Hangfire;
using Hangfire.MemoryStorage;
using Services.Clients;
using Hangfire.Storage.SQLite;
using WebApp.Filters;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Services.Helpers;
using Services.Models.DTO;
using Services.Models.Settings;

var builder = WebApplication.CreateBuilder(args);

// Configurações
var configuration = builder.Configuration;

// Injeção de dependências

// Configurar DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("Conn"))
);

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Disposition")
    );
});

// Autenticação
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            //ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://api.verifica.ao",
            ValidAudience = "https://verifica.ao",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("testetestetesteteste"))
        };
    });

// Configurações de SMTP e serviços adicionais
builder.Services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
builder.Services.AddSingleton(new AppSettings
{
    ApplicationUrl = configuration.GetValue<string>("ApplicationUrl")
});
builder.Services.AddSingleton(new WeSenderApiClient(configuration.GetValue<string>("WesenderApiKey")));
builder.Services.AddHttpContextAccessor();

// Adiciona serviços para a aplicação
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();

// Configuração do Swagger
builder.Services.AddSwaggerGen(c =>
{
       c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT con el prefijo 'Bearer ' antes del token"
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
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Verifica Ao API v1",
        Version = "v1",
        Description = "Verifica Ao API v1",
        Contact = new OpenApiContact
        {
            Name = "Verifica Ao API v1",
            Email = "developer@tis.ao"
        },
    });
});

// Configuração do Hangfire
builder.Services.AddHangfire(configuration => configuration
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSQLiteStorage()
);

// Registra serviços específicos
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IComunicationService, ComunicationService>();
builder.Services.AddTransient<IAppSettingsService, AppSettingsService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<INewsService, NewsService>();
builder.Services.AddScoped<IVerificationService, VerificationService>();
builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
builder.Services.AddScoped<FileUploadService, FileUploadService>();

var app = builder.Build();

// Configurações de Middleware

// Seed de permissões
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        PermissionSeeder.SeedPermissionsAsync(services).Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ocorreu um erro durante o seeding das permissões.");
    }
}

// Middleware de desenvolvimento e produção
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Estrelas Ao Palco - API v1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

// Middleware para redirecionamento de erros 404
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/_404";
        await next();
    }
});

// Configuração de CORS
app.UseCors("CorsPolicy");

// Configuração de arquivos estáticos
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireServer();
app.UseHangfireDashboard("/e7d07bb4-89a7-4872-aee8-1a2bd98386f9", new DashboardOptions
{
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

// Configuração de endpoints
app.MapControllerRoute(
    name: "home",
    pattern: "{controller=admin}/{action=Index}/{id?}"
);

app.Run();
