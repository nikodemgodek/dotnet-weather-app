using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Globalization;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    //

    public class WeatherForecast
    {
        public DateTime Date { get; set; }
        public String Description { get; set; }
        public double Temperature { get; set; }
    }

    public partial class MainWindow : Window
    {

        string API_KEY = "b9e2e0e4e943a70d1e0bcc1114a8dbe3";
        string city = "Dresden";

        private bool isFetchingData = false;

        private DispatcherTimer timer;

        List<WeatherForecast> forecasts = new List<WeatherForecast>
        {
            new WeatherForecast { Date = DateTime.Now, Description = "Słonecznie", Temperature = 32 },
            new WeatherForecast { Date = DateTime.Now.AddDays(1), Description = "Pochmurnie", Temperature = 25.5 }
        };


        public MainWindow()
        {
            InitializeComponent();
            LoadDefaults();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            timer.Start();

            //get date

            DateTime currentDate = DateTime.Now;
            CultureInfo cultureInfo = new CultureInfo("pl-PL");
            string formattedDate = currentDate.ToString("d MMMM yyyy", cultureInfo);
            dateContainer.Text = formattedDate;
            weatherListBox.ItemsSource = forecasts;

        }

        private void LoadDefaults()
        {
            timeContainer.Text = "00:00:00";
            dateContainer.Text = "1 stycznia 2000";
            cityContainer.Text = "---";
            tempContainer.Text = "-- ℃";
            errorLabel.Content = "";
        }

        private async void LoadWeatherData()
        {
            errorLabel.Content = "";
            WeatherRetriever wr = new WeatherRetriever(API_KEY);
            string data = await wr.GetWeatherData(searchContainer.Text);

            if(data == "Failed to fetch data.")
            {
                errorLabel.Content = "Wystąpił błąd podczas pobierania danych pogody.";
                return;
            }
            WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(data);
            cityContainer.Text = weatherData.Name;
            tempContainer.Text = Math.Round(weatherData.Main.Temp).ToString() + " ℃";
        }

        private void Timer_Tick(object send, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("HH:mm:ss");
            timeContainer.Text = formattedTime;
           
        }

        private async void searchContainer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !isFetchingData)
            {
                LoadWeatherData();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadWeatherData();
        }
    }
}
