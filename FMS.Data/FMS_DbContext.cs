using FMS.Business.DatabaseObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FMS.Data
{
    public class FMS_DbContext : DbContext
    {
        public FMS_DbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Users> Users { get; set; }

        public DbSet<UserType> UserType { get; set; }

        public DbSet<Menu> Menu { get; set; }

        public DbSet<FoodAvailability> FoodAvailability { get; set; }

        public DbSet<FoodOptions> FoodOptions { get; set; }

        public DbSet<UserFood> UserFood { get; set; }
    }
}
