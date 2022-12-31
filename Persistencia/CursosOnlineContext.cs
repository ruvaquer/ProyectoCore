using Dominio;
using Microsoft.EntityFrameworkCore;

namespace Persistencia
{
    public class CursosOnlineContext : DbContext
    {
        //Para realizar las migraciones de entidades y como puente de las Inyecciones
        public CursosOnlineContext(DbContextOptions options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            //Defino la clave primaria compuesta
            modelBuilder.Entity<CursoInstructor>().HasKey(ci => new {ci.CursoId, ci.InstructorId});
        }

        public DbSet<Comentario> Comentario {get; set;}
        public DbSet<Curso> Curso {get; set;}
        public DbSet<CursoInstructor> CursoInstructor {get; set;}
        public DbSet<Instructor> Instructor {get; set;}
        public DbSet<Precio> Precio {get; set;}

    }
}