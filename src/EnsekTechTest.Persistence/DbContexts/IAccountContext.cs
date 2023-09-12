using EnsekTechTest.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace EnsekTechTest.Persistence.DbContexts
{
    internal interface IAccountContext
    {
        DbSet<Account> Accounts { get; }
    }
}
