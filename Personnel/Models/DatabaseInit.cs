using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Personnel.Models
{
    public class DatabaseInit
    {
        public static void INIT(IServiceProvider ServiceProvider)
        {
            var context = new DatabaseContext(ServiceProvider.GetRequiredService<DbContextOptions<DatabaseContext>>());
            // If database does not exist then the database and all its schema are created
            context.Database.EnsureCreated();
        }
    }
}
