using System;
using System.Threading.Tasks;

namespace Account.API.Domain
{
    public interface IAccountRepository
    {
        Task<Account> CreateAsync(Account account);
        Task<Account> Get(Guid id);
        Task UpdateBalance(Account account);
        Task<bool> Commit();
    }
}