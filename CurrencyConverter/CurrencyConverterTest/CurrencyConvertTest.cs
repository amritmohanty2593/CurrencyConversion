using CurrencyConverterAPI.Controllers;
using CurrencyConverterCore.Models;
using CurrencyConverterCore;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyConverterTest
{
    public class Tests
    {
        private ICurrencyConvert _icurrencyConverter;
        private Mock<ILogger<CurrencyConversionController>> _ilogger;
        private IExchangeRates _iExchangeRates;
        private Mock<Serilog.ILogger> _ilogger1;
        private CurrencyConversionController _currencyController;

        [SetUp]
        public void Setup()
        {

            _ilogger = new Mock<ILogger<CurrencyConversionController>>();

            _iExchangeRates = new ExchangeRates();
            _iExchangeRates.USD_TO_INR = 0.1m;
            _iExchangeRates.EUR_TO_INR = 0.2m;
            _iExchangeRates.EUR_TO_USD = 0.3m;
            _iExchangeRates.INR_TO_USD = 0.4m;
            _iExchangeRates.USD_TO_EUR = 0.5m;
            _iExchangeRates.INR_TO_EUR = 0.6m;


            _ilogger1 = new Mock<Serilog.ILogger>();

            _icurrencyConverter = new CurrencyConversionLogic(_iExchangeRates, _ilogger1.Object);
            _currencyController = new CurrencyConversionController(_icurrencyConverter, _ilogger.Object);


        }

        [Test]
        public void ActionExecutes_ReturnsViewForGet()
        {
            var result = _currencyController.Get("USD", "INR", 5);
            Assert.IsInstanceOf<IActionResult>(result);
        }
        [Test]
        public void ConvertCurrencyTest()
        {
            var result = _icurrencyConverter.ConvertCurrency(SetUpInput("USD", "INR", 5));
            Assert.IsNotNull(result.ExchangeRate);

            result = _icurrencyConverter.ConvertCurrency(SetUpInput("INR", "USD", 5));
            Assert.That(0.4, Is.EqualTo(result.ExchangeRate));



        }
        public ConversionRequest SetUpInput(string sourceCurrency, string targetCurrency, decimal amount)
        {
            return new ConversionRequest()
            {
                SourceCurrency = sourceCurrency,
                TargetCurrency = targetCurrency,
                Amount = amount
            };
        }
    }
}