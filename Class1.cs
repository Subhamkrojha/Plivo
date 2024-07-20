using CsvHelper;
using CsvHelper.Configuration;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentSubhamKumarOjhaPlivo
{
    public class Class1
    {

        [Test]
        static void Main(string[] args)
        {
            // Initialize Chrome WebDriver
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--headless"); // Optional: run headlessly
            IWebDriver driver = new ChromeDriver(options);

            // Read cities from CSV
            List<string> cities = ReadCitiesFromCsv("city.csv");

            // Initialize API key (replace with your OpenWeatherMap API key)
            string apiKey = "cf22704e2473187c2e49b0d14a24f8c8";

            // Get weather statistics for coldest cities and highest humidity cities
            int topN = 3; // Default top N
            var coldestCities = GetTopNCitiesWithLowestTemperature(cities, apiKey, topN);
            var humidCities = GetTopNCitiesWithHighestHumidity(cities, apiKey, topN);

            // Save results to CSV
            SaveToCsv(coldestCities, humidCities, "city_stats.csv");

            // Close the WebDriver
            driver.Quit();
        }

        private static void SaveToCsv(List<string> coldestCities, List<string> humidCities, string v)
        {
            throw new NotImplementedException();
        }

        static List<string> ReadCitiesFromCsv(string filePath)
        {
            List<string> cities = new List<string>();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader((IParser)reader))
            {
                while (csv.Read())
                {
                    cities.Add(csv.GetField("City"));
                }
            }

            return cities;
        }

        static List<string> GetTopNCitiesWithLowestTemperature(List<string> cities, string apiKey, int topN)
        {
            List<string> coldestCities = new List<string>();

            foreach (var city in cities)
            {
                // Fetch weather data for the city
                var weatherData = FetchWeatherData(city, apiKey);

                // Process weather data to get temperature
                if (weatherData != null)
                {
                    double temperature = Convert.ToDouble(weatherData["main"]["temp"]);
                    coldestCities.Add($"{city}: {temperature}°C");
                }
            }

            // Sort coldestCities by temperature and take top N
            coldestCities.Sort((a, b) =>
            {
                double tempA = Convert.ToDouble(a.Split(':')[1].TrimEnd('°', 'C'));
                double tempB = Convert.ToDouble(b.Split(':')[1].TrimEnd('°', 'C'));
                return tempA.CompareTo(tempB);
            });

            return coldestCities.Take(topN).ToList();
        }

        static List<string> GetTopNCitiesWithHighestHumidity(List<string> cities, string apiKey, int topN)
        {
            List<string> humidCities = new List<string>();

            foreach (var city in cities)
            {
                // Fetch weather data for the city
                var weatherData = FetchWeatherData(city, apiKey);

                // Process weather data to get humidity
                if (weatherData != null)
                {
                    double humidity = Convert.ToDouble(weatherData["main"]["humidity"]);
                    humidCities.Add($"{city}: {humidity}%");
                }
            }

            // Sort humidCities by humidity and take top N
            humidCities.Sort((a, b) =>
            {
                double humidityA = Convert.ToDouble(a.Split(':')[1].TrimEnd('%'));
                double humidityB = Convert.ToDouble(b.Split(':')[1].TrimEnd('%'));
                return humidityB.CompareTo(humidityA); // Descending order
            });

            return humidCities.Take(topN).ToList();
        }

        static JObject FetchWeatherData(string city, string apiKey)
        {
            // Implement logic to fetch weather data using OpenWeatherMap's One Call API
            // This involves making HTTP requests and parsing JSON responses
            // Example implementation is omitted for brevity
            // Should return a JObject containing weather data
            return null; // Replace with actual implementation
        }

        static void savetocsv(List<string> coldestcities, List<string> humidcities, string filepath)
        {
            using (var writer = new StreamWriter(filepath))
            using (var csv = new CsvHelper.CsvWriter(writer, new CsvConfiguration(HasHeaderRecord = true)))
            {
                csv.WriteField("top coldest cities");
                csv.NextRecord();
                foreach (var city in coldestcities)
                {
                    csv.WriteField(city);
                    csv.NextRecord();
                }

                csv.WriteField("top humid cities");
                csv.NextRecord();
                foreach (var city in humidcities)
                {
                    csv.WriteField(city);
                    csv.NextRecord();
                }
            }


        }
    }
}


