using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
namespace Test
{
    public class StocksServiceTest
    {
        private readonly IStocksService _stocksService;

        public StocksServiceTest()
        {
            _stocksService = new StocksService(new StockMarketDbContext(new DbContextOptionsBuilder<StockMarketDbContext>().Options));
        }


        #region CreateBuyOrder

        public static BuyOrderRequest CreateNewBuyOrder(uint Quantity)
        {
            BuyOrderRequest buyOrderRequest = new() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = Quantity };
            return buyOrderRequest;
        }
        public static BuyOrderRequest CreateNewBuyOrder(string dateTime)
        {
            BuyOrderRequest buyOrderRequest = new() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime(dateTime), Price = 1, Quantity = 1 };
            return buyOrderRequest;
        }
        public static BuyOrderRequest CreateNewBuyOrder(double price, string? stockSymbol = "MSFT")
        {
            BuyOrderRequest buyOrderRequest = new() { StockSymbol = stockSymbol, StockName = "Microsoft", Price = price, Quantity = 1 };
            return buyOrderRequest;
        }


        // supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateBuyOrder_NullBuyOrderRequest()
        {
            BuyOrderRequest? buyOrderRequest = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async() =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_InvalidBuyOrderQuantity(uint buyOrderQuantity)
        {
            BuyOrderRequest buyOrderRequest = CreateNewBuyOrder(buyOrderQuantity);

            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Theory]
        [InlineData(100001)]
        public async Task CreateBuyOrder_MaximumbuyOrderQuantity(uint buyOrderQuantity)
        {
            BuyOrderRequest buyOrderRequest = CreateNewBuyOrder(buyOrderQuantity);

            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        // When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory]
        [InlineData(0)]
        public async Task CreateBuyOrder_MinimumSellOrderPrice(double SellOrderPrice)
        {
            BuyOrderRequest buyOrderRequest = CreateNewBuyOrder(SellOrderPrice);
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Theory]
        [InlineData(10001)]
        public async Task CreateBuyOrder_MaximumSellOrderPrice(double SellOrderPrice)
        {
            BuyOrderRequest buyOrderRequest = CreateNewBuyOrder(SellOrderPrice);
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_NullStockSymbol()
        {
            BuyOrderRequest buyOrderRequest = CreateNewBuyOrder(1,null);
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateBuyOrder_InvalidDateAndTime()
        {
            BuyOrderRequest buyOrderRequest = CreateNewBuyOrder("1999-12-31");
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async Task CreateBuyOrder_validData()
        {
            BuyOrderRequest buyOrderRequest = CreateNewBuyOrder("2024-12-31");
            
            BuyOrderResponse buyOrderResponseFromCreate = await _stocksService.CreateBuyOrder(buyOrderRequest);

            Assert.NotEqual(Guid.Empty, buyOrderResponseFromCreate.BuyOrderID);
        }
        #endregion

        #region CreateSellOrder

        public static SellOrderRequest CreateNewSellOrder(uint Quantity)
        {
            SellOrderRequest? sellOrderRequest = new() { StockSymbol = "MSFT", StockName = "Microsoft", Price = 1, Quantity = Quantity };
            return sellOrderRequest;
        }
        public static SellOrderRequest CreateNewSellOrder(string dateTime)
        {
            SellOrderRequest? sellOrderRequest = new() { StockSymbol = "MSFT", StockName = "Microsoft", DateAndTimeOfOrder = Convert.ToDateTime(dateTime), Price = 1, Quantity = 1 };
            return sellOrderRequest;
        }
        public static SellOrderRequest CreateNewSellOrder(double price, string? stockSymbol = "MSFT")
        {
            SellOrderRequest? sellOrderRequest = new() { StockSymbol = stockSymbol, StockName = "Microsoft", Price = price, Quantity = 1 };
            return sellOrderRequest;
        }


        // supply CreateSellOrder as null, it should throw ArgumentNullException.
        [Fact]
        public async Task CreateSellOrder_NullSellOrderRequest()
        {
            SellOrderRequest? sellOrderRequest = null;
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        // When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_MinimumSellOrderQuantity(uint sellOrderQuantity)
        {
            SellOrderRequest sellOrderRequest = CreateNewSellOrder(sellOrderQuantity);

            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Theory]
        [InlineData(100001)]
        public async Task CreateSellOrder_MaximumSellOrderQuantity(uint sellOrderQuantity)
        {
            SellOrderRequest sellOrderRequest = CreateNewSellOrder(sellOrderQuantity);

            await Assert.ThrowsAsync<ArgumentException>(async () => { 
                await _stocksService.CreateSellOrder(sellOrderRequest); });
        }

        // When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Theory]
        [InlineData(0)]
        public async Task CreateSellOrder_MinimumSellOrderPrice(double SellOrderPrice)
        {
            SellOrderRequest sellOrderRequest = CreateNewSellOrder(SellOrderPrice);
            await Assert.ThrowsAsync<ArgumentException>(async () => { 
                await _stocksService.CreateSellOrder(sellOrderRequest); });
        }

        //When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Theory]
        [InlineData(100001)]
        public async Task CreateSellOrder_MaximumSellOrderPrice(double SellOrderPrice)
        {
            SellOrderRequest sellOrderRequest = CreateNewSellOrder(SellOrderPrice);
            await Assert.ThrowsAsync<ArgumentException>(async () => { await _stocksService.CreateSellOrder(sellOrderRequest); });
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_NullStockSymbol()
        {
            SellOrderRequest sellOrderRequest = CreateNewSellOrder(1, null);
            await Assert.ThrowsAsync<ArgumentException>(async() =>
            {
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async Task CreateSellOrder_InvalidDateAndTime()
        {
            SellOrderRequest? sellOrderRequest = CreateNewSellOrder("1992-12-31");
            await Assert.ThrowsAsync<ArgumentException>(async () => { 
                await _stocksService.CreateSellOrder(sellOrderRequest); });
        }

        //If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async Task CreateSellOrder_validData()
        {
            SellOrderRequest sellOrderRequest = CreateNewSellOrder("2024-12-31");

            SellOrderResponse sellOrderResponseFromCreate = await _stocksService.CreateSellOrder(sellOrderRequest);

            Assert.NotEqual(Guid.Empty, sellOrderResponseFromCreate.SellOrderID);
        }
        #endregion

        #region GetAllBuyOrders

        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetAllBuyOrders_DefaultList()
        {
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
            Assert.Empty(buyOrderResponses);
        }

        //When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async Task GetAllBuyOrders_WithFewBuyOrders()
        {
            BuyOrderRequest buyOrderRequest1 = CreateNewBuyOrder("2015-01-01");
            BuyOrderRequest buyOrderRequest2 = CreateNewBuyOrder("2015-11-11");

            List<BuyOrderRequest> buyOrder_requestlist = new()
            {
                buyOrderRequest1, buyOrderRequest2
            };

            List<BuyOrderResponse> buyOrder_response_list = new();

            foreach(BuyOrderRequest orderRequest in buyOrder_requestlist)
            {
                buyOrder_response_list.Add(await _stocksService.CreateBuyOrder(orderRequest));
            }

            List<BuyOrderResponse> buyOrdersResponse = await _stocksService.GetBuyOrders();

            foreach(BuyOrderResponse orderResponse in buyOrdersResponse)
            {
                Assert.Contains(orderResponse, buyOrdersResponse);
            }
        }
        #endregion

        #region GetAllSellOrders

        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async Task GetAllSellOrders_DefaultList()
        {
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
            Assert.Empty(buyOrderResponses);
        }

        //When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async void GetAllSellOrders_WithFewBuyOrders()
        {
            SellOrderRequest sellOrderRequest1 = CreateNewSellOrder("2015-01-01");
            SellOrderRequest sellOrderRequest2 = CreateNewSellOrder("2015-11-11");

            List<SellOrderRequest> sellOrder_requestlist = new()
            {
                sellOrderRequest1, sellOrderRequest2
            };

            List<SellOrderResponse> sellOrder_response_list = new();

            foreach (SellOrderRequest orderRequest in sellOrder_requestlist)
            {
                sellOrder_response_list.Add( await _stocksService.CreateSellOrder(orderRequest));
            }

            List<SellOrderResponse> sellOrdersResponse = await _stocksService.GetSellOrders();

            foreach (SellOrderResponse orderResponse in sellOrdersResponse)
            {
                Assert.Contains(orderResponse, sellOrdersResponse);
            }
        }
        #endregion
    }
}