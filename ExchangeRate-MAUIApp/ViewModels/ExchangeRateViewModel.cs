using ExchangeRate_MAUIApp.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ExchangeRate_MAUIApp.Models;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using Microcharts;

namespace ExchangeRate_MAUIApp.ViewModels 
{
    public partial class ExchangeRateViewModel : ObservableObject
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateViewModel(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
            Rates = new ObservableCollection<ExchangeRate>();

            LoadTodayCommand = new AsyncRelayCommand(LoadTodayAsync);
            LoadChartCommand = new AsyncRelayCommand(LoadChartAsync);
        }


        [ObservableProperty]
        DateTime startDate = DateTime.Today;

        [ObservableProperty]
        DateTime endDate = DateTime.Today;

        [ObservableProperty]
        private string selectedCurrency;


        public ObservableCollection<ExchangeRate> Rates { get; set; }

        public IAsyncRelayCommand LoadTodayCommand { get; }
        public IAsyncRelayCommand LoadChartCommand { get; }

        private async Task LoadTodayAsync()
        {
            Rates.Clear();

            var list = await _exchangeRateService.GetExchangeRatesAsync(startDate, endDate);

            foreach (var l in list)
            {
                Rates.Add(l);
            }
        }

        [ObservableProperty]
        private Microcharts.LineChart chartModel;

        private async Task LoadChartAsync()
        {
            if (string.IsNullOrWhiteSpace(selectedCurrency))
            {
                return;
            }

            var list = await _exchangeRateService.GetExchangeRatesAsync(startDate, endDate);

            var filtered = list
                .Where(x => x.Oznaka == selectedCurrency)
                .OrderBy(x=>x.Datum)
                .ToList();

            var entries = filtered.Select(f=> new Microcharts.ChartEntry((float)f.Sreden)

                {
                    Label = f.Datum.ToString("dd.MM.yyyy"),
                    ValueLabel = f.Sreden.ToString(),
                    Color = SKColor.Parse("#2E86DE")
                }).ToList();

            ChartModel = new Microcharts.LineChart
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                LineSize = 3,
                PointMode = PointMode.Circle,
                PointSize = 5
            };
        }

        
    }
}
