using System.Reflection.Metadata.Ecma335;
using System.Security;

namespace Core
{
    public class CurrencyManager
    {
        //public Currency Get()
        //{

        //    Currency currency = new Currency();
        //    currency.Get(currency);
        //    return currency;
        //}
        public List<Currency> GetList()
        {

            Currency currency = new Currency();
            return currency.GetList(); 
        }

        public Currency Update(Currency currency)
        {
            return new Currency();
        }
    }
}