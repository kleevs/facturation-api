using FacturationApi.Spi;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Threading.Tasks;
using System.Xml.XPath;
using Web.Tools;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Configuration : AppConfiguration
    {
        public string BasePath { get; set; }
        public string ConnectionStrings { get; set; }
        public string ConnectionStringFtp { get; set; }
        public string ConnectionStringSmtp { get; set; }
        public string Key { get; set; }
        public string Iv { get; set; }
        public bool IsLocal { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface AppConfiguration
    {
        string ConnectionStrings { get; }
        string ConnectionStringFtp { get; }
        string ConnectionStringSmtp { get; }
        string Key { get; }
        string Iv { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            configuration.Bind(Configuration = new Configuration());
        }

        public Configuration Configuration;

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            var basePath = Configuration.BasePath;
            var isLocal = Configuration.IsLocal;
            
            services.AddScoped(typeof(Logger<>));
            services.AddScoped<Db.IHasher, Hasher>();
            services.AddScoped<IAuthenticationProvider, AuthenticationProvider>();
            services.AddSingleton<AppConfiguration>(Configuration);
            services.AddHttpContextAccessor();
            var healthchecks = services.AddHealthChecks();

            if (true/*isLocal*/) 
            {
                services.AddScoped<Db.IProvider, Db.LocalProvider>();
                services.AddDbContext<Db.LocalProvider>(options => options.UseInMemoryDatabase("database"));
            }
            else
            {
                services.AddScoped<Db.IProvider, Db.MySqlProvider>();
                services.AddDbContext<Db.MySqlProvider>(options => options.UseMySql(Configuration.ConnectionStrings));
                healthchecks.AddDbContextCheck<Db.MySqlProvider>();
            }

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = async (context) =>
                        {
                            await Task.Run(() =>
                            {
                                context.Response.StatusCode = 401;
                            });
                        }
                    };
                    options.LoginPath = "/accounts/login";
                    options.AccessDeniedPath = new PathString("/account/login");
                    options.Cookie.Path = "/";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                });
            services.AddMvc(option =>
            {
                option.Filters.Add(new AuthorizeFilter());
                option.Filters.Add(new BusinessExceptionFilter());
                option.EnableEndpointRouting = false;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            })  
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<PathPrefixInsertDocumentFilter>(basePath);
                c.CustomSchemaIds(type => type.ToString());
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FacturationApi", Version = "v1" });
                c.IncludeXmlComments(() =>
                {
                    var basePath = Directory.GetCurrentDirectory();
                    var fileName = $"Web.xml";
                    return new XPathDocument(Path.Combine(basePath, fileName));
                });
            });

            services.AddHealthChecksUI("facturationhealthchecksdb", setup =>
            {
                setup.AddHealthCheckEndpoint("Facturation microservice", "http://localhost/health");
            });
            services.AddCors();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Db.IProvider provider)
        {
            var basePath = Configuration.BasePath;
            if (env.EnvironmentName == "Development")
            {
                app.UseDeveloperExceptionPage();
            }

            if (Configuration.IsLocal)
            {
                var localProvider = provider as Db.LocalProvider;
                localProvider.AccountProvider.Add(new Repository.Models.Account
                {
                    Id = 1,
                    Email = "test@mail.com",
                    Password = "1234"
                });
                localProvider.UtilisateurData.Add(new Repository.Models.UtilisateurData 
                {
                    Id = 1,
                    UserId = 1,
                    LastName = "Test",
                    FirstName = "test",
                    Street = "1 rue du test",
                    Complement = "",
                    ZipCode = "75001",
                    Country = "France",
                    City = "Paris",
                    Phone = "0142355072",
                    NumTva = "FR80820841021",
                    Siret = "82084102100018",
                    Email = "test@mail.com"
                });
                localProvider.Facture.Add(new Repository.Models.Facture 
                {
                    Id = 1,
                    UserDataId = 1,
                    Numero = 1,
                    RaisonSociale = "Raison sociale",
                    DateCreation = new System.DateTime(2020, 06, 12),
                    DateEcheance = new System.DateTime(2020, 08, 31),
                    PaymentOption = 1,
                    LastName = "Doe",
                    FirstName = "John",
                    Street = "11 route de saint leu",
                    Complement = "",
                    ZipCode = "93430",
                    Country = "France",
                    City = "Villetaneuse",
                    Services = new System.Collections.Generic.List<Repository.Models.Service> 
                    {
                        new Repository.Models.Service 
                        {
                            Id = 1,
                            FactureId = 1,
                            Description = "Dev web 15 jours",
                            Price = 500,
                            Quantity = 15,
                            Tva = 20,
                            Unite = "jour",
                        }
                    },
                    Paiements = new System.Collections.Generic.List<Repository.Models.Paiement> 
                    {
                        new Repository.Models.Paiement 
                        {
                            Id = 1,
                            FactureId = 1,
                            DateCreation = new System.DateTime(2020, 06, 12),
                            Value = 9000
                        }
                    }
                });
                localProvider.SaveChanges();
            }

            app.UseRouting();

            app.UseCors(builder => builder.WithOrigins("http://localhost:8080", "http://localhost:9000")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{basePath}/swagger/v1/swagger.json", "Facturation api v1");
                c.InjectStylesheet("https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css");
                c.InjectJavascript("https://code.jquery.com/jquery-3.3.1.min.js");
                c.InjectJavascript("https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js");
                c.InjectJavascript("https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js");
                c.InjectJavascript($"{basePath}/swagger-ui.js");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            app.UseHealthChecksUI(config => config.UIPath = $"/hc-ui");

            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Facturation}/{action=Index}/{id?}");
            });
        }
    }
}
