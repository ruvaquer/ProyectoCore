using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Persistencia.DapperConexion
{
    public class ConexionConfiguracion
    {
        //Le tenemos que poner el mismo nombre que la propiedad que hemos aplicado en appsetting.json (Seccion ConnectionStrings), en el starup se aplica esa configuración recuperamos la sección que nos retorna un objeto y dentro de esa sección esta el nombre de la propiedad que hemos aplicado
        public string DefaultConnection {get; set;}
    }
}