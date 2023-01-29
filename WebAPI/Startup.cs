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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Authentication;
using Aplicacion.Interfaces;
using Seguridad.TokenSeguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            services.TryAddSingleton<ISystemClock, SystemClock>();

            //Inyectamos esta interface y esta clase lo hago ya que así podemos acceder a los métdos que me va generar en seguridad 
            services.AddScoped<IJwtGenerador, JwtGenerador>();

            //Agregamos la lógica para que no permita consumir endpoints sin tener la seguridad del token
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi Palabra Secreta"));//Aqui ponemos la palabra clave que habiamos puesto dentro de Seguridad/TokenSeguridad/JwtGenerador.cs, Ya pondremos una más dificil
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
                
                //Así le aplicamos las restircciones que van a trabajar con ls clientes que tengan este tipo de dns /url y verificar quien me envía el token
                opt.TokenValidationParameters =  new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,//Cuando cualquier tipo de requiest debe ser validado por la lógica que hemos puesto dentgro del token
                    IssuerSigningKey = Key,//Pasamos la palabra clave mi clave secreta, envuelto dentro de un objeto Key
                    ValidateAudience = false, //Quien va poder crear esos tokens ahora lo pomnemos a false para que de momento lo pueda hacer cualquiera pero si solo se que van a ser unas determinadas compañias debemos conseguir su ip y configurarlo dentro de la aplicación
                    ValidateIssuer = false //Esto es como el envío del token, lo pongo a false para que de momento no envie nada a las ips que he dado autorización como de momento no valido eso lo dejo a false
                };

            });

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

            //Aqui debemos indicarle que mi app va usar la authenticacion que acabo de configurar
            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
