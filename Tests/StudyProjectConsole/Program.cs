
using System.Globalization;
using System.Net;

namespace StudyProjectConsole 
{
    class Program
    {
        private const string dataUrl = @"https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_global.csv";
        
        private static async Task<Stream> GetDataStream()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(dataUrl, HttpCompletionOption.ResponseHeadersRead);
            return await response.Content.ReadAsStreamAsync();
        }

        private static IEnumerable<string> GetDataLines()
        {
            using var dataStream = GetDataStream().Result;
            using var dataReader = new StreamReader(dataStream);

            while (!dataReader.EndOfStream)
            {
                var line = dataReader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                yield return line;
            }
        }

        private static DateTime[] GetDates() => GetDataLines()
            .First()
            .Split(',')
            .Skip(4)
            .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))
            .ToArray();



        static void Main(string[] args)
        {
            //WebClient webClient = new WebClient();
            //var client = new HttpClient();

            //var response = client.GetAsync(dataUrl).Result;
            //var csvStr = response.Content.ReadAsStringAsync().Result;

            //foreach (var dataLine in GetDataLines())
            //{
            //    Console.WriteLine(dataLine);
            //}

            var dates = GetDates();
            Console.WriteLine(String.Join("\r\n", dates));

            Console.ReadLine();       
        }
    }
}