using CurrencyConverterCore;
using CurrencyConverterCore.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CurrencyConverterAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyConversionController : ControllerBase
    {
        private ICurrencyConvert _icurrencyConverter;
        private ILogger<CurrencyConversionController> _ilogger;
        public CurrencyConversionController(ICurrencyConvert icurrencyConverter, ILogger<CurrencyConversionController> iLogger)
        {
            _icurrencyConverter = icurrencyConverter;
            _ilogger = iLogger;
        }
        
        // GET api/<CurrencyConversionController>/5
        [HttpGet("{sourceCurrency}/{targetCurrency}/{amount}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary ="Get Exchange rate and converted Amount")]
        public IActionResult Get([Required] string sourceCurrency, [Required] string targetCurrency, [Required] decimal amount)
        {
            var response = new ConversionResponse();
            try
            {
               
                var requestModel = new ConversionRequest()
                {
                    SourceCurrency = sourceCurrency.ToUpper(),
                    TargetCurrency = targetCurrency.ToUpper(),
                    Amount = amount
                };
                response = _icurrencyConverter.ConvertCurrency(requestModel);

            }
            catch (Exception ex)
            {
                var logError = new
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace

                };
                _ilogger.LogError("@ErrorMessage", logError);
            }
            if (response == null)
            {
                return BadRequest();
            }
            else
            {
                _ilogger.LogInformation(JsonConvert.SerializeObject(response));
                return Ok(response);
            }
        }

        [HttpGet]
        [Route("/ErrorDev")]
        [NonAction]
        public IActionResult Error(IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException("Invalid Operation Non Dev Environment");
            }
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            return Problem(
                detail: context?.Error.StackTrace,
                title: context?.Error.Message);
        }



        [HttpPatch("{key}/{exchangeValue}")]
        [SwaggerOperation(Summary = "Use eg: USD_TO_INR and Change the exchange value in runtime Note: currently data updation working reload the setting need to work on")]
        public IActionResult Put([Required] string key, [Required] decimal exchangeValue)
        {

            try
            {
                var response = _icurrencyConverter.UpdateExchangeRate(key.ToUpper(), exchangeValue, Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location));
            }
            catch (Exception ex)
            {
                var logError = new
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace

                };
                _ilogger.LogError("@ErrorMessage", logError);
            }
            return Ok(new { key, exchangeValue });
        }

    }
}
