namespace Domain.Models.People.PeopleAggregate.ValueObjects.Person;

public class LastNameValue(string value)
{
    public const int FieldMinLength = 1;
    public const int FieldMaxLength = 50;

    public string Value { get; } = value;
}