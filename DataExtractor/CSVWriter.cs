using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DataExtractor
{
    class CSVWriter : IWriter
    {
        static readonly CsvConfiguration WriterSettings = new(CultureInfo.CurrentCulture) { HasHeaderRecord = true };

        public void Write(string fileName, IEnumerable<StockOutputEntry> records)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentException("Output fileName not provided");

            records ??= Enumerable.Empty<StockOutputEntry>(); 

            using var streamWriter = new StreamWriter(fileName);
            using var csvWriter = new CsvWriter(streamWriter, WriterSettings);

            csvWriter.Context.RegisterClassMap<StockOutputEntryMap>();
            csvWriter.WriteHeader<StockOutputEntry>();
            csvWriter.NextRecord();

            foreach (var record in records)
            {
                csvWriter.WriteRecord(record);
                csvWriter.NextRecord();
            }
        }

        class StockOutputEntryMap : ClassMap<StockOutputEntry>
        {
            public StockOutputEntryMap()
            {
                Map(m => m.ISIN).Index(0);
                Map(m => m.CFICode).Index(1);
                Map(m => m.Venue).Index(2);
                Map(m => m.PriceMultiplier).Index(3).Name("ContractSize");
            }
        }

    }

}
