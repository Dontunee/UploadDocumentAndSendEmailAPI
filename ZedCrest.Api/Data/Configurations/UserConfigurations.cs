using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrest.Api.Models.Entities;

namespace ZedCrest.Api.Data.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.FirstName).HasMaxLength(128).IsRequired();
            builder.Property(x => x.LastName).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(128).IsRequired();
            builder.Property(x => x.MobileNumber).HasMaxLength(14);
            builder.HasIndex(X => X.Email).IsUnique(true);
        }


    }
}
