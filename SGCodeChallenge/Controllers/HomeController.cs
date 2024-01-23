using Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGCodeChallenge.Models;
using System.Diagnostics;
using System.Text;

namespace SGCodeChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            CurrencyManager currencyManager = new CurrencyManager();
            IndexViewModel indexViewModel = new IndexViewModel();

            var storedData = HttpContext.Session.Get("IndexViewModel");
            if (storedData == null)
            {
                indexViewModel.Currencies = new List<Currency>();
                List<Currency> currencyList = currencyManager.GetList();
                foreach (Currency currency in currencyList)
                {
                    indexViewModel.Currencies.Add(currency);
                }
                indexViewModel.SelectedCurrency = 0;
            }
            else
            {
                indexViewModel = JsonConvert.DeserializeObject<IndexViewModel>(Encoding.UTF8.GetString(storedData));
            }



            return View(indexViewModel);
        }

        [HttpPost]
        public IActionResult GetSelectedAccount([FromBody] IndexViewModel indexViewModel)
        {
            AccountManager accountManager = new AccountManager();
            Account? account = accountManager.Get(indexViewModel.SelectedCurrency);
            indexViewModel.SelectedAccount = account;
            HttpContext.Session.Set("IndexViewModel", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(indexViewModel)));

            return View("Index", indexViewModel);
        }
        
        [HttpPost]
        public IActionResult OpenAccount([FromBody] IndexViewModel indexViewModel)
        {
            AccountManager accountManager = new AccountManager();
            Account account = accountManager.Create(indexViewModel.SelectedCurrency);
            indexViewModel.SelectedAccount = account;
            HttpContext.Session.Set("IndexViewModel", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(indexViewModel)));

            return View("Index", indexViewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}