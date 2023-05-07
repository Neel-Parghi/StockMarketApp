using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IStocksService
    {
        /// <summary>
        /// Inserts a new buy order into the database table called 'BuyOrders'
        /// </summary>
        /// <param name="BuyOrderRequest"></param>
        /// <returns></returns>
        BuyOrderResponse CreateBuyOrder(BuyOrderRequest? buyOrderRequest);

        /// <summary>
        ///  Inserts a new sell order into the database table called 'SellOrders'
        /// </summary>
        /// <param name="sellOrderRequest"></param>
        /// <returns></returns>
        SellOrderResponse CreateSellOrder(SellOrderRequest? sellOrderRequest);

        /// <summary>
        /// Returns the existing list of buy orders retrieved from database table called 'BuyOrders'
        /// </summary>
        /// <returns></returns>
        List<BuyOrderResponse> GetBuyOrders();

        /// <summary>
        /// Returns the existing list of sell orders retrieved from database table called 'SellOrders'
        /// </summary>
        /// <returns></returns>
        List<SellOrderResponse> GetSellOrders();
    }
}
