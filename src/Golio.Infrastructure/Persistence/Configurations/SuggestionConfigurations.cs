using Golio.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Golio.Infrastructure.Persistence.Configurations
{
    public class SuggestionConfigurations : IEntityTypeConfiguration<Suggestion>
    {
        public void Configure(EntityTypeBuilder<Suggestion> builder)
        {
            builder
                .HasKey(s => s.Id);

            builder
                .Property(p => p.AutorName)
                .IsRequired();

            builder
                .Property(p => p.AutorEmail)
                .IsRequired();

            builder
                .Property(p => p.Value)
                .IsRequired();
        }
    }
}
