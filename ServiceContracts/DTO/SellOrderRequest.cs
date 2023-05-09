using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class SellOrderRequest : IValidatableObject
    {
        [Required(ErrorMessage = "Stock Symbol can't be null or empty")]
        public string StockSymbol { get; set; }

        [Required(ErrorMessage = "Stock Name can't be null or empty")]
        public string StockName { get; set; }

        public DateTime DateAndTimeOfOrder { get; set; }

        [Range(1, 100000, ErrorMessage = "You can sell maximum of 100000 shares in single order. Minimum is 1.")]
        public uint Quantity { get; set; }

        [Range(1, 100000, ErrorMessage = "The maximum price of stock is 10000. Minimum is 1.")]
        public double Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> ValidationResults = new();

            //Date of order should be less than Jan 01, 2000
            if (DateAndTimeOfOrder < Convert.ToDateTime("2000-01-01"))
            {
                ValidationResults.Add(new ValidationResult("Date of the order should not be older than Jan 01, 2000."));
            }
            return ValidationResults;
        }
        public SellOrder ConvertToSellOrder()
        {
            return new SellOrder()
            {
                StockSymbol = StockSymbol,
                StockName = StockName,
                Price = Price,
                DateAndTimeOfOrder = DateAndTimeOfOrder,
                Quantity = Quantity
            };
        }
    }
}
