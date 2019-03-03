using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data.EntityConfiguration
{
    public class DoctorConfig : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasKey(x => x.DoctorId);

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsUnicode();

            builder.Property(x => x.Specialty)
                .HasMaxLength(100)
                .IsUnicode();

            builder.HasMany(x => x.Visitations)
                .WithOne(x => x.Doctor)
                .HasForeignKey(x => x.DoctorId);
        }
    }
}
