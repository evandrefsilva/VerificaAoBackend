using System;
using System.Collections.Generic;
using System.Linq;
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
using Application.Filters;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;

namespace Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Conn"))
            );
            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Verifica Ao API v1",
                    Version = "v1",
                    Description = "Verifica Ao API v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Verifica Ao API v1",
                        Email = "developer@tis.ao",
                        //Url = new Uri("https://digitalfactory.co.ao"),
                    },
                });
            });

            services.AddAuthentication(x =>
        {
            x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        })
           .AddCookie(config =>
           {
               //config.Cookie.IsEssential = true;
               config.Cookie.Name = $"verifica.Cookie";
               config.LoginPath = "/account/login";
               config.AccessDeniedPath = "/_401";

           });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin() // Permitir solicitações de qualquer origem
                        .AllowAnyMethod() // Permitir qualquer método HTTP
                        .AllowAnyHeader() // Permitir qualquer cabeçalho HTTP
                        .WithExposedHeaders("Content-Disposition") // Permitir cabeçalhos específicos
                );
            });
            services.AddHangfire(configuration => configuration
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSQLiteStorage());

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IComunicationService, ComunicationService>();
            services.AddTransient<IAppSettingsService, AppSettingsService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<INewsService, NewsService>();
            services.AddScoped<FileUploadService, FileUploadService>();

            var apiKey = Configuration.GetValue<string>("WesenderApiKey");
            var wesenderCliente = new WeSenderApiClient(apiKey);
            services.AddSingleton(wesenderCliente);
            //services.AddScoped<IVolunteerOpportunityService, VolunteerOpportunityService>();

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/_404";
                    await next();
                }
            });
            app.UseCors("CorsPolicy"); // Habilitar CORS
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Estrelas Ao Palco - API v1");
            });
            app.UseSwagger();
            app.UseHangfireServer();
            // Use Hangfire Dashboard
            app.UseHangfireDashboard("/e7d07bb4-89a7-4872-aee8-1a2bd98386f9", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "home",
                    pattern: "{controller=admin}/{action=Index}/{id?}");
            });
        }
    }
}
