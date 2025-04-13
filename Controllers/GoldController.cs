using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace GoldPricePredictor_UI.Controllers
{
    public class GoldController : Controller
    {
        private readonly HttpClient _http;

        public GoldController()
        {
            _http = new HttpClient();
            _http.BaseAddress = new System.Uri("http://127.0.0.1:5000/");
        }

        // UI View for Dashboard
        public IActionResult Index(string activeTab = "predict", string date = "")
        {
            ViewBag.ActiveTab = activeTab;
            ViewBag.ForecastDate = TempData["ForecastDate"];
            ViewBag.ForecastResult = TempData["ForecastResult"];
            ViewBag.UsdInr = TempData["UsdInr"];
            ViewBag.GoldUsd = TempData["GoldUsd"];
            ViewBag.PredictedPrice = TempData["PredictedPrice"];
            return View();
        }



        // POST (HTML form) - What-if Simulation tab
        [HttpPost]
        public async Task<IActionResult> Predict(double usd_inr, double gold_usd)
        {
            var input = new { usd_inr, gold_usd };
            var json = JsonConvert.SerializeObject(input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("predict", content);
            var result = await response.Content.ReadAsStringAsync();
            dynamic? data = JsonConvert.DeserializeObject(result);

            ViewBag.PredictedPrice = data?.predicted_price;
            ViewBag.UsdInr = usd_inr;
            ViewBag.GoldUsd = gold_usd;

            return View("Index");
        }

        // GET (HTML view) - Forecast Chart tab
        public async Task<IActionResult> Forecast()
        {
            var response = await _http.GetAsync("forecast?days=180");
            var result = await response.Content.ReadAsStringAsync();
            var forecast = JsonConvert.DeserializeObject<List<ForecastItem>>(result);
            return View(forecast);
        }

        // GET (HTML View) - Forecast By Date tab
        public async Task<IActionResult> ForecastByDate(string date)
        {
            var response = await _http.GetAsync($"forecast-by-date?date={date}");
            var result = await response.Content.ReadAsStringAsync();
            dynamic? data = JsonConvert.DeserializeObject(result);

            TempData["ForecastDate"] = date;
            TempData["ForecastResult"] = data?.predicted_price?.ToString() ?? "N/A";

            return RedirectToAction("Index", new { activeTab = "date" });
        }




        // POST - /Gold/PredictApi
        [HttpPost]
        public async Task<IActionResult> PredictApi([FromBody] PredictionInput input)
        {
            var json = JsonConvert.SerializeObject(input);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _http.PostAsync("predict", content);
            var result = await response.Content.ReadAsStringAsync();

            return Content(result, "application/json");
        }

        // GET - /Gold/ForecastApi
        [HttpGet]
        public async Task<IActionResult> ForecastApi(int days = 180)
        {
            var response = await _http.GetAsync($"forecast?days={days}");
            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }

        // GET - /Gold/ForecastByDateApi
        [HttpGet]
        public async Task<IActionResult> ForecastByDateApi(string date)
        {
            var response = await _http.GetAsync($"forecast-by-date?date={date}");
            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }
    }

    public class ForecastItem
    {
        public string? ds { get; set; }
        public double yhat { get; set; }
        public double yhat_upper { get; set; }
        public double yhat_lower { get; set; }
    }

    public class PredictionInput
    {
        public double usd_inr { get; set; }
        public double gold_usd { get; set; }
    }
}
