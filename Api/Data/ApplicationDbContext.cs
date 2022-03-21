#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserLog> UserLog { get; set; }
        public DbSet<Customers> customerInfo { get; set; }
        public DbSet<CustomerAddress> customerAddress { get; set; }
        public DbSet<MapAddress> mapAddresses { get; set; }
}
