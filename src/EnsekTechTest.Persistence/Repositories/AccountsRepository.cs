﻿using EnsekTechTest.Application.Infrastructure.Repositories;
using EnsekTechTest.Persistence.DbContexts;
using EnsekTechTest.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;
using DomainAccount = EnsekTechTest.Domain.AggregateRoots.Account;

namespace EnsekTechTest.Persistence.Repositories
{
    internal class AccountsRepository : IAccountsRepository
    {
        private readonly PersistenceContext _context;

        public AccountsRepository()
        {
            _context = new PersistenceContext();
        }

        public async Task<DomainAccount> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var account = await _context.Accounts
                .Include(account => account.MeterReadings)
                .SingleOrDefaultAsync(account => account.Id == id, cancellationToken);

            return AccountMapper.Map(account);
        }     
    }
}
