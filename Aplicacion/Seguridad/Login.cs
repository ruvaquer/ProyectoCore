using System.Threading;
using System.Threading.Tasks;
using Aplicacion.HandLerError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using FluentValidation;
using Aplicacion.Interfaces;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<UsuarioDatos>{
            public string Email{get; set;}
            public string Password{get; set;}
        }

        //Reglas de validación de la clase Ejecuta donde están los parámetros del usuario que pediremos en el login
        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion(){
                RuleFor(x=>x.Email).NotEmpty();
                RuleFor(x=>x.Password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, UsuarioDatos>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            private readonly IJwtGenerador _jwtGenerador;
            //Inyection
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, IJwtGenerador jwtGenerador){
                _userManager = userManager;
                _signInManager = signInManager;
                _jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioDatos> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Logica para el login
                //1º Ver que el usuario exista
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                
                if(usuario == null){
                    throw new HandLerExcepcion(HttpStatusCode.Unauthorized);
                }

                //2º Ahora si pasamos esa validación podemos hacer el login
                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);
                if(resultado.Succeeded){
                    //Esto lo cambiaremos no devolveremos el usuario devolveremos un token esto en pruebas
                    //return UsuarioDatos;
                    //Retornamos UsuarioDatos
                    return new UsuarioDatos{
                        NombreCompleto = usuario.NombreCompleto,
                        Token= _jwtGenerador.CrearToken(usuario),//Llamamos a nuestra interface que genera el string del token que pasaremos en el login
                        UserName = usuario.UserName,
                        Email = usuario.Email,
                        Imagen = null
                    };
                }

                throw new HandLerExcepcion(HttpStatusCode.Unauthorized);

            }
        }
    }
}