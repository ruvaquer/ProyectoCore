using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistencia;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Aplicacion.Cursos;
using FluentValidation.AspNetCore;
using WebAPI.Middleware;
using Dominio;
using Microsoft.AspNetCore.Identity;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            //Aquí agrego el nombre del contexto con el que estoy trabajando la persistencia, también indicamos el tipo de conexión
            services.AddDbContext<CursosOnlineContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //Agregamos la configuración de IMedator de Consulta Cursos
            services.AddMediatR(typeof(Consulta.Handler).Assembly);
            
            //Realizamos una modificación despues de instalar la librería de FluentValidation modificando el AddControllers agregando un nuevo método llamandolo:
            //1º - AddFluentValidation(), si da error debemos importarlo desde la librería que instalamos de FluentValidation en el proyecto Aplicacion, debemos agregar obsoleto para quitar el warning
            //     - Configuración adicional indicandole que archivo debe validar, en este caso le indico que quiero validar la clase Nuevo de curso
            services.AddControllers().AddFluentValidation(cfg => cfg.RegisterValidatorsFromAssemblyContaining<Nuevo>());

            #region PARA EL CONTROL DE LA SEGURIDAD CON COREIDENTITY
            //Agrego CoreIdentity
            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            //Ahora agrego la instancia del entityFramework
            identityBuilder.AddEntityFrameworkStores<CursosOnlineContext>();
            //Manejador del acceso de los usuarios
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();
            #endregion FIN COREIDENTITY

            //Agregado por defecto
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ManejadorErrorMiddleware>();//Agregamos a la configuración nuestra propia clase para manejar los errores
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();//Por defecto la crea el proyecto la comento porque vamos a usar nuetra propia clase de manejador de errores
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
