using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data.EntityConfiguration
{
    public class DiagnoseConfig : IEntityTypeConfiguration<Diagnose>
    {
        public void Configure(EntityTypeBuilder<Diagnose> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode(true);

            builder.Property(x => x.Comments)
                .HasMaxLength(50)
                .IsUnicode(true);
        }
    }
}
