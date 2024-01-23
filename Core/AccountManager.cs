using System.Reflection.Metadata.Ecma335;
using System.Security;

namespace Core
{
    public class AccountManager
    {
        public Account? Get(int currencyID)
        {
            try { 
                Account? account = new Account();
                return account.Get(currencyID);
            }
            catch(Exception) { throw; }
        }
        public Account Create(int currencyID)
        {
            try { 
                Account account = new Account();
                return account.Create(currencyID);
            }
            catch (Exception) { throw; }
        }
        public Account? Close(int currencyID)
        {
            try
            {
                Account? account = new Account();
                return account.Close(currencyID); 
            }
            catch (Exception) { throw; }
        }

        public Account Deposit(int currencyID, decimal amount)
        {
            try { 
                Account account = new Account();
                return account.Deposit(currencyID, amount);
            }
            catch (Exception) { throw; }
        }
        public Account Withdraw(int currencyID, decimal amount)
        {
            try
            {
                Account account = new Account();
                return account.Withdraw(currencyID, amount);
            }
            catch (Exception) { throw; }
        }
    }
}