using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dominio;
using MediatR;
using Aplicacion.Cursos;
using Microsoft.AspNetCore.Authorization;
using System;

namespace WebAPI.Controllers
{
    //http://localhost:5000/api/Cursos
    [Route("api/[controller]")]
    [ApiController]
    public class CursosController : MiControllerBase
    {
        
        [HttpGet]
        public async Task<ActionResult<List<CursoDto>>> Get(){
            return await Mediator.Send(new Consulta.ListaCursos());
        }

        //http://localhost:5000/api/Cursos/{Guid}
        //http://localhost:5000/api/Cursos/b716b622-d7a0-44f6-1992-08db01e03d78
        [HttpGet("{id}")]
        public async Task<ActionResult<CursoDto>> Detalles(Guid id){
            return await Mediator.Send(new ConsultaId.CursoUnico{Id = id});
        }

        //Retorna un flat(Bandera), indicando como ha ido la transacción
        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta datos){
            return await Mediator.Send(datos);
        }

        //Retorna un flat(Bandera), indicando como ha ido la transacción
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Editar(Guid id, Editar.Ejecuta datos){
            datos.CursoId = id;
            return await Mediator.Send(datos);
        }

        //Retorna un flat(Bandera), indicando como ha ido la transacción
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(int id){
            return await Mediator.Send(new Eliminar.Ejecuta{Id = id});
        }


    }
}
