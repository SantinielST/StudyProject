
using System.Net;

namespace StudyProjectConsole 
{
    class Program
    {
        private const string dataUrl = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        
        static void Main(string[] args)
        {
            //WebClient webClient = new WebClient();
            var client = new HttpClient();

            var response = client.GetAsync(dataUrl).Result;
            var csvStr = response.Content.ReadAsStringAsync().Result;

            Console.ReadLine();        }
    }
}