using Microsoft.EntityFrameworkCore;
using TBotData;

namespace TelegramBotData
{
    public class TelegebBotDbContext : DbContext
    {
        public  DbSet<UserData> UserDatas { get; set; }

        public DbSet<TaskData> TaskDatas { get; set; }

        public TelegebBotDbContext(DbContextOptions options) : base(options) { } 

    }
}