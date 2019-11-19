using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personnel.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<CheckInOut> CheckInOuts { get; set; }
        public DbSet<CheckRowInOut> CheckRowInOuts { get; set; }
        public DbSet<Login> Logins { get; set; }

        public DbQuery<CheckLastUpdate> CheckLastUpdates { get; set; }
        public DbQuery<SourceAutoComplete> SourceAutoCompletes { get; set; }



    }
}
