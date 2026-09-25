using WeatherApp.ViewModel;

namespace WeatherApp.View;

public partial class WeatherForecastPage : ContentPage
{

    public List<Model.List> WeatherList { get; set; } = new List<Model.List>();
    public double latitude;
    public double longitude;


    public WeatherForecastPage()
    {
        InitializeComponent();
    }


    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await GetLocation();
        await GetWeatherByLocation(latitude, longitude);

    }

    public async Task GetWeatherByLocation(double lattitude, double longitude)
    {
        var result = await ApiService.GetWeather(latitude, longitude);
        UpdateUI(result);
    }

    public async Task GetLocation()
    {
        try
        {
            var location = await Geolocation.GetLocationAsync();
            if (location is not null)
            {
                latitude = location.Latitude;
                longitude = location.Longitude;
            }
            else
            {
                await DisplayAlertAsync("Error", "Nem lehet helyet szerezni", "Ok");
            }
        }
        catch (Exception ex)
        {
            latitude = 47.211;
            longitude = 17.729;
        }
    }

    public void UpdateUI(dynamic result)
    {
        foreach (var item in result.List)
        {
            WeatherList.Add(item);
        }

        collectionView.ItemsSource = WeatherList;


        try
        {
            string weather = result.List[0].Weather[0].CustomIcon;

            switch (weather)
            {
                case "icon_01d.png":
                case "icon_01n.png":
                    bgImg.Source = "misty.jpg";
                    break;

                case "icon_13n.png":
                case "icon_13d.png":
                    bgImg.Source = "snowy.jpg";
                    break;
                default:
                    bgImg.Source = "stormy.jpg";
                    break;

            }
        }
        catch { }
    }

}