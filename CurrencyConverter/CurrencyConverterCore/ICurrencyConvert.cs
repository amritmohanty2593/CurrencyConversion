using CurrencyConverterCore.Models;

namespace CurrencyConverterCore
{
    public interface ICurrencyConvert
    {
        public ConversionResponse ConvertCurrency(ConversionRequest request);
        (string keyName, decimal valRate) UpdateExchangeRate(string key, decimal val, string path);


    }
}
