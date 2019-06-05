using System;
using System.Threading.Tasks;
using Account.API.Domain;
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

        public async Task Commit()
        {
            await _accountContext.SaveChangesAsync();
        }
    }
}
