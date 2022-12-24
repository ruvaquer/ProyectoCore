using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dominio;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CursosOnlineContext _context;
        //Inyection dependencia context
        public WeatherForecastController(CursosOnlineContext context){
            _context = context; //<-- Esto es la Inyection de dependencia que es crear un servicio externo dentro de una clase
        }

        [HttpGet]
        public IEnumerable<Curso> Get(){
            return _context.Curso.ToList();
        }
    }
}
