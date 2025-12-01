using ExchangeRate_MAUIApp.Services;
using ExchangeRate_MAUIApp.ViewModels;

namespace ExchangeRate_MAUIApp.Views;

public partial class MainPage : ContentPage
{
	private readonly ExchangeRateViewModel _viewModel;
	public MainPage()
	{
		InitializeComponent();
		_viewModel = new ExchangeRateViewModel(new ExchangeRateService());
		BindingContext = _viewModel;
	}

	private async void OnShowChartClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///ChartPage");
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if(_viewModel.Rates.Count == 0)
		{
			await _viewModel.LoadTodayCommand.ExecuteAsync(null);
		}
	}
}