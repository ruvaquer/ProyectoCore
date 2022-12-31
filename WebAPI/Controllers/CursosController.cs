using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Aplicacion.Cursos;

namespace WebAPI.Controllers
{
    //http://localhost:5000/api/Cursos
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

        //http://localhost:5000/api/Cursos/{id}
        //http://localhost:5000/api/Cursos/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Curso>> Detalles(int id){
            return await _mediator.Send(new ConsultaId.CursoUnico{Id = id});
        }

        //Retorna un flat(Bandera), indicando como ha ido la transacción
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta datos){
            return await _mediator.Send(datos);
        }


    }
}
