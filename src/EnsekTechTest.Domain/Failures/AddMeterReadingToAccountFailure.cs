namespace EnsekTechTest.Domain.Failures
{
    public enum AddMeterReadingToAccountFailure
    {
        AlreadyAdded,
        NewerReadingExists,
        ValueOutOfRange
    }
}
