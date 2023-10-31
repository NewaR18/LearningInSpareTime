using LearnJWT.AppDbContext;
using LearnJWT.Repository;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TryPKI.CertificatesFunctions;
using TryPKI.Middleware;

namespace TryPKI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddTransient<CertificateValidation>();
            services.AddAuthentication(
                //options =>
                //{
                //    options.DefaultAuthenticateScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
                //    options.DefaultChallengeScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
                //    options.DefaultScheme = CertificateAuthenticationDefaults.AuthenticationScheme;
                //}
            )
            .AddCertificate(options => {
                options.AllowedCertificateTypes = CertificateTypes.SelfSigned;
                options.RevocationMode = X509RevocationMode.NoCheck;
                options.Events = new CertificateAuthenticationEvents
                {
                    OnCertificateValidated = context =>
                    {
                        var validationService = context.HttpContext.RequestServices.GetService<CertificateValidation>();
                        if (validationService.ValidateCertificate(context.ClientCertificate))
                        {
                            context.Success();
                        }
                        else
                        {
                            context.Fail("Invalid certificate");
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.Fail("Invalid certificate");
                        return Task.CompletedTask;
                    }
                };
            })
            .AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TryPKI", Version = "v1" });
            });
            var connStr = Configuration.GetConnectionString("myconnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connStr));
            services.AddScoped<IJwtUser, JwtUserRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TryPKI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseMiddleware<ChooseAuthenticationMethodMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
