using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Data.Configurations
{
    public interface IDbContextOptionsConfigurator<TContext> where TContext : DbContext
    {
        public void Configure(DbContextOptionsBuilder<TContext> options);
    }
}
