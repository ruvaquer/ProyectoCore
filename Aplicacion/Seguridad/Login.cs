using System.Threading;
using System.Threading.Tasks;
using Aplicacion.HandLerError;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Net;
using FluentValidation;

namespace Aplicacion.Seguridad
{
    public class Login
    {
        public class Ejecuta : IRequest<Usuario>{
            public string Email{get; set;}
            public string password{get; set;}
        }

        //Reglas de validación de la clase Ejecuta donde están los parámetros del usuario que pediremos en el login
        public class EjecutaValidacion : AbstractValidator<Ejecuta>{
            public EjecutaValidacion(){
                RuleFor(x=>x.Email).NotEmpty();
                RuleFor(x=>x.password).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, Usuario>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly SignInManager<Usuario> _signInManager;
            public Manejador(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager){
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<Usuario> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Logica para el login
                //1º Ver que el usuario exista
                var usuario = await _userManager.FindByEmailAsync(request.Email);
                if(usuario == null){
                    throw new HandLerExcepcion(HttpStatusCode.Unauthorized);
                }

                //2º Ahora si pasamos esa validación podemos hacer el login
                var resultado = await _signInManager.CheckPasswordSignInAsync(usuario, request.password, false);
                if(resultado.Succeeded){
                    return usuario;
                }

                throw new HandLerExcepcion(HttpStatusCode.Unauthorized);

            }
        }
    }
}