using Chat.Domain;
using Microsoft.EntityFrameworkCore;

namespace Chat.Infrastructure.Data.Contexts
{
    public class DataContext(DbContextOptions<DataContext> options) :
        DbContext(options)
    {

        public DbSet<User> Users { get; set; }
        public DbSet<ChatRoom> Chats { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => CustomModelBuilder.OnModelCreating(modelBuilder);
        
    }
}