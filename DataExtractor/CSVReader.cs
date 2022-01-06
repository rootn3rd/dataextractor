using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DataExtractor
{
    class CSVReader : IReader
    {
        static readonly CsvConfiguration ReaderSettings = new(CultureInfo.CurrentCulture) { HasHeaderRecord = true };

        public IEnumerable<StockEntry> Read(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Input fileName not provided");
            if (!File.Exists(fileName)) throw new FileNotFoundException($"No such file found - {fileName}");

            using var streamReader = File.OpenText(fileName);

            //ignore first line
            streamReader.ReadLine();

            using var csvReader = new CsvReader(streamReader, ReaderSettings);

            return csvReader.GetRecords<StockEntry>().ToList();
        }
    }

}
