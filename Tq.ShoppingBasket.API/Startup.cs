using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using Tq.ShoppingBasket.API.Middleware;
using Tq.ShoppingBasket.Application.Service;
using Tq.ShoppingBasket.Application.Validators;
using Tq.ShoppingBasket.Domain.Repositories;
using Tq.ShoppingBasket.Infrastructure;

namespace Tq.ShoppingBasket.API
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
            services.AddScoped<IShoppingBasketService, ShoppingBasketService>();
            services.AddScoped<IShoppingBasketValidator, ShoppingBasketValidator>();
            services.AddSingleton<IShoppingBasketRepository, ShoppingBasketRepository>();

            services.AddMediatR(AppDomain.CurrentDomain.Load("Tq.ShoppingBasket.Application"));
            services.AddAutoMapper(AppDomain.CurrentDomain.Load("Tq.ShoppingBasket.Application"));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tq.ShoppingBasket.API", Version = "v1", Description = "Domain-Driven Design Architecture Demo" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition(ApiKeyAuth.APIKEY, new OpenApiSecurityScheme
                {
                    Description = "Api key needed to access the endpoints. ApiKey: 123",
                    In = ParameterLocation.Header,
                    Name = ApiKeyAuth.APIKEY,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = ApiKeyAuth.APIKEY,
                            Type = SecuritySchemeType.ApiKey,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = ApiKeyAuth.APIKEY
                            }
                        },
                        new string[] {}
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
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tq.ShoppingBasket.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();

            app.UseMiddleware<ApiKeyAuth>();
            app.UseMiddleware<ErrorHandler>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
