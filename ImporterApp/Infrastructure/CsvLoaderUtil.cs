using System;
using System.Collections.Generic;
using System.IO;

namespace ImporterApp.Infrastructure
{
    public static class CsvLoaderUtil
    {
        public static List<T> LoadFromCsv<T>(string csvPath, Func<string[], T?> mapFunc) where T : class
        {
            var list = new List<T>();
            using (var reader = new StreamReader(csvPath))
            {
                string header = reader.ReadLine(); // skip header
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var cols = line.Split(',');
                    var obj = mapFunc(cols);
                    if (obj != null)
                        list.Add(obj);
                }
            }
            return list;
        }
    }
}
