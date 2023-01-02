using System;
using System.Net;

namespace Aplicacion.HandLerError
{
    public class HandLerExcepcion : Exception
    {
        public HttpStatusCode _codigo {get;}
        public Object _errores {get;}
        
        public HandLerExcepcion(HttpStatusCode codigo, object errores = null){
            _codigo = codigo;
            _errores = errores;
        }
    }
}