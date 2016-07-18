using GestoraSample.Dominio.Entidades;
using System.Data.Entity;

namespace GestoraSample.Infra.Dados.Contexto
{
    public class GestoraContexto : DbContext
    {
        public GestoraContexto()
            : base("DefaultConnection")
        {

        }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
