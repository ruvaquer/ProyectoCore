using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    //Representara lños datos que quiero devolver al cliente
    public class UsuarioDatos
    {
        public string NombreCompleto{get; set;}
        public string Token{get; set;}
        public string Email{get; set;}
        public string UserName{get; set;}
        public string Imagen{get; set;}
    }
}