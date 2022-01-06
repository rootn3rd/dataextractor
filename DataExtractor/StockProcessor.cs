using System;
using System.Collections.Generic;
using System.Linq;

namespace DataExtractor
{
    class StockProcessor : IProcessor
    {
        readonly Func<string, IEnumerable<StockEntry>> _reader;
        readonly Action<string, IEnumerable<StockOutputEntry>> _writer;
        readonly Func<StockEntry, StockOutputEntry> _selector;

        private StockProcessor(
            Func<string, IEnumerable<StockEntry>> reader,
            Action<string, IEnumerable<StockOutputEntry>> writer,
            Func<StockEntry, StockOutputEntry> selector)
        {
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _selector = selector ?? throw new ArgumentNullException(nameof(selector));

        }

        public void Process(string inputFile, string outputFile)
        {
            var stockEntries = _reader(inputFile);

            var stockOutputEntries = stockEntries.Select(_selector);

            _writer(outputFile, stockOutputEntries);
        }


        internal static IProcessor Create(IReader reader, IWriter writer, Func<StockEntry, StockOutputEntry> selector)
        {
            Func<string, IEnumerable<StockEntry>> readFunc = reader == null? null : reader.Read;
            Action<string, IEnumerable<StockOutputEntry>> writeFunc = writer == null? null : writer.Write;
            
            return new StockProcessor(readFunc, writeFunc, selector);
        }

        internal static IProcessor Create(
            Func<string, IEnumerable<StockEntry>> reader,
            Action<string, IEnumerable<StockOutputEntry>> writer,
            Func<StockEntry, StockOutputEntry> selector)
           => new StockProcessor(reader, writer, selector);



    }

}
