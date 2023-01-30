using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Seguridad.TokenSeguridad
{
    public class UsuarioSesion : IUsuarioSesion
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public UsuarioSesion(IHttpContextAccessor httpContextAccessor){
            _httpContextAccessor = httpContextAccessor;
        }

        public string ObtenerUsuarioSesion()
        {
            //Le pongo la interrogaciónpara validar ya que User podría ser null, ya que puede ser que no tenga nada en session, los datos almacenados se les llama claims
            var userName = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            return userName;
        }
    }
}