using Microsoft.EntityFrameworkCore;

namespace DiscoveringAsp.netCore.Entities
{
    public class DiscoveringAspnetCoreContext : DbContext
    {
        public DiscoveringAspnetCoreContext(DbContextOptions<DiscoveringAspnetCoreContext> options) : base(options)
        {   
        }


        public DbSet<Customer> Customers { get; set; }
    }
}
