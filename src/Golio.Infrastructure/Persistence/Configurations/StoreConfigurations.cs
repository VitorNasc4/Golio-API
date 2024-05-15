using Golio.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golio.Infrastructure.Persistence.Configurations
{
    public class StoreConfigurations : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .Property(p => p.Name)
                .IsRequired();

            builder
                .Property(p => p.City)
                .IsRequired();

            builder
                .Property(p => p.State)
                .IsRequired();

            builder
                .Property(p => p.ZipCode)
                .IsRequired();
        }
    }
}
