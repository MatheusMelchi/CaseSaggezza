using CaseSaggezza_Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseSaggezza_Dal.Contexts
{
    public class IdentificationDbContext : IdentityDbContext<User>
    {
        public IdentificationDbContext(DbContextOptions<IdentificationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().Property(x => x.Name).HasMaxLength(255);

            builder.HasDefaultSchema("CaseSaggezzaIdentity");
        }
    }
}
