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
    
    private void TapLocation_Tapped(object sender, TappedEventArgs e)
    {

    }

    private void TapSearch_Tapped(object sender, TappedEventArgs e)
    {

    }
}