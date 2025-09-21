using Microsoft.EntityFrameworkCore;
using VendasApi.Models;

namespace VendasApi.Data
{
    public class ClienteDbContext : DbContext
    {
        public ClienteDbContext(DbContextOptions<ClienteDbContext> options) : base(options)
        {
        }
        public DbSet<Cliente> Clientes => Set<Cliente>();
    }
}

