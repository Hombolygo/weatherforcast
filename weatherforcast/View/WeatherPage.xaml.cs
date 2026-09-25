using WeatherApp.ViewModel;

namespace WeatherApp.View;

public partial class WeatherPage : ContentPage
{
    public List<Model.List> WeatherList { get; set; } = new List<Model.List>();
    public double latitude;
    public double longitude;

    

    public WeatherPage()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await GetLocation();
        await GetWeatherByLocation(latitude, longitude);

    }

    public void UpdateUI(dynamic result)
    {
        foreach (var item in result.List)
        {
            WeatherList.Add(item);
        }
        lblCity.Text = result.City.Name;
        lblWeather.Text = result.List[0].Weather[0].Description;
        lblHumidity.Text = result.List[0].Main.Humidity + " %";
        lblTemp.Text = result.List[0].Main.Temperature + " °C";
        lblWind.Text = result.List[0].Wind.Speed;

        WeatherIcon.Source = result.List[0].Weather.CustomIcon;

        double temp = result.List[0].Main.Temperature;

        if (temp >= 30 && temp < 40) lblTemp.TextColor = Colors.Red; 
        if (temp >= 20 && temp < 30) lblTemp.TextColor = Colors.Orange;
        if (temp >= 10 && temp < 20) lblTemp.TextColor = Colors.Yellow;
        if (temp >= 0 && temp < 10) lblTemp.TextColor = Colors.CornflowerBlue;
        if (temp < 0 ) lblTemp.TextColor = Colors.Blue;

        string weather = result.List[0].Weather.CustomIcon;

        switch (weather)
        {
            case "icon_01d.png":
            case "icon_01n.png":
                bdImg.Source = "misty.jpg";
                break;

            case "icon_13n.png":
            case "icon_13d.png":
                bdImg.Source = "snowy.jpg";
                break;
            default:
                bdImg.Source = "stormy.jpg";
                break;

        }


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
        catch (Exception ex) {
            latitude = 47.211;
            longitude = 17.729;
        }
    }

    public async Task GetWeatherByLocation(double lattitude, double longitude)
    {
        var result = await ApiService.GetWeather(latitude, longitude);
        UpdateUI(result);
    }

    public async Task GetWeatherDataByCity(string city)
    {
        var result = await ApiService.GetWeatherByCity(city);
        UpdateUI(result);
    }

    private async void TapLocation_Tapped(object sender, TappedEventArgs e)
    {
        await GetLocation();
        await GetWeatherByLocation(latitude, longitude);
    }

    private async void TapSearch_Tapped(object sender, TappedEventArgs e)
    {
        var response = await DisplayPromptAsync(
                title: "Keresés...",
                message: "Település neve: ",
                placeholder: "Ide jön  a név",
                accept: "Ok",
                cancel: "mégse"
            );

        if (response is null)
        {
            await DisplayAlertAsync("Mávelet vége: ", "Keresés meg lett szakítva", "Sajnos");
            return;
        }

        if (string.IsNullOrWhiteSpace(response))
        {
            await DisplayAlertAsync("Mávelet vége: ", "Keresés üres", "Sajnos");
            return;
        }

        await GetWeatherDataByCity(response);



    }
}