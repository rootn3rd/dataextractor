using System;

namespace DataExtractor
{
    public static class StockProcessingEngine
    {
        public static void Execute(string inputFile, string outputFile)
        {
            var reader = new CSVReader();
            var writer = new CSVWriter();
            Func<StockEntry, StockOutputEntry> converter = (e) => e.ToOutputEntry();

            var processor = StockProcessor.Create(reader, writer, converter);

            processor.Process(inputFile, outputFile);
        }

        public static void ShowHelp()
        {
            Console.WriteLine("\nUSAGE - DataExtractor.exe <input_file> <output_file>");
            Console.WriteLine("Example - DataExtractor.exe Data\\input.csv Data\\output.csv\n");
        }
    }

}
