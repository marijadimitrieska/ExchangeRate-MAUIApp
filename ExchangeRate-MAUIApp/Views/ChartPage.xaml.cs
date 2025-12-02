using ExchangeRate_MAUIApp.Services;
using ExchangeRate_MAUIApp.ViewModels;
using OxyPlot.Maui.Skia;
using System.Threading.Tasks;

namespace ExchangeRate_MAUIApp.Views;

public partial class ChartPage : ContentPage
{
	private readonly ExchangeRateViewModel _viewModel;
	private bool _isFirstLoad = true;
	public ChartPage(ExchangeRateViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = viewModel;
		
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		if (_isFirstLoad)
		{
			_isFirstLoad = false;
			try
			{
				await _viewModel.LoadTodayCommand.ExecuteAsync(null);
			}
			catch (Exception ex)
			{
				await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
			}
	
		}
	}
	public async void OnBackToMainPageClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("/MainPage");
	}

    private void button_Clicked(object sender, EventArgs e)
    {

    }
}