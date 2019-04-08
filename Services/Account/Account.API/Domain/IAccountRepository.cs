using System.Threading.Tasks;

namespace Account.API.Domain
{
    public interface IAccountRepository
    {
        Task<Account> CreateAsync(Account account);
        Task Commit();
    }
}