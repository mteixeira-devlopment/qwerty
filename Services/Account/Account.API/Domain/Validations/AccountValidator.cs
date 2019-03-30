using Account.API.Domain.Seed;

namespace Account.API.Domain.Validations
{
    public abstract class AccountValidator : Validation<Account> 
    {
        protected AccountValidator(Account account) : base(account)
        {
        }
    }
}