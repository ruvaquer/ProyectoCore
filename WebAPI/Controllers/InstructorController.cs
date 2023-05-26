using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Instructores;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Persistencia.DapperConexion.Instructor;

namespace WebAPI.Controllers
{
    public class InstructorController : MiControllerBase
    {
        public InstructorController(){
            
        }

        [HttpGet]
        public async Task<ActionResult<List<InstructorModel>>> ObtenerInstructores(){
            var instructores = await Mediator.Send(new Consulta.Lista());
            return instructores.ToList();
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta item){
            return await Mediator.Send(item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualizar(Guid id, Editar.Ejecuta item){
            item.InstructorId = id;
            return await Mediator.Send(item);
        }
    }
}