using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Aplicacion.Interfaces;
using Dominio;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Seguridad.TokenSeguridad
{
    //Inyectamos como un servicio dentro del WebAPI, esto lo hacemos dentro de la clase Stastup
    public class JwtGenerador : IJwtGenerador
    {

        public string CrearToken(Usuario usuario)
        {
                    //1º Lista de Cleans (Dtos dle usuario que queremos compartir con el cliente)
                    var claims = new List<Claim>{
                        //De momento tengo un Claim podría agregar más pero tener cuidado con no agregar claims pesados como imagenes o archivos
                        new Claim(JwtRegisteredClaimNames.NameId, usuario.UserName)
                    };

                    //2º Credenciales de Acceso
                    var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi Palabra Secreta"));//Esta es la palabra secreta que desencriptara a futuro el token

                    //3º Credenciales
                    var credenciales = new SigningCredentials(Key,SecurityAlgorithms.HmacSha512Signature);

                    //4º La descrfipción dle Token
                    var tokenDescripcion = new SecurityTokenDescriptor{
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddDays(30),//Lo definimos que este vivo 30 días pero es solo para pruebas para relise mucho menos tiempo
                        SigningCredentials = credenciales
                    };

                    //5º Agregamos el Token Hanller para poder escribir el Token
                    var TokenHandler = new JwtSecurityTokenHandler();
                    var token = TokenHandler.CreateToken(tokenDescripcion);//El token se basa en esta descripción para poder ser creado

                    //6º Por último devolvemos un string
                    return TokenHandler.WriteToken(token);

        }
                
                
        
    }
}