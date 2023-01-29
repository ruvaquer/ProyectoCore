using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Interfaces;
using Dominio;

namespace Seguridad.TokenSeguridad
{
    //Inyectamos como un servicio dentro del WebAPI, esto lo hacemos dentro de la clase Stastup
    public class JwtGenerador : IJwtGenerador
    {
        

        public string CrearToken(Usuario usuario)
        {
            throw new NotImplementedException();
        }
    }
}