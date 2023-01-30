using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Aplicacion.Interfaces;
using Dominio;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioActual
    {
        public class Ejecutar : IRequest<UsuarioDatos>{}

        public class Handler : IRequestHandler<Ejecutar, UsuarioDatos>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly IJwtGenerador _jwtGenerador;
            private readonly IUsuarioSesion _usuarioSesion;

            public Handler(UserManager<Usuario> userManager, IJwtGenerador jwtGenerador, IUsuarioSesion usuarioSesion){
                _userManager = userManager;
                _jwtGenerador = jwtGenerador;
                _usuarioSesion = usuarioSesion;
            }

            public async Task<UsuarioDatos> Handle(Ejecutar request, CancellationToken cancellationToken)
            {
                var usuario = await _userManager.FindByNameAsync(_usuarioSesion.ObtenerUsuarioSesion());
                //Los datos que voy a devolver al recuperar el usuario actual
                return new UsuarioDatos{
                  NombreCompleto = usuario.NombreCompleto,
                  Token = _jwtGenerador.CrearToken(usuario),
                  UserName = usuario.UserName,
                  Email = usuario.Email,
                  Imagen = null
                };

            }
        }
    }
}