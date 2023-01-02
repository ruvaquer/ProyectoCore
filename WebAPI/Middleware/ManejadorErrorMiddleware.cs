using System;
using System.Net;
using System.Threading.Tasks;
using Aplicacion.HandLerError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    //Interceptador de requerimientos capturabndo posibles errores de la app
    public class ManejadorErrorMiddleware
    {
        //Propiedades de inyection
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorErrorMiddleware> _logger;

        //Recibe 2 parámetros para poder manejar los estados de respuesta hacia el cliente
        public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger){
            _next = next;
            _logger = logger;
        }

        //Este método nos ayudará a pasar el evento del controlador a al evento aplicación
        public async Task Invocar(HttpContext context){
            try{
                await _next(context);
            }catch(Exception ex){
                //Ante cualquier error controlamos con nuestro método personal e imprimir con su detalle
                await ManejadorExcepcionAsincrono(context, ex, _logger);
            }
        }

        /*
        Tipos de errores:
            2XX = Transaccion correcta -- NO ES ERROR!!!
            3XX = No se modifico
            4XX = Errores en el FrontEnd
            5XX = Errores en el servidor
        */
        private async Task ManejadorExcepcionAsincrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> logger){
            //Ahora declaro los errores que a futuro se pueden dar en la validación
            object errores = null;

            switch(ex){
                case HandLerExcepcion me://Clase creada en proyecto Aplicacion controlamos este tipo y asi le indicamos como debe pintarnos el logger
                    //Que em aparezca el detalle y el tipo
                    logger.LogError(ex, "Manejador Error");//Lo imprimo dentro de un logger
                    //Pongo los mensajes de error que se van almacenando
                    errores = me._errores;
                    //Además en el context le asigno el status del error
                    context.Response.StatusCode = (int)me._codigo;
                break;
                case Exception e://Cuando sea una excepción general
                    logger.LogError(ex, "Error de servidor");
                    //El valor de Message, Puede traer un Jzon o u XML lo que vamos hacer para controlar eso es convertirlo a un string
                    errores = string.IsNullOrWhiteSpace(e.Message) ? "Error": e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                break;
            }
            //Este va ser el tipo de respuesta que va devolver (Le indico el formato en el que lo muestro)
            context.Response.ContentType = "application/json";
            //Valido si el objeto errores tiene valores o no
            if(errores != null){
                //Lo que hacemos es capturar estos errores dentro de un json usando la librería JsonConvert (Si no esta instalada instalada del NUGET)
                var resultados = JsonConvert.SerializeObject(new {errores});
                await context.Response.WriteAsync(resultados);//Para que imprima los resultados del cliente
            }

        }
    }
}