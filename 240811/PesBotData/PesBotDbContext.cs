using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PesBotData
{
    public class PesBotDbContext : DbContext
    {
        public DbSet<AccountData> AccountDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=test2;Username=postgres;Password=1587panda");
        }
    }
}
