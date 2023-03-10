using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Instructores;
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
    }
}