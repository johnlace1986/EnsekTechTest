using EnsekTechTest.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace EnsekTechTest.Persistence.DbContexts
{
    internal interface IMeterReadingContext
    {
        DbSet<MeterReading> MeterReadings { get; }
    }
}
