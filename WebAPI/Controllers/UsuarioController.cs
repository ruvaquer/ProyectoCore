using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class UsuarioController : MiControllerBase
    {
        //Endpoit --> http://localhost:5000/api/Usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<Usuario>> Login(Login.Ejecuta parametros){
            return  await Mediator.Send(parametros);
        }
    }
}