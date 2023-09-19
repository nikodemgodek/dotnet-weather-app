using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfApp1
{
    public class WeatherRetriever
    {
        private readonly string apiKey;

        public WeatherRetriever(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public async Task<string> GetWeatherData(string city)
        {
            using(HttpClient httpClient = new HttpClient())
            {
                try
                {
                    string apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if(response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                        
                    } else
                    {
                        return "Failed to fetch data.";
                    }

                } catch(Exception ex)
                {
                    return $"Wystąpił błąd: {ex.Message}";
                }
            }
        }
    }
}
