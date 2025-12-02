using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExchangeRate_MAUIApp.Models;
using ExchangeRate_MAUIApp.Services;
using Microcharts;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Collections.ObjectModel;
using static Microsoft.Maui.Controls.Internals.Profile;

namespace ExchangeRate_MAUIApp.ViewModels 
{
    public partial class ExchangeRateViewModel : ObservableObject
    {
        private readonly IExchangeRateService _exchangeRateService;

        public ExchangeRateViewModel(IExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
            Rates = new ObservableCollection<ExchangeRate>();
            dataChart = new ObservableCollection<ChartData>();

            LoadTodayCommand = new AsyncRelayCommand(LoadTodayAsync);
            LoadChartCommand = new AsyncRelayCommand(LoadChartAsync);
        }

        [ObservableProperty]
        DateTime startDate = DateTime.Today;

        [ObservableProperty]
        DateTime endDate = DateTime.Today;

        [ObservableProperty]
        private string selectedCurrency;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DisplayDates))]
        [NotifyPropertyChangedFor(nameof(CreateDate))]
        [NotifyPropertyChangedFor(nameof(ValidationDate))]
        private ExchangeRate? exchangeRate;

        public string? CreateDate => exchangeRate?.Datum_f.ToString("dd.MM.yyyy");
        public string? ValidationDate => exchangeRate?.Datum.ToString("dd.MM.yyyy");

        public string DisplayDates => exchangeRate != null ? $"Датум на креирање на курсот: {CreateDate} и датум на валидација: {ValidationDate}"
            : "Нема достапни податоци за курс";

        public ObservableCollection<ExchangeRate> Rates { get; set; }

        [ObservableProperty]
        private ObservableCollection<ChartData> dataChart;

        public IAsyncRelayCommand LoadTodayCommand { get; }
        public IAsyncRelayCommand LoadChartCommand { get; }
        public ObservableCollection<string> Currencies { get; set; } = new();

        [ObservableProperty]
        private PlotModel plotModel;

        private async Task LoadTodayAsync()
        {
            Rates.Clear();
            Currencies.Clear();
            try
            {
                var list = await _exchangeRateService.GetExchangeRatesAsync(startDate, endDate);

                foreach (var l in list)
                {
                    Rates.Add(l);
                    if (!Currencies.Contains(l.Oznaka))
                    {
                        Currencies.Add(l.Oznaka);
                    }
                }
                ExchangeRate = list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Грешка", "Почетниот датум не може да биде после крајнот датум", "OK");
            }
        }


        private async Task LoadChartAsync()
        {
            if (string.IsNullOrWhiteSpace(selectedCurrency))
            {
                await Application.Current.MainPage.DisplayAlert("Предупредување", "Ве молиме изберете валута", "OK");

                return;
            }
            try
            {
                var list = await _exchangeRateService.GetExchangeRatesAsync(startDate, endDate);

                var filtered = list
                    .Where(x => x.Oznaka == selectedCurrency)
                    .OrderBy(x => x.Datum)
                    .ToList();

                //var model = new PlotModel { Title = $"Курс за {selectedCurrency}" };

                //model.Axes.Add(new DateTimeAxis
                //{
                //    Position = AxisPosition.Bottom,
                //    StringFormat = "dd.MM.yyyy",
                //    Title = "Датум",
                //    IntervalType = DateTimeIntervalType.Days,
                //    MinorIntervalType = DateTimeIntervalType.Days
                //});

                //model.Axes.Add(new LinearAxis
                //{
                //    Position = AxisPosition.Left,
                //    Title = "Курс"
                //});

                //var series = new LineSeries
                //{
                //    MarkerType = MarkerType.Circle,
                //    MarkerSize = 4,
                //    MarkerStroke = OxyColors.DarkBlue,
                //    StrokeThickness = 2
                //};

                DataChart.Clear();
                foreach (var f in filtered)
                {

                    DataChart.Add(new ChartData
                    {
                        Date = f.Datum,
                        Rate = f.Sreden,
                });
                }
                //model.Series.Add(series);
                //PlotModel = model;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Грешка", $"Не може да се вчита графикот: {ex.Message}", "OK");
            }
        }
        
    }
}
