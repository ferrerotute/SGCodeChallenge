using Core;

namespace SGCodeChallenge.Models
{
    public class IndexViewModel
    {
        public Account SelectedAccount{ get; set; }
        public List<Currency> Currencies {get;set;}
        public int SelectedCurrency { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}