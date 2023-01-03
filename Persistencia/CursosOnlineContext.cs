using Dominio;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    //Hacemos que herede de IdentityDbContext previo para el mapeo a BBDD y que me genere las tablas en la BBDD
    public class CursosOnlineContext : IdentityDbContext<Usuario>
    {
        //Para realizar las migraciones de entidades y como puente de las Inyecciones
        public CursosOnlineContext(DbContextOptions options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            //Para poder crear el archivo de migración que va tener toda la logica que en un futuro va crear las tablas
            base.OnModelCreating(modelBuilder);

            //Defino la clave primaria compuesta de la  tabla CursoInstructor
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new {ci.CursoId, ci.InstructorId});
        }

        public DbSet<Comentario> Comentario {get; set;}
        public DbSet<Curso> Curso {get; set;}
        public DbSet<CursoInstructor> CursoInstructor {get; set;}
        public DbSet<Instructor> Instructor {get; set;}
        public DbSet<Precio> Precio {get; set;}

    }
}