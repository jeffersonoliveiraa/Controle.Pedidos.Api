using Controle.Pedidos.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Controle.Pedidos.Api.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Clientes>? Clientes { get; set; } = null;
    public DbSet<Produtos>? Produtos { get; set; } = null;
    public DbSet<Pedido>? Pedidos { get; set; } = null;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Clientes>(e => 
        { 
            e.HasKey(de => de.Id);
        });

        modelBuilder.Entity<Produtos>(e =>
        {
            e.HasKey(de => de.Id);
        });

        modelBuilder.Entity<Pedido>(e =>
        {
            e.HasKey(de => de.Id);
        });
    }
}
