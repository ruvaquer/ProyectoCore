using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Aplicacion.Cursos;

namespace WebAPI.Controllers
{
    // http://localhost:5000/api/Cursos
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : ControllerBase
    {
        //La librería Handler(Manejador) es la que va inyectar la dependencia a la BBDD que esta en Aplicacion ya inyectado
        private readonly IMediator _mediator;

        public CursosController(IMediator mediator){
                _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Curso>>> Get(){
            return await _mediator.Send(new Consulta.ListaCursos());
        }
    }
}
