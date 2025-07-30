using Newtonsoft.Json.Converters;


namespace PokeApiNueva.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "c70139fcc15039e8202d1c1cb1857acb";
        public WeatherService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetWeatherAsync(string city) 
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric";
            var response=await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode) 
            
                return "WEATHER IS NOT AVAILABLE";

                var json = await response.Content.ReadAsStringAsync();
                dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                string description = data.weather[0].description;
                double temp = data.main.temp;

                return $"{description} y {temp}°C";
            

        }
    }
}
