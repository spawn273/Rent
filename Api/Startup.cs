using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using RentApi.Infrastructure.Database;
using Microsoft.OpenApi.Models;
using RentApi.Infrastructure;
using SmartAnalytics.BASF.Backend.Application.Authorization;

namespace RentApi
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
            services.SetUpAuthorization(Configuration);


            services.AddScoped<ApiHelper>();

            //services.AddCors(o => o.AddPolicy("cors", builder =>
            //{
            //    builder.AllowAnyOrigin()
            //           .AllowAnyMethod()
            //           .AllowAnyHeader()
            //            .WithExposedHeaders("X-Total-Count", "Content-Range"); ;
            //}));

            services.AddControllers();

            services.AddHttpContextAccessor();

            // Options
            services.AddOptions();

            services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql("Host=localhost;Port=5432;Database=rent;Username=postgres;Password=123"));

            // Register the Swagger generator, defining 1 or more Swagger documents
            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"Please enter into field the word 'Bearer' following by space and JWT. Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
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
                        new string[] { }
                   }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseCors(confPolicy =>
            {
                confPolicy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                        .WithExposedHeaders("X-Total-Count", "Content-Range");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                    //.RequireAuthorization();
            });
        }
    }
}
