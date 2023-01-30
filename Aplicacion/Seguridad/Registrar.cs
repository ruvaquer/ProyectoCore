using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.HandLerError;
using Aplicacion.Interfaces;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistencia;

namespace Aplicacion.Seguridad
{
    //El IRequest es lo que retornara al ingreasar los datos el usuario en el formulario
    public class Registrar
    {
        public class Ejecuta : IRequest<UsuarioDatos>{
            //Parámetros
            public string Nombre{get; set;}
            public string Apellidos{get; set;}
            public string Email{get; set;}
            public string Password{get; set;}
            public string UserName{get; set;}
        }

        //Validacion que evalua a las propiedades de la clase ejecuta para controlar por ejemplo que no deje un valor vacío
        public class EjecutaValidator : AbstractValidator<Ejecuta>{
            public EjecutaValidator(){
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellidos).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Email).EmailAddress();//Validamos el formato del email agregado
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.UserName).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Ejecuta, UsuarioDatos>
        {
            private readonly CursosOnlineContext _context; 
            private readonly UserManager<Usuario> _userManager; 
            private readonly IJwtGenerador _jwtGenerador;

            public Handler(CursosOnlineContext context, UserManager<Usuario> userManager, IJwtGenerador jwtGenerador){
                //Inyection
                _context = context;
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
            }

            public async Task<UsuarioDatos> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                //Aquí implementamos la lógica
                //1º Comprobamos si tengo un emaiil como el que ha metido el usuario
                var existe = await _context.Users.Where(x => x.Email == request.Email).AnyAsync();
                if(existe){
                    throw new HandLerException(HttpStatusCode.BadRequest, new {mensaje = "Existe ya un usuario registrado con este Email."});
                }

                var existeUserName = await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync();
                if(existeUserName){
                    throw new HandLerException(HttpStatusCode.BadRequest, new {mensaje = "Existe ya un usuario registrado con este nombre de usuario."});
                }

                //Representa el usuario de Core Identity
                var usuario = new Usuario{
                    NombreCompleto = request.Nombre+" "+request.Apellidos,
                    Email = request.Email,
                    UserName = request.UserName
                };

                //Llamar a un método que me ayude a insertar el valor
                var resultado = await _userManager.CreateAsync(usuario,request.Password);

                if(resultado.Succeeded){
                 return new UsuarioDatos{
                    NombreCompleto = usuario.NombreCompleto,
                    Token = _jwtGenerador.CrearToken(usuario),
                    Email = usuario.Email,
                    UserName = usuario.UserName,
                    Imagen = null
                 };
                }

                throw new Exception("No se pudo agregar al nuevo usuario");

            }
        }
    }
}