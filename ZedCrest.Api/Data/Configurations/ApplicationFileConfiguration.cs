using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZedCrest.Api.Models.Entities;

namespace ZedCrest.Api.Data.Configurations
{
    public class ApplicationFileConfiguration : IEntityTypeConfiguration<ApplicationFile>
    {
        public void Configure(EntityTypeBuilder<ApplicationFile> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Path).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(128).IsRequired();
            builder.HasOne(x => x.Owner).WithMany().HasForeignKey(x => x.OwnerId);
        }


    }
}
