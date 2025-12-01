using ExchangeRate_MAUIApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate_MAUIApp.Services
{
    public interface IExchangeRateService
    {
       Task<List<ExchangeRate>> GetExchangeRatesAsync(DateTime? StartDate, DateTime? EndDate);
    }
}
