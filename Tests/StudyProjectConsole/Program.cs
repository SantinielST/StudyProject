
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
                yield return line.Replace("Korea,", "Korea -");
            }
        }

        private static DateTime[] GetDates() => GetDataLines()
            .First()
            .Split(',')
            .Skip(4)
            .Select(s => DateTime.Parse(s, CultureInfo.InvariantCulture))
            .ToArray();

        private static IEnumerable<(string Country, string Province, int[] Counts)> GetData()
        {
            var lines = GetDataLines()
                .Skip(1)
                .Select(line => line.Split(','));

            foreach (var row in lines)
            {
                var province = row[0].Trim();
                var countryName = row[1].Trim(' ', '"');
                var counts = row.Skip(4).Select(int.Parse).ToArray();

                yield return (countryName, province, counts);
            }
        }

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

            //var dates = GetDates();
            //Console.WriteLine(String.Join("\r\n", dates));

            //var russiaData = GetData().First(v => v.Country.Equals("Seychelles", StringComparison.OrdinalIgnoreCase));

            var datas = GetData().First(v => v.Country.Equals("Italy"));

            //Console.WriteLine(string.Join("\r\n", GetDates().Zip(russiaData.Counts, (date, count) => $"{date:dd:MM} - {count}")));
            Console.WriteLine(string.Join("\r\n", GetDates().Zip(datas.Counts, (date, count) => $"{date:dd:MM:yy} - {count}")));

            Console.ReadLine();       
        }
    }
}