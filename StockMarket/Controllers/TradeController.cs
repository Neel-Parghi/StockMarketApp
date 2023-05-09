using Microsoft.AspNetCore.Mvc;
using StockMarket.Models;
using ServiceContracts;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using ServiceContracts.DTO;
using System.Collections.Immutable;

namespace StockMarket.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly TradingOptions _tradingOptions;
        private readonly IStocksService _stocksService;
        private readonly IFinnhubService _finnhubService;
        private readonly IConfiguration _configuration;

        public TradeController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService, IConfiguration configuration, IStocksService stocksService)
        {
            _tradingOptions = tradingOptions.Value;
            _finnhubService = finnhubService;
            _configuration = configuration;
            _stocksService = stocksService;
        }

        [Route("/")]
        [Route("[action]")]
        [Route("~/[controller]")]
        public IActionResult Index()
        {
            if(string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
                _tradingOptions.DefaultStockSymbol = "MSFT";

            Dictionary<string, object>? companyProfileDictionary = _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);

            Dictionary<string, object>? stockQuoteDictionary = _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);

            StockTrade stockTradeData = new() { StockSymbol = _tradingOptions.DefaultStockSymbol };

            if (companyProfileDictionary != null && stockQuoteDictionary != null)
            {
                stockTradeData = new()
                {
                    StockSymbol = Convert.ToString(companyProfileDictionary["ticker"]),
                    StockName = Convert.ToString(companyProfileDictionary["name"]),
                    Price = Convert.ToDouble(stockQuoteDictionary["c"].ToString()),
                    Quantity = _tradingOptions.DefaultOrderQuantity ?? 0,
                };
            }

            return View(stockTradeData);
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            ModelState.Clear();
            TryValidateModel(buyOrderRequest);
            if(!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(V => V.Errors).Select(E => E.ErrorMessage).ToList();
                StockTrade stockTrade = new () { StockName = buyOrderRequest.StockName, Quantity = buyOrderRequest.Quantity, StockSymbol = buyOrderRequest.StockSymbol };
                return View("Index", stockTrade);
            }

            BuyOrderResponse buyOrderResponse = _stocksService.CreateBuyOrder(buyOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        [HttpPost]
        public IActionResult SellOrder(SellOrderRequest sellOrderRequest)
        {
            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            ModelState.Clear();
            TryValidateModel(sellOrderRequest);
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(V => V.Errors).Select(E=>E.ErrorMessage).ToList();
                StockTrade stockTrade = new()
                {
                    StockName = sellOrderRequest.StockName,
                    Quantity = sellOrderRequest.Quantity,
                    StockSymbol = sellOrderRequest.StockSymbol
                };
                return View("Index", stockTrade);
            }
            SellOrderResponse sellOrderResponse = _stocksService.CreateSellOrder(sellOrderRequest);
            return RedirectToAction(nameof(Orders));
        }

        [Route("[action]")]
        public IActionResult Orders()
        {
            List<BuyOrderResponse> buyOrderResponses = _stocksService.GetBuyOrders(); 
            List<SellOrderResponse> sellOrderResponses = _stocksService.GetSellOrders();

            Orders orders = new ()
            {
                BuyOrders = buyOrderResponses, SellOrders = sellOrderResponses
            };
            ViewBag.TradingOptions = _tradingOptions;

            return View(orders);
        }
    }
}
