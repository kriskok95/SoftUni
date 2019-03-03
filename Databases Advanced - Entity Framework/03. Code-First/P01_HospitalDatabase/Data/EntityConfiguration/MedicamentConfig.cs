using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_HospitalDatabase.Data.Models;

namespace P01_HospitalDatabase.Data.EntityConfiguration
{
    public class MedicamentConfig : IEntityTypeConfiguration<Medicament>
    {
        public void Configure(EntityTypeBuilder<Medicament> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsUnicode(true);

            builder.HasMany(x => x.Prescriptions)
                .WithOne(x => x.Medicament)
                .HasForeignKey(x => x.MedicamentId);
        }
    }
}
