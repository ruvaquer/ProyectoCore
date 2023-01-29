using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Aplicacion.Interfaces;
using Dominio;

namespace Seguridad.TokenSeguridad
{
    //Inyectamos como un servicio dentro del WebAPI, esto lo hacemos dentro de la clase Stastup
    public class JwtGenerador : IJwtGenerador
    {

        /*
                public string CrearToken(Usuario usuario)
                {
                    //1º Lista de Cleans (Dtos dle usuario que queremos compartir con el cliente)
                    var claims = new List<Claim>{
                        new Claim(JwtRegisterClaimNames)
                    };

                }
                */
        public string CrearToken(Usuario usuario)
        {
            throw new NotImplementedException();
        }
    }
}