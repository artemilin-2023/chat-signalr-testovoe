using Chat.Infrastructure.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Chat.Infrastructure.Data.Contexts
{
    public static class CustomModelBuilder
    {
        public static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureForecastModel();
            modelBuilder.SetDefualtDateTimeKind(DateTimeKind.Utc);
        }

        private static void ConfigureForecastModel(this ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
