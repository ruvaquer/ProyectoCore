using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Program
    {
        //Modificamos este método para ejecutar nuestro context que esta en persistencia usado para poder ejecutar la migración
        public static void Main(string[] args)
        {
            var hostServer =  CreateHostBuilder(args).Build();
            //Ahora creo un contexto
            using(var ambiente = hostServer.Services.CreateScope()){
                var services = ambiente.ServiceProvider;
                //Aqui es donde le indico la instancia al CursoOnlineContext
                var context = services.GetRequiredService<CursosOnlineContext>();
                //También valido que se ejecute correctamente este procedimiento metiendolo en un try
                try{
                    context.Database.Migrate();
                }catch(Exception ex){
                  var logging = services.GetRequiredService<ILogger<Program>>();
                  logging.LogError(ex, "Ocurio un error en la migración.");
                }
            }

            hostServer.Run();

            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
