using ExchangeRate_MAUIApp.Services;
using ExchangeRate_MAUIApp.ViewModels;

namespace ExchangeRate_MAUIApp.Views;

public partial class ChartPage : ContentPage
{
	public ChartPage(ExchangeRateViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}