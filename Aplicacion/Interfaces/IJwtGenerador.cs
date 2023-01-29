using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;

namespace Aplicacion.Interfaces
{
    public interface IJwtGenerador
    {
        string CrearToken(Usuario usuario);
    }
}