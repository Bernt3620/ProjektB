using MauiProjectBWeather.Models;
using MauiProjectBWeather.Views;

namespace MauiProjectBWeather
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            foreach (CityPicture city in CityPicture.List)
            {
                var sc = new ShellContent
                {
                    Title = city.Name,
                    Route = city.ImageSrc.ToLower().Replace(".jpg", null),
                    ContentTemplate = new DataTemplate(() => new ForecastPage(city))
                };
                this.Items.Add(sc);
            }
        }
    }
}
