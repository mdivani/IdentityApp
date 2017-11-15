using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using IdentityDomain.Entities;

namespace IdentityRepository.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("IdentityApp")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }

        internal virtual DbSet<User> Users { get; set; }
        internal virtual DbSet<Claim> Claims { get; set; }
        internal virtual DbSet<ExternalLogin> ExternalLogins { get; set; }
        internal virtual DbSet<Role> Roles { get; set; }

    }
}
