using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using MauiProjectBWeather.Models;
using MauiProjectBWeather.Services;

namespace MauiProjectBWeather.Views
{
    public class GroupedForecast
    {
        public string City { get; set; }
        public IEnumerable<IGrouping<DateTime, ForecastItem>> Items { get; set; }
    }

    public partial class ForecastPage : ContentPage
    {
        OpenWeatherService service;
        GroupedForecast groupedforecast;
        CityPicture city;

        public ForecastPage(CityPicture city)
        {
            InitializeComponent();

            this.city = city;
            service = new OpenWeatherService();
            groupedforecast = new GroupedForecast();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = $"Forecast for {city.Name}";

            MainThread.BeginInvokeOnMainThread(async () => {await LoadForecast();});
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            await LoadForecast();
        }

        private async Task LoadForecast()
        {
            Forecast forecast = await service.GetForecastAsync(city.Name);

            //Here Group your forecast and bind it to your
            //ListView ItemSource
        }
    }
}