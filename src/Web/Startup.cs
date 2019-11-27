using FacturationApi.Spi;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Web.Tools;

namespace Web
{
    public class Configuration : AppConfiguration
    {
        public string ConnectionStrings { get; set; }
        public string FtpUser { get; set; }
        public string FtpPassword { get; set; }
        public string FtpHost { get; set; }
        public int FtpPort { get; set; }
    }

    public interface AppConfiguration
    {
        string FtpUser { get; }
        string FtpPassword { get; }
        string FtpHost { get; }
        int FtpPort { get; }
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            configuration.Bind(Configuration = new Configuration());
        }

        public Configuration Configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<Db.Provider>();
            services.AddScoped<Db.IHasher, Hasher>();
            services.AddScoped<IAuthenticationProvider, AuthenticationProvider>();
            services.AddSingleton<AppConfiguration>(Configuration);
            services.AddHttpContextAccessor();

            services.AddDbContext<Db.Provider>(options =>
                options.UseMySQL(Configuration.ConnectionStrings));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = async (context) =>
                        {
                            await Task.Run(() =>
                            {
                                context.Response.StatusCode = 301;
                            });
                        }
                    };
                    options.LoginPath = "/accounts/login";
                    options.AccessDeniedPath = new PathString("/account/login");
                    options.Cookie.Path = "/";
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Cookie.SameSite = SameSiteMode.None;
                });
            services.AddMvc(option =>
            {
                option.Filters.Add(new AuthorizeFilter());
            })
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatString = "dd/MM/yyyy";
            })  
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200", "http://localhost:9000")
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Facturation}/{action=Index}/{id?}");
            });
        }
    }
}
