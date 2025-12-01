using ExchangeRate_MAUIApp.Views;

namespace ExchangeRate_MAUIApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("ChartPage", typeof(ChartPage));
        }
    }
}
