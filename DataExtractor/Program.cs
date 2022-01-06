using System;
using System.Threading.Tasks;

namespace DataExtractor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                StockProcessingEngine.ShowHelp();
                return;
            }

            var inputFile = args[0];
            var outputFile = args[1];
            StockProcessingEngine.Execute(inputFile, outputFile);

            Console.WriteLine("Completed!!");

        }
      
    }

}
