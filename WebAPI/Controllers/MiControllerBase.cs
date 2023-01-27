using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class MiControllerBase : ControllerBase
    {
        //El objetivo de este contorlador propio es aplicar configuraciones dentro del controlador para no tener que implementar funcionalidades recurrentes en el resto de Controlladores
        //Si no que teniendolo en esta y heredando de la misma ya podemos hacer uso de la mmisma por ejemplo la interfaz IMediator que nos ayuda a controlar toda la logica de acceso a BBDD etc...

        //La librería Handler(Manejador) es la que va inyectar la dependencia a la BBDD que esta en Aplicacion ya inyectado
         private  IMediator _mediator;

         protected IMediator Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediator>());

    }
}