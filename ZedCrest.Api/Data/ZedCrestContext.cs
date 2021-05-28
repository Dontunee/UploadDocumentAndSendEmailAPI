using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrest.Api.Models.Entities;

namespace ZedCrest.Api.Data
{
    public class ZedCrestContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<ApplicationFile> ApplicationFiles { get; set; }


        public ZedCrestContext(DbContextOptions<ZedCrestContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(ZedCrestContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
