using ExchangeRate_MAUIApp.Models;
using System.Globalization;
using ExchangeRate_MAUIApp.Converters;
using System.Text.Json;


namespace ExchangeRate_MAUIApp.Services
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly HttpClient _http;

        public ExchangeRateService(HttpClient httpClient = null)
        {
            _http = httpClient ?? new HttpClient();
            _http.BaseAddress = new Uri("https://www.nbrm.mk/");
        }

        private string FormatDate(DateTime dateTime) =>
            dateTime.ToString("dd-MMM-yyyy", CultureInfo.InvariantCulture);

        public async Task<List<ExchangeRate>> GetExchangeRatesAsync(DateTime? StartDate, DateTime? EndDate)
        {
            try
            {
                var startDate = StartDate ?? DateTime.Now.Date;
                var endDate = EndDate ?? DateTime.Now.Date;

                var url = $"KLServiceNOV/GetExchangeRateD?StartDate={FormatDate(startDate)}&EndDate={FormatDate(endDate)}&format=json";

                System.Diagnostics.Debug.WriteLine($"Fetching: {url}");

                var resp = await _http.GetAsync(url);
                resp.EnsureSuccessStatusCode();

                var json = await resp.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"Response: {json.Substring(0, Math.Min(500, json.Length))}...");


                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                options.Converters.Add(new DateTimeConverter());
                options.Converters.Add(new CommaDecimalConverter());
                options.Converters.Add(new CommaIntConverter());

                var result = JsonSerializer.Deserialize<List<ExchangeRate>>(json, options);


                return result ?? new List<ExchangeRate>();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error fetching exchange rates: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
