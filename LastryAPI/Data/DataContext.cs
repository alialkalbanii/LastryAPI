using Microsoft.EntityFrameworkCore;

namespace LastryAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Request> Requests { get; set; }
    }
}
