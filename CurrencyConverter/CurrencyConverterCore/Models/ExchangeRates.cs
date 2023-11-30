using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyConverterCore.Models
{
    public class ExchangeRates:IExchangeRates
    { 
        public decimal USD_TO_INR { get; set; }
        public decimal INR_TO_USD { get; set; }
        public decimal USD_TO_EUR { get; set; }
        public decimal EUR_TO_USD { get; set; }
        public decimal INR_TO_EUR { get; set; }
        public decimal EUR_TO_INR { get; set; }
    }
}
