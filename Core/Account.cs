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
                throw new Exception("No se pudo obtener la cuenta");
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
                ValidateCreate(currencyID);
                dataConnection.setQuery(@"  Insert into accounts (CurrencyID, Balance)
                                            Values (@currencyID,0)");
                dataConnection.setParameter("@currencyID", currencyID);
                dataConnection.execute();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo crear la cuenta");
            }
            finally { dataConnection.closeConnection(); }

            return Get(currencyID);
        }

        private void ValidateCreate(int currencyID)
        {
            DataConnection dataConnection = new DataConnection();
            try
            {
                dataConnection.setQuery("select 1 from accounts where currencyID = @currencyID and state = 1");
                dataConnection.setParameter("@currencyID", currencyID);
                dataConnection.read();

                if (dataConnection.Reader.Read())
                {
                    throw new Exception("Ya existe una cuenta dada de alta con esa moneda");
                }
            }catch (Exception)
            {
                throw;
            }
        }
        public Account? Close(int currencyID)
        {
            DataConnection dataConnection = new DataConnection();
            Account? account = new Account();

            try
            {
                dataConnection.setQuery(@"  update accounts set state = 0
                                            where currencyId = @currencyID");
                dataConnection.setParameter("@currencyID", currencyID);
                dataConnection.execute();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo cerrar la cuenta");
            }
            finally { dataConnection.closeConnection(); }
            
            return Get(currencyID);
        }
        public Account Deposit(int currencyID,decimal amount)
        {
            DataConnection dataConnection = new DataConnection();
            Account account = new Account();

            try
            {
                dataConnection.setQuery(@"  update accounts set balance = balance + @amount
                                            where currencyID = @currencyID
                                            and State = 1");
                dataConnection.setParameter("@currencyID", currencyID);
                dataConnection.setParameter("@amount", amount);
                dataConnection.execute();
            }
            catch (Exception ex)
            {
                throw new Exception("No se pudo realizar el deposito");
            }
            finally { dataConnection.closeConnection(); }
            
            return Get(currencyID);
        }
        public Account Withdraw(int currencyID,decimal amount)
        {
            DataConnection dataConnection = new DataConnection();
            Account account = new Account();

            try
            {
                ValidateWithdraw(currencyID, amount);
                dataConnection.setQuery(@"  update accounts set balance = balance - @amount
                                            where currencyID = @currencyID
                                            and State = 1");
                dataConnection.setParameter("@currencyID", currencyID);
                dataConnection.setParameter("@amount", amount);
                dataConnection.execute();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally { dataConnection.closeConnection(); }
            
            return Get(currencyID);
        }

        private void ValidateWithdraw(int currencyID, decimal amount)
        {
            DataConnection dataConnection = new DataConnection();
            try
            {
                dataConnection.setQuery("select Balance from accounts where currencyID=@currencyID and state=1");
                dataConnection.setParameter("@currencyID", currencyID);
                dataConnection.read();
                if (dataConnection.Reader.Read())
                {
                    decimal balance = (decimal)dataConnection.Reader["Balance"];

                    if(balance - amount < 0) { throw new Exception("No se puede extraer mas que el balance disponible"); }
                }
                else { throw new Exception("Error validando extraccion"); }
            }
            catch(Exception ex) 
            { 
                throw;
            }
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
