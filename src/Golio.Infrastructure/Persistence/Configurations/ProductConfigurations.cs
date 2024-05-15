using Golio.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golio.Infrastructure.Persistence.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .Property(p => p.Name)
                .IsRequired();

            builder
                .Property(p => p.Brand)
                .IsRequired();

            builder
                .Property(p => p.Volume)
                .IsRequired();

            builder
                .HasMany(p => p.Prices)
                .WithOne()
                .HasForeignKey(price => price.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
