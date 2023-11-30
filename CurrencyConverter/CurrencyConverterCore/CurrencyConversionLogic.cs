using CurrencyConverterCore.Models;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace CurrencyConverterCore
{
    public class CurrencyConversionLogic : ICurrencyConvert
    {
        private readonly IExchangeRates _iExchangeRates;
        private readonly ILogger _ilogger;
        public CurrencyConversionLogic(IExchangeRates iExchangeRate,ILogger iLogger)
        {

            _iExchangeRates = iExchangeRate;
            _ilogger = iLogger;   
        }
        public ConversionResponse ConvertCurrency(ConversionRequest request)
        {
            string sourceTarget = request.SourceCurrency + "_TO_" + request.TargetCurrency;
            ConversionResponse response = new ConversionResponse();
            try
            {

                switch (sourceTarget)
                {

                    case "USD_TO_INR":
                        response.ConvertedAmount = (request.Amount * _iExchangeRates.USD_TO_INR);
                        response.ExchangeRate = _iExchangeRates.USD_TO_INR;
                        break;
                    case "INR_TO_USD":
                        response.ConvertedAmount = (request.Amount * _iExchangeRates.INR_TO_USD);
                        response.ExchangeRate = _iExchangeRates.INR_TO_USD;
                        break;
                    case "USD_TO_EUR":
                        response.ConvertedAmount = (request.Amount * _iExchangeRates.USD_TO_EUR);
                        response.ExchangeRate = _iExchangeRates.USD_TO_EUR;
                        break;
                    case "EUR_TO_USD":
                        response.ConvertedAmount = (request.Amount * _iExchangeRates.EUR_TO_USD);
                        response.ExchangeRate = _iExchangeRates.EUR_TO_USD;
                        break;
                    case "INR_TO_EUR":
                        response.ConvertedAmount = (request.Amount * _iExchangeRates.INR_TO_EUR);
                        response.ExchangeRate = _iExchangeRates.INR_TO_EUR;
                        break;
                    case "EUR_TO_INR":
                        response.ConvertedAmount = (request.Amount * _iExchangeRates.EUR_TO_INR);
                        response.ExchangeRate = _iExchangeRates.EUR_TO_INR;
                        break;

                    default:
                        break;


                }

            }
            catch (Exception ex)
            {
                var logError = new
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace
                    
                };
                _ilogger.Error("@ErrorMessage", logError);

            }
            return response;
        }
        public (string keyName, decimal valRate) UpdateExchangeRate(string keyName, decimal valRate,string path)
        {
            try
            {

                string json = File.ReadAllText(Path.Combine(path, "exchangeRate.json"));
                dynamic? jsonObj = JsonConvert.DeserializeObject(json);
                if (!string.IsNullOrEmpty(keyName))
                {
                    jsonObj["ExchangeRates"][keyName] = valRate; 

                }
                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(Path.Combine(path, "exchangeRate.json"), output);
            }
            catch (Exception)
            {

                throw;
            }
            return (keyName, valRate);
        }

    }
}