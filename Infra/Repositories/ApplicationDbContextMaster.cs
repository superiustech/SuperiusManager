using Microsoft.EntityFrameworkCore;
using Domain.Entities; 

namespace Infra
{
    public class ApplicationDbContextMaster : DbContext
    {
        public ApplicationDbContextMaster(DbContextOptions<ApplicationDbContextMaster> options) : base(options) { }
        public DbSet<CWCliente> Cliente { get; set; }
        public DbSet<CWClienteUsuario> ClienteUsuario { get; set; }
        public DbSet<CWUsuarioMaster> UsuarioMaster { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region CLIENTE 
            modelBuilder.Entity<CWCliente>(entity =>
            {
                entity.HasKey(c => c.nCdCliente);
                entity.Property(c => c.nCdCliente).UseIdentityColumn();
                entity.HasMany(c => c.Usuarios).WithOne(u => u.Cliente).HasForeignKey(u => u.nCdCliente).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CWCliente>().ToTable("CLIENTE");
            modelBuilder.Entity<CWClienteUsuario>().ToTable("CLIENTE_USUARIO");
            #endregion

            #region USUARIO MASTER
            modelBuilder.Entity<CWUsuarioMaster>().HasKey(c => c.sCdUsuario);
            modelBuilder.Entity<CWUsuarioMaster>().ToTable("USUARIO_MASTER");
            #endregion  
        }
    }
}
