using System;
using System.Threading.Tasks;
using Account.API.Domain;
using Microsoft.EntityFrameworkCore;
using Acc = Account.API.Domain.Account;

namespace Account.API.Infrastructure.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountContext _accountContext;

        public AccountRepository(AccountContext accountContext)
        {
            _accountContext = accountContext;
        }

        public async Task<Acc> CreateAsync(Acc account)
        {
            await _accountContext.AddAsync(account);
            return account;
        }

        public async Task<Acc> Get(Guid id)
            => await _accountContext.Set<Acc>()
                .FirstOrDefaultAsync(acc => acc.Id == id);

        public async Task UpdateBalance(Acc account)
            => await Task.FromResult(_accountContext
                .Entry(account)
                .Property(acc => acc.Balance)
                .IsModified = true);

        public async Task<bool> Commit()
            => await _accountContext.SaveChangesAsync() > 0;
        
    }
}
