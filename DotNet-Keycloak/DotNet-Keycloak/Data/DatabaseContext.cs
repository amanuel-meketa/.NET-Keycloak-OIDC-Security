using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DotNet_Keycloak.Data.Model;

namespace DotNet_Keycloak.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext (DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<DotNet_Keycloak.Data.Model.Config> Config { get; set; } = default!;
    }
}
