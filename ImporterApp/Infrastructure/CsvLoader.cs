using System.Collections.Generic;
using System.IO;

namespace ImporterApp.Infrastructure
{
    // CSVをメモリロードする
    public static class CsvLoader
    {
        public static List<Dictionary<string, string>> LoadCsv(string filePath)
        {
            var result = new List<Dictionary<string, string>>();
            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 0) return result;

            var headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                var row = new Dictionary<string, string>();

                for (int j = 0; j < headers.Length; j++)
                {
                    row[headers[j]] = values[j];
                }

                result.Add(row);
            }

            return result;
        }
    }
}
