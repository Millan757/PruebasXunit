using Chat.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chat.Server.Library.Data
{
    public class ChatDbContext : DbContext
    {
        public ChatDbContext()
            : base()
        { }

        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            : base(options)
        { }


        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ChatUser> ChatUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlite("Data Source=chat.db", b => b.MigrationsAssembly("Chat.Server"));
            }
        }
    }
}
