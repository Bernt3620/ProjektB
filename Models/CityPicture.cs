using System.Collections.Generic;
using System.Linq;

namespace MauiProjectBWeather.Models
{
    public class CityPicture
    {
        public string Name { get; set; }
        public string ImageSrc { get; set; }
        public override string ToString() => $"Name: {Name} Picture: {ImageSrc}";

        public static IEnumerable<CityPicture> List => new List<CityPicture>()
            {
                new CityPicture() { Name = "Uppsala", ImageSrc = "uppsala.jpg"},
                new CityPicture() { Name = "Stockholm", ImageSrc = "stockholm.jpg"},
                new CityPicture() { Name = "New York", ImageSrc = "newyork.jpg"},
                new CityPicture() { Name = "Los Angeles", ImageSrc = "losangeles.jpg"},
                new CityPicture() { Name = "Bombay", ImageSrc = "bombay.jpg"},
                new CityPicture() { Name = "Bangkok", ImageSrc = "bangkok.jpg"},
                new CityPicture() { Name = "São Paulo", ImageSrc = "saopaulo.jpg"},
                new CityPicture() { Name = "Buenos Aires", ImageSrc = "buenosaires.jpg"},
                new CityPicture() { Name = "Tunis", ImageSrc = "tunis.jpg"},
                new CityPicture() { Name = "Cape Town", ImageSrc = "capetown.jpg"},
                new CityPicture() { Name = "Sydney", ImageSrc = "sydney.jpg"},
                new CityPicture() { Name = "Canberra", ImageSrc = "canberra.jpg"},
            };
    }
}
