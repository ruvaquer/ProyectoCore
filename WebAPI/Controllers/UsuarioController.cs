using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Dominio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class UsuarioController : MiControllerBase
    {
        //Endpoit --> http://localhost:5000/api/Usuario/login
        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDatos>> Login(Login.Ejecuta parametros){
            return  await Mediator.Send(parametros);
        }

        //Endpoit --> http://localhost:5000/api/Usuario/registrar
        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDatos>> Registrar(Registrar.Ejecuta parametros){
            return await Mediator.Send(parametros);
        }
    }
}