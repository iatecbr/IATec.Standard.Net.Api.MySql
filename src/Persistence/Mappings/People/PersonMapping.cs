using Domain.Models.People.PeopleAggregate.Entities;
using Domain.Models.People.PeopleAggregate.ValueObjects.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Mappings.People;

public class PersonMapping : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ExternalId)
            .HasColumnType("char")
            .HasMaxLength(26)
            .IsRequired();

        builder.OwnsOne(x => x.FirstName)
            .Property(x => x.Value)
            .HasColumnType("varchar")
            .HasMaxLength(FirstNameValue.FieldMaxLength)
            .IsRequired();

        builder.OwnsOne(x => x.MiddleName)
            .Property(x => x.Value)
            .HasColumnType("varchar")
            .HasMaxLength(FirstNameValue.FieldMaxLength);

        builder.OwnsOne(x => x.LastName)
            .Property(x => x.Value)
            .HasColumnType("varchar")
            .HasMaxLength(FirstNameValue.FieldMaxLength)
            .IsRequired();

        builder.ToTable("person");
    }
}