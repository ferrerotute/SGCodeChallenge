namespace Core
{
    public class Account
    {
        #region "Attributes"
        public int Id { get; set; }
        public string Balance { get; set; }
        public bool State { get; set; }
        public Currency Currency { get; set; }
        #endregion

        #region "Logic"
        public Account? Get(int currencyID)
        {
            DataConnection dataConnection = new DataConnection();
            Account? account = null;
            Currency currency = new Currency();

            try
            {
                dataConnection.setQuery(@"  Select Id, Balance from accounts
                                            where currencyID = @currencyID and state = 1");
                dataConnection.setParameter("@currencyID", currencyID);
                dataConnection.read();

                if(dataConnection.Reader.Read())
                {
                    account = new Account();
                    account.Id = (int)dataConnection.Reader["Id"];
                    decimal balance = (decimal)dataConnection.Reader["Balance"];
                    account.Balance = FormatBalance(balance.ToString());
                    account.Currency = currency.Get(currencyID);
                    account.State = true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { dataConnection.closeConnection(); }

            return account;
        }

        public Account Create(int currencyID)
        {
            DataConnection dataConnection = new DataConnection();
            Account account = new Account();

            try
            {
                dataConnection.setQuery(@"  Insert into accounts (CurrencyID, Balance)
                                            Values (@currencyID,0)");
                dataConnection.setParameter("@currencyID", currencyID);
                dataConnection.execute();
            }
            catch (Exception ex)
            {
                throw;
            }finally { dataConnection.closeConnection(); }
            
            return Get(currencyID);
        }

        private string FormatBalance(string balance)
        {
            balance = balance.TrimEnd('0');
            if (balance.EndsWith("."))
                balance = balance.Replace(".", "");
            return balance;
        }
        #endregion
    }
}
