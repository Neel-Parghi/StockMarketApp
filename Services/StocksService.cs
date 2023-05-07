using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Services.Helpers;
using System.Collections.Generic;

namespace Services
{
    public class StocksService : IStocksService
    {
        private readonly List<BuyOrder> _buyOrders;
        private readonly List<SellOrder> _sellOrders;
        public StocksService() { 
            _buyOrders = new List<BuyOrder>();
            _sellOrders = new List<SellOrder>();
        }

        public BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
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

            _buyOrders.Add(buyOrder);

            return buyOrder.ConvertTobuyOrderResposne();
        }

        public SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            if (sellOrderRequest == null)
                throw new ArgumentNullException(nameof(sellOrderRequest));

            ValidationHelper.ModelValidation(sellOrderRequest);

            SellOrder sellOrder = sellOrderRequest.ConvertToSellOrder();

            sellOrder.SellOrderID = Guid.NewGuid();

            _sellOrders.Add(sellOrder);

            return sellOrder.ConvertToSellOrderResponse();
        }

        public List<BuyOrderResponse> GetBuyOrders()
        {
            //Convert all BuyOrder objects into BuyOrderResponse objects
            return _buyOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).Select(temp=>temp.ConvertTobuyOrderResposne()).ToList();
        }

        public List<SellOrderResponse> GetSellOrders()
        {
            return _sellOrders.OrderByDescending(temp => temp.DateAndTimeOfOrder).Select(temp => temp.ConvertToSellOrderResponse()).ToList();
        }
    }
}
