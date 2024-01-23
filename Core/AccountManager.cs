using System.Reflection.Metadata.Ecma335;
using System.Security;

namespace Core
{
    public class AccountManager
    {
        public Account Get(int currencyID)
        {

            Account account = new Account();
            return account.Get(currencyID); 
        }

        public Currency Update(Currency currency)
        {
            return new Currency();
        }
    }
}