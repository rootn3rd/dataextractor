using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataExtractor.Tests
{
    [TestClass]
    public class WriterTests
    {
        [TestMethod]
        public void AbleToWriteProperly()
        {
            var writer = new CSVWriter();
            var outputFileName = @"someoutput";

            if (File.Exists(outputFileName)) File.Delete(outputFileName);

            var entries = new List<StockOutputEntry>
            {
                new StockOutputEntry(ISIN: "DE000ABCDEFG", CFICode: "FFICSX", Venue: "XEUR", PriceMultiplier: "20.0"),
                new StockOutputEntry(ISIN: "PL0ABCDEFGHI", CFICode: "FFICSX", Venue: "WDER", PriceMultiplier: "25.0"),
            };

            writer.Write(outputFileName, entries);
            Assert.IsTrue(File.Exists(outputFileName));
        }
        [TestMethod]
        public void HandlesNullOutputFileName()
        {
            var writer = new CSVWriter();
            Assert.ThrowsException<ArgumentException>(()=>writer.Write(null, null));
        }


        [TestMethod]
        public void HandlesNullOutputOutputContent()
        {
            var writer = new CSVWriter();
            var outputFileName = @"emptyoutputcase";

            if (File.Exists(outputFileName)) File.Delete(outputFileName);

            writer.Write(outputFileName, null);

            Assert.IsTrue(File.Exists(outputFileName));
            Assert.IsTrue(File.ReadAllLines(outputFileName).Count() == 1); //only the header stays
        }
    }

}
