using Domain.Models.People.PeopleAggregate.Entities;
using Domain.Models.People.PeopleAggregate.ValueObjects.Document;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Mappings.People;

public class DocumentMapping : IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Person)
            .WithMany(x => x.Documents)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.Property(x => x.ExternalId)
            .HasColumnType("char")
            .HasMaxLength(26)
            .IsRequired();

        builder.OwnsOne(x => x.Value)
            .Property(x => x.Value)
            .HasColumnType("varchar")
            .HasMaxLength(ValueValue.FieldMaxLength)
            .IsRequired();

        builder.OwnsOne(x => x.Issuer)
            .Property(x => x.Value)
            .HasColumnType("varchar")
            .HasMaxLength(IssuerValue.FieldMaxLength);

        builder.ToTable("document");
    }
}