using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Core
{
    public class Currency
    {
        #region "Attributes"
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Symbol { get; set; }
        #endregion

        #region "Logic"

        public Currency Get(int currencyID)
        {
            DataConnection dataConnection = new DataConnection();
            Currency currency = new Currency();
            try
            {
                dataConnection.setQuery(@"  Select name, symbol, abbreviation
                                            from currencies where id = @currencyID");
                dataConnection.setParameter("@currencyID", currencyID);

                dataConnection.read();
                if (dataConnection.Reader.Read())
                {
                    currency.Name = (string)dataConnection.Reader["name"];
                    currency.Symbol = formatSymbol((string)dataConnection.Reader["symbol"]);
                    currency.Abbreviation = (string)dataConnection.Reader["abbreviation"];
                }
                return currency;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<Currency> GetList()
        {
            DataConnection dataConnection = new DataConnection();
            List <Currency> currenciesList = new List <Currency>();
            try
            {
                dataConnection.setQuery(@"  Select id, name, symbol, abbreviation
                                            from currencies");

                dataConnection.read();
                while (dataConnection.Reader.Read())
                {
                    Currency currency = new Currency();
                    currency.Id = (int)dataConnection.Reader["Id"];
                    currency.Name = (string)dataConnection.Reader["name"];
                    currency.Symbol = formatSymbol((string)dataConnection.Reader["symbol"]);
                    currency.Abbreviation = (string)dataConnection.Reader["abbreviation"];
                    currenciesList.Add(currency);
                }
                return currenciesList;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally{ dataConnection.closeConnection(); }
        }

        private string formatSymbol(string symbol)
        {
            if (symbol.StartsWith("\\u"))
            {
                string unicodeValue = symbol.Substring(2);
                if (int.TryParse(unicodeValue, System.Globalization.NumberStyles.HexNumber, null, out int intValue))
                {
                    symbol = char.ConvertFromUtf32(intValue);
                }
            }
            return symbol;
        }

        #endregion
    }
}