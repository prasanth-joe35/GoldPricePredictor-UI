using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoldPricePredictor.Services
{
    public class GoldPriceService
    {
        private readonly HttpClient _httpClient;

        public GoldPriceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Method to call Flask API and get the prediction
        public async Task<string> GetPredictedPrice(decimal usdInr, decimal goldUsd)
        {
            var predictionRequest = new
            {
                usd_inr = usdInr,
                gold_usd = goldUsd
            };

            var content = new StringContent(JsonConvert.SerializeObject(predictionRequest), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("http://127.0.0.1:7860/predict", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return result; // You can parse the response to extract the predicted value
            }
            else
            {
                return "Error occurred while fetching prediction.";
            }
        }
    }
}
