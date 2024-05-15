using Golio.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golio.Infrastructure.Persistence.Configurations
{
    public class PriceConfigurations : IEntityTypeConfiguration<Price>
    {
        public void Configure(EntityTypeBuilder<Price> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .Property(p => p.Value)
                .IsRequired();

            builder
                .HasOne(p => p.Store)
                .WithMany()
                .HasForeignKey(p => p.StoreId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasMany(p => p.Suggestions)
                .WithOne()
                .HasForeignKey(suggestion => suggestion.PriceId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
