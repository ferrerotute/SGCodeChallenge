using Core;

namespace SGCodeChallenge.Models
{
    public class IndexViewModel
    {
        public Account SelectedAccount{ get; set; }
        public List<Currency> Currencies {get;set;}
        public int SelectedCurrency { get; set; }
    }
}