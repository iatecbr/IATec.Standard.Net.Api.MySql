using Domain.Models.People.PeopleAggregate.ValueObjects.Document;
using IATec.Shared.Domain.SeedWorks;

namespace Domain.Models.People.PeopleAggregate.Entities;

public class Document : EntityUlidInt32
{
    public int PersonId { get; private set; }
    public Person? Person { get; private init; }
    public ValueValue Value { get; private set; }
    public IssuerValue Issuer { get; private set; }

    private Document()
    {
    }
}