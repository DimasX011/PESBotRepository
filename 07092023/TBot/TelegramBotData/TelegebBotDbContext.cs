using Microsoft.EntityFrameworkCore;
using TBotData;

namespace TelegramBotData
{
    public class TelegebBotDbContext : DbContext
    {
        public  DbSet<UserData> UserDatas { get; set; }

        public DbSet<TaskData> TaskDatas { get; set; }

        public DbSet<TaskUsers> TaskUsers { get; set; }

        public DbSet<UserYandex> UserYandex { get; set; }

        public DbSet<UnRegisteredUser> UnRegisteredUsers { get; set; }

        public DbSet<UsersDataEntry> UsersDataEntries { get; set; }

        public TelegebBotDbContext(DbContextOptions options) : base(options) { } 

    }
}