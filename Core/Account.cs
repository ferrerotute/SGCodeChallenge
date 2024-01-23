namespace Core
{
    public class Account
    {
        #region "Attributes"
        public int Id { get; set; }
        public decimal Balance { get; set; }
        public bool State { get; set; }
        public Currency Currency { get; set; }
        #endregion

        #region "Logic"
        public Account Get(int currencyID)
        {
            DataConnection dataConnection = new DataConnection();
            Account account = new Account();
            Currency currency = new Currency();

            try
            {
                dataConnection.setQuery(@"  Select Id, Balance from accounts
                                            where currencyID = @currencyID and state = 1");
                dataConnection.setParameter("@currencyID", currencyID);
                dataConnection.read();

                if(dataConnection.Reader.Read())
                {
                    account.Id = (int)dataConnection.Reader["Id"];
                    account.Balance = (decimal)dataConnection.Reader["Balance"];
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
        #endregion
    }
}
