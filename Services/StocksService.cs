using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Services.Helpers;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly StockMarketDbContext _db;
        public StocksService(StockMarketDbContext stockMarketDbContext) { 
            _db = stockMarketDbContext;
        }

        public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            if(buyOrderRequest == null)
                throw new ArgumentNullException(nameof(buyOrderRequest));
            
            //Model validation
            ValidationHelper.ModelValidation(buyOrderRequest);

            //convert buyOrderRequest into BuyOrder type
            BuyOrder buyOrder = buyOrderRequest.ConvertToBuyOrder();

            //generate BuyOrderID
            buyOrder.BuyOrderID = Guid.NewGuid();

            // add buy order object to buy orders list

            _db.BuyOrders.Add(buyOrder);
            await _db.SaveChangesAsync();

            return buyOrder.ConvertTobuyOrderResposne();
        }

        public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ConvertToSellOrder();

            sellOrder.SellOrderID = Guid.NewGuid();

            _db.sellOrders.Add(sellOrder);
            await _db.SaveChangesAsync();

            return sellOrder.ConvertToSellOrderResponse();
        }

        public async Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            //Convert all BuyOrder objects into BuyOrderResponse objects
            List<BuyOrder> buyOrders = await _db.BuyOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToListAsync();
                
            return buyOrders.Select(temp=>temp.ConvertTobuyOrderResposne()).ToList();
        }

        public async Task<List<SellOrderResponse>> GetSellOrders()
        {
            List<SellOrder> sellOrders = await _db.sellOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToListAsync();

            return sellOrders.Select(temp => temp.ConvertToSellOrderResponse()).ToList();
        }
    }
}
