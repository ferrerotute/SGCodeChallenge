﻿using Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SGCodeChallenge.Models;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace SGCodeChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public async Task<IActionResult> Index()
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
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", "SGCodeChallengeUser"),
                    new KeyValuePair<string, string>("password", "SGCodeChallengePassword"),
                });

                HttpResponseMessage response = await client.PostAsync("/tokens/connect", content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

                    indexViewModel.Token = token.access_token;
                }

            }
            return View("Index", indexViewModel);

        }

        [Authorize]
        [HttpPost]
        public IActionResult GetSelectedAccount([FromBody]IndexViewModel indexViewModel)
        {
            AccountManager accountManager = new AccountManager();
            try
            {
                Account? account = accountManager.Get(indexViewModel.SelectedCurrency);
                indexViewModel.SelectedAccount = account;
                indexViewModel.Message = account == null ? "No existe cuenta con este fondo" : "";
            }
            catch (Exception ex)
            {
                indexViewModel.Message = ex.Message;
            }
            HttpContext.Session.Set("IndexViewModel", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(indexViewModel)));
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return View("Index", indexViewModel);
            }
            else { return Json(indexViewModel); }
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetCurrencies()
        {
            CurrencyManager currencyManager = new CurrencyManager();
            List<Currency> currencies;
            try
            {
                currencies = currencyManager.GetList();
            }
            catch (Exception ex)
            {
                throw;
            }
            return Json(currencies); 
        }

        [Authorize]
        [HttpPost]
        public IActionResult OpenAccount([FromBody] IndexViewModel indexViewModel)
        {
            AccountManager accountManager = new AccountManager();
            try
            {
                Account account = accountManager.Create(indexViewModel.SelectedCurrency);
                indexViewModel.SelectedAccount = account;
            }
            catch (Exception ex)
            {
                indexViewModel.Message = ex.Message;
            }
            HttpContext.Session.Set("IndexViewModel", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(indexViewModel)));

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return View("Index", indexViewModel);
            }
            else { return Json(indexViewModel); }
        }

        [Authorize]
        [HttpPost]
        public IActionResult CloseAccount([FromBody] IndexViewModel indexViewModel)
        {
            AccountManager accountManager = new AccountManager();
            try
            {
                Account? account = accountManager.Close(indexViewModel.SelectedCurrency);
                indexViewModel.SelectedAccount = account;
            }
            catch (Exception ex)
            {
                indexViewModel.Message = ex.Message;
            }
            HttpContext.Session.Set("IndexViewModel", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(indexViewModel)));

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return View("Index", indexViewModel);
            }
            else { return Json(indexViewModel); }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Deposit([FromBody] IndexViewModel indexViewModel)
        {
            AccountManager accountManager = new AccountManager();
            try
            {
                Account account = accountManager.Deposit(indexViewModel.SelectedCurrency, indexViewModel.Amount);
                indexViewModel.SelectedAccount = account;
            }
            catch (Exception ex)
            {
                indexViewModel.Message = ex.Message;
                indexViewModel.SelectedAccount = accountManager.Get(indexViewModel.SelectedCurrency);

            }
            HttpContext.Session.Set("IndexViewModel", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(indexViewModel)));

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return View("Index", indexViewModel);
            }
            else { return Json(indexViewModel); }
        }

        [Authorize]
        [HttpPost]
        public IActionResult Withdraw([FromBody] IndexViewModel indexViewModel)
        {
            AccountManager accountManager = new AccountManager();
            try
            {

                Account account = accountManager.Withdraw(indexViewModel.SelectedCurrency, indexViewModel.Amount);
                indexViewModel.SelectedAccount = account;
            }
            catch (Exception ex)
            {
                indexViewModel.Message = ex.Message;
                indexViewModel.SelectedAccount = accountManager.Get(indexViewModel.SelectedCurrency);
            }
            HttpContext.Session.Set("IndexViewModel", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(indexViewModel)));

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return View("Index", indexViewModel);
            }
            else { return Json(indexViewModel); }
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