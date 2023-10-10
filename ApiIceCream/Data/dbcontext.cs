using Microsoft.EntityFrameworkCore;
using ApiIceCream.Models;
namespace ApiIceCream.Data
{
    public class dbcontext : DbContext
    {
        public dbcontext(DbContextOptions<dbcontext> options ) : base(options) { 
        
        }
        public DbSet<IceCream> Helados => Set<IceCream>();
    }
}
