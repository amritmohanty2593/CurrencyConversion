
using CurrencyConverterCore.Models;
using CurrencyConverterCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;

namespace CurrencyConversionApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region Logging
            var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(builder.Configuration)
                        .Enrich.FromLogContext()
                        .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            #endregion


            builder.Configuration
                .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location))
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", EnvironmentVariableTarget.Machine) ?? "Development"}.json", optional: false, reloadOnChange: true)
                .AddJsonFile("exchangeRate.json", optional: false, reloadOnChange: true);

            IExchangeRates exchangeRate = new ExchangeRates();
            builder.Configuration.Bind("ExchangeRates", exchangeRate);
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Currency Conversion API", Version = "v1" });
            });
            builder.Services.AddSingleton<ICurrencyConvert, CurrencyConversionLogic>(serviceProvider => new CurrencyConversionLogic(iExchangeRate: exchangeRate, iLogger: logger));
            var app = builder.Build();



            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseExceptionHandler("/ErrorDev");
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

        }
    }
}