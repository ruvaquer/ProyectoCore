using System;
using System.Net;

namespace Aplicacion.HandLerError
{
    public class HandLerException : Exception
    {
        public HttpStatusCode _codigo {get;}
        public Object _errores {get;}
        
        public HandLerException(HttpStatusCode codigo, object errores = null){
            _codigo = codigo;
            _errores = errores;
        }
    }
}